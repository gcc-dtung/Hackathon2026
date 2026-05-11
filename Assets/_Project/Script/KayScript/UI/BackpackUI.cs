using System.Text;
using TMPro;
using UnityEngine;

public class BackpackUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventoryText;

    private void Start()
    {
        if (Backpack.Instance != null)
        {
            Backpack.Instance.OnInventoryChanged += HandleInventoryChanged;
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        if (Backpack.Instance != null)
        {
            Backpack.Instance.OnInventoryChanged -= HandleInventoryChanged;
        }
    }

    private void HandleInventoryChanged(ResourceType type, int amount)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (inventoryText == null || Backpack.Instance == null) return;

        var items = Backpack.Instance.GetAllItems();
        StringBuilder sb = new StringBuilder();

        foreach (var kvp in items)
        {
            sb.AppendLine($"{kvp.Key}: {kvp.Value}");
        }

        inventoryText.text = sb.ToString();
    }
}
