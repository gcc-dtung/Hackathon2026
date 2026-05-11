using UnityEngine;

public class TrashObject : MonoBehaviour, IHarvestable
{
    [SerializeField] private ResourceType trashType = ResourceType.Steel; // Can select in Inspector
    [SerializeField] private int harvestAmount = 1;

    public HarvestCategory Category => HarvestCategory.Trash;
    public ResourceType ResourceType => trashType;
    public int HarvestAmount => harvestAmount;

    public void OnHarvested()
    {
        Destroy(gameObject);
    }
}
