using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace NodeSystem
{
    public partial class BaseGraphView<TSupportedNode, TNodeView> : GraphView where TSupportedNode : BaseNode where TNodeView : NodeView
    {
        public UnityAction<BaseNode> OnNodeSelectedEvent;

        public NodeTree m_Tree;

        private NodeSearchWindow m_SearchWindow;

        public BaseGraphView(StyleSheet styleSheet = null)
        {

            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            CreateMiniMap();

            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);

            }

            AddSearchWindow();
        }

        private void AddSearchWindow()
        {
            if (m_SearchWindow == null)
            {

                m_SearchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
                m_SearchWindow.Initialize(typeof(TSupportedNode));
                m_SearchWindow.OnCreateNodeEvent = CreateNode;
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), m_SearchWindow);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);

            if (evt.target == this)
            {
                var types = TypeCache.GetTypesDerivedFrom<TSupportedNode>().Where(t => !t.IsAbstract);
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, a.eventInfo.localMousePosition));
                }

                evt.menu.AppendAction("Group", a => CreateGroup(a.eventInfo.localMousePosition));
            }
        }

        public void PopulateGraph(NodeTree tree)
        {
            m_Tree = tree;

            graphViewChanged -= OnGraphChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphChanged;

            //create views
            m_Tree.Nodes.ForEach(n => CreateNodeView(n as TSupportedNode));

            //create edges

            foreach (var info in m_Tree.NodeConnections)
            {
                var parent = GetNodeByGuid(info.ParentID) as TNodeView;
                var child = GetNodeByGuid(info.ChildID) as TNodeView;

                Edge edge = parent.Outputs[info.OuputIndex].ConnectTo(child.Inputs[info.InputIndex]);
                AddElement(edge);
            }

            foreach (var node in nodes)
            {
                (node as TNodeView).SetPortCallbacks();
            }
        }

        private void CreateMiniMap()
        {
            var background_color = new StyleColor(new Color32(29, 29, 29, 255));
            var border_color = new StyleColor(new Color32(60, 60, 60, 255));

            MiniMap miniMap = new MiniMap { anchored = true };
            miniMap.SetPosition(new Rect(0, 0, 200, 100));
            miniMap.style.backgroundColor = background_color;
            miniMap.style.borderLeftColor = border_color;
            miniMap.style.borderRightColor = border_color;
            miniMap.style.borderTopColor = border_color;
            miniMap.style.borderBottomColor = border_color;

            Add(miniMap);
        }

        private void CreateGroup(Vector2 position)
        {
            Group group = new Group { title = "Group" };
            group.SetPosition(new Rect(position, Vector2.zero));

            AddElement(group);
        }

        public void CreateNode(System.Type type, Vector2 pos)
        {
            TSupportedNode node = m_Tree.CreateNode(type, pos) as TSupportedNode;
            CreateNodeView(node);
        }

        private TNodeView FindNodeView(TSupportedNode node)
        {
            return GetNodeByGuid(node.ID) as TNodeView;
        }

        private void CreateNodeView(TSupportedNode node)
        {
            if (node == null)
                return;

            var view = Activator.CreateInstance(typeof(TNodeView), node, node.name) as TNodeView;
            view.OnSelectedEvent += OnNodeSelectedEvent;
            view.OnSetAsRoot += SetRootNode;
            view.OnPortConnectedEvent += OnPortConnected;
            view.OnPortDisConnectedEvent += OnPortDisConnected;
            AddElement(view);
        }

        private void OnPortDisConnected(Node parent, Node child, int input, int output)
        {
            var info = m_Tree.NodeConnections.FirstOrDefault(x =>
             x.ParentID == parent.viewDataKey &&
             x.ChildID == child.viewDataKey &&
             x.InputIndex == input &&
             x.OuputIndex == output);

            m_Tree.NodeConnections.Remove(info);
        }

        private void OnPortConnected(Node parent, Node child, int input, int output)
        {
            m_Tree.NodeConnections.Add(new ConnectionInfo
            {
                ParentID = parent.viewDataKey,
                ChildID = child.viewDataKey,
                InputIndex = input,
                OuputIndex = output
            }
            );
        }

        private GraphViewChange OnGraphChanged(GraphViewChange changed)
        {
            if (changed.elementsToRemove != null)
            {
                changed.elementsToRemove.ForEach(e =>
                {
                    var view = e as TNodeView;
                    if (view != null)
                    {
                        m_Tree.RemoveNode(view.Node);
                    }

                    var edge = e as Edge;
                    if (edge != null)
                    {
                        TNodeView parent = edge.output.node as TNodeView;
                        TNodeView child = edge.input.node as TNodeView;

                        m_Tree.RemoveChild(parent.Node, child.Node);
                    }
                });
            }

            if (changed.edgesToCreate != null)
            {
                changed.edgesToCreate.ForEach(edge =>
                {
                    TNodeView parent = edge.output.node as TNodeView;
                    TNodeView child = edge.input.node as TNodeView;

                    m_Tree.AddChild(parent.Node, child.Node);
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

        private void SetRootNode(BaseNode node)
        {
            if (m_Tree != null)
                m_Tree.Current = node;
        }

        public void UpdateGraph()
        {
            nodes.ForEach(n =>
            {
                var node = n as TNodeView;
                if (node != null)
                {
                    node.UpdatNodeState();
                }
            });
        }
    }
}
