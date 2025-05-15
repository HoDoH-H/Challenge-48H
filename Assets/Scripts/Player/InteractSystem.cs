using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class InteractSystem : MonoBehaviour
{
    [Header("References")]
    public FirstPersonCam cam;

    [Header("Settings")]
    public LayerMask interactableLayer;

    [Header("Debug")]
    [SerializeField] private InteractableObject currentObject;
    private bool doMoveToTarget = true;


    public static InteractSystem instance;

    PlayerInputActions actions;
    InputAction interact;

    private void Awake()
    {
        instance = this;

        actions = new PlayerInputActions();
        interact = actions.Player.Interact;
    }

    private void OnEnable() => actions.Enable();

    private void OnDisable() => actions.Disable();

    private void Start()
    {
        
    }

    IEnumerator Take(InteractableObject obj)
    {
        if (obj == null)
            yield break;

        yield return new WaitForSeconds(0.5f);

        SetCurrentObject(obj);
    }

    void ClearCurrentObject()
    {
        if(currentObject == null)
            return;

        SetCurrentObject(null);
    }

    void SetCurrentObject(InteractableObject obj)
    {
        currentObject = obj;
    }

    private void Update()
    {
        Interact();
    }

    void Interact()
    {
        if (interact.WasPressedThisFrame())
        {
            Debug.Log("Interact pressed");
            if (currentObject == null)
            {
                // Check if looking at a levitatable object
                GameObject obj = cam.CheckIfLookingAtALayer(interactableLayer);
                if (obj == null)
                    return;

                // Take Item
                Debug.Log("Taking item: " + obj.name);
            }
        }
    }
}
