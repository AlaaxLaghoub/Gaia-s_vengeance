using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI interactions

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;  // Assign in Unity Inspector
    public Button mainMenuButton;  // Assign in Unity Inspector
    public static bool isPaused = false;

    private void Start()
    {
        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu UI is missing! Assign it in the Inspector.");
            return;
        }

        pauseMenu.SetActive(false); // Ensure menu is hidden on start

        // Assign button listeners (Make sure buttons are assigned in Inspector)
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is reset before switching scenes
        SceneManager.LoadScene("mainmenu"); // Ensure the scene name is correct!
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit! (Only works in a built game, not in the Unity Editor)");
    }
}
