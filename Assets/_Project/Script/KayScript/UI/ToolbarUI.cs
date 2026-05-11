using KayScript.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace KayScript.UI
{
    public class ToolbarUI : MonoBehaviour
    {
        [SerializeField] private ToolSelector toolSelector;
        [SerializeField] private Button[] toolButtons; // Indices should match ToolSelector's array
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.green;

        private void Start()
        {
            if (toolSelector == null) return;

            toolSelector.OnToolChanged += HandleToolChanged;

            for (int i = 0; i < toolButtons.Length; i++)
            {
                int index = i; // local copy for closure
                toolButtons[i].onClick.AddListener(() => toolSelector.SelectTool(index));
            }

            // Init state
            HandleToolChanged(0);
        }

        private void OnDestroy()
        {
            if (toolSelector != null)
            {
                toolSelector.OnToolChanged -= HandleToolChanged;
            }
        }

        private void HandleToolChanged(int index)
        {
            for (int i = 0; i < toolButtons.Length; i++)
            {
                Image bg = toolButtons[i].GetComponent<Image>();
                if (bg != null)
                {
                    bg.color = (i == index) ? selectedColor : normalColor;
                }
            }
        }
    }
}
