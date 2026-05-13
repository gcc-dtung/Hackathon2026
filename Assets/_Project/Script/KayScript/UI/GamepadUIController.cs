using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Xử lý tất cả UI input cho Gamepad và Keyboard.
/// Gắn script này lên cùng GameObject với PlayerMovement (hoặc một Manager).
/// Kéo tất cả các InputActionReference và reference UI vào Inspector.
/// </summary>
public class GamepadUIController : MonoBehaviour
{
    // ─── Input Actions ─────────────────────────────────────────────────────────
    [Header("Input Actions")]
    [SerializeField] private InputActionReference toggleInventoryAction;
    [SerializeField] private InputActionReference toggleShopAction;
    [SerializeField] private InputActionReference shopBuyTabAction;
    [SerializeField] private InputActionReference shopSellTabAction;
    [SerializeField] private InputActionReference shopConfirmAction;
    [SerializeField] private InputActionReference shopSellAllAction;
    [SerializeField] private InputActionReference shopNavigateAction;
    [SerializeField] private InputActionReference nextToolAction;
    [SerializeField] private InputActionReference prevToolAction;

    // ─── References ────────────────────────────────────────────────────────────
    [Header("References")]
    [SerializeField] private InventoryGridUI inventoryGridUI;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private ToolSelector toolSelector;

    // ─── Shop Navigation State ─────────────────────────────────────────────────
    // Danh sách các item đang hiển thị trong tab hiện tại
    private List<ShopItemUI> _currentShopItems = new List<ShopItemUI>();
    private int _shopSelectedIndex = 0;
    private bool _isBuyTab = true;

    // Cooldown để tránh navigate quá nhanh khi giữ stick
    private float _navCooldown = 0f;
    private const float NAV_COOLDOWN_TIME = 0.25f;

    // ─── Lifecycle ─────────────────────────────────────────────────────────────

    private void OnEnable()
    {
        Enable(toggleInventoryAction);
        Enable(toggleShopAction);
        Enable(shopBuyTabAction);
        Enable(shopSellTabAction);
        Enable(shopConfirmAction);
        Enable(shopSellAllAction);
        Enable(shopNavigateAction);
        Enable(nextToolAction);
        Enable(prevToolAction);
    }

    private void OnDisable()
    {
        Disable(toggleInventoryAction);
        Disable(toggleShopAction);
        Disable(shopBuyTabAction);
        Disable(shopSellTabAction);
        Disable(shopConfirmAction);
        Disable(shopSellAllAction);
        Disable(shopNavigateAction);
        Disable(nextToolAction);
        Disable(prevToolAction);
    }

    private void Update()
    {
        HandleToggleInventory();
        HandleToggleShop();
        HandleShopTabs();
        HandleShopNavigate();
        HandleShopConfirm();
        HandleShopSellAll();
        HandleToolSwitch();
    }

    // ─── Inventory ─────────────────────────────────────────────────────────────

    private void HandleToggleInventory()
    {
        if (toggleInventoryAction == null) return;
        if (!toggleInventoryAction.action.WasPressedThisFrame()) return;
        if (inventoryGridUI == null) return;

        // Toggle: nếu panel đang active thì đóng, ngược lại mở
        bool isOpen = inventoryGridUI.IsInventoryOpen();
        if (isOpen)
            inventoryGridUI.CloseInventory();
        else
            inventoryGridUI.OpenInventory();
    }

    // ─── Shop Toggle ───────────────────────────────────────────────────────────

    private void HandleToggleShop()
    {
        if (toggleShopAction == null) return;
        if (!toggleShopAction.action.WasPressedThisFrame()) return;
        if (shopUI == null) return;

        bool isOpen = shopUI.IsShopOpen();
        if (isOpen)
        {
            shopUI.CloseShop(false);
            RefreshShopItems();
        }
        else
        {
            // Chỉ mở được khi shopUI cho phép (player đang ở vùng shop)
            if (shopUI.CanOpen())
            {
                shopUI.OpenShop();
                RefreshShopItems();
            }
        }
    }

    // ─── Shop Tabs ─────────────────────────────────────────────────────────────

    private void HandleShopTabs()
    {
        if (shopUI == null || !shopUI.IsShopOpen()) return;

        if (shopBuyTabAction != null && shopBuyTabAction.action.WasPressedThisFrame())
        {
            _isBuyTab = true;
            shopUI.SwitchTabPublic(true);
            _shopSelectedIndex = 0;
            RefreshShopItems();
        }

        if (shopSellTabAction != null && shopSellTabAction.action.WasPressedThisFrame())
        {
            _isBuyTab = false;
            shopUI.SwitchTabPublic(false);
            _shopSelectedIndex = 0;
            RefreshShopItems();
        }
    }

    // ─── Shop Navigate (stick / dpad) ──────────────────────────────────────────

    private void HandleShopNavigate()
    {
        if (shopUI == null || !shopUI.IsShopOpen()) return;
        if (shopNavigateAction == null) return;
        if (_currentShopItems.Count == 0) return;

        _navCooldown -= Time.deltaTime;
        if (_navCooldown > 0f) return;

        Vector2 nav = shopNavigateAction.action.ReadValue<Vector2>();

        int delta = 0;
        if (nav.y < -0.5f) delta = 1;   // stick down → next item
        else if (nav.y > 0.5f) delta = -1; // stick up → prev item

        if (delta == 0) return;

        _navCooldown = NAV_COOLDOWN_TIME;
        _shopSelectedIndex = Mathf.Clamp(_shopSelectedIndex + delta, 0, _currentShopItems.Count - 1);
        HighlightShopItem(_shopSelectedIndex);
    }

    // ─── Shop Confirm (buy / sell selected item) ───────────────────────────────

    private void HandleShopConfirm()
    {
        if (shopUI == null || !shopUI.IsShopOpen()) return;
        if (shopConfirmAction == null) return;
        if (!shopConfirmAction.action.WasPressedThisFrame()) return;
        if (_currentShopItems.Count == 0) return;

        if (_shopSelectedIndex >= 0 && _shopSelectedIndex < _currentShopItems.Count)
        {
            _currentShopItems[_shopSelectedIndex].TriggerAction();
        }
    }

    // ─── Shop Sell All (B / Circle / R) ────────────────────────────────────────

    private void HandleShopSellAll()
    {
        if (shopUI == null || !shopUI.IsShopOpen()) return;
        if (shopSellAllAction == null) return;
        if (!shopSellAllAction.action.WasPressedThisFrame()) return;
        if (!ShopManager.InstanceExists) return;

        // Chỉ cho phép bán nhanh nếu đang ở tab Sell
        if (!_isBuyTab)
        {
            ShopManager.Instance.SellAllTrash();
            
            // Có thể thêm sound effect hoặc flash cho bảng shop tại đây nếu muốn
        }
    }

    // ─── Tool Switch ───────────────────────────────────────────────────────────

    private void HandleToolSwitch()
    {
        if (toolSelector == null) return;

        if (nextToolAction != null && nextToolAction.action.WasPressedThisFrame())
            CycleToolBy(1);

        if (prevToolAction != null && prevToolAction.action.WasPressedThisFrame())
            CycleToolBy(-1);
    }

    /// <summary>Cycle qua các tool đã được unlock.</summary>
    private void CycleToolBy(int direction)
    {
        // Tìm index hiện tại bằng cách tìm tool match CurrentTool
        int toolCount = toolSelector.ToolCount;
        if (toolCount == 0) return;

        int current = toolSelector.CurrentToolIndex;
        int next = current;

        for (int attempt = 0; attempt < toolCount; attempt++)
        {
            next = (next + direction + toolCount) % toolCount;
            if (toolSelector.IsToolUnlocked(next))
            {
                toolSelector.SelectTool(next);
                return;
            }
        }
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private void RefreshShopItems()
    {
        _currentShopItems.Clear();
        if (shopUI == null) return;

        ShopItemUI[] items = shopUI.GetCurrentTabItems();
        if (items != null)
            _currentShopItems.AddRange(items);

        _shopSelectedIndex = 0;
        if (_currentShopItems.Count > 0)
            HighlightShopItem(0);
    }

    private void HighlightShopItem(int index)
    {
        for (int i = 0; i < _currentShopItems.Count; i++)
            _currentShopItems[i].SetHighlight(i == index);
    }

    private static void Enable(InputActionReference r)
    {
        if (r != null) r.action.Enable();
    }

    private static void Disable(InputActionReference r)
    {
        if (r != null) r.action.Disable();
    }
}
