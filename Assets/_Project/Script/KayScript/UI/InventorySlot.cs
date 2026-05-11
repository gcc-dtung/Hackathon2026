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

        if (count > 0)
        {
            if (emptyState != null) emptyState.SetActive(false);
            if (filledState != null) filledState.SetActive(true);

            if (countText != null) countText.text = count.ToString();
            if (iconImage != null && icon != null) iconImage.sprite = icon;
            if (iconImage != null) iconImage.color = Color.white;
        }
        else
        {
            ClearSlot();
        }
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
