using TMPro;
using UnityEngine;

public class ConsumableCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI seedText;
    [SerializeField] private TextMeshProUGUI oxygenTankText;

    private bool _isSubscribed;

    private void OnEnable()
    {
        SubscribeToBackpack();
        UpdateCounts();
    }

    private void OnDisable()
    {
        UnsubscribeFromBackpack();
    }

    private void SubscribeToBackpack()
    {
        if (_isSubscribed || !Backpack.InstanceExists) return;

        Backpack.Instance.OnInventoryChanged += HandleInventoryChanged;
        _isSubscribed = true;
    }

    private void UnsubscribeFromBackpack()
    {
        if (!_isSubscribed || !Backpack.InstanceExists) return;

        Backpack.Instance.OnInventoryChanged -= HandleInventoryChanged;
        _isSubscribed = false;
    }

    private void HandleInventoryChanged(ResourceType type, int amount)
    {
        if (type == ResourceType.Seed || type == ResourceType.OxygenTank)
        {
            UpdateCounts();
        }
    }

    private void UpdateCounts()
    {
        if (Backpack.Instance == null) return;

        if (seedText != null)
        {
            seedText.text = Backpack.Instance.GetItemCount(ResourceType.Seed).ToString();
        }

        if (oxygenTankText != null)
        {
            oxygenTankText.text = Backpack.Instance.GetItemCount(ResourceType.OxygenTank).ToString();
        }
    }
}
