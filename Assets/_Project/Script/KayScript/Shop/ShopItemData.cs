using UnityEngine;

public enum ShopAction
{
    Buy,
    Sell
}

public enum ItemCategory
{
    Resource, // Trash, Wood, Stone, etc.
    Consumable, // OxygenTank, Seed
    Tool // Axe, Shovel
}

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Breathe/Shop Item")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    public ShopAction action;
    public ItemCategory category;
    public int purchaseLimit;
    
    // For resources/consumables
    public ResourceType resourceType;
    
    // For tools (index in ToolSelector)
    // 0 = Hand, 1 = Axe, 2 = Shovel
    public int toolIndex;
}
