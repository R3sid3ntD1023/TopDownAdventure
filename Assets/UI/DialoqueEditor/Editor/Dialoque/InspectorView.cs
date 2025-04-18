using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Events;
using UnityEngine.UIElements;

public partial class InspectorView<T> : VisualElement where T : BaseNode
{
    Editor m_Editor;


    internal void UpdateSelection(T node)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(m_Editor);
        m_Editor = Editor.CreateEditor(node);
        var element = m_Editor.CreateInspectorGUI();
        element.style.flexGrow = 1;
        element.Bind(new SerializedObject(node));
        Add(element);
    }
}

public class NodeButton : Button
{
    public UnityAction<Type> OnButtonClicked;

    private Type _Type;

    public NodeButton(Type type)
    {
        this._Type = type;

        var label = new Label();
        label.text = type.Name;

        hierarchy.Add(label);

        this.clicked += OnClicked;
    }

    private void OnClicked()
    {
        OnButtonClicked.Invoke(_Type);
    }
}

[UxmlElement]
public partial class NodeListView<T> : VisualElement where T : BaseNode
{
    public UnityAction<Type> OnItemSelected;


    public NodeListView()
    {
        var types = TypeCache.GetTypesDerivedFrom<T>().Where(t => !t.IsAbstract);

        foreach (var type in types)
        {
            hierarchy.Add(GetListItem(type));
        }
    }


    private VisualElement GetListItem(Type type)
    {
        var button = new NodeButton(type);
        button.OnButtonClicked += OnItemClicked;
        return button;
    }

    private void OnItemClicked(Type type)
    {
        OnItemSelected?.Invoke(type);
    }


}
