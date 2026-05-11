using UnityEngine;
using UnityEditor;

public class CheatTools
{
    [MenuItem("Breathe/Cheats/Add 1000 Coins")]
    public static void AddCoins()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Cheat can only be used in Play Mode!");
            return;
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(1000);
            Debug.Log("Cheat Activated: Added 1000 Coins!");
        }
        else
        {
            Debug.LogError("CurrencyManager not found in scene!");
        }
    }
}
