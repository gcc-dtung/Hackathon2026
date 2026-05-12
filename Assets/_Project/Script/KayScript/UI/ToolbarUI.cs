using UnityEngine;
using UnityEngine.UI;

public class ToolbarUI : MonoBehaviour
{
    [SerializeField] private ToolSelector toolSelector;
    [SerializeField] private Button[] toolButtons; // Indices should match ToolSelector's array
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.green;

    [SerializeField] private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void Start()
    {
        if (toolSelector == null)
        {
            Debug.LogError("[ToolbarUI] toolSelector reference is NULL! Assign it in Inspector.");
            return;
        }

        toolSelector.OnToolChanged += HandleToolChanged;
        toolSelector.OnToolUnlocked += HandleToolUnlocked;

        for (int i = 0; i < toolButtons.Length; i++)
        {
            int index = i; // local copy for closure
            toolButtons[i].onClick.AddListener(() =>
            {
                Debug.Log($"[ToolbarUI] Button {index} clicked!");
                toolSelector.SelectTool(index);
            });
        }

        // Init state
        UpdateAllButtonsUI();
    }

    private void OnDestroy()
    {
        if (toolSelector != null)
        {
            toolSelector.OnToolChanged -= HandleToolChanged;
            toolSelector.OnToolUnlocked -= HandleToolUnlocked;
        }
    }

    private void HandleToolUnlocked(int index)
    {
        UpdateAllButtonsUI();
    }

    private void HandleToolChanged(int index)
    {
        UpdateAllButtonsUI(index);
    }

    private void UpdateAllButtonsUI(int selectedIndex = -1)
    {
        if (toolSelector == null) return;
        
        if (selectedIndex == -1)
        {
            selectedIndex = toolSelector.CurrentToolIndex;
        }

        for (int i = 0; i < toolButtons.Length; i++)
        {
            if (toolButtons[i] == null) continue;

            bool isUnlocked = toolSelector.IsToolUnlocked(i);
            bool isSelected = (i == selectedIndex);

            // Disable button interaction cho tool bị lock
            toolButtons[i].interactable = isUnlocked;

            Image bg = toolButtons[i].GetComponent<Image>();
            if (bg != null)
            {
                if (!isUnlocked)
                {
                    bg.color = lockedColor;
                }
                else
                {
                    bg.color = isSelected ? selectedColor : normalColor;
                }
            }
        }
    }
}
