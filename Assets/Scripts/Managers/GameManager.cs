using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Child Drawing Properties")]
    public GameObject childDrawingPrefab;
    public int CD_GSMin = 0;

    [Header("Small Train Properties")]
    public GameObject smallTrainPrefab;
    public int ST_GSMin = 0;

    [Header("CarPiece Properties")]
    public GameObject carPiecePrefab;
    public int CP_GSMin = 2;

    [Header("Newspaper Properties")]
    public GameObject newspaperPrefab;
    public int N_GSMin = 3;

    [Header("Car Key Properties")]
    public GameObject keyPrefab;
    public int K_GSMin = 4;

    [Header("Doll Properties")]
    public GameObject dollPrefab;
    public int D_GSMin = 5;

    [Header("Phone Properties")]
    public GameObject[] phonePrefab;
    public int P_GSMin = 6;

    public static GameManager instance;
    public int gameStage = 0;
    public bool gotCurrentObject = false;
    public bool canPause = true;

    private GameObject[] instantiatedObjects;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        smallTrainPrefab.SetActive(false);
        childDrawingPrefab.SetActive(false);
        carPiecePrefab.SetActive(false);
        newspaperPrefab.SetActive(false);
        keyPrefab.SetActive(false);
        dollPrefab.SetActive(false);
        phonePrefab[0].SetActive(false);
        phonePrefab[1].SetActive(false);

        UpdateGameStage();
    }

    public void UpdateGameStage()
    {
        gotCurrentObject = false;
        if (gameStage == 0)
        {
            smallTrainPrefab.SetActive(true);
            childDrawingPrefab.SetActive(true);
        }
        else if (gameStage == 2)
        {
            carPiecePrefab.SetActive(true);
        }
        else if (gameStage == 3)
        {
            newspaperPrefab.SetActive(true);
        }
        else if (gameStage == 4)
        {
            keyPrefab.SetActive(true);
        }
        else if (gameStage == 5)
        {
            dollPrefab.SetActive(true);
            dollPrefab.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(WaitToEnableDoll());
        }
        else if (gameStage == 6)
        {
            phonePrefab[LevelManager.Instance.stateGame > 0 ? 0 : 1].SetActive(true);
        }
        else
        {
            Debug.LogWarning("No object to spawn for game stage: " + gameStage);
        }
    }

    IEnumerator WaitToEnableDoll()
    {
        yield return new WaitForSeconds(0.7f);
        dollPrefab.GetComponent<BoxCollider>().enabled = true;
    }

    public void IncreaseGameStage()
    {
        gameStage++;
        UpdateGameStage();
        Debug.Log("Game stage increased to: " + gameStage);
    }
}
