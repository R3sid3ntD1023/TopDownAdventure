using UnityEngine;

public class Dialoque : MonoBehaviour
{
    public DialoqueTree DialoqueTree;

    [SerializeField, ReadOnlyProperty]
    private DialoqueTree _dialogueTreeInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DialoqueTree != null)
        {
            _dialogueTreeInstance = DialoqueTree.Clone() as DialoqueTree;
        }

    }

    public void Speak()
    {
        DialoqueManager.Instance.SetCurrentTree(_dialogueTreeInstance);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Speak();
        }
    }
}
