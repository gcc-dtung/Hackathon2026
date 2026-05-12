using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Image bgImage;
    [SerializeField] private Color defaultBgColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    [SerializeField] private Color successColor = new Color(0f, 0.8f, 0f, 0.5f);
    [SerializeField] private Color failColor = new Color(0.8f, 0f, 0f, 0.5f);

    private ShopItemData _itemData;

    private void Awake()
    {
        SetupLayout();
    }

    private void SetupLayout()
    {
        var rootLayoutElement = gameObject.GetComponent<LayoutElement>();
        if (rootLayoutElement == null) rootLayoutElement = gameObject.AddComponent<LayoutElement>();
        rootLayoutElement.minHeight = 100f;

        var layout = gameObject.GetComponent<HorizontalLayoutGroup>();
        if (layout == null) layout = gameObject.AddComponent<HorizontalLayoutGroup>();
        
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.spacing = 30f;
        layout.padding = new RectOffset(20, 20, 15, 15);

        if (bgImage != null && bgImage.gameObject != gameObject)
        {
            var le = bgImage.gameObject.GetComponent<LayoutElement>();
            if (le == null) le = bgImage.gameObject.AddComponent<LayoutElement>();
            le.ignoreLayout = true;
            
            RectTransform bgRect = bgImage.rectTransform;
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            bgImage.transform.SetAsFirstSibling();
        }

        if (iconImage != null && iconImage.gameObject != gameObject)
        {
            iconImage.transform.SetAsLastSibling();
            var le = iconImage.gameObject.GetComponent<LayoutElement>();
            if (le == null) le = iconImage.gameObject.AddComponent<LayoutElement>();
            le.minWidth = 80f;
            le.preferredWidth = 80f;
        }

        if (nameText != null && nameText.gameObject != gameObject)
        {
            nameText.transform.SetAsLastSibling();
            var le = nameText.gameObject.GetComponent<LayoutElement>();
            if (le == null) le = nameText.gameObject.AddComponent<LayoutElement>();
            le.minWidth = 150f;
            le.flexibleWidth = 1f;
        }

        if (priceText != null && priceText.gameObject != gameObject)
        {
            priceText.transform.SetAsLastSibling();
            var le = priceText.gameObject.GetComponent<LayoutElement>();
            if (le == null) le = priceText.gameObject.AddComponent<LayoutElement>();
            le.minWidth = 120f;
        }

        if (actionButton != null && actionButton.gameObject != gameObject)
        {
            actionButton.transform.SetAsLastSibling();
            var le = actionButton.gameObject.GetComponent<LayoutElement>();
            if (le == null) le = actionButton.gameObject.AddComponent<LayoutElement>();
            le.minWidth = 150f;
            le.minHeight = 60f;
        }
    }

    public void SetItem(ShopItemData itemData)
    {
        _itemData = itemData;

        if (iconImage != null && itemData.icon != null) iconImage.sprite = itemData.icon;
        if (nameText != null) nameText.text = itemData.itemName;
        if (priceText != null) priceText.text = $"{itemData.price} Coins";
        
        if (buttonText != null)
        {
            buttonText.text = itemData.action == ShopAction.Buy ? "Buy" : "Sell";
        }

        if (bgImage != null) bgImage.color = defaultBgColor;

        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(OnActionButtonClicked);
        }
    }

    private void OnActionButtonClicked()
    {
        if (_itemData == null || ShopManager.Instance == null) return;

        bool success = false;

        if (_itemData.action == ShopAction.Buy)
        {
            success = ShopManager.Instance.BuyItem(_itemData);
        }
        else if (_itemData.action == ShopAction.Sell)
        {
            success = ShopManager.Instance.SellItem(_itemData);
        }

        ShowFeedback(success);
    }

    /// <summary>Gọi từ GamepadUIController để thực hiện mua/bán.</summary>
    public void TriggerAction() => OnActionButtonClicked();

    /// <summary>Bật/tắt highlight khi navigate bằng gamepad.</summary>
    public void SetHighlight(bool highlighted)
    {
        if (bgImage == null) return;
        bgImage.color = highlighted
            ? new Color(1f, 0.85f, 0f, 0.5f)  // gold highlight
            : defaultBgColor;
    }

    private void ShowFeedback(bool success)
    {
        if (bgImage != null)
        {
            // Simple color flash for feedback
            Color targetColor = success ? successColor : failColor;
            bgImage.color = targetColor;
            
            // Revert back to default after 0.2s using simple invoke
            CancelInvoke(nameof(RevertColor));
            Invoke(nameof(RevertColor), 0.2f);
        }
    }

    private void RevertColor()
    {
        if (bgImage != null) bgImage.color = defaultBgColor;
    }
}
