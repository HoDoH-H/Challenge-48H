using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyIntecract : MonoBehaviour
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
            if (GameManager.instance.gameStage == GameManager.instance.K_GSMin)
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

            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        GameManager.instance.IncreaseGameStage();
        StartCoroutine(AudioManager.Instance.PlaySFX(InteractSystem.instance.currentObject.objectBase.DreamSfx));
        InteractSystem.instance.currentObject = null;

        yield return new WaitForSeconds(1);

        CarInstance.instance.OpenDoor();
    }
}
