using UnityEngine;

public class Dialoque : MonoBehaviour
{
    public DialoqueTree DialoqueTree;

    [SerializeField, ReadOnlyProperty]
    private DialoqueTree _dialogueTreeInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (DialoqueTree != null)
        {
            _dialogueTreeInstance = DialoqueTree.Clone() as DialoqueTree;
            _dialogueTreeInstance.Initialize(gameObject);
        }

    }

    public DialoqueTree GetTree()
    {
        return _dialogueTreeInstance;
    }

    public void Speak()
    {
        DialoqueManager.Instance.SetCurrentTree(_dialogueTreeInstance);
    }
}
