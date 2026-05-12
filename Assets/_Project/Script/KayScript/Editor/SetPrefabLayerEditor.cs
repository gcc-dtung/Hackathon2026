using UnityEngine;
using UnityEditor;

public static class SetPrefabLayerEditor
{
    [MenuItem("Tools/Setup PlantedTree Layer (One-Shot)")]
    public static void Run()
    {
        string path = "Assets/_Project/Prefabs/PlantedTree.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        
        if (prefab != null)
        {
            int interactLayer = LayerMask.NameToLayer("Interact");
            if (interactLayer == -1)
            {
                Debug.LogError("Interact layer not found!");
                return;
            }

            SetLayerRecursively(prefab, interactLayer);
            EditorUtility.SetDirty(prefab);
            AssetDatabase.SaveAssets();
            Debug.Log("✅ Set PlantedTree prefab layer to Interact (" + interactLayer + ")");
        }
        else
        {
            Debug.LogError("Could not find PlantedTree prefab at " + path);
        }
    }

    private static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
