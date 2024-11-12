using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuUIHandler : MonoBehaviour
{
    // Other components
    Canvas canvas;
    bool isMenuVisible = false;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        // Hook up events
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        // Check if ESC is pressed and toggle the menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void OnRaceAgain()
    {
        // Reset the game state before reloading the scene
        GameManager.instance.ResetGameState();

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitToMainMenu()
    {
        // Reset the game state before returning to the main menu
        GameManager.instance.ResetGameState();

        // Set the time scale back to normal (in case it's paused)
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene("Menu");
    }

    private void ToggleMenu()
    {
        // Toggle the visibility of the menu canvas
        isMenuVisible = !isMenuVisible;
        canvas.enabled = isMenuVisible;

        // Optionally pause the game when the menu is visible
        if (isMenuVisible)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    IEnumerator ShowMenuCO()
    {
        yield return new WaitForSeconds(1);
        canvas.enabled = true;
    }

    // Event handler for game state changes
    void OnGameStateChanged(GameManager gameManager)
    {
        if (GameManager.instance.GetGameState() == GameStates.raceOver)
        {
            StartCoroutine(ShowMenuCO());
        }
    }

    void OnDestroy()
    {
        // Unhook events
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}


