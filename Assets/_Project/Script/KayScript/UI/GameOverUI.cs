using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : Singleton<GameOverUI>
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button retryButton;
    [SerializeField] private OxygenBar oxygenBar;

    void OnEnable()
    {
        if (oxygenBar == null)
        {
            oxygenBar = FindFirstObjectByType<OxygenBar>();
        }

        if (oxygenBar != null)
        {
            oxygenBar.OnOxygenDepleted += ShowGameOver;
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartGame);
        }
    }
    void OnDisable()
    {
        if (oxygenBar != null)
        {
            oxygenBar.OnOxygenDepleted -= ShowGameOver;
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            // Unlock cursor if it was locked
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void RestartGame()
    {
        // Reset time scale to 1f before reloading the scene
        Time.timeScale = 1f;
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
