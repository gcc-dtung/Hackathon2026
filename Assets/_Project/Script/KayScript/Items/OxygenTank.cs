using UnityEngine;

public class OxygenTank : MonoBehaviour
{
    [SerializeField] private float restoreAmount = 30f;
    [SerializeField] private OxygenBar oxygenBar;

    public void UseFromInventory()
    {
        if (Backpack.Instance == null) return;

        if (oxygenBar == null)
        {
            oxygenBar = FindFirstObjectByType<OxygenBar>();
        }

        if (oxygenBar == null || oxygenBar.IsFull) return;

        if (Backpack.Instance.RemoveItem(ResourceType.OxygenTank, 1))
        {
            oxygenBar.AddOxygen(restoreAmount);
        }
    }
}
