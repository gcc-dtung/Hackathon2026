
public interface ITool
{
    string ToolName { get; }
    bool CanInteract(IHarvestable target);
    void Interact(IHarvestable target);
}
