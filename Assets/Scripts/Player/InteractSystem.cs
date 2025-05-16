using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class InteractSystem : MonoBehaviour
{
    [Header("References")]
    public FirstPersonCam cam;
    public CloseDisplayTarget closeDisplayTarget;

    [Header("Settings")]
    public LayerMask interactableLayer;

    [Header("Debug")]
    [SerializeField] private InteractableObject currentObject;
    private bool doMoveToTarget = true;


    public static InteractSystem instance;

    PlayerInputActions actions;
    InputAction interact, escape, look;

    private void Awake()
    {
        instance = this;

        actions = new PlayerInputActions();
        interact = actions.Player.Interact;
        escape = actions.Player.Escape;
        look = actions.Player.Look;
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
        StartCoroutine(Interact());
    }

    IEnumerator Interact()
    {
        if (interact.WasPressedThisFrame())
        {
            if (currentObject == null)
            {
                // Check if looking at a levitatable object
                GameObject obj = cam.CheckIfLookingAtALayer(interactableLayer);
                if (obj == null)
                    yield break;

                // Take Item
                var iObj = obj.GetComponent<InteractableObject>();
                if (iObj == null)
                    yield break;



                if (iObj.objectBase.ObjectType == ObjectType.JustShow)
                {
                    // Show item in UI
                    PlayerMovement.instance.canMove = false;
                    FirstPersonCam.instance.canLook = false;
                    iObj.GetComponent<BoxCollider>().enabled = false;
                    iObj.gameObject.SetActive(false);
                    yield return DisplayObjectClosely(iObj.gameObject);
                    Destroy(iObj.gameObject);
                    PlayerMovement.instance.canMove = true;
                    FirstPersonCam.instance.canLook = true;
                    GameManager.instance.IncreaseGameStage();
                }
                else if (iObj.objectBase.ObjectType == ObjectType.NeedShowAndRotation)
                {
                    // Show item in UI
                    PlayerMovement.instance.canMove = false;
                    FirstPersonCam.instance.canLook = false;
                    iObj.GetComponent<BoxCollider>().enabled = false;
                    iObj.gameObject.SetActive(false);
                    yield return DisplayObjectClosely(iObj.gameObject, needRotation: true);
                    Destroy(iObj.gameObject);
                    PlayerMovement.instance.canMove = true;
                    FirstPersonCam.instance.canLook = true;
                    GameManager.instance.IncreaseGameStage();
                }
                else if (iObj.objectBase.ObjectType == ObjectType.NeedChoices)
                {
                    PlayerMovement.instance.canMove = false;
                    FirstPersonCam.instance.canLook = false;
                    Interaction_choice.Instance.ShowChoices(iObj.objectBase.DreamText, iObj.objectBase.NightmareText);
                }
                else if (iObj.objectBase.ObjectType == ObjectType.NeedSpecialChoices)
                {
                    iObj.objectBase.Interact();
                }
            }
        }
    }

    IEnumerator DisplayObjectClosely(GameObject obj, bool needRotation = false)
    {
        var iObj = Instantiate(obj, closeDisplayTarget.transform.position, Quaternion.identity, closeDisplayTarget.transform);
        Destroy(iObj.transform.GetChild(0).gameObject);
        iObj.transform.localRotation = Quaternion.Euler(0, 90, 0);
        iObj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        iObj.SetActive(true);

        if(!needRotation)
            yield return new WaitUntil(() => escape.WasPerformedThisFrame());
        else
        {
            float yRotation = 90f;
            float zRotation = 0f;
            while (!escape.WasPerformedThisFrame())
            {
                
                float mouseX = look.ReadValue<Vector2>().x * FirstPersonCam.instance.rotationSpeed.x * Time.deltaTime;
                float mouseY = look.ReadValue<Vector2>().y * FirstPersonCam.instance.rotationSpeed.y * Time.deltaTime;
                yRotation -= mouseX;
                zRotation += mouseY;
                iObj.transform.localRotation = Quaternion.Euler(0, yRotation, zRotation);

                yield return new WaitForEndOfFrame();
            }
        }
        Destroy(iObj);
    }
}
