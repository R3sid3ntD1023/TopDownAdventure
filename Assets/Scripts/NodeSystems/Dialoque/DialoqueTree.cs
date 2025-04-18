
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnDialoqueTreeFinishedEvent : UnityEvent<DialoqueTree> { }

[CreateAssetMenu(fileName = "DialoqueTree", menuName = "Dialoque/Tree")]
public class DialoqueTree : NodeTree<DialoqueBaseNode, DialoqueBaseNode>
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
            Current = Current?.GetNext();
            Debug.Log($"{Current}");
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

        Traverse(Current);
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

    public override object Clone()
    {
        var tree = Instantiate(this);
        tree.Blackboard = Blackboard.Instantiate(Blackboard);
        tree.Current = Current?.Clone() as DialoqueBaseNode;
        return tree;
    }

    private void Traverse(DialoqueBaseNode root)
    {
        DialoqueBaseNode current = root;
        while (current != null)
        {
            current = current.GetNext();
        }
    }
}
