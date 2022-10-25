using UnityEngine;

public class GameSessionConfiguration : MonoBehaviour
{
    [field: SerializeField] public int ScorePerKill { get; private set; }
    [field: SerializeField] public float EnemySpawnTime { get; set; }
    [field: SerializeField] public float GameSessionTime { get; set; }

    public static GameSessionConfiguration Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}