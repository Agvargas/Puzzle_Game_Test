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

    // Método para inicializar o reiniciar el estado del juego.
    private void InitializeGame()
    {
        Score = 0;
        Moves = startingMoves;

        // Notificamos a los suscriptores (la UI) sobre los valores iniciales.
        OnScoreUpdated?.Invoke(Score);
        OnMovesUpdated?.Invoke(Moves);
    }

    public void MakeMove()
    {
        // Evitamos que se puedan hacer movimientos si el juego ya terminó.
        if (Moves <= 0) return;

        Moves--;
        Score += 10;

        // Notificamos los cambios.
        OnMovesUpdated?.Invoke(Moves);
        OnScoreUpdated?.Invoke(Score);

        // Comprobamos la condición de fin de juego.
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