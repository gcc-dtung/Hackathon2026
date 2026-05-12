using UnityEngine;
using UnityEditor;

/// <summary>
/// One-shot editor utility - tự động gán fields cho SeedPlacer.
/// Chạy: Hamburger menu (≡) hoặc Tools > Setup SeedPlacer.
/// Script sẽ tự xóa sau khi chạy xong.
/// </summary>
public static class SeedPlacerSetupEditor
{
    [MenuItem("Tools/Setup SeedPlacer (One-Shot)")]
    public static void Run()
    {
        // Tìm Player
        GameObject player = GameObject.Find("Player");
        if (player == null) { Debug.LogError("[Setup] Player not found!"); return; }

        // Lấy hoặc thêm SeedPlacer
        SeedPlacer sp = player.GetComponent<SeedPlacer>();
        if (sp == null) { Debug.LogError("[Setup] SeedPlacer not on Player!"); return; }

        SerializedObject so = new SerializedObject(sp);

        // playerCamera -> Player/Main Camera
        Camera cam = player.GetComponentInChildren<Camera>();
        if (cam == null) { Debug.LogError("[Setup] Camera not found under Player!"); return; }
        so.FindProperty("playerCamera").objectReferenceValue = cam;
        Debug.Log($"[Setup] playerCamera -> {cam.name}");

        // placeRange = 10
        so.FindProperty("placeRange").floatValue = 10f;
        Debug.Log("[Setup] placeRange = 10");

        // groundLayer = Ground (layer 8)
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer < 0) { Debug.LogWarning("[Setup] Ground layer not found! Did you add it in Tags & Layers?"); }
        so.FindProperty("groundLayer").intValue = 1 << groundLayer;
        Debug.Log($"[Setup] groundLayer = Ground (1<<{groundLayer} = {1 << groundLayer})");

        // treePrefab - tìm PlantedTree prefab trong Assets
        string[] guids = AssetDatabase.FindAssets("t:Prefab PlantedTree");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                PlantedTree pt = prefab.GetComponent<PlantedTree>();
                if (pt != null)
                {
                    so.FindProperty("treePrefab").objectReferenceValue = pt;
                    Debug.Log($"[Setup] treePrefab -> {path}");
                }
                else
                {
                    Debug.LogWarning($"[Setup] Prefab at {path} has no PlantedTree component - skipping treePrefab");
                }
            }
        }
        else
        {
            Debug.LogWarning("[Setup] No PlantedTree prefab found in Assets. Please create one and drag it manually.");
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(sp);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(player.scene);
        Debug.Log("[Setup] ✅ SeedPlacer configured successfully!");

        // Wiring Button "Plan Tree" -> SeedPlacer.TogglePlantingMode
        SetupPlanTreeButton(player, sp);
    }

    private static void SetupPlanTreeButton(GameObject player, SeedPlacer sp)
    {
        // Tìm button Plan Tree
        GameObject btnGO = GameObject.Find("Plan Tree");
        if (btnGO == null)
        {
            Debug.LogWarning("[Setup] Button 'Plan Tree' not found in scene. Please wire it manually.");
            return;
        }

        UnityEngine.UI.Button btn = btnGO.GetComponent<UnityEngine.UI.Button>();
        if (btn == null)
        {
            Debug.LogWarning("[Setup] No Button component on 'Plan Tree'.");
            return;
        }

        SerializedObject btnSO = new SerializedObject(btn);
        SerializedProperty onClick = btnSO.FindProperty("m_OnClick");
        SerializedProperty calls = onClick.FindPropertyRelative("m_PersistentCalls.m_Calls");

        // Kiểm tra đã có TogglePlantingMode chưa
        bool alreadyWired = false;
        for (int i = 0; i < calls.arraySize; i++)
        {
            var call = calls.GetArrayElementAtIndex(i);
            string methodName = call.FindPropertyRelative("m_MethodName").stringValue;
            if (methodName == "TogglePlantingMode") { alreadyWired = true; break; }
        }

        if (alreadyWired)
        {
            Debug.Log("[Setup] Button 'Plan Tree' already wired to TogglePlantingMode.");
            return;
        }

        calls.arraySize++;
        SerializedProperty newCall = calls.GetArrayElementAtIndex(calls.arraySize - 1);
        newCall.FindPropertyRelative("m_Target").objectReferenceValue = sp;
        newCall.FindPropertyRelative("m_TargetAssemblyTypeName").stringValue = "SeedPlacer, Assembly-CSharp";
        newCall.FindPropertyRelative("m_MethodName").stringValue = "TogglePlantingMode";
        newCall.FindPropertyRelative("m_Mode").intValue = 1; // void/no-args
        newCall.FindPropertyRelative("m_CallState").intValue = 2; // Runtime only

        btnSO.ApplyModifiedProperties();
        EditorUtility.SetDirty(btn);
        Debug.Log("[Setup] ✅ Button 'Plan Tree' wired to SeedPlacer.TogglePlantingMode!");
    }
}
