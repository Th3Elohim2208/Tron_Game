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
            int x = random.Next(Size);
            int y = random.Next(Size);

            while (Nodes[x, y].Item != null) // Asegura que el nodo no esté ocupado por otro ítem
            {
                x = random.Next(Size);
                y = random.Next(Size);
            }

            Nodes[x, y].Item = item;
        }

        // Verifica si hay un ítem en la posición actual de la moto
        public Item RecogerItem(int x, int y)
        {
            var nodo = Nodes[x, y];
            if (nodo.Item != null)
            {
                Item itemRecogido = nodo.Item;
                nodo.Item = null; // Elimina el ítem del nodo
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



        // Obtiene todos los ítems restantes en la malla
        public List<Item> ObtenerItemsRestantes()
        {
            List<Item> itemsRestantes = new List<Item>();

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    GridNode celda = GetCelda(x, y); // Usar el método GetCelda

                    if (celda != null && celda.Item != null) // Verifica si el nodo contiene un ítem
                    {
                        itemsRestantes.Add(celda.Item);
                    }
                }
            }

            return itemsRestantes;
        }

    }

}

