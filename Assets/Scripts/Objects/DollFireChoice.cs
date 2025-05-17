using UnityEngine;
using UnityEngine.InputSystem;

public class DollFireChoice : MonoBehaviour
{
    private bool canInteract = false;

    PlayerInputActions actions;
    InputAction interact;

    private void OnEnable() => actions.Enable();

    private void OnDisable() => actions.Disable();

    private void Awake()
    {
        actions = new PlayerInputActions();
        interact = actions.Player.Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the game stage is greater than or equal to CP_GSMin
            if (GameManager.instance.gameStage == GameManager.instance.D_GSMin)
            {
                Debug.Log("CanInteract");
                canInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void Update()
    {
        if (interact.WasPerformedThisFrame())
        {
            if (!canInteract || !GameManager.instance.gotCurrentObject)
                return;


            StartCoroutine(AudioManager.Instance.PlaySFX(InteractSystem.instance.currentObject.objectBase.NightmareSfx));
            InteractSystem.instance.currentObject = null;

            LevelManager.Instance.ChangeState(-1);
            GameManager.instance.IncreaseGameStage();
        }
    }
}
