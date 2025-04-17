
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnDialoqueTreeFinishedEvent : UnityEvent<DialoqueTree> { }

[CreateAssetMenu(fileName = "DialoqueTree", menuName = "Dialoque/Tree")]
public class DialoqueTree : NodeTree<DialoqueBaseNode, DialoqueBaseNode>
{
    public OnDialoqueTreeFinishedEvent OnDialoqueTreeFinished;

    public UnityAction<DialoqueNode> OnDialoqueExecuted;

    private bool _Started = false;

    public override void Execute()
    {
        if (!_Started)
        {
            Traverse(Current);
            _Started = true;
        }

        if (Current == null)
        {
            OnDialoqueTreeFinished.Invoke(this);
            return;
        }

        Current.Execute();
        Current = Current.GetNext();
    }

    public override object Clone()
    {
        var tree = Instantiate(this);
        tree.Current = Current.Clone() as DialoqueBaseNode;
        return tree;
    }

    private void Traverse(DialoqueBaseNode root)
    {
        DialoqueBaseNode current = root;
        while (current != null)
        {
            if (current is DialoqueNode _dialoque && _dialoque != null)
            {
                _dialoque.OnExecuted.AddListener(OnDialoqueChanged);
            }
            current = current.GetNext();
        }
    }

    private void OnDialoqueChanged(DialoqueNode node)
    {
        OnDialoqueExecuted.Invoke(node);
    }
}
