using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI statsText;

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (statsText != null)
            {
                int treesSaved = TreeManager.Instance != null ? TreeManager.Instance.TreeCount : 0;
                statsText.text = $"You survived 7 days!\nTrees alive: {treesSaved}";
            }
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
