using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class InteractSystem : MonoBehaviour
{
    [Header("References")]
    public FirstPersonCam cam;
    public CloseDisplayTarget closeDisplayTarget;
    public AudioClip[] clips;
    public GameObject endUI;

    [Header("Settings")]
    public LayerMask interactableLayer;

    [Header("Debug")]
    [SerializeField] public InteractableObject currentObject;
    private bool doMoveToTarget = true;


    public static InteractSystem instance;

    PlayerInputActions actions;
    InputAction interact, escape, look, secInt;

    private void Awake()
    {
        instance = this;

        actions = new PlayerInputActions();
        interact = actions.Player.Interact;
        escape = actions.Player.Escape;
        look = actions.Player.Look;
        secInt = actions.Player.SecondaryAction;
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

                currentObject = iObj;

                if (iObj.objectBase.ObjectType == ObjectType.LittleTrain)
                {
                    // Show item in UI
                    AudioManager.Instance.PlaySFX(AudioId.Take);
                    PlayerMovement.instance.canMove = false;
                    FirstPersonCam.instance.canLook = false;
                    iObj.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(AudioManager.Instance.PlaySFX(iObj.objectBase.Sfx));
                    yield return DisplayObjectClosely(iObj.gameObject, new Vector3(0, 90, 0), distance: 0.75f, clipToPlay: clips[2], delay: clips[2].length + 1);
                    Destroy(iObj.gameObject);
                    PlayerMovement.instance.canMove = true;
                    FirstPersonCam.instance.canLook = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    GameManager.instance.IncreaseGameStage();
                    LevelManager.Instance.ChangeState(-1);
                    currentObject = null;
                }
                else if (iObj.objectBase.ObjectType == ObjectType.ChildDrawing)
                {
                    // Show item in UI
                    AudioManager.Instance.PlaySFX(AudioId.Take);
                    PlayerMovement.instance.canMove = false;
                    FirstPersonCam.instance.canLook = false;
                    iObj.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(AudioManager.Instance.PlaySFX(clips[1]));
                    yield return DisplayObjectClosely(iObj.gameObject, new Vector3(0, 90, -80), distance: 0.4f);
                    Destroy(iObj.gameObject);
                    PlayerMovement.instance.canMove = true;
                    FirstPersonCam.instance.canLook = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    GameManager.instance.IncreaseGameStage();
                    LevelManager.Instance.ChangeState(1);
                    currentObject = null;
                }
                else if (iObj.objectBase.ObjectType == ObjectType.CarPiece)
                {
                    AudioManager.Instance.PlaySFX(AudioId.Take);
                    GameManager.instance.gotCurrentObject = true;
                    Destroy(iObj.gameObject);
                }
                else if (iObj.objectBase.ObjectType == ObjectType.NewsPaper)
                {
                    AudioManager.Instance.PlaySFX(AudioId.Take);
                    PlayerMovement.instance.canMove = false;
                    FirstPersonCam.instance.canLook = false;
                    currentObject = iObj;
                    GameManager.instance.IncreaseGameStage();
                    StartCoroutine(AudioManager.Instance.PlaySFX(clips[3]));
                    Interaction_choice.Instance.ShowChoices(iObj.objectBase.DreamText, iObj.objectBase.NightmareText);
                }
                else if(iObj.objectBase.ObjectType == ObjectType.Key)
                {
                    AudioManager.Instance.PlaySFX(AudioId.Take);
                    StartCoroutine(AudioManager.Instance.PlaySFX(iObj.objectBase.Sfx));
                    GameManager.instance.gotCurrentObject = true;
                    Destroy(iObj.gameObject);
                }
                else if (iObj.objectBase.ObjectType == ObjectType.Doll)
                {
                    AudioManager.Instance.PlaySFX(AudioId.Take);
                    GameManager.instance.gotCurrentObject = true;
                    var objBase = iObj.objectBase;
                    Destroy(iObj.gameObject);

                    while (GameManager.instance.gotCurrentObject)
                    {
                        if (secInt.WasPerformedThisFrame())
                        {
                            LevelManager.Instance.ChangeState(1);
                            GameManager.instance.IncreaseGameStage();
                            StartCoroutine(AudioManager.Instance.PlaySFX(objBase.DreamSfx));
                            yield return new WaitForSeconds(.5f);
                            StartCoroutine(AudioManager.Instance.PlaySFX(clips[1]));
                        }

                        yield return new WaitForEndOfFrame();
                    }
                }
                else if(iObj.objectBase.ObjectType == ObjectType.Phone)
                {
                    AudioManager.Instance.PlaySFX(AudioId.Take);
                    if (LevelManager.Instance.stateGame > 0)
                    {
                        //Player wins
                        PlayerMovement.instance.canMove = false;
                        FirstPersonCam.instance.canLook = false;
                        iObj.GetComponent<BoxCollider>().enabled = false;
                        StartCoroutine(AudioManager.Instance.PlaySFX(clips[5]));
                        yield return new WaitForSeconds(clips[5].length);
                        StartCoroutine(AudioManager.Instance.PlaySFX(clips[6]));
                        Destroy(iObj.gameObject);
                        Cursor.lockState = CursorLockMode.Locked;
                        endUI.SetActive(true);
                        StartCoroutine(EndUI.Instance.Credits());
                    }
                    else
                    {
                        //Player loses
                        PlayerMovement.instance.canMove = false;
                        FirstPersonCam.instance.canLook = false;
                        iObj.GetComponent<BoxCollider>().enabled = false;
                        StartCoroutine(AudioManager.Instance.PlaySFX(clips[0]));
                        yield return DisplayObjectClosely(iObj.gameObject, new Vector3(0, 90, 0), needRotation: true, distance: 0.22f);
                        Destroy(iObj.gameObject);
                        PlayerMovement.instance.canMove = true;
                        FirstPersonCam.instance.canLook = true;
                        Cursor.lockState = CursorLockMode.Locked;

                        yield return new WaitForSeconds(1);
                        StartCoroutine(AudioManager.Instance.PlaySFX(clips[4]));
                        yield return new WaitForSeconds(clips[4].length);
                        endUI.SetActive(true);
                        StartCoroutine(EndUI.Instance.Credits());
                    }
                }
            }
        }
    }

    IEnumerator DisplayObjectClosely(GameObject obj, Vector3 rotation, bool needRotation = false, float distance = 0.3f, AudioClip clipToPlay = null, float delay = 0f)
    {
        closeDisplayTarget.transform.localPosition = new Vector3(0, -0.02f, distance);
        obj.transform.parent = closeDisplayTarget.transform;
        obj.transform.position = closeDisplayTarget.transform.position;
        obj.transform.localRotation = Quaternion.Euler(rotation);

        if(clipToPlay != null)
        {
            yield return new WaitForSeconds(delay);
        }

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
                obj.transform.localRotation = Quaternion.Euler(0, yRotation, zRotation);

                yield return new WaitForEndOfFrame();
            }
        }

        StartCoroutine(AudioManager.Instance.PlaySFX(clipToPlay));
        Destroy(obj);
    }
}
