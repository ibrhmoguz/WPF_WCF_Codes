using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorMixer.Domain
{
    public class Graph
    {
        public int NodeCount { get; set; }
        public Node[] Nodes { get; set; }

        public Graph(int size)
        {
            Nodes = new Node[size];
        }
    }
}
