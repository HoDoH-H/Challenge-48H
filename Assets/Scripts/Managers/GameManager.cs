using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Child Drawing Properties")]
    public GameObject childDrawingPrefab;
    public Vector3 childDrawingPosition;
    public Vector3 childDrawingRotation;
    public Vector3 childDrawingScale;
    public int CD_GSMin = 0;

    [Header("Small Train Properties")]
    public GameObject smallTrainPrefab;
    public Vector3 smallTrainPosition;
    public Vector3 smallTrainRotation;
    public Vector3 smallTrainScale;
    public int ST_GSMin = 0;

    [Header("CarPiece Properties")]
    public GameObject carPiecePrefab;
    public Vector3 carPiecePosition;
    public Vector3 carPieceRotation;
    public Vector3 carPieceScale;
    public int CP_GSMin = 2;

    [Header("Newspaper Properties")]
    public GameObject newspaperPrefab;
    public Vector3 newspaperPosition;
    public Vector3 newspaperRotation;
    public Vector3 newspaperScale;
    public int N_GSMin = 3;

    [Header("Doll Properties")]
    public GameObject dollPrefab;
    public Vector3 dollPosition;
    public Vector3 dollRotation;
    public Vector3 dollScale;
    public int D_GSMin = 4;

    [Header("Phone Properties")]
    public GameObject phonePrefab;
    public Vector3 phonePosition;
    public Vector3 phoneRotation;
    public Vector3 phoneScale;
    public int P_GSMin = 5;

    public static GameManager instance;
    public int gameStage = 0;
    public bool gotCurrentObject = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateGameStage();
    }

    public void UpdateGameStage()
    {
        gotCurrentObject = false;
        if (gameStage == 0)
        {
            Instantiate(smallTrainPrefab, smallTrainRotation, Quaternion.Euler(smallTrainRotation));
            Instantiate(childDrawingPrefab, childDrawingPosition, Quaternion.Euler(childDrawingRotation));
        }
        else if (gameStage == 2)
        {
            Instantiate(carPiecePrefab, carPiecePosition, Quaternion.Euler(carPieceRotation));
        }
        else if (gameStage == 3)
        {
            Instantiate(Resources.Load("Newspaper"), newspaperPosition, Quaternion.Euler(newspaperRotation));
        }
        else if (gameStage == 4)
        {
            Instantiate(Resources.Load("Doll"), dollPosition, Quaternion.Euler(dollRotation));
        }
        else if (gameStage == 5)
        {
            Instantiate(Resources.Load("Phone"), phonePosition, Quaternion.Euler(phoneRotation));
        }
        else
        {
            Debug.LogWarning("No object to spawn for game stage: " + gameStage);
        }
    }

    public void IncreaseGameStage()
    {
        gameStage++;
        UpdateGameStage();
        Debug.Log("Game stage increased to: " + gameStage);
    }
}
