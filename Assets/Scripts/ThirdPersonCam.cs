using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public ObjectOrientation orientation;
    public Character character;
    public CharacterGraphic characterGraphic;
    public Camera cam;
    public Rigidbody rb;
    public CinemachineOrbitalFollow basicOrbitalFollow;
    public CinemachineOrbitalFollow combatOrbitalFollow;

    [Header("Camera Style")]
    public GameObject basicCamera;
    public GameObject combatCamera;

    private Vector2 moveInputValue;

    public float rotationSpeed = 7;
    private bool isGamePaused;
    private GameObject currentObject;

    public static ThirdPersonCam instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnMove(InputValue value)
    {
        if (!PlayerMovement.instance.canMove)
            return;

        moveInputValue = value.Get<Vector2>();
    }

    public void OnPause(InputValue value)
    {
        isGamePaused = !isGamePaused;
        Cursor.lockState = isGamePaused ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isGamePaused;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        // Rotate Orientation
        Vector3 viewDir = character.transform.position - new Vector3(cam.transform.position.x, character.transform.position.y, cam.transform.position.z);
        orientation.transform.forward = viewDir.normalized;

        Vector3 inputDir = orientation.transform.forward * moveInputValue.y + orientation.transform.right * moveInputValue.x;

        if (inputDir != Vector3.zero)
            characterGraphic.transform.forward = Vector3.Slerp(characterGraphic.transform.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

        MakeItemGlowOnSight();
    }

    void MakeItemGlowOnSight()
    {
        GameObject oldObject = currentObject;
        currentObject = CheckIfLookingAtALayer(Telekinesis.instance.levitatableLayer);

        if (oldObject != null)
            if(oldObject.transform.GetComponentInParent<InteractableObject>() != null)
                oldObject.transform.GetComponentInParent<InteractableObject>().isGlowing = false;

        if (currentObject != null)
            if (currentObject.transform.GetComponentInParent<InteractableObject>() != null)
                if(currentObject.transform.GetComponentInParent<InteractableObject>().isLevitating == false)
                    currentObject.transform.GetComponentInParent<InteractableObject>().isGlowing = true;
    }

    public GameObject CheckIfLookingAtALayer(LayerMask layer)
    {
        RaycastHit hit;
        if (Physics.Raycast(character.transform.position + new Vector3(0, 0.8f, 0), cam.transform.forward, out hit, 6.5f, layer))
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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(character.transform.position + new Vector3(0, 0.8f, 0), character.transform.position + new Vector3(0, 0.8f, 0) + cam.transform.forward * 6.5f);
    }
}
