using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron_Game
{
    internal class Grid
    {
        public GridNode[,] Nodes { get; private set; }
        public int Size { get; private set; }

        // Constructor
        public Grid(int size)
        {
            Size = size;
            Nodes = new GridNode[size, size];
            InitializeGrid();
        }

        // Inicializa la malla y establece las referencias entre nodos
        private void InitializeGrid()
        {
            // Crear nodos
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Nodes[x, y] = new GridNode(x, y);
                }
            }

            // Establecer referencias entre nodos
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (x > 0) Nodes[x, y].Left = Nodes[x - 1, y];
                    if (x < Size - 1) Nodes[x, y].Right = Nodes[x + 1, y];
                    if (y > 0) Nodes[x, y].Up = Nodes[x, y - 1];
                    if (y < Size - 1) Nodes[x, y].Down = Nodes[x, y + 1];
                }
            }
        }
    }
}
