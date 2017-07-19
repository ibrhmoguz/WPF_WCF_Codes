using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ColorMixer.Domain;

namespace ColorMixer.Business
{
    public class BusinessHandler
    {
        public Graph graph { get; set; }

        public BusinessHandler(int size)
        {
            this.graph = new Graph(size);
        }

        public Node CreateNode()
        {
            return CreateNode(Color.DodgerBlue);
        }

        public Node CreateNode(Color color)
        {
            var node = new Node()
            {
                Color = color,
                Id = graph.NodeCount,
                Connections = new List<Node>()
            };

            graph.Nodes[graph.NodeCount] = node;
            graph.NodeCount++;
            return node;
        }

        public Node AddColorToNode(Node node, Color color)
        {
            node.Color = color;
            return node;
        }

        public void BindNodes(int source, int destination)
        {
            Node sourceNode = this.graph.Nodes[source];
            Node destinationNode = this.graph.Nodes[destination];
            destinationNode.Color = MergeTwoColors(sourceNode.Color, destinationNode.Color, 0.5f);
            // Update colors of destination node connections
            foreach (Node connectionNode in destinationNode.Connections)
            {
                connectionNode.Color = MergeTwoColors(destinationNode.Color, connectionNode.Color, 0.5f);
               
            }
            sourceNode.Connections.Add(destinationNode);
        }

        public Color MergeTwoColors(Color sourceColor, Color destinationColor, float percent)
        {
            float amountFrom = 1.0f - percent;

            return Color.FromArgb(
            (int)(sourceColor.A * amountFrom + destinationColor.A * percent),
            (int)(sourceColor.R * amountFrom + destinationColor.R * percent),
            (int)(sourceColor.G * amountFrom + destinationColor.G * percent),
            (int)(sourceColor.B * amountFrom + destinationColor.B * percent));
        }

        public Node FindNodeInGraph(int id)
        {
            return this.graph.Nodes[id];
        }

        public bool UnLinkedBefore(int sourceId,int targetId)
        {
            if (this.graph.Nodes[sourceId].Connections.Contains(FindNodeInGraph(targetId)) || this.graph.Nodes[targetId].Connections.Contains(FindNodeInGraph(sourceId)))
                return false;
            else
                return true;

        }
    }
}
