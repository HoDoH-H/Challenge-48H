using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class FirstPersonCam : MonoBehaviour
{
    [Header("References")]
    public ObjectOrientation orientation;
    public Player character;
    public Character characterGraphic;
    public Camera cam;
    public CameraPosition cameraPosition;
    public Rigidbody rb;

    private Vector2 moveInputValue;

    public Vector2 rotationSpeed = new Vector2(7f, 7f);
    public bool canLook = true;
    private bool isGamePaused;
    private GameObject currentObject;

    float xRotation;
    float yRotation;

    PlayerInputActions actions;
    InputAction look;

    public static FirstPersonCam instance;

    private void Awake()
    {
        instance = this;

        actions = new PlayerInputActions();
        look = actions.Player.Look;
    }

    public void OnPause(InputValue value)
    {
        isGamePaused = !isGamePaused;
        Cursor.lockState = isGamePaused ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isGamePaused;
    }

    private void OnEnable() => actions.Enable();

    private void OnDisable() => actions.Disable();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        if (canLook)
        {
            float mouseX = look.ReadValue<Vector2>().x * rotationSpeed.x * Time.deltaTime;
            float mouseY = look.ReadValue<Vector2>().y * rotationSpeed.y * Time.deltaTime;
            xRotation -= mouseY;
            yRotation += mouseX;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);

            MakeItemGlowOnSight();
        }
        else
        {
            if (currentObject != null)
                if (currentObject.transform.GetComponentInParent<InteractableObject>() != null)
                    currentObject.transform.GetComponentInParent<InteractableObject>().isGlowing = false;
            currentObject = null;
        }
    }

    void MakeItemGlowOnSight()
    {
        GameObject oldObject = currentObject;
        currentObject = CheckIfLookingAtALayer(InteractSystem.instance.interactableLayer);

        if (oldObject != null)
            if (oldObject.transform.GetComponentInParent<InteractableObject>() != null)
                oldObject.transform.GetComponentInParent<InteractableObject>().isGlowing = false;

        if (currentObject != null)
            if (currentObject.transform.GetComponentInParent<InteractableObject>() != null)
                currentObject.transform.GetComponentInParent<InteractableObject>().isGlowing = true;
    }

    public GameObject CheckIfLookingAtALayer(LayerMask layer)
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPosition.transform.position, cam.transform.forward, out hit, 6.5f, layer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void OnValidate()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(cameraPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(cameraPosition.transform.position, cameraPosition.transform.position + cam.transform.forward * 6.5f);
        }
    }
}
