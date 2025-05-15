using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int stateGame;
    
    void Start()
    {
        stateGame = 0;
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            print(stateGame);
            stateGame++;
        }
    }
}
