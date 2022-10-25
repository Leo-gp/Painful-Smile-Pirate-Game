using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI TimerText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI FinalScoreText { get; private set; }
    [field: SerializeField] public GameObject GameOverPanel { get; private set; }
    [field: SerializeField] public GameObject PauseGamePanel { get; private set; }
    [field: SerializeField] public Button GameOverPanelFirstSelectedButton { get; private set; }
    [field: SerializeField] public Button PauseGamePanelFirstSelectedButton { get; private set; }
    [field: SerializeField] public Image FrontalShootFillableImage { get; private set; }
    [field: SerializeField] public Image SideShootFillableImage { get; private set; }
    [field: SerializeField] public Color FillableImageLoadingColor { get; private set; }
    [field: SerializeField] public Color FillableImageReadyColor { get; private set; }

    public static GameManager Instance { get; private set; }

    private InputActions inputActions;
    private float timer;
    private bool gameIsPaused;
    private bool gameIsOver;
    private int score;

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

        inputActions = new InputActions();
        inputActions.UI.Enable();
        inputActions.UI.TogglePause.performed += TogglePause;
        inputActions.UI.Unpause.performed += Unpause;
    }

    private void Start()
    {
        timer = GameSessionConfiguration.Instance.GameSessionTime + 1f / 60f;
        IncreaseScore(0);
    }

    private void Update()
    {
        if (!gameIsOver)
        {
            HandleTimer();
        }
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    public void GameOver()
    {
        gameIsOver = true;
        Time.timeScale = 0f;
        DeactivateAllShips();
        GameOverPanel.SetActive(true);
        FinalScoreText.text = "Score: " + score.ToString();
        if (Gamepad.current != null)
        {
            GameOverPanelFirstSelectedButton.Select();
        }
    }

    public void IncreaseScore(int value)
    {
        score += value;
        ScoreText.text = "Score: " + score.ToString();
    }

    public void Unpause()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        ActivateAllShips();
        PauseGamePanel.SetActive(false);
    }

    public void UpdateShootCooldownImages(float frontalShootRemainingPercentage, float sideShootRemainingPercentage)
    {
        FrontalShootFillableImage.fillAmount = (100f - frontalShootRemainingPercentage) / 100f;
        FrontalShootFillableImage.color = (FrontalShootFillableImage.fillAmount < 1f) ? FillableImageLoadingColor : FillableImageReadyColor;
        SideShootFillableImage.fillAmount = (100f - sideShootRemainingPercentage) / 100f;
        SideShootFillableImage.color = (SideShootFillableImage.fillAmount < 1f) ? FillableImageLoadingColor : FillableImageReadyColor;
    }

    private void Unpause(InputAction.CallbackContext context)
    {
        Unpause();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (!gameIsPaused)
        {
            gameIsPaused = true;
            Time.timeScale = 0f;
            DeactivateAllShips();
            PauseGamePanel.SetActive(true);
            if (Gamepad.current != null)
            {
                PauseGamePanelFirstSelectedButton.Select();
            }
        }
        else
        {
            gameIsPaused = false;
            Time.timeScale = 1f;
            ActivateAllShips();
            PauseGamePanel.SetActive(false);
        }
    }

    private void DeactivateAllShips()
    {
        var ships = FindObjectsOfType<Ship>();
        foreach (var ship in ships)
        {
            ship.enabled = false;
        }
    }

    private void ActivateAllShips()
    {
        var ships = FindObjectsOfType<Ship>();
        foreach (var ship in ships)
        {
            ship.enabled = true;
        }
    }

    private void HandleTimer()
    {
        if (timer * 60f > 1f)
        {
            timer -= Time.deltaTime / 60f;
        }
        else
        {
            GameOver();
        }

        string minutes = Mathf.Floor(timer).ToString();
        string seconds = Mathf.Floor(timer * 60f % 60f).ToString("00");

        TimerText.text = "Time: " + minutes + ":" + seconds;

        if (timer <= 0.5f)
        {
            TimerText.color = Color.red;
        }
    }
}