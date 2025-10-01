using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;

    [Header("UI Screens")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameplayUI;

    [Header("UI Buttons")]
    [SerializeField] private Button replayButton;


    private void Awake()
    {
        if (replayButton != null)
        {
            replayButton.onClick.AddListener(() => GameManager.Instance.Replay());
            replayButton.onClick.AddListener(ResetUI);
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnScoreUpdated += UpdateScoreText;
        GameManager.Instance.OnMovesUpdated += UpdateMovesText;
        GameManager.Instance.OnGameOver += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScoreUpdated -= UpdateScoreText;
        GameManager.Instance.OnMovesUpdated -= UpdateMovesText;
        GameManager.Instance.OnGameOver -= ShowGameOverScreen;

        if (replayButton != null)
        {
            replayButton.onClick.RemoveAllListeners();
        }
    }

    private void Start()
    {
        // We make sure the UI starts in the correct state.
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(true);
        }
    }

    private void UpdateScoreText(int newScore)
    {
        scoreText.text = $"{newScore}";
    }

    private void UpdateMovesText(int newMoves)
    {
        movesText.text = $"{newMoves}";
    }

    private void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        gameplayUI.SetActive(false);
    }

    public void ResetUI()
    {
        gameOverScreen.SetActive(false);
        gameplayUI.SetActive(true);
    }
}