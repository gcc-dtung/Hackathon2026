using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SeedPlacer : MonoBehaviour
{
    [SerializeField] private PlantedTree treePrefab;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float placeRange = 4f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Input Reference")]
    [SerializeField] private InputActionReference interactAction; // Dùng chung Interact action (vd: phím E / nút A)

    private bool _isPlantingMode = false;

    public bool IsPlantingMode => _isPlantingMode;

    private void OnEnable()
    {
        if (interactAction != null) interactAction.action.Enable();
    }

    private void OnDisable()
    {
        if (interactAction != null) interactAction.action.Disable();
    }

    public void TogglePlantingMode()
    {
        if (!_isPlantingMode && Backpack.Instance != null && !Backpack.Instance.HasItem(ResourceType.Seed, 1))
        {
            Debug.Log("Không có hạt giống để trồng!");
            return;
        }
        _isPlantingMode = !_isPlantingMode;
        Debug.Log($"Planting Mode: {_isPlantingMode}");
    }

    private void Update()
    {
        if (!_isPlantingMode) return;

        if (IsGamepadActive())
        {
            HandleGamepadInput();
        }
        else
        {
            HandlePointerInput();
        }
    }

    private void HandleGamepadInput()
    {
        bool wasPressed = false;

        // Ưu tiên dùng InputAction (nếu đã gán)
        if (interactAction != null)
        {
            wasPressed = interactAction.action.WasPressedThisFrame();
        }
        else
        {
            // Fallback nếu chưa gán InputAction
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) wasPressed = true;
            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) wasPressed = true;
        }

        if (wasPressed)
        {
            // Bắn raycast từ tâm màn hình
            Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            TryPlantSeed(center);
        }
    }

    private void HandlePointerInput()
    {
        var pointer = Pointer.current;
        if (pointer != null && pointer.press.wasPressedThisFrame)
        {
            // Bỏ qua touch/click nếu đang bấm vào UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            TryPlantSeed(pointer.position.ReadValue());
        }
    }

    private bool IsGamepadActive()
    {
        if (interactAction != null && interactAction.action.activeControl != null)
        {
            var device = interactAction.action.activeControl.device;
            return device is Gamepad || device is Keyboard;
        }

        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame) return true;
        if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame) return true;

        return false;
    }

    private void TryPlantSeed(Vector2 screenPos)
    {
        if (playerCamera == null || treePrefab == null || Backpack.Instance == null) return;

        // Check if we have seeds
        if (!Backpack.Instance.HasItem(ResourceType.Seed, 1))
        {
            Debug.Log("Đã hết hạt giống, tự động thoát Planting Mode.");
            _isPlantingMode = false;
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, placeRange, groundLayer))
        {
            // Consume seed
            if (Backpack.Instance.RemoveItem(ResourceType.Seed, 1))
            {
                // Spawn tree
                Instantiate(treePrefab, hit.point, Quaternion.identity);
                Debug.Log($"Đã trồng cây tại {hit.point}");
                
                // Tự động tắt planting mode sau khi trồng 1 cây
                _isPlantingMode = false;
            }
        }
    }
    public void SetPlantingMode(bool active)
    {
        _isPlantingMode = active;
    }
}
