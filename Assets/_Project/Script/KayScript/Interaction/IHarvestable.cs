using KayScript.Inventory;

namespace KayScript.Interaction
{
    public interface IHarvestable
    {
        HarvestCategory Category { get; }
        ResourceType ResourceType { get; }
        int HarvestAmount { get; }
        void OnHarvested();
    }
}
