using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private List<ShopItemData> availableItems;
    [SerializeField] private ToolSelector toolSelector; // Needed for unlocking tools

    public List<ShopItemData> AvailableItems => availableItems;

    private ToolSelector CurrentToolSelector
    {
        get
        {
            if (toolSelector == null)
            {
                toolSelector = FindFirstObjectByType<ToolSelector>();
            }

            return toolSelector;
        }
    }

    public int GetOwnedToolCount(int toolIndex)
    {
        ToolSelector selector = CurrentToolSelector;
        if (selector == null) return 0;
        return selector.IsToolUnlocked(toolIndex) ? 1 : 0;
    }

    public int GetOwnedItemCount(ShopItemData itemData)
    {
        if (itemData == null) return 0;

        if (itemData.category == ItemCategory.Tool)
        {
            return GetOwnedToolCount(itemData.toolIndex);
        }

        if (Backpack.Instance == null) return 0;
        return Backpack.Instance.GetItemCount(itemData.resourceType);
    }

    public int GetToolPurchaseLimit(ShopItemData itemData)
    {
        if (itemData == null) return 0;
        if (itemData.purchaseLimit > 0) return itemData.purchaseLimit;
        return itemData.category == ItemCategory.Tool ? 1 : 0;
    }

    public bool SellItem(ShopItemData itemData)
    {
        if (itemData.action != ShopAction.Sell) return false;

        // Check if player has the item to sell
        if (Backpack.Instance.HasItem(itemData.resourceType, 1))
        {
            if (Backpack.Instance.RemoveItem(itemData.resourceType, 1))
            {
                CurrencyManager.Instance.AddCoins(itemData.price);
                return true;
            }
        }
        
        return false;
    }

    public bool BuyItem(ShopItemData itemData)
    {
        if (itemData.action != ShopAction.Buy) return false;
        if (CurrencyManager.Instance == null || Backpack.Instance == null) return false;

        if (CurrencyManager.Instance.Coins >= itemData.price)
        {
            if (itemData.category == ItemCategory.Consumable)
            {
                // Check if backpack has space
                if (Backpack.Instance.AddItem(itemData.resourceType, 1))
                {
                    CurrencyManager.Instance.SpendCoins(itemData.price);
                    return true;
                }
                else
                {
                    Debug.Log("Cannot buy, backpack is full!");
                    return false;
                }
            }
            else if (itemData.category == ItemCategory.Tool)
            {
                ToolSelector selector = CurrentToolSelector;
                if (selector != null && !selector.IsToolUnlocked(itemData.toolIndex))
                {
                    if (CurrencyManager.Instance.SpendCoins(itemData.price))
                    {
                        selector.UnlockTool(itemData.toolIndex);
                        return true;
                    }
                }
                else
                {
                    Debug.Log("Tool is already unlocked or ToolSelector not referenced!");
                }
            }
        }
        else
        {
            Debug.Log("Not enough coins!");
        }

        return false;
    }
    
    public void SellAllTrash()
    {
        // Sell all trash types: Steel, Paper, Plastic, Glass, Can
        SellAllOfType(ResourceType.Steel, 1);
        SellAllOfType(ResourceType.Paper, 1);
        SellAllOfType(ResourceType.Plastic, 1);
        SellAllOfType(ResourceType.Glass, 1);
        SellAllOfType(ResourceType.Can, 1);
        
        // Sell Wood and Stone
        SellAllOfType(ResourceType.Wood, 1);
        SellAllOfType(ResourceType.Stone, 1);
    }
    
    private void SellAllOfType(ResourceType type, int unitPrice)
    {
        int count = Backpack.Instance.GetItemCount(type);
        if (count > 0 && Backpack.Instance.RemoveItem(type, count))
        {
            CurrencyManager.Instance.AddCoins(count * unitPrice);
        }
    }
}
