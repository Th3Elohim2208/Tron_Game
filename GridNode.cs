using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron_Game
{
    internal class GridNode
    {
        // Posición del nodo en el grid
        public int X { get; set; }
        public int Y { get; set; }

        // Referencias a los nodos vecinos
        public GridNode Up { get; set; }
        public GridNode Down { get; set; }
        public GridNode Left { get; set; }
        public GridNode Right { get; set; }

        // Constructor
        public GridNode(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
