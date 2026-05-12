using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform gridParent;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private TextMeshProUGUI capacityText;

    [Header("Buttons")]
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [Header("Icons — Assign in Inspector (optional)")]
    [Tooltip("If left empty, icons will be loaded from Resources/Icons/{ResourceType} at runtime")]
    [SerializeField] private Sprite steelIcon;
    [SerializeField] private Sprite paperIcon;
    [SerializeField] private Sprite plasticIcon;
    [SerializeField] private Sprite glassIcon;
    [SerializeField] private Sprite canIcon;
    [SerializeField] private Sprite woodIcon;
    [SerializeField] private Sprite stoneIcon;
    [SerializeField] private Sprite seedIcon;
    [SerializeField] private Sprite oxygenTankIcon;

    [Header("Grid Settings")]
    [SerializeField] private Vector2 cellSize = new Vector2(120, 120);
    [SerializeField] private Vector2 spacing = new Vector2(15, 15);
    [SerializeField] private int columnCount = 5;
    [SerializeField] private float paddingHorizontal = 20f;
    [SerializeField] private float paddingVertical = 20f;

    private Dictionary<ResourceType, InventorySlot> _slotMap = new Dictionary<ResourceType, InventorySlot>();
    private Dictionary<ResourceType, Sprite> _iconCache = new Dictionary<ResourceType, Sprite>();
    private bool _isSubscribed;

    private void Start()
    {
        // Start closed
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (openButton != null)
        {
            openButton.gameObject.SetActive(true);
            openButton.onClick.AddListener(OpenInventory);
        }
        if (closeButton != null)
        {
            closeButton.gameObject.SetActive(false);
            closeButton.onClick.AddListener(CloseInventory);
        }

        CacheIcons();
        InitializeGrid();
        SubscribeToBackpack();
        UpdateGrid();
    }

    private void OnEnable()
    {
        SubscribeToBackpack();
        UpdateGrid();
    }

    private void OnDisable()
    {
        UnsubscribeFromBackpack();
    }

    private void OnDestroy()
    {
        UnsubscribeFromBackpack();
        if (openButton != null) openButton.onClick.RemoveListener(OpenInventory);
        if (closeButton != null) closeButton.onClick.RemoveListener(CloseInventory);
    }

    private void SubscribeToBackpack()
    {
        if (_isSubscribed || Backpack.Instance == null) return;

        Backpack.Instance.OnInventoryChanged += HandleInventoryChanged;
        _isSubscribed = true;
    }

    private void UnsubscribeFromBackpack()
    {
        if (!_isSubscribed || Backpack.Instance == null) return;

        Backpack.Instance.OnInventoryChanged -= HandleInventoryChanged;
        _isSubscribed = false;
    }

    public bool IsInventoryOpen()
    {
        return inventoryPanel != null && inventoryPanel.activeSelf;
    }

    public void OpenInventory()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(true);
        if (openButton != null) openButton.gameObject.SetActive(false);
        if (closeButton != null) closeButton.gameObject.SetActive(true);
        UpdateGrid();
    }

    public void CloseInventory()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (openButton != null) openButton.gameObject.SetActive(true);
        if (closeButton != null) closeButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Build icon cache: use Inspector sprites first, fallback to Resources/Icons/{name}
    /// </summary>
    private void CacheIcons()
    {
        _iconCache[ResourceType.Steel] = steelIcon;
        _iconCache[ResourceType.Paper] = paperIcon;
        _iconCache[ResourceType.Plastic] = plasticIcon;
        _iconCache[ResourceType.Glass] = glassIcon;
        _iconCache[ResourceType.Can] = canIcon;
        _iconCache[ResourceType.Wood] = woodIcon;
        _iconCache[ResourceType.Stone] = stoneIcon;
        _iconCache[ResourceType.Seed] = seedIcon;
        _iconCache[ResourceType.OxygenTank] = oxygenTankIcon;

        // Auto-load missing icons from Resources/Icons/
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            if (_iconCache.TryGetValue(type, out Sprite cached) && cached != null)
                continue;

            Sprite loaded = Resources.Load<Sprite>($"Icons/{type}");
            if (loaded != null)
            {
                _iconCache[type] = loaded;
                Debug.Log($"[InventoryGridUI] Auto-loaded icon for {type} from Resources/Icons/{type}");
            }
            else
            {
                Debug.LogWarning($"[InventoryGridUI] No icon found for {type}. Assign in Inspector or place sprite at Resources/Icons/{type}");
            }
        }
    }

    private void InitializeGrid()
    {
        if (Backpack.Instance == null || slotPrefab == null || gridParent == null) return;

        // Setup GridLayoutGroup on the parent
        SetupGridLayout();

        // Create one slot for every ResourceType in the enum
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            InventorySlot slot = Instantiate(slotPrefab, gridParent);
            Sprite icon = GetIconForType(type);
            slot.SetSlot(type, 0, icon); // Default count = 0
            _slotMap[type] = slot;
        }
    }

    [ContextMenu("Apply Grid Layout")]
    private void ApplyGridLayout()
    {
        if (gridParent == null)
        {
            Debug.LogWarning("[InventoryGridUI] gridParent is not assigned!");
            return;
        }
        SetupGridLayout();
        Debug.Log("[InventoryGridUI] Grid layout applied.");
    }

    private void SetupGridLayout()
    {
        var grid = gridParent.GetComponent<GridLayoutGroup>();
        if (grid == null)
            grid = gridParent.gameObject.AddComponent<GridLayoutGroup>();

        // Use Inspector values directly — no auto-calculation
        grid.padding = new RectOffset(
            (int)paddingHorizontal,
            (int)paddingHorizontal,
            (int)paddingVertical,
            (int)paddingVertical
        );
        grid.cellSize = cellSize;
        grid.spacing = spacing;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columnCount;
        grid.childAlignment = TextAnchor.MiddleCenter;
    }

    private void HandleInventoryChanged(ResourceType type, int amount)
    {
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        if (Backpack.Instance == null) return;

        // Update every slot with its current count (including 0)
        foreach (var kvp in _slotMap)
        {
            int count = Backpack.Instance.GetItemCount(kvp.Key);
            Sprite icon = GetIconForType(kvp.Key);
            kvp.Value.SetSlot(kvp.Key, count, icon);
        }

        // Update capacity text
        if (capacityText != null)
        {
            capacityText.text = $"{Backpack.Instance.TotalItemCount} / {Backpack.Instance.MaxSlots}";
        }
    }

    private Sprite GetIconForType(ResourceType type)
    {
        return _iconCache.TryGetValue(type, out Sprite icon) ? icon : null;
    }
}
