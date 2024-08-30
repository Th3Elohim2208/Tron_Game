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

        public Moto(int xInicial, int yInicial)
        {
            X = xInicial;
            Y = yInicial;
            Velocidad = new Random().Next(1, 11); // Velocidad aleatoria entre 1 y 10
            Combustible = 100; // Combustible inicial

            deltaX = 1;
            deltaY = 0;

            Estela = new LinkedList<NodoEstela>();

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
        public void Mover(int dx, int dy)
        {
            // Actualizar la posición de la moto
            X += dx;
            Y += dy;

            // Crear un nuevo nodo al frente de la lista
            NodoEstela nuevoNodo = new NodoEstela(X, Y);
            nuevoNodo.Siguiente = Cabeza;
            Cabeza = nuevoNodo;

            // Actualizar la estela
            Estela.AddFirst(new NodoEstela(X, Y));

            // Eliminar el nodo más antiguo si la estela es más larga de lo que debería ser
            if (Estela.Count > 4)
            {
                Estela.RemoveLast();
            }

            // Consumir combustible
            Combustible -= Velocidad / 5;

           
        }
    }
}
