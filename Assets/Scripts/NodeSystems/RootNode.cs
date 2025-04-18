using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Root", menuName = "Nodes/Root")]
public class RootNode : BaseNode, IHaveChildrenInterface<BaseNode>
{
    [AssetReference]
    public BaseNode Child;

    protected override void OnExecute()
    {
        if (Child != null)
            Child.Execute();
    }

    public override object Clone()
    {
        var node = base.Clone() as RootNode;
        node.Child = Child?.Clone() as BaseNode;
        return node;
    }

    public void AddChild(BaseNode child)
    {
        Child = child;
    }

    public void RemoveChild(BaseNode child)
    {
        Child = null;
    }

    public BaseNode GetChild(int index)
    {
        return Child;
    }

    public bool HasChild(int index)
    {
        return Child != null;
    }

    public List<BaseNode> GetChildren()
    {
        return new List<BaseNode> { Child };
    }

}
