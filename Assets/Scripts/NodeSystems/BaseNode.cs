using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode : ScriptableObject, ICloneable
{
    [SerializeField, ReadOnlyProperty]
    public NodeID ID;

    public string Title = "Title";

    [TextArea]
    public string Description = "Description...";

    public virtual List<BaseNode> GetChildren()
    {
        return null;
    }

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
