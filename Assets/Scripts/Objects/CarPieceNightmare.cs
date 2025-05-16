using UnityEngine;
using UnityEngine.InputSystem;

public class CarPieceNightmare : MonoBehaviour
{
    private bool canInteract = false;

    PlayerInputActions actions;
    InputAction interact;

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
            if (GameManager.instance.gameStage >= GameManager.instance.CP_GSMin)
            {
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
        if (canInteract && interact.WasPerformedThisFrame() && GameManager.instance.gotCurrentObject)
        {
            // Perform the interaction logic here
            Debug.Log("Interacted with CarPieceNightmare");
            // You can add your interaction code here
            LevelManager.Instance.ChangeState(-1);
            GameManager.instance.IncreaseGameStage();
        }
    }
}
