using CustomAttributes;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeSystem
{
    public enum ENodeState
    {
        Started,
        Executing,
        Finished
    }

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

        private ENodeState m_State = ENodeState.Started;


        public virtual object Clone()
        {
            return Instantiate(this);
        }

        public virtual VisualElement CreateInspectorGUI()
        {
            return null;
        }

        public ENodeState Execute()
        {
            if (m_State == ENodeState.Started)
            {
                OnBeginExecute();
                m_State = ENodeState.Executing;

                Debug.Log("Started!");
            }

            if (m_State != ENodeState.Finished)
            {
                var state = OnExecute();

                Debug.Log("Executing...");

                if (state == ENodeState.Finished)
                {
                    OnEndExecute();
                    m_State = ENodeState.Finished;
                }
            }

            return m_State;
        }

        protected virtual void OnBeginExecute() { Debug.Log("Begin Execute"); }

        protected abstract ENodeState OnExecute();

        protected virtual void OnEndExecute() { Debug.Log("End Execute"); }

        protected Blackboard GetBlackboard()
        {
            return ParentTree.Blackboard;
        }
    }
}