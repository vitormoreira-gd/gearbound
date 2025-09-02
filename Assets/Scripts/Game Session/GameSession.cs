using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        StatsData.LoadStats();
    }
}