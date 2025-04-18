using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class NodeView<T> : Node where T : BaseNode
{
    public T Node { get; private set; }
    public List<Port> Inputs { get; private set; } = new List<Port>();
    public List<Port> Outputs { get; private set; } = new List<Port>();

    public UnityAction<T> OnSelectedEvent;

    public NodeView(T node, string title)
    {
        this.Node = node;
        this.title = title;
        this.viewDataKey = node.ID.ID;


        this.style.left = node.Position.x;
        this.style.top = Node.Position.y;

        InitilaizePorts();
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        this.Node.Position = newPos.min;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnSelectedEvent?.Invoke(Node);
    }

    private void InitilaizePorts()
    {
        var type = Node.GetType();

        var input_attributes = type.GetAttributes<InputPinAttribute>(true);
        var output_attributes = type.GetAttributes<OutputPinAttribute>(true);

        foreach (var attr in input_attributes)
        {
            var port = InstantiatePort(attr.Orientation, Direction.Input, attr.Capacity, attr.InputType);
            inputContainer.Add(port);

            Inputs.Add(port);
        }

        foreach (var attr in output_attributes)
        {
            var port = InstantiatePort(attr.Orientation, Direction.Output, attr.Capacity, attr.InputType);
            outputContainer.Add(port);

            Outputs.Add(port);
        }
    }
}
