using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [field: SerializeField] public Slider GameSessionTimeSlider { get; private set; }
    [field: SerializeField] public TextMeshProUGUI GameSessionTimeText { get; private set; }
    [field: SerializeField] public Slider EnemySpawnTimeSlider { get; private set; }
    [field: SerializeField] public TextMeshProUGUI EnemySpawnTimeText { get; private set; }

    private void Start()
    {
        if (Gamepad.current == null)
        {
            EventSystem.current.firstSelectedGameObject = null;
        }
        RetrieveGameSessionTime();
        RetrieveEnemySpawnTime();
        GameSessionTimeSlider.onValueChanged.AddListener(delegate { UpdateGameSessionTime(); });
        EnemySpawnTimeSlider.onValueChanged.AddListener(delegate { UpdateEnemySpawnTime(); });
    }

    private void RetrieveGameSessionTime()
    {
        if (GameSessionConfiguration.Instance.GameSessionTime > 0f)
        {
            GameSessionTimeSlider.value = GameSessionConfiguration.Instance.GameSessionTime;
            GameSessionTimeText.text = GameSessionTimeSlider.value + " min";
        }
        else
        {
            UpdateGameSessionTime();
        }
    }

    private void UpdateGameSessionTime()
    {
        GameSessionConfiguration.Instance.GameSessionTime = GameSessionTimeSlider.value;
        GameSessionTimeText.text = GameSessionTimeSlider.value + " min";
    }

    private void RetrieveEnemySpawnTime()
    {
        if (GameSessionConfiguration.Instance.EnemySpawnTime > 0f)
        {
            EnemySpawnTimeSlider.value = GameSessionConfiguration.Instance.EnemySpawnTime;
            EnemySpawnTimeText.text = EnemySpawnTimeSlider.value + " sec";
        }
        else
        {
            UpdateEnemySpawnTime();
        }
    }

    private void UpdateEnemySpawnTime()
    {
        GameSessionConfiguration.Instance.EnemySpawnTime = EnemySpawnTimeSlider.value;
        EnemySpawnTimeText.text = EnemySpawnTimeSlider.value + " sec";
    }
}