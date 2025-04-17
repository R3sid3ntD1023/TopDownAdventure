using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialoqueInfo
{
    public string Speaker;

    [TextArea]
    public string Message;

    public Sprite Sprite;
}

[System.Serializable]
public class OnDialoqueExecutedEvent : UnityEvent<DialoqueNode> { }

[CreateAssetMenu(fileName = "Dialoque", menuName = "Dialoque/DialoqueNode")]
public class DialoqueNode : DialoqueBaseNode
{
    public DialoqueInfo Info;

    public OnDialoqueExecutedEvent OnExecuted;

    protected override void OnExecute()
    {
        Debug.Log($"Speaker: {Info.Speaker} - {Info.Message}");
        OnExecuted.Invoke(this);
    }
}
