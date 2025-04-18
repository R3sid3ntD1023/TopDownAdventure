using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[UxmlElement]
public partial class DialoqueTreeView : GraphView
{
    public UnityAction<DialoqueBaseNode> OnNodeSelectedEvent;

    private DialoqueTree m_DialoqueTree;

    public DialoqueTreeView()
    {

        var stylesheet = Resources.Load<DialoqueSettings>("DialoqueSettings");

        styleSheets.Add(stylesheet.StyleSheet);

        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        if (evt.target == this)
        {
            var types = TypeCache.GetTypesDerivedFrom<DialoqueBaseNode>().Where(t => !t.IsAbstract);
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, a.eventInfo.localMousePosition));
            }
        }
    }

    public void PopulateGraph(DialoqueTree tree)
    {
        this.m_DialoqueTree = tree;

        graphViewChanged -= OnGraphChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphChanged;

        //create views
        m_DialoqueTree.Nodes.ForEach(n => CreateNodeView(n));

        //create edges
        m_DialoqueTree.Nodes.ForEach(n =>
        {
            if (n as IHaveChildrenInterface<BaseNode> is var _node)
            {
                var parent_view = FindNodeView(n);
                var children = _node.GetChildren();
                int i = 0;
                children.ForEach(c =>
                {
                    if (c != null)
                    {
                        var child_view = FindNodeView(c);
                    }
                });
            }
        });

    }

    public void CreateNode(Type type, Vector2 pos)
    {
        DialoqueBaseNode node = m_DialoqueTree.CreateNode(type, pos);
        CreateNodeView(node);
    }

    private DialoqueNodeView FindNodeView(BaseNode node)
    {
        return GetNodeByGuid(node.ID.ID) as DialoqueNodeView;
    }

    private void CreateNodeView(DialoqueBaseNode node)
    {
        if (node == null)
            return;

        var view = new DialoqueNodeView(node, node.name);
        view.OnSelectedEvent += OnNodeSelectedEvent;
        view.OnSetAsRoot += SetRootNode;
        AddElement(view);
    }

    private GraphViewChange OnGraphChanged(GraphViewChange changed)
    {
        if (changed.elementsToRemove != null)
        {
            changed.elementsToRemove.ForEach(e =>
            {
                var view = e as DialoqueNodeView;
                if (view != null)
                {
                    m_DialoqueTree.RemoveNode(view.Node);
                }

                var edge = e as Edge;
                if (edge != null)
                {
                    DialoqueNodeView parent = edge.output.node as DialoqueNodeView;
                    DialoqueNodeView child = edge.input.node as DialoqueNodeView;

                    m_DialoqueTree.RemoveChild(parent.Node, child.Node);
                }
            });
        }

        if (changed.edgesToCreate != null)
        {
            changed.edgesToCreate.ForEach(e =>
            {
                DialoqueNodeView parent = e.output.node as DialoqueNodeView;
                DialoqueNodeView child = e.input.node as DialoqueNodeView;

                m_DialoqueTree.AddChild(parent.Node, child.Node);
            });
        }
        return changed;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(e =>
        e.direction != startPort.direction &&
        e.node != startPort.node &&
        e.portType.IsAssignableFrom(startPort.portType)).ToList();
    }

    private void SetRootNode(DialoqueBaseNode node)
    {
        if (m_DialoqueTree != null)
            m_DialoqueTree.Current = node;
    }
}

[UxmlElement]
public partial class DialoqueNodeList : NodeListView<DialoqueBaseNode>
{

}

public partial class DialoqueNodeView : NodeView<DialoqueBaseNode>
{

    public DialoqueNodeView(DialoqueBaseNode node, string title)
        : base(node, title, AssetDatabase.GetAssetPath(DialoqueSettings.GetOrCreateSettings().NodeElementUxml))
    {
    }
}

public partial class DialogueInspectorView : InspectorView<DialoqueBaseNode> { }
