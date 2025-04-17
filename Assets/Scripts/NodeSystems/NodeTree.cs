using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class NodeTree<NodeType, RootType> : ScriptableObject, ICloneable
{
    public RootType Current;

    public List<NodeType> Nodes;

    public abstract void Execute();

    public abstract object Clone();
}
