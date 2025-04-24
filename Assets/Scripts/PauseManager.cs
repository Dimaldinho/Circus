using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;    // Assign PauseMenu panel here
    public GameObject pauseButtonUI;  // Assign PauseButton here

    private bool isPaused = false;

    // Called by the PauseButton’s OnClick
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }
    public void PauseGame()
    {
        // Show pause UI, hide the pause button
        pauseMenuUI.SetActive(true);
        pauseButtonUI.SetActive(false);

        // Freeze the game
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Hide pause UI, show the pause button
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true);

        // Resume the game
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Called by the “Main Menu” button in your pause menu
    public void LoadMainMenu(int menuSceneBuildIndex)
    {
        // Make sure timeScale is reset before changing scenes
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneBuildIndex);
    }

    private void OnDisable()
    {
        // In case this object is destroyed, ensure game isn't left paused
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // 1) Un-pause so time flows normally
        Time.timeScale = 1f;

        GameControl.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
