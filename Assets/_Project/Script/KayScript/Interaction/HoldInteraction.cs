using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HoldInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float holdDuration = 1.5f;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private float dragThreshold = 10f; // in pixels

    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private ToolSelector toolSelector;
    [SerializeField] private Camera playerCamera;

    [Header("Input Reference")]
    [SerializeField] private InputActionReference interactAction;

    public event System.Action<float> OnHoldProgress;
    public event System.Action OnInteractionComplete;

    private float _holdTimer = 0f;
    private bool _isHolding = false;
    private Vector2 _startTouchPosition;
    private IHarvestable _currentTarget;

    private void OnEnable()
    {
        if (interactAction != null) interactAction.action.Enable();
    }

    private void OnDisable()
    {
        if (interactAction != null) interactAction.action.Disable();
    }

    private void Update()
    {
        if (IsGamepadActive())
            HandleGamepadInput();
        else
            HandlePointerInput();
    }

    // ─── Gamepad / Keyboard (FPS Mode) ────────────────────────────────────────

    private void HandleGamepadInput()
    {
        if (interactAction == null) return;

        // Gamepad / Keyboard: interact NGAY khi nhấn (instant press)
        if (!interactAction.action.WasPressedThisFrame()) return;

        // Raycast từ tâm màn hình (FPS crosshair)
        Vector2 centerScreen = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        FindTarget(centerScreen);

        if (_currentTarget != null)
        {
            ITool currentTool = toolSelector.CurrentTool;
            if (currentTool != null && currentTool.CanInteract(_currentTarget))
            {
                UpdateProgress(1f);
                currentTool.Interact(_currentTarget);
                OnInteractionComplete?.Invoke();
            }
        }

        // Reset dù có interact hay không
        ResetHold();
    }

    // ─── Touch / Mouse (Pointer Mode) ─────────────────────────────────────────

    private void HandlePointerInput()
    {
        var pointer = Pointer.current;
        if (pointer == null) return;

        bool inputDown = pointer.press.wasPressedThisFrame;
        bool inputHeld = pointer.press.isPressed;
        bool inputUp   = pointer.press.wasReleasedThisFrame;
        Vector2 currentPosition = pointer.position.ReadValue();

        if (inputDown)
        {
            if (IsPointerOverUI())
                return;

            _isHolding = true;
            _startTouchPosition = currentPosition;
            _holdTimer = 0f;
            _currentTarget = null;
            UpdateProgress(0f);
        }
        else if (inputHeld && _isHolding)
        {
            // Check drag
            float distance = Vector2.Distance(_startTouchPosition, currentPosition);
            if (distance > dragThreshold)
            {
                ResetHold();
                return;
            }

            if (_currentTarget == null)
                FindTarget(currentPosition);

            if (_currentTarget != null)
            {
                ITool currentTool = toolSelector.CurrentTool;
                if (currentTool != null && currentTool.CanInteract(_currentTarget))
                {
                    _holdTimer += Time.deltaTime;
                    UpdateProgress(_holdTimer / holdDuration);

                    if (_holdTimer >= holdDuration)
                    {
                        currentTool.Interact(_currentTarget);
                        OnInteractionComplete?.Invoke();
                        ResetHold();
                    }
                }
                else
                {
                    ResetHold();
                }
            }
        }
        else if (inputUp)
        {
            ResetHold();
        }
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Trả về true nếu thiết bị input hiện tại là Gamepad HOẶC Keyboard.
    /// Cả hai đều dùng FPS mode (raycast từ tâm màn hình).
    /// </summary>
    private bool IsGamepadActive()
    {
        // Ưu tiên kiểm tra interactAction nếu có
        if (interactAction != null && interactAction.action.activeControl != null)
        {
            var device = interactAction.action.activeControl.device;
            return device is Gamepad || device is Keyboard;
        }

        // Fallback: kiểm tra thiết bị cuối cùng được dùng
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame) return true;
        if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame) return true;

        return false;
    }

    private void FindTarget(Vector2 screenPosition)
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            _currentTarget = hit.collider.GetComponent<IHarvestable>();
        }
    }

    private void ResetHold()
    {
        _isHolding = false;
        _holdTimer = 0f;
        _currentTarget = null;
        UpdateProgress(0f);
    }

    private void UpdateProgress(float progress)
    {
        OnHoldProgress?.Invoke(Mathf.Clamp01(progress));
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        var pointer = Pointer.current;
        if (pointer != null)
            eventData.position = pointer.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<TouchField>() != null)
                continue;
            return true;
        }

        return false;
    }
}
