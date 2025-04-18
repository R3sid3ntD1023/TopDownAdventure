
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
        if (Current == null && !_Finished)
        {
            OnFinished.Invoke(this);
            _Finished = true;
            return;
        }

        if (!_Started)
        {
            Traverse(Current);
            _Started = true;
        }

        Current.Execute();
        Current = Current.GetNext();
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
