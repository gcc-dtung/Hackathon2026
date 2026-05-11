using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryGridUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private TextMeshProUGUI capacityText;

    [Header("Icons")]
    [SerializeField] private Sprite trashIcon;
    [SerializeField] private Sprite woodIcon;
    [SerializeField] private Sprite stoneIcon;
    [SerializeField] private Sprite seedIcon;
    [SerializeField] private Sprite oxygenTankIcon;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    private void Start()
    {
        InitializeGrid();

        if (Backpack.Instance != null)
        {
            Backpack.Instance.OnInventoryChanged += HandleInventoryChanged;
            UpdateGrid();
        }
    }

    private void OnDestroy()
    {
        if (Backpack.Instance != null)
        {
            Backpack.Instance.OnInventoryChanged -= HandleInventoryChanged;
        }
    }

    private void InitializeGrid()
    {
        if (Backpack.Instance == null || slotPrefab == null || gridParent == null) return;

        // Create initial slots
        int maxSlots = Backpack.Instance.MaxSlots;
        for (int i = 0; i < maxSlots; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, gridParent);
            slot.ClearSlot();
            _slots.Add(slot);
        }
    }

    private void HandleInventoryChanged(ResourceType type, int amount)
    {
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        if (Backpack.Instance == null) return;

        var items = Backpack.Instance.GetAllItems();
        int slotIndex = 0;

        // Fill slots with items
        foreach (var kvp in items)
        {
            if (kvp.Value > 0)
            {
                if (slotIndex < _slots.Count)
                {
                    Sprite icon = GetIconForType(kvp.Key);
                    _slots[slotIndex].SetSlot(kvp.Key, kvp.Value, icon);
                    slotIndex++;
                }
            }
        }

        // Clear remaining slots
        for (int i = slotIndex; i < _slots.Count; i++)
        {
            _slots[i].ClearSlot();
        }

        // Update capacity text
        if (capacityText != null)
        {
            capacityText.text = $"{Backpack.Instance.TotalItemCount} / {Backpack.Instance.MaxSlots}";
        }
    }

    private Sprite GetIconForType(ResourceType type)
    {
        // Simple mapping, can be moved to a ScriptableObject later
        return type switch
        {
            ResourceType.Wood => woodIcon,
            ResourceType.Stone => stoneIcon,
            ResourceType.Seed => seedIcon,
            ResourceType.OxygenTank => oxygenTankIcon,
            _ => trashIcon // All trash types share one icon for now
        };
    }
}
