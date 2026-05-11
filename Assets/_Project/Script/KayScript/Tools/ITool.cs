using KayScript.Interaction;

namespace KayScript.Tools
{
    public interface ITool
    {
        string ToolName { get; }
        bool CanInteract(IHarvestable target);
        void Interact(IHarvestable target);
    }
}
