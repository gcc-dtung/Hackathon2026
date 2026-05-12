using UnityEngine;
using UnityEngine.EventSystems;

public class SeedPlacer : MonoBehaviour
{
    [SerializeField] private PlantedTree treePrefab;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float placeRange = 4f;
    [SerializeField] private LayerMask groundLayer;

    private bool _isPlantingMode = false;

    public bool IsPlantingMode => _isPlantingMode;

    /// <summary>
    /// Được gọi bởi UI Button để bật/tắt chế độ trồng cây.
    /// </summary>
    public void TogglePlantingMode()
    {
        // Kiểm tra có seed không trước khi cho vào planting mode
        if (!_isPlantingMode && Backpack.Instance != null 
            && !Backpack.Instance.HasItem(ResourceType.Seed, 1))
        {
            Debug.Log("Không có hạt giống!");
            return;
        }
        _isPlantingMode = !_isPlantingMode;
    }

    public void SetPlantingMode(bool active)
    {
        _isPlantingMode = active;
    }

    private bool IsGamepadActive()
    {
        return UnityEngine.InputSystem.Gamepad.current != null && UnityEngine.InputSystem.Gamepad.current.wasUpdatedThisFrame;
    }

    private void Update()
    {
        if (!_isPlantingMode) return;

        // Gamepad/Keyboard: raycast từ tâm màn hình
        if (IsGamepadActive() || UnityEngine.InputSystem.Keyboard.current != null)
        {
            if ((UnityEngine.InputSystem.Gamepad.current != null && UnityEngine.InputSystem.Gamepad.current.buttonSouth.wasPressedThisFrame) ||
                (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame))
            {
                Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                TryPlantSeed(center);
                return;
            }
        }

        // Simple tap to plant, checking if we hold seed
        var pointer = UnityEngine.InputSystem.Pointer.current;
        if (pointer != null && pointer.press.wasPressedThisFrame)
        {
            // Ignore UI touches
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            TryPlantSeed(pointer.position.ReadValue());
        }
    }

    private void TryPlantSeed(Vector2 screenPos)
    {
        if (playerCamera == null || treePrefab == null || Backpack.Instance == null) return;

        // Check if we have seeds
        if (!Backpack.Instance.HasItem(ResourceType.Seed, 1)) return;

        Ray ray = playerCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, placeRange, groundLayer))
        {
            // Consume seed
            if (Backpack.Instance.RemoveItem(ResourceType.Seed, 1))
            {
                // Spawn tree
                PlantedTree newTree = Instantiate(treePrefab, hit.point, Quaternion.identity);
                Debug.Log($"Đã trồng cây tại {hit.point}");
                
                // Tự động tắt planting mode sau khi trồng
                _isPlantingMode = false;
            }
        }
    }
}
