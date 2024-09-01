using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tron_Game
{
    internal class Moto
    {
        public LinkedList<NodoEstela> Estela { get; private set; } // Propiedad pública para la estela
        public NodoEstela Cabeza { get; private set; } // Inicio de la lista enlazada (estela)
        public int Velocidad { get; private set; } // Valor aleatorio entre 1 y 10
        public int TamañoEstela { get; private set; }
        public int Combustible { get; private set; } // Valor entre 0 y 100
        public int X { get; private set; } // Posición X de la moto
        public int Y { get; private set; } // Posición Y de la moto
        public int deltaX; // Dirección en el eje X
        public int deltaY; // Dirección en el eje Y
        private Queue<Item> colaDeItems; // Cola para almacenar los ítems
        private Form1 form;
        private int celdasRecorridas; // Contador de celdas recorridas


        // Nodo interno para representar la estela de la moto (lista enlazada simple)
        public class NodoEstela
        {
            public int X { get; set; }
            public int Y { get; set; }
            public NodoEstela Siguiente { get; set; }

            public NodoEstela(int x, int y)
            {
                X = x;
                Y = y;
                Siguiente = null;
            }
        }


        public Moto(int xInicial, int yInicial, Form1 formInstance)
        {
            X = xInicial;
            Y = yInicial;
            this.form = formInstance;
            Velocidad = new Random().Next(1, 11); // Velocidad aleatoria entre 1 y 10
            Combustible = 100; // Combustible inicial
            celdasRecorridas = 0; // Inicializa el contador de celdas recorridas

            deltaX = 1;
            deltaY = 0;

            Estela = new LinkedList<NodoEstela>();

            colaDeItems = new Queue<Item>();

            // Inicializar la estela con 3 posiciones
            for (int i = 0; i < 3; i++)
            {
                var nodo = new NodoEstela(X, Y); // Inicia en la posición actual de la moto
                Estela.AddLast(nodo);
                if (i == 0) Cabeza = nodo; // El primer nodo se convierte en la cabeza
            }
        }



        public void CambiarDireccion(int nuevoDeltaX, int nuevoDeltaY)
        {
            // Evitar que la moto se mueva hacia atrás sobre sí misma
            if (deltaX + nuevoDeltaX != 0 || deltaY + nuevoDeltaY != 0)
            {
                deltaX = nuevoDeltaX;
                deltaY = nuevoDeltaY;
            }
        }



        public bool VerificarColision(int gridSize)
        {
            // Verificar si la moto se sale de los límites del grid
            if (X < 0 || Y < 0 || X >= gridSize || Y >= gridSize)
            {
                return true;
            }

            // Verificar si la moto choca con su propia estela
            NodoEstela actual = Cabeza.Siguiente; // La cabeza no se verifica contra sí misma
            while (actual != null)
            {
                if (actual.X == X && actual.Y == Y)
                {
                    return true;
                }
                actual = actual.Siguiente;
            }

            return false;
        }



        // Mover la moto en la dirección indicada
        public void Mover(int dx, int dy, Grid gameGrid)
        {
            // Actualizar la posición de la moto
            X += dx;
            Y += dy;


            // Verificar si hay un ítem en la nueva posición
            GridNode celdaActual = gameGrid.GetCelda(X, Y);
            if (celdaActual != null && celdaActual.Item != null)
            {
                RecogerItem(celdaActual.Item); // Recoger el ítem
                celdaActual.Item = null; // Limpiar el ítem de la celda
            }


            // Verificar colisión antes de actualizar la estela
            if (VerificarColision(gameGrid.Size))
            {
                form.FinDelJuego("¡Colisión! Fin del juego.");
                return;
            }


            // Crear un nuevo nodo al frente de la lista
            NodoEstela nuevoNodo = new NodoEstela(X, Y);
            nuevoNodo.Siguiente = Cabeza;
            Cabeza = nuevoNodo;

            // Actualizar la estela
            Estela.AddFirst(nuevoNodo);

            // Eliminar el nodo más antiguo si la estela es más larga de lo que debería ser
            if (Estela.Count > TamañoEstela)
            {
                Estela.RemoveLast();
            }

            // Consumir combustible

            // Incrementar el contador de celdas recorridas
            celdasRecorridas++;

            // Reducir el combustible cada 5 celdas recorridas
            if (celdasRecorridas >= 5)
            {
                Combustible -= 1;
                celdasRecorridas = 0; // Reiniciar el contador
            }


        }


        public void RecogerItem(Item item)
        {
            colaDeItems.Enqueue(item); // Añadir el ítem a la cola
            ProcesarItems(); // Iniciar el procesamiento de ítems
        }



        private async void ProcesarItems()
        {
            while (colaDeItems.Count > 0)
            {
                Item item = colaDeItems.Dequeue();

                switch (item.Tipo)
                {
                    case Item.TipoItem.Combustible:
                        if (Combustible < 100)
                        {
                            Combustible += item.Valor;
                            if (Combustible > 100) Combustible = 100;
                        }
                        else
                        {
                            colaDeItems.Enqueue(item); // Reinsertar si el combustible está lleno
                        }
                        break;

                    case Item.TipoItem.CrecimientoEstela:
                        TamañoEstela += item.Valor;
                        break;

                    case Item.TipoItem.Bomba:

                        form.FinDelJuego("¡Exploto! Fin del juego.");
                        break;
                }

                await Task.Delay(1000); // Delay de 1 segundo
            }
        }
    }

}
