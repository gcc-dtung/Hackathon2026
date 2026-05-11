using System;
using System.Collections.Generic;
using UnityEngine;

namespace KayScript.Inventory
{
    public class Backpack : Singleton<Backpack>
    {
        private Dictionary<ResourceType, int> _items = new Dictionary<ResourceType, int>();

        public event Action<ResourceType, int> OnInventoryChanged;

        public void AddItem(ResourceType type, int amount)
        {
            if (!_items.ContainsKey(type))
            {
                _items[type] = 0;
            }
            _items[type] += amount;
            OnInventoryChanged?.Invoke(type, _items[type]);
        }

        public int GetItemCount(ResourceType type)
        {
            return _items.TryGetValue(type, out int count) ? count : 0;
        }

        public Dictionary<ResourceType, int> GetAllItems()
        {
            return new Dictionary<ResourceType, int>(_items);
        }
    }
}
