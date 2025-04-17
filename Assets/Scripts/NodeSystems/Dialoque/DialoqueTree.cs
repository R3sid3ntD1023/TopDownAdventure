
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnDialoqueTreeFinishedEvent : UnityEvent<DialoqueTree> { }

[CreateAssetMenu(fileName = "DialoqueTree", menuName = "Dialoque/Tree")]
public class DialoqueTree : NodeTree<DialoqueBaseNode, DialoqueBaseNode>
{
    public OnDialoqueTreeFinishedEvent OnDialoqueTreeFinished;

    public OnDialoqueExecutedEvent OnDialoqueExecuted;

    public void Init()
    {
        Traverse(Current);
    }

    public override void Execute()
    {
        if (Current == null)
        {
            OnDialoqueTreeFinished.Invoke(this);
            return;
        }

        Current.Execute();
        Current = Current.GetNext() as DialoqueBaseNode;
    }

    public override object Clone()
    {
        var tree = Instantiate(this);
        tree.Current = Current.Clone() as DialoqueBaseNode;
        return tree;
    }

    public void Traverse(DialoqueBaseNode root)
    {
        DialoqueBaseNode current = root;
        while (current != null)
        {
            current = current.GetNext();
            if (current is DialoqueNode _dialoque && _dialoque != null)
            {
                _dialoque.OnExecuted.AddListener(OnDialoqueChanged);
            }
        }
    }

    private void OnDialoqueChanged(DialoqueNode node)
    {
        OnDialoqueExecuted.Invoke(node);
    }
}
