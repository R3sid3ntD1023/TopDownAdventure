using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseNode : ScriptableObject, ICloneable
{
    [AssetReference]
    public NodeTreeBase ParentTree;

    [HideInInspector]
    public NodeID ID;

    [HideInInspector]
    public Vector2 Position;

    public string Title = "Title";

    [TextArea]
    public string Description = "Description...";


    public virtual object Clone()
    {
        return Instantiate(this);
    }

    public virtual VisualElement CreateInspectorGUI()
    {
        return null;
    }

    public void Execute()
    {
        OnExecute();
    }

    protected abstract void OnExecute();

    protected Blackboard GetBlackboard()
    {
        return ParentTree.Blackboard;
    }
}
