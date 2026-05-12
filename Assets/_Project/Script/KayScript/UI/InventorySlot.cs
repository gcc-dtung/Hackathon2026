using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject emptyState;
    [SerializeField] private GameObject filledState;

    private ResourceType _currentType;
    private int _currentCount;

    public ResourceType CurrentType => _currentType;
    public int CurrentCount => _currentCount;
    public bool IsEmpty => _currentCount <= 0;

    public void SetSlot(ResourceType type, int count, Sprite icon = null)
    {
        _currentType = type;
        _currentCount = count;

        // Always show the icon
        if (iconImage != null && icon != null) iconImage.sprite = icon;

        // Dim icon when count is 0, full brightness when > 0
        if (iconImage != null)
            iconImage.color = count > 0 ? Color.white : new Color(1f, 1f, 1f, 0.3f);

        // Always show count text (including "0")
        if (countText != null) countText.text = count.ToString();

        // Keep filledState active so icon + text are always visible
        if (emptyState != null) emptyState.SetActive(false);
        if (filledState != null) filledState.SetActive(true);
    }

    public void ClearSlot()
    {
        _currentCount = 0;

        if (emptyState != null) emptyState.SetActive(true);
        if (filledState != null) filledState.SetActive(false);

        if (countText != null) countText.text = "";
        if (iconImage != null) iconImage.color = Color.clear;
    }
}
