using KayScript.Inventory;
using UnityEngine;

namespace KayScript.Interaction.Harvestables
{
    public class TreeObject : MonoBehaviour, IHarvestable
    {
        [SerializeField] private int harvestAmount = 3;

        public HarvestCategory Category => HarvestCategory.Tree;
        public ResourceType ResourceType => ResourceType.Wood;
        public int HarvestAmount => harvestAmount;

        public void OnHarvested()
        {
            Destroy(gameObject);
        }
    }
}
