using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Telekinesis : MonoBehaviour
{
    [Header("References")]
    public ThirdPersonCam cam;
    public TelekinesisTarget target;

    [Header("Settings")]
    public LayerMask levitatableLayer;
    public float throwForce = 10f;

    [Header("Debug")]
    [SerializeField] private InteractableObject currentObject;
    private bool doMoveToTarget = true;


    public static Telekinesis instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        target.levitatingObjectGottenTooFar += ClearCurrentObject;
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            if (currentObject == null)
            {
                // Check if looking at a levitatable object
                GameObject obj = cam.CheckIfLookingAtALayer(levitatableLayer);
                if (obj == null)
                    return;

                // Take Item
                //PlayerMovement.instance.characterAnimator.SetTrigger("Take");
                //StartCoroutine(Take(obj.GetComponentInParent<LevitatableObject>()));
                //StartCoroutine(PlayerMovement.instance.DeactivateMovementFor(0.6f));
            }
            else
            {
                // Drop Item

                ClearCurrentObject();
            }
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            if (currentObject == null)
                return;

            PlayerMovement.instance.characterAnimator.SetTrigger("Throw");
            StartCoroutine(Throw());
            StartCoroutine(PlayerMovement.instance.DeactivateMovementFor(1.2f));
        }
    }

    IEnumerator Throw()
    {
        Vector3 basePos = target.transform.localPosition;
        Vector3 targetPos = new Vector3(0, basePos.y, Mathf.Abs(basePos.x));

        float time = 0;
        while (time < 0.45f)
        {
            float t = time / 0.45f;
            target.transform.localPosition = new Vector3(Mathf.Lerp(basePos.x, targetPos.x, t), targetPos.y, Mathf.Lerp(basePos.z, targetPos.z, t));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.15f);

        target.transform.localPosition = targetPos;
        doMoveToTarget = !doMoveToTarget;
        currentObject.rb.useGravity = true;
        currentObject.isLevitating = false;
        currentObject.rb.linearVelocity = Vector3.zero;
        yield return new WaitForEndOfFrame();

        currentObject.rb.AddForce(cam.cam.transform.forward * throwForce, ForceMode.Impulse);
        SetCurrentObject(null);

        target.transform.localPosition = basePos;
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

        currentObject.isLevitating = false;
        currentObject.rb.useGravity = true;
        SetCurrentObject(null);
    }

    void SetCurrentObject(InteractableObject obj)
    {
        currentObject = obj;
        target.currentObject = obj;
    }

    private void Update()
    {
        
    }
}
