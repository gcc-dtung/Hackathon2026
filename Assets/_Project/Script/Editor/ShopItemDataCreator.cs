using UnityEngine;
using UnityEditor;

public class ShopItemDataCreator
{
    [MenuItem("Breathe/Auto Create Shop Items")]
    public static void CreateAllShopItems()
    {
        string folderPath = "Assets/_Project/ShopItem";
        if (!AssetDatabase.IsValidFolder("Assets/_Project/ShopItem"))
        {
            if (!AssetDatabase.IsValidFolder("Assets/_Project"))
            {
                AssetDatabase.CreateFolder("Assets", "_Project");
            }
            AssetDatabase.CreateFolder("Assets/_Project", "ShopItem");
        }

        // Tab Buy
        CreateItem("BuyAxe", "Axe", ShopAction.Buy, ItemCategory.Tool, default, 1, 50);
        CreateItem("BuyShovel", "Shovel", ShopAction.Buy, ItemCategory.Tool, default, 2, 50);
        CreateItem("BuySeed", "Seed", ShopAction.Buy, ItemCategory.Consumable, ResourceType.Seed, 0, 10);
        CreateItem("BuyOxygenTank", "Oxygen Tank", ShopAction.Buy, ItemCategory.Consumable, ResourceType.OxygenTank, 0, 20);

        // Tab Sell
        CreateItem("SellSteel", "Steel", ShopAction.Sell, ItemCategory.Resource, ResourceType.Steel, 0, 1);
        CreateItem("SellPaper", "Paper", ShopAction.Sell, ItemCategory.Resource, ResourceType.Paper, 0, 1);
        CreateItem("SellPlastic", "Plastic", ShopAction.Sell, ItemCategory.Resource, ResourceType.Plastic, 0, 1);
        CreateItem("SellGlass", "Glass", ShopAction.Sell, ItemCategory.Resource, ResourceType.Glass, 0, 1);
        CreateItem("SellCan", "Can", ShopAction.Sell, ItemCategory.Resource, ResourceType.Can, 0, 1);
        CreateItem("SellWood", "Wood", ShopAction.Sell, ItemCategory.Resource, ResourceType.Wood, 0, 2);
        CreateItem("SellStone", "Stone", ShopAction.Sell, ItemCategory.Resource, ResourceType.Stone, 0, 2);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Created/Updated 11 Shop Item Assets in " + folderPath);
    }

    private static void CreateItem(string assetName, string itemName, ShopAction action, ItemCategory category, ResourceType resourceType, int toolIndex, int price)
    {
        string path = $"Assets/_Project/ShopItem/{assetName}.asset";
        
        ShopItemData item = AssetDatabase.LoadAssetAtPath<ShopItemData>(path);
        bool exists = item != null;
        
        if (!exists)
        {
            item = ScriptableObject.CreateInstance<ShopItemData>();
        }

        item.itemName = itemName;
        item.action = action;
        item.category = category;
        item.resourceType = resourceType;
        item.toolIndex = toolIndex;
        item.price = price;

        if (!exists)
        {
            AssetDatabase.CreateAsset(item, path);
        }
        else
        {
            EditorUtility.SetDirty(item);
        }
    }
}
