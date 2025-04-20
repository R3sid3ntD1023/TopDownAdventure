using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace NodeSystem
{
    public class PortView : Port
    {
        public int PortId { get; private set; }

        public UnityAction<Node, Node, int, int> OnPortConnected;

        public UnityAction<Node, Node, int, int> OnPortDisConnected;

        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private GraphViewChange m_GraphViewChange;

            private List<Edge> m_EdgesToCreate;

            private List<GraphElement> m_EdgesToDelete;

            public DefaultEdgeConnectorListener()
            {
                m_EdgesToCreate = new List<Edge>();
                m_EdgesToDelete = new List<GraphElement>();
                m_GraphViewChange.edgesToCreate = m_EdgesToCreate;
            }

            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
            }

            public void OnDrop(GraphView graphView, Edge edge)
            {
                m_EdgesToCreate.Clear();
                m_EdgesToCreate.Add(edge);
                m_EdgesToDelete.Clear();
                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.input.connections)
                    {
                        if (connection != edge)
                        {
                            m_EdgesToDelete.Add(connection);
                        }
                    }
                }

                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (Edge connection2 in edge.output.connections)
                    {
                        if (connection2 != edge)
                        {
                            m_EdgesToDelete.Add(connection2);
                        }
                    }
                }

                if (m_EdgesToDelete.Count > 0)
                {
                    graphView.DeleteElements(m_EdgesToDelete);
                }

                List<Edge> edgesToCreate = m_EdgesToCreate;
                if (graphView.graphViewChanged != null)
                {
                    edgesToCreate = graphView.graphViewChanged(m_GraphViewChange).edgesToCreate;
                }

                foreach (Edge item in edgesToCreate)
                {
                    graphView.AddElement(item);
                    edge.input.Connect(item);
                    edge.output.Connect(item);
                }
            }
        }


        public PortView(int index, Orientation orientation, Direction direction,
            Capacity capacity, Type type)
            : base(orientation, direction, capacity, type)
        {
            PortId = index;


            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            m_EdgeConnector = new EdgeConnector<Edge>(listener);
            this.AddManipulator(m_EdgeConnector);
        }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);

            OnPortConnected?.Invoke(this.node, edge.input.node, PortId, (edge.input as PortView).PortId);

        }

        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);

            OnPortDisConnected?.Invoke(this.node, edge.input.node, PortId, (edge.input as PortView).PortId);
        }

        public override void DisconnectAll()
        {
            base.DisconnectAll();

            foreach (var edge in connections)
            {
                OnPortDisConnected?.Invoke(this.node, edge.input.node, PortId, (edge.input as PortView).PortId);
            }
        }
    }
}
