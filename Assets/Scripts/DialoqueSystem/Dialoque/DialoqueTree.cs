
using NodeSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DialoqueSystem
{
    [System.Serializable]
    public class OnDialoqueTreeFinishedEvent : UnityEvent<DialoqueTree> { }

    [CreateAssetMenu(fileName = "DialoqueTree", menuName = "Dialoque/Tree")]
    public class DialoqueTree : NodeTree
    {
        public UnityAction<DialoqueTree> OnFinished;

        private bool _Started = false;

        private bool _Finished = false;

        public void Initialize(GameObject owner)
        {
            Blackboard.SetKey("Self", owner);
        }

        public override void Execute()
        {
            if (IsFinished())
                return;

            Initialize();

            var state = Current?.Execute();
            if (state == ENodeState.Finished)
            {
                Current = (Current as DialoqueBaseNode)?.GetNext();
            }

            if (Current == null)
            {
                Finish();
                return;
            }

        }

        private void Initialize()
        {
            if (_Started)
                return;

            Traverse(Current as DialoqueBaseNode);
            _Started = true;
        }

        public bool IsFinished()
        {
            return _Finished;
        }

        private void Finish()
        {
            OnFinished.Invoke(this);
            _Finished = true;
        }


        private void Traverse(DialoqueBaseNode root)
        {
            DialoqueBaseNode current = root;
            while (current != null)
            {
                current.ParentTree = this;
                current = current.GetNext();
            }
        }
    }
}
