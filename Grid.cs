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
        private Random random = new Random();

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

        // Coloca un ítem aleatoriamente en el grid
        public void ColocarItem(Item item)
        {
            int x, y;
            GridNode celda;

            do
            {
                x = random.Next(Size);// Tamaño de la grid
                y = random.Next(Size);
                celda = GetCelda(x, y);
            } while (celda != null && celda.Item != null); // Asegura que el nodo no esté ocupado por otro ítem

            if (celda != null)
            {
                celda.Item = item;
            }
        }

        // Verifica si hay un ítem en la posición actual de la moto
        public Item RecogerItem(int x, int y)
        {
            GridNode celda = GetCelda(x, y);
            if (celda != null && celda.Item != null)
            {
                Item itemRecogido = celda.Item;
                celda.Item = null; // Elimina el ítem del nodo
                return itemRecogido;
            }
            return null;
        }


        // Obtener el nodo en una posición específica
        public GridNode GetCelda(int x, int y)
        {
            if (x >= 0 && x < Size && y >= 0 && y < Size)
            {
                return Nodes[x, y];
            }
            return null;
        }


        // Coloca un poder aleatoriamente en el grid
        public void ColocarPoder(Power poder)
        {
            int x, y;
            GridNode celda;

            do
            {
                x = random.Next(0, Size); // Tamaño de la grid
                y = random.Next(0, Size);
                celda = GetCelda(x, y);
            } while (celda != null && celda.Item != null); // Asegurarse de no colocar el poder en una celda ya ocupada

            if (celda != null)
            {
                celda.Poder = poder; // Asignar el poder a la celda seleccionada
            }
        }
    }

}

