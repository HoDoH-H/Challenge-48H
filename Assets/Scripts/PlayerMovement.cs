using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public ObjectOrientation orientation;
    public Rigidbody rb;
    public CharacterGraphic playerCharacter;
    public Animator characterAnimator;
    public ThirdPersonCam camController;

    [Header("Movement")]
    private float moveSpeed = 4.5f;
    public float walkSpeed = 4.5f;
    public float runSpeed = 9f;
    public float groundDrag = 5f;
    public bool canMove = true;

    [Header("Jumping")]
    public float TimeBeforeJumping = 0.35f;
    public float jumpForce = 7.5f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    private bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public float groundDistance = 0.05f;
    public LayerMask whatIsGround;
    public bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 35f;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    private Vector2 moveInputValue;
    private bool isJumping;
    private bool isRunning;
    private Vector3 moveDirection;

    [SerializeField] float currentSpeed;
    [SerializeField] PlayerState state;

    public static PlayerMovement instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnMove(InputValue value)
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        moveInputValue = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        isJumping = value.Get<float>() == 1;
    }

    public void OnSprint(InputValue value)
    {
        isRunning = value.Get<float>() == 1;
    }

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        playerHeight = playerCharacter.GetComponent<CapsuleCollider>().height;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Check if player is grounded
        isGrounded = Physics.Raycast(rb.transform.position, Vector3.down, playerHeight * 0.5f + groundDistance, whatIsGround) || OnSlope();

        Inputs();
        SpeedControl();
        StateHandler();
        UpdateAnimator();

        // Apply drag
        rb.linearDamping = isGrounded ? groundDrag : 0f;

        Vector2 horizontalVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z);
        currentSpeed = horizontalVelocity.magnitude;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void UpdateAnimator()
    {
        
    }

    void Inputs()
    {
        if (readyToJump && isGrounded && isJumping)
        {
            StartCoroutine(Jump());
        }
    }

    void MovePlayer()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Calculate move direction
        moveDirection = orientation.transform.forward * moveInputValue.y + orientation.transform.right * moveInputValue.x;

        // On Slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMovementDirection() * moveSpeed * 20f, ForceMode.Force);

            //if (rb.linearVelocity.y > 0.1)
            rb.AddForce(-slopeHit.normal.normalized * 80f, ForceMode.Force);
        }

        if (isGrounded)
        { 
            rb.AddForce(moveDirection.normalized * moveSpeed * 20f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 20f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        if(OnSlope() && !exitingSlope)
        {
            if(rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    IEnumerator Jump()
    {
        readyToJump = false;
        if(currentSpeed < 4.6f)
            yield return new WaitForSeconds(TimeBeforeJumping);
        Invoke(nameof(ResetJump), jumpCooldown);

        exitingSlope = true;

        // Reset Y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce((moveDirection.normalized * currentSpeed) / 2 + Vector3.up * jumpForce * 2, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(playerCharacter.transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void StateHandler()
    {
        // Mode - Running
        if(isRunning && isGrounded)
        {
            state = PlayerState.Running;
            moveSpeed = runSpeed;
        }

        // Mode - Walking
        else if (!isRunning && isGrounded)
        {
            state = PlayerState.Walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else if (!isGrounded)
        {
            state = PlayerState.Air;
        }
    }

    public IEnumerator DeactivateMovementFor(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.transform.position, rb.transform.position + Vector3.down * (playerHeight * 0.5f + groundDistance));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rb.transform.position, rb.transform.position + moveDirection);
        Gizmos.DrawLine(rb.transform.position, rb.transform.position + GetSlopeMovementDirection());


        Gizmos.color = Color.green;
        Gizmos.DrawLine(rb.transform.position, rb.transform.position + Vector3.down * (playerHeight * 0.5f + 0.4f));
    }

    private void OnValidate()
    {
        
    }

    public enum PlayerState
    {
        Walking,
        Running,
        Air
    }
}
