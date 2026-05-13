using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Victory
}

public class GameManager : Singleton<GameManager>
{
    private GameState _currentState = GameState.Playing;
    public GameState CurrentState => _currentState;

    private void Start()
    {
        if (DayCycleManager.Instance != null)
        {
            DayCycleManager.Instance.OnFinalDayEnd += TriggerVictory;
        }

        var playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied += TriggerGameOver;
        }
    }

    private void TriggerVictory()
    {
        _currentState = GameState.Victory;
        Time.timeScale = 0f; // Pause game logic
        
        // Show Victory UI
        var victoryUI = FindFirstObjectByType<VictoryUI>();
        if (victoryUI != null)
        {
            victoryUI.ShowVictory();
        }
    }

    private void TriggerGameOver()
    {
        _currentState = GameState.GameOver;
        // Assuming GameOverUI is handled by PlayerHealth or directly
    }

    public void PauseGame()
    {
        if (_currentState == GameState.Playing)
        {
            _currentState = GameState.Paused;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (_currentState == GameState.Paused)
        {
            _currentState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        // Cleanup event subscriptions to avoid dangling references
        if (DayCycleManager.InstanceExists)
        {
            DayCycleManager.Instance.OnFinalDayEnd -= TriggerVictory;
        }

        var playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied -= TriggerGameOver;
        }

        Time.timeScale = 1f; // Reset time scale on destroy to avoid freezing next loaded scene
    }
}
