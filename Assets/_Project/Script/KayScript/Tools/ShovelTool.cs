using KayScript.Interaction;
using KayScript.Inventory;
using UnityEngine;

namespace KayScript.Tools
{
    public class ShovelTool : MonoBehaviour, ITool
    {
        public string ToolName => "Shovel";

        public bool CanInteract(IHarvestable target)
        {
            return target.Category == HarvestCategory.Rock;
        }

        public void Interact(IHarvestable target)
        {
            if (CanInteract(target))
            {
                Backpack.Instance.AddItem(target.ResourceType, target.HarvestAmount);
                target.OnHarvested();
            }
        }
    }
}
