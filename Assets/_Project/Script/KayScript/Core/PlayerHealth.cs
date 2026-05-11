using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private OxygenBar oxygenBar;
    
    public event System.Action OnPlayerDied;
    
    private void Start()
    {
        if (oxygenBar == null)
        {
            oxygenBar = FindFirstObjectByType<OxygenBar>();
        }

        if (oxygenBar != null)
        {
            oxygenBar.OnOxygenDepleted += Die;
        }
    }

    private void OnDestroy()
    {
        if (oxygenBar != null)
        {
            oxygenBar.OnOxygenDepleted -= Die;
        }
    }

    private void Die()
    {
        // Clear Inventory
        if (Backpack.Instance != null)
        {
            Backpack.Instance.ClearAll();
        }

        OnPlayerDied?.Invoke();

        // Show Game Over screen
        if (GameOverUI.Instance != null)
        {
            GameOverUI.Instance.ShowGameOver();
        }
        
        // Optionally disable player movement
        var movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
    }
}
