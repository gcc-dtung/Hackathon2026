using KayScript.Inventory;
using UnityEngine;

namespace KayScript.Interaction.Harvestables
{
    public class RockObject : MonoBehaviour, IHarvestable
    {
        [SerializeField] private int harvestAmount = 2;

        public HarvestCategory Category => HarvestCategory.Rock;
        public ResourceType ResourceType => ResourceType.Stone;
        public int HarvestAmount => harvestAmount;

        public void OnHarvested()
        {
            Destroy(gameObject);
        }
    }
}
