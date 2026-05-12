using System;
using UnityEngine;

public class ToolSelector : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] tools; // Array of MonoBehaviour implementing ITool
    
    [Header("Unlock Status (0=Hand, 1=Axe, 2=Shovel)")]
    [SerializeField] private bool[] unlockedTools = new bool[] { true, false, false };

    private int _currentIndex = 0;

    public int CurrentToolIndex => _currentIndex;
    public int ToolCount => tools != null ? tools.Length : 0;
    public ITool CurrentTool => tools != null && tools.Length > 0 ? tools[_currentIndex] as ITool : null;
    public event Action<int> OnToolChanged;
    public event Action<int> OnToolUnlocked;

    public void SelectTool(int index)
    {
        if (tools == null || tools.Length == 0) return;
        if (!IsToolUnlocked(index)) return;

        _currentIndex = Mathf.Clamp(index, 0, tools.Length - 1);
        OnToolChanged?.Invoke(_currentIndex);
    }

    public bool IsToolUnlocked(int index)
    {
        if (index < 0 || index >= unlockedTools.Length) return false;
        return unlockedTools[index];
    }

    public void UnlockTool(int index)
    {
        if (index >= 0 && index < unlockedTools.Length && !unlockedTools[index])
        {
            unlockedTools[index] = true;
            OnToolUnlocked?.Invoke(index);
        }
    }
}
