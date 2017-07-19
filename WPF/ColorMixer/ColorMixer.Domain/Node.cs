using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace ColorMixer.Domain
{
    public class Node 
    {
        public int Id { get; set; }
        public Color Color { get; set; }
        public List<Node> Connections { get; set; }
    }
}
