using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int startingMoves = 5;

    public int Score { get; private set; }
    public int Moves { get; private set; }

    public UnityAction<int> OnScoreUpdated;
    public UnityAction<int> OnMovesUpdated;
    public UnityAction OnGameOver;
    public UnityAction OnGameStarted;

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
    }

    private void Start()
    {
        InitializeGame();
    }

    // Method to initialize or reset the game state.
    private void InitializeGame()
    {
        Score = 0;
        Moves = startingMoves;

        // Notify subscribers about the initial values.
        OnScoreUpdated?.Invoke(Score);
        OnMovesUpdated?.Invoke(Moves);
        OnGameStarted?.Invoke();
    }

    public void MakeMove(int blocksCollected)
    {
        // Prevent moves if the game is already over.
        if (Moves <= 0) return;

        Moves--;

        int scoreGained = blocksCollected;
        Score += scoreGained;

        OnScoreUpdated?.Invoke(Score);
        OnMovesUpdated?.Invoke(Moves);

        // Check for game over condition.
        if (Moves <= 0)
        {
            OnGameOver?.Invoke();
        }
    }

    public void Replay()
    {
        InitializeGame();
    }
}