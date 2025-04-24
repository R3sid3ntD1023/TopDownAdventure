using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeSystem
{
    public enum ENodeState
    {
        Running,
        Success,
        Failure
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

        public ENodeState State { get; private set; }


        private bool m_IsStarted = false;

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
            if (!m_IsStarted)
            {
                OnBeginExecute();
                m_IsStarted = true;

            }

            State = OnExecute();

            if (State != ENodeState.Running)
            {
                OnEndExecute();
                m_IsStarted = false;
            }

            return State;
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