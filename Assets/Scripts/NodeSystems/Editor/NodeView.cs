using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace NodeSystem
{
    public class NodeView : Node
    {
        public BaseNode Node { get; private set; }
        public List<Port> Inputs { get; private set; } = new List<Port>();
        public List<Port> Outputs { get; private set; } = new List<Port>();

        public UnityAction<BaseNode> OnSelectedEvent;

        public UnityAction<BaseNode> OnSetAsRoot;

        public UnityAction<Node, Node, int, int> OnPortConnectedEvent;

        public UnityAction<Node, Node, int, int> OnPortDisConnectedEvent;

        public NodeView(BaseNode node, string title)
        {
            Initialize(node, title);
        }

        public NodeView(BaseNode node, string title, string uiFile)
            : base(uiFile)
        {
            Initialize(node, title);
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

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            if (evt.target == this)
            {
                evt.menu.AppendAction("Set Root", a => { OnSetAsRoot?.Invoke(Node); });
            }
        }

        private void Initialize(BaseNode node, string title)
        {
            this.Node = node;
            this.title = title;
            this.viewDataKey = node.ID;
            this.dataSource = node;

            this.style.left = node.Position.x;
            this.style.top = node.Position.y;

            var content_element = this.Q<VisualElement>("content");
            var content = node.CreateInspectorGUI();
            if (content != null && content_element != null)
            {
                content.Bind(new SerializedObject(node));
                content_element.Add(content);
            }


            InitilaizePorts();
        }

        private void InitilaizePorts()
        {
            var type = Node.GetType();

            var input_attributes = type.GetCustomAttributes<InputPinAttribute>(true);
            var output_attributes = type.GetCustomAttributes<OutputPinAttribute>(true);

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

        public void SetPortCallbacks()
        {
            foreach (var port in Inputs)
            {
                (port as PortView).OnPortConnected += OnPortConnected;
                (port as PortView).OnPortDisConnected += OnPortDisConnected;
            }

            foreach (var port in Outputs)
            {
                (port as PortView).OnPortConnected += OnPortConnected;
                (port as PortView).OnPortDisConnected += OnPortDisConnected;
            }
        }

        public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
        {
            int index = direction == Direction.Input ? Inputs.Count : Outputs.Count;
            var port = new PortView(index, orientation, direction, capacity, type);
            return port;
        }

        private void OnPortDisConnected(Node arg0, Node arg1, int arg2, int arg3)
        {
            OnPortDisConnectedEvent?.Invoke(arg0, arg1, arg2, arg3);
        }

        private void OnPortConnected(Node parent, Node child, int input, int output)
        {
            OnPortConnectedEvent?.Invoke(parent, child, input, output);
        }
    }
}