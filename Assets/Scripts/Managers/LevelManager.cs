using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int stateGame = 0;
    public bool gameEnd = false;

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
        print("State modifié : " + stateGame);
    }

    void UpdateAmbiance()
    {
        if (stateGame > 0)
        {

        }
        else if (stateGame < 0)
        {

        }
    }

    public void SetGameEnd()
    {
        gameEnd = true;
    }

    void Win()
    {
        print("You Win!");
    }

    void Lose()
    {
        print("You Lose!");
    }
}
