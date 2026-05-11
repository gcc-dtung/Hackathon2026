using System;
using UnityEngine;

namespace KayScript.Tools
{
    public class ToolSelector : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] tools; // Array of MonoBehaviour implementing ITool
        private int _currentIndex = 0;

        public ITool CurrentTool => tools.Length > 0 ? tools[_currentIndex] as ITool : null;
        public event Action<int> OnToolChanged;

        public void SelectTool(int index)
        {
            if (tools == null || tools.Length == 0) return;

            _currentIndex = Mathf.Clamp(index, 0, tools.Length - 1);
            OnToolChanged?.Invoke(_currentIndex);
        }
    }
}
