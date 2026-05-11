using UnityEngine;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
{
    [Header("Panels")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button openShopButton;
    [SerializeField] private Button closeShopButton;
    [SerializeField] private Button sellAllTrashButton; // NEW
    
    [Header("Tabs")]
    [SerializeField] private Button buyTabButton;
    [SerializeField] private Button sellTabButton;
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject sellPanel;

    [Header("Dynamic Setup")]
    [SerializeField] private ShopItemUI shopItemPrefab;
    [SerializeField] private Transform buyContentTransform;
    [SerializeField] private Transform sellContentTransform;

    private bool _isPopulated = false;

    private void Start()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
        if (openShopButton != null)
        {
            openShopButton.gameObject.SetActive(false);
            openShopButton.onClick.AddListener(OpenShop);
        }
        if (closeShopButton != null) closeShopButton.onClick.AddListener(() => CloseShop(true));
        
        if (buyTabButton != null) buyTabButton.onClick.AddListener(() => SwitchTab(true));
        if (sellTabButton != null) sellTabButton.onClick.AddListener(() => SwitchTab(false));

        if (sellAllTrashButton != null)
        {
            sellAllTrashButton.onClick.AddListener(() => {
                if (ShopManager.Instance != null) ShopManager.Instance.SellAllTrash();
            });
        }
    }

    public void ShowShopButton(bool show)
    {
        if (openShopButton != null) openShopButton.gameObject.SetActive(show);
    }

    public void OpenShop()
    {
        if (!_isPopulated)
        {
            PopulateShop();
        }

        if (shopPanel != null) shopPanel.SetActive(true);
        ShowShopButton(false);
        SwitchTab(true); // Default to Buy tab
        
        // Optionally pause game or lock cursor here
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseShop(bool showButton = true)
    {
        if (shopPanel != null) shopPanel.SetActive(false);
        
        if (showButton) 
        {
            ShowShopButton(true); // Hiển thị lại nút Open khi đóng Shop
        }
        else
        {
            ShowShopButton(false);
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SwitchTab(bool isBuyTab)
    {
        if (buyPanel != null) buyPanel.SetActive(isBuyTab);
        if (sellPanel != null) sellPanel.SetActive(!isBuyTab);
    }

    private void PopulateShop()
    {
        if (ShopManager.Instance == null || shopItemPrefab == null) return;
        
        if (buyPanel != null) buyPanel.transform.localScale = Vector3.one;
        if (sellPanel != null) sellPanel.transform.localScale = Vector3.one;

        // Clear existing children if any (in case of re-population)
        if (buyContentTransform != null)
        {
            SetupLayout(buyContentTransform);
            foreach (Transform child in buyContentTransform) Destroy(child.gameObject);
        }
        if (sellContentTransform != null)
        {
            SetupLayout(sellContentTransform);
            foreach (Transform child in sellContentTransform) Destroy(child.gameObject);
        }

        // Spawn new items
        foreach (var item in ShopManager.Instance.AvailableItems)
        {
            Transform parent = (item.action == ShopAction.Buy) ? buyContentTransform : sellContentTransform;
            if (parent != null)
            {
                ShopItemUI newItem = Instantiate(shopItemPrefab, parent, false);
                newItem.SetItem(item);
                newItem.transform.localScale = Vector3.one;
            }
        }

        _isPopulated = true;
    }

    private void SetupLayout(Transform panelTransform)
    {
        if (panelTransform == null) return;
        
        var layout = panelTransform.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
        if (layout == null)
        {
            layout = panelTransform.gameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
        }
        
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;
        layout.spacing = 30f;
        layout.padding = new RectOffset(100, 100, 20, 20);
    }
}
