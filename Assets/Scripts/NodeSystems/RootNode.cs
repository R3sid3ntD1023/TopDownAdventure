using System.Collections.Generic;
using UnityEngine;

namespace NodeSystem
{

    [CreateAssetMenu(fileName = "Root", menuName = "Nodes/Root")]
    public class RootNode : BaseNode, IHaveChildrenInterface<BaseNode>
    {
        [HideInInspector]
        public BaseNode Child;

        protected override ENodeState OnExecute()
        {
            if (Child != null)
                return Child.Execute();

            return ENodeState.Success;
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
}