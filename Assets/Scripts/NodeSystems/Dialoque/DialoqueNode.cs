using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]
public struct DialoqueInfo
{
    public string Speaker;

    [TextArea]
    public string Message;

    [AssetReference]
    public Sprite Sprite;

    public OnDialoqueExecutedEvent OnExecuted;

}

[System.Serializable]
public class OnDialoqueExecutedEvent : UnityEvent<DialoqueNode> { }

[CreateAssetMenu(fileName = "Dialoque", menuName = "Dialoque/DialoqueNode")]
public class DialoqueNode : DialoqueBaseNode
{
    public DialoqueInfo Info;


    protected override void OnExecute()
    {
        Debug.Log($"Speaker: {Info.Speaker} - {Info.Message}");
        Info.OnExecuted.Invoke(this);
    }

    public override VisualElement CreateInspectorGUI()
    {
        PropertyField field = new PropertyField();
        field.bindingPath = "Info";

        VisualElement visualElement = new VisualElement();
        visualElement.Bind(new UnityEditor.SerializedObject(this));
        visualElement.Add(field);

        return visualElement;
    }
}
