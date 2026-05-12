using UnityEngine;
using UnityEditor;

public static class PlantingButtonSetupEditor
{
    [MenuItem("Tools/Setup Planting Button (One-Shot)")]
    public static void Run()
    {
        GameObject btnGO = GameObject.Find("Plan Tree");
        if (btnGO == null) { Debug.LogError("Plan Tree button not found"); return; }

        GameObject player = GameObject.Find("Player");
        if (player == null) { Debug.LogError("Player not found"); return; }

        SeedPlacer sp = player.GetComponent<SeedPlacer>();
        if (sp == null) { Debug.LogError("SeedPlacer not found on Player"); return; }

        UnityEngine.UI.Image img = btnGO.GetComponent<UnityEngine.UI.Image>();
        if (img == null) { Debug.LogError("Image not found on Plan Tree button"); return; }

        PlantingButtonUI ui = btnGO.GetComponent<PlantingButtonUI>();
        if (ui == null)
        {
            ui = btnGO.AddComponent<PlantingButtonUI>();
            Debug.Log("Added PlantingButtonUI component to Plan Tree button.");
        }

        SerializedObject so = new SerializedObject(ui);
        so.FindProperty("seedPlacer").objectReferenceValue = sp;
        so.FindProperty("buttonImage").objectReferenceValue = img;
        so.FindProperty("normalColor").colorValue = Color.white;
        so.FindProperty("activeColor").colorValue = Color.green;

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(ui);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(btnGO.scene);

        Debug.Log("✅ PlantingButtonUI configured successfully!");
    }
}
