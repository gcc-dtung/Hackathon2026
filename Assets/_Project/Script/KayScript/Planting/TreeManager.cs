using System;
using System.Collections.Generic;

public class TreeManager : Singleton<TreeManager>
{
    private List<PlantedTree> _livingTrees = new List<PlantedTree>();

    public int TreeCount => _livingTrees.Count;
    public event Action<int> OnTreeCountChanged;

    public void RegisterTree(PlantedTree tree)
    {
        if (!_livingTrees.Contains(tree))
        {
            _livingTrees.Add(tree);
            OnTreeCountChanged?.Invoke(_livingTrees.Count);
        }
    }

    public void UnregisterTree(PlantedTree tree)
    {
        if (_livingTrees.Contains(tree))
        {
            _livingTrees.Remove(tree);
            OnTreeCountChanged?.Invoke(_livingTrees.Count);
        }
    }
}
