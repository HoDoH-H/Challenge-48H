using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class Menu : MonoBehaviour
{
    PlayerInputActions actions;
    InputAction escape;
    public GameObject ui;

    private void Awake()
    {
        actions = new PlayerInputActions();
        escape = actions.Player.Escape;

    }

    private void OnEnable()
    {
        escape.Enable();
        escape.performed += OnEscape;
    }

    private void OnDisable()
    {
        escape.performed -= OnEscape;
        escape.Disable();
    }

    private void OnEscape(InputAction.CallbackContext context)
    {
        Toggle();
    }
    void Update()
    {  

    }

    public void Toggle()
    {
        if (ui != null)
        {
            bool isActive = !ui.activeSelf;
            ui.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f;

            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Debug.LogError("UI GameObject is not assigned in the inspector.");
        }
    }
    
    public void ReTry()
    {
        SceneManager.LoadScene("Dev - Lucas");
        LevelManager.Instance.stateGame = 0;
        Time.timeScale = 1f;
    }
    
    public void HandleExitGame()
    {
        Application.Quit();
    }
}
