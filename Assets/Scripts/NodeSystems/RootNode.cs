using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Root", menuName = "Nodes/Root")]
public class RootNode : BaseNode
{
    public BaseNode Child;

    public override List<BaseNode> GetChildren()
    {
        return new List<BaseNode> { Child };
    }

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
}
