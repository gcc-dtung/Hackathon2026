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
        if (toolSelector == null) return;

        toolSelector.OnToolChanged += HandleToolChanged;
        toolSelector.OnToolUnlocked += HandleToolUnlocked;

        for (int i = 0; i < toolButtons.Length; i++)
        {
            int index = i; // local copy for closure
            toolButtons[i].onClick.AddListener(() => toolSelector.SelectTool(index));
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
        if (selectedIndex == -1 && toolSelector != null && toolSelector.CurrentTool != null)
        {
            // Simple way to get current index, though we don't have direct access.
            // It's okay, we can just rely on the event or checking if we need to.
            // For now, we will just update lock states if selectedIndex is not provided.
        }

        for (int i = 0; i < toolButtons.Length; i++)
        {
            Image bg = toolButtons[i].GetComponent<Image>();
            if (bg != null)
            {
                bool isUnlocked = toolSelector.IsToolUnlocked(i);
                bool isSelected = (i == selectedIndex);

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
