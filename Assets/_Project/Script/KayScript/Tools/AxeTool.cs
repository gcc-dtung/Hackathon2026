using KayScript.Interaction;
using KayScript.Inventory;
using UnityEngine;

namespace KayScript.Tools
{
    public class AxeTool : MonoBehaviour, ITool
    {
        public string ToolName => "Axe";

        public bool CanInteract(IHarvestable target)
        {
            return target.Category == HarvestCategory.Tree;
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
