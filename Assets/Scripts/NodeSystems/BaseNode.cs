using System;
using UnityEngine;

public abstract class BaseNode : ScriptableObject, ICloneable
{
    [ReadOnlyProperty]
    public NodeID ID;

    [ReadOnlyProperty]
    public Vector2 Position;

    public string Title = "Title";

    [TextArea]
    public string Description = "Description...";


    public virtual object Clone()
    {
        return Instantiate(this);
    }

    public void Execute()
    {
        OnExecute();
    }

    protected abstract void OnExecute();
}
