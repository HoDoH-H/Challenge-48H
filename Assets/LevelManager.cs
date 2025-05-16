using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int stateGame = 0;
    public bool gameEnd = false;

    public Color intenseColor = Color.red;
    public Color calmColor = Color.blue;
    public Light ambianceLight;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        //UpdateAmbiance();

        if (gameEnd)
        {
            if (stateGame > 0)
                Win();
            else if (stateGame < 0)
                Lose();
        }
    }
    
    public void ChangeState(int delta)
    {
        stateGame += delta;
        Debug.Log("State modifié : " + stateGame);
    }

    void UpdateAmbiance()
    {
        if (ambianceLight == null) return;

        if (stateGame > 0)
            ambianceLight.color = calmColor;
        else if (stateGame < 0)
            ambianceLight.color = intenseColor;
    }

    public void SetGameEnd()
    {
        gameEnd = true;
    }

    void Win()
    {
        Debug.Log("You Win!");
    }

    void Lose()
    {
        Debug.Log("You Lose!");
    }
}
