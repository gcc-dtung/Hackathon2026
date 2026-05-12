using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    // Require a collider with IsTrigger = true
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Open shop UI
            if (ShopUI.Instance != null)
            {
                ShopUI.Instance.SetCanOpen(true);
                ShopUI.Instance.ShowShopButton(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Close shop UI
            if (ShopUI.Instance != null)
            {
                ShopUI.Instance.SetCanOpen(false);
                ShopUI.Instance.CloseShop(false);
            }
        }
    }
}
