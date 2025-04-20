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
        [HideInInspector]
        public NodeTreeBase ParentTree;

        [HideInInspector]
        public string ID;

        [HideInInspector]
        public Vector2 Position;

        public string Title = "Title";

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

            }

            if (m_State != ENodeState.Finished)
            {
                var state = OnExecute();


                if (state == ENodeState.Finished)
                {
                    OnEndExecute();
                    m_State = ENodeState.Finished;
                }
            }

            return m_State;
        }

        protected virtual void OnBeginExecute() { }

        protected abstract ENodeState OnExecute();

        protected virtual void OnEndExecute() { }

        protected Blackboard GetBlackboard()
        {
            return ParentTree.Blackboard;
        }
    }
}