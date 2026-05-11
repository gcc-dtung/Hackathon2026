using UnityEngine;

public class HandTool : MonoBehaviour, ITool
{
    public string ToolName => "Hand";

    public bool CanInteract(IHarvestable target)
    {
        return target.Category == HarvestCategory.Trash;
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
