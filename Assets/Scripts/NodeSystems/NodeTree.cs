using CustomAttributes;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NodeSystem
{
    public abstract class NodeTreeBase : ScriptableObject
    {
        [AssetReference]
        public Blackboard Blackboard;

        public abstract void Execute();

        public abstract object Clone();

    }

    public abstract class NodeTree<NodeType, RootType> : NodeTreeBase, ICloneable where NodeType : BaseNode
    {

        public RootType Current;

        public List<NodeType> Nodes = new List<NodeType>();


        public NodeType CreateNode(Type type, Vector2 pos)
        {
            var new_node = ScriptableObject.CreateInstance(type) as NodeType;
            new_node.Position = pos;
            new_node.name = type.Name;
            new_node.ParentTree = this;

            AddNewNode(new_node);
            return new_node;
        }

        public void AddNewNode(NodeType node)
        {
            Undo.RecordObject(this, "Added Node");

            Nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);

            Undo.RegisterCreatedObjectUndo(node, "Created Node");

            EditorUtility.SetDirty(this);


        }

        public void RemoveNode(NodeType node)
        {
            Undo.RecordObject(this, "Removed Node");

            Nodes.Remove(node);

            Undo.DestroyObjectImmediate(node);

            EditorUtility.SetDirty(this);
        }

        public void AddChild(NodeType parent, NodeType child)
        {
            var _p = parent as IHaveChildrenInterface<BaseNode>;
            if (_p != null)
            {
                Undo.RecordObject(parent, "Add Child");

                _p.AddChild(child);
                EditorUtility.SetDirty(parent);
            }

        }

        public void RemoveChild(NodeType parent, NodeType child)
        {
            var _p = parent as IHaveChildrenInterface<BaseNode>;
            if (_p != null)
            {
                Undo.RecordObject(parent, "Remove Child");

                _p.RemoveChild(child);
                EditorUtility.SetDirty(parent);
            }

        }
    }
}
