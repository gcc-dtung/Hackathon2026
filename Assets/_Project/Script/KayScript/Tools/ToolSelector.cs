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

    private void Start()
    {
        // Đảm bảo tool đầu tiên (index 0) luôn được unlock
        if (unlockedTools != null && unlockedTools.Length > 0)
        {
            unlockedTools[0] = true;
        }

        UpdateToolVisibility();
        // Fire initial event để ToolbarUI đồng bộ trạng thái ngay từ đầu
        OnToolChanged?.Invoke(_currentIndex);
    }

    public void SelectTool(int index)
    {
        if (tools == null || tools.Length == 0)
        {
            Debug.LogWarning("[ToolSelector] tools array is null or empty!");
            return;
        }
        if (index < 0 || index >= tools.Length)
        {
            Debug.LogWarning($"[ToolSelector] Invalid tool index: {index}");
            return;
        }
        if (!IsToolUnlocked(index))
        {
            Debug.Log($"[ToolSelector] Tool {index} is locked, cannot select.");
            return;
        }

        _currentIndex = index;
        UpdateToolVisibility();
        OnToolChanged?.Invoke(_currentIndex);
        Debug.Log($"[ToolSelector] Selected tool {_currentIndex}");
    }

    private void UpdateToolVisibility()
    {
        if (tools == null) return;
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i] != null)
            {
                tools[i].gameObject.SetActive(i == _currentIndex);
            }
        }
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
