using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameManager instance = null;

    // States
    GameStates gameState = GameStates.countDown;

    // Time
    float raceStartedTime = 0;
    float raceCompletedTime = 0;

    // Events
    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the game state on scene load
        LevelStart();
    }

    void LevelStart()
    {
        gameState = GameStates.countDown;

        Debug.Log("Level started");
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;

            // Invoke game state change event
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
            return 0;
        else if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else return Time.time - raceStartedTime;
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");

        raceStartedTime = Time.time;

        ChangeGameState(GameStates.running);
        Time.timeScale = 1f; // Ensure time is running after the race starts
    }

    public void OnRaceCompleted()
    {
        Debug.Log("OnRaceCompleted");

        raceCompletedTime = Time.time;

        ChangeGameState(GameStates.raceOver);
    }

    // Reset the game state when restarting or exiting
    public void ResetGameState()
    {
        // Reset time and state
        raceStartedTime = 0f;
        raceCompletedTime = 0f;
        gameState = GameStates.countDown;

        // Set time scale to normal
        Time.timeScale = 1f;

        Debug.Log("Game state reset.");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart(); // Initialize game state after scene is loaded
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}




