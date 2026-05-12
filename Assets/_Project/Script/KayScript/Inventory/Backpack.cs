using System;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : Singleton<Backpack>
{
    [SerializeField] private int maxSlots = 20;
    private Dictionary<ResourceType, int> _items = new Dictionary<ResourceType, int>();

    public event Action<ResourceType, int> OnInventoryChanged;

    public int MaxSlots => maxSlots;
    
    public int TotalItemCount
    {
        get
        {
            int count = 0;
            foreach (var amount in _items.Values)
            {
                count += amount;
            }
            return count;
        }
    }

    public bool AddItem(ResourceType type, int amount)
    {
        if (TotalItemCount + amount > maxSlots)
        {
            Debug.Log("Backpack is full!");
            return false; // Can't add, full
        }

        if (!_items.ContainsKey(type))
        {
            _items[type] = 0;
        }
        _items[type] += amount;
        Debug.Log($"[Backpack] Added {amount}x {type}. New total: {_items[type]}. Backpack total: {TotalItemCount}/{maxSlots}. Listeners: {OnInventoryChanged?.GetInvocationList()?.Length ?? 0}");
        OnInventoryChanged?.Invoke(type, _items[type]);
        return true;
    }

    public int GetItemCount(ResourceType type)
    {
        return _items.TryGetValue(type, out int count) ? count : 0;
    }

    public Dictionary<ResourceType, int> GetAllItems()
    {
        return new Dictionary<ResourceType, int>(_items);
    }

    public bool HasItem(ResourceType type, int amount)
    {
        return GetItemCount(type) >= amount;
    }

    public bool RemoveItem(ResourceType type, int amount)
    {
        if (HasItem(type, amount))
        {
            _items[type] -= amount;
            if (_items[type] == 0)
            {
                _items.Remove(type);
            }
            OnInventoryChanged?.Invoke(type, GetItemCount(type));
            return true;
        }
        return false;
    }

    public void ClearAll()
    {
        _items.Clear();
        // Invoke event with 0 for each type to clear UI
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            OnInventoryChanged?.Invoke(type, 0);
        }
    }
}
