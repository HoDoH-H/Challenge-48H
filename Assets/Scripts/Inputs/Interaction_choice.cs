using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_choice : MonoBehaviour
{ 
    public static Interaction_choice Instance;
    
    public GameObject panel;
    public TextMeshProUGUI choiceOneText;
    public TextMeshProUGUI choiceTwoText;
    public Button choiceOneButton;
    public Button choiceTwoButton;

    private bool isChoiceOneDream;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        panel.SetActive(false);
    }

    public void ShowChoices(string dreamText, string nightmareText)
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isChoiceOneDream = Random.value > 0.5f;

        if (isChoiceOneDream)
        {
            choiceOneText.text = dreamText;
            choiceTwoText.text = nightmareText;
        }
        else
        {
            choiceOneText.text = nightmareText;
            choiceTwoText.text = dreamText;
        }

        choiceOneButton.onClick.RemoveAllListeners();
        choiceTwoButton.onClick.RemoveAllListeners();

        choiceOneButton.onClick.AddListener(() => OnChoiceMade(isChoiceOneDream));
        choiceTwoButton.onClick.AddListener(() => OnChoiceMade(!isChoiceOneDream));
    }

    void OnChoiceMade(bool choseDream)
    {
        if (choseDream)
            LevelManager.Instance.ChangeState(1);
        else
            LevelManager.Instance.ChangeState(-1);

        Debug.Log("Choix effectué, nouveau stateGame : " + LevelManager.Instance.stateGame);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        panel.SetActive(false);

        PlayerMovement.instance.canMove = true;
        FirstPersonCam.instance.canLook = true;
        GameManager.instance.IncreaseGameStage();
    }

}
