using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class NodeTree<NodeType, RootType> : ScriptableObject, ICloneable where NodeType : BaseNode
{
    public RootType Current;

    public List<NodeType> Nodes = new List<NodeType>();

    public abstract void Execute();

    public abstract object Clone();

    public NodeType CreateNode(Type type, Vector2 pos)
    {
        var new_node = ScriptableObject.CreateInstance(type) as NodeType;
        new_node.Position = pos;
        new_node.name = type.Name;

        Undo.RegisterCreatedObjectUndo(new_node, "Created Node");
        AddNewNode(new_node);
        return new_node;
    }

    public void AddNewNode(NodeType node)
    {
        Nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        Undo.RegisterFullObjectHierarchyUndo(this, "Added Node");
    }

    public void RemoveNode(NodeType node)
    {
        Nodes.Remove(node);

        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();

        Undo.RegisterFullObjectHierarchyUndo(this, "Removed Node");

    }

    public void AddChild(NodeType parent, NodeType child)
    {
        var _p = parent as IHaveChildrenInterface<BaseNode>;
        if (_p != null)
        {
            _p.AddChild(child);
        }
    }

    public void RemoveChild(NodeType parent, NodeType child)
    {
        var _p = parent as IHaveChildrenInterface<BaseNode>;
        if (_p != null)
        {
            _p.RemoveChild(child);
        }

    }
}
