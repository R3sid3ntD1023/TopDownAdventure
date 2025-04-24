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

    [System.Serializable]
    public struct ConnectionInfo
    {
        public string ParentID;
        public string ChildID;
        public int InputIndex;
        public int OuputIndex;
    }

    [System.Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> _Keys = new List<TKey>();

        [SerializeField]
        private List<TValue> _Values = new List<TValue>();


        public void OnBeforeSerialize()
        {
            _Keys.Clear(); _Values.Clear();
            foreach (var kv in this)
            {
                _Keys.Add(kv.Key);
                _Values.Add(kv.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (_Keys.Count != _Values.Count)
                throw new Exception($"there are {_Keys.Count} and {_Values.Count} values after deserialization");

            for (int i = 0; i < _Keys.Count; i++)
            {
                Add(_Keys[i], _Values[i]);
            }
        }

    }

    public abstract class NodeTree : NodeTreeBase, ICloneable
    {

        public BaseNode Current;

        public List<BaseNode> Nodes;

        public List<ConnectionInfo> NodeConnections;

        public BaseNode CreateNode(Type type, Vector2 pos)
        {
            var new_node = ScriptableObject.CreateInstance(type.FullName) as BaseNode;
            new_node.Position = pos;
            new_node.name = type.Name;
            new_node.Title = type.Name;
            new_node.ParentTree = this;
            new_node.ID = Guid.NewGuid().ToString();

            AddNewNode(new_node);
            return new_node;
        }

        public void AddNewNode(BaseNode node)
        {
            Undo.RecordObject(this, "Added Node");

            Nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);


            Undo.RegisterCreatedObjectUndo(node, "Created Node");

            EditorUtility.SetDirty(this);


        }

        public void RemoveNode(BaseNode node)
        {
            Undo.RecordObject(this, "Removed Node");

            Nodes.Remove(node);

            Undo.DestroyObjectImmediate(node);

            EditorUtility.SetDirty(this);
        }

        public void AddChild(BaseNode parent, BaseNode child)
        {
            var _p = parent as IHaveChildrenInterface<BaseNode>;
            if (_p != null)
            {
                Undo.RecordObject(parent, "Add Child");

                _p.AddChild(child);

                EditorUtility.SetDirty(parent);
            }
        }

        public override object Clone()
        {
            var tree = Instantiate(this);
            tree.Blackboard = Instantiate(Blackboard);
            tree.Current = Current?.Clone() as BaseNode;
            tree.NodeConnections = NodeConnections;
            return tree;
        }

        public void RemoveChild(BaseNode parent, BaseNode child)
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
