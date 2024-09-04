using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;

namespace Tron_Game
{
    public partial class Form1 : Form
    {
        private Grid? gameGrid;// La malla del juego
        public System.Windows.Forms.Timer? timer;//temporizador para el movimiento de la moto
        private System.Windows.Forms.Timer? gameTimer;//temporizador del tiempo de juego
        private Moto playerMoto; // Variable para la moto del jugador
        private List<Moto> listaDeMotos; // Lista para contener todas las motos en el juego
        private int gridSize = 100; // Tamaño del grid
        private int cellSize = 10; // Tamaño de cada celda en píxeles (puedes ajustar esto según necesites)
        private bool juegoTerminado = false; // Indica si el juego ha terminado
        private Label lblCombustible; // Etiqueta para mostrar el valor del combustible
        private Label lblVelocidad; // Etiqueta para mostrar el valor de la velocidad
        private Label lblLargoEstela; // Etiqueta para mostrar el valor del largo de la estela
        private Label lblInfoItems; // Etiqueta para mostrar la información de los ítems
        private Brush motoBrush = Brushes.Green; // Color inicial de la moto
        private Label lblPoderActual; // Etiqueta para mostrar el poder actual

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            GenerarItemsYPoderesAleatorios();
        }


        private void InitializeGame()
        {
            int xInicial = 10;
            int yInicial = 10;


            //Inicializa los campos
            gameGrid = new Grid(gridSize);
            listaDeMotos = new List<Moto>();
            playerMoto = new Moto(xInicial, yInicial, this);
            listaDeMotos.Add(playerMoto);


            // Ajustar el tamaño del formulario
            this.ClientSize = new Size(gridSize * cellSize + 190, gridSize * cellSize);


            // Crear y configurar la etiqueta de velocidad
            lblVelocidad = new Label();
            lblVelocidad.Location = new Point(gridSize * cellSize + 10, 50); // Posicionar la etiqueta fuera de la malla
            lblVelocidad.Size = new Size(160, 20);
            lblVelocidad.Text = "Velocidad: " + playerMoto.Velocidad;
            this.Controls.Add(lblVelocidad); // Añadir la etiqueta al formulario


            // Crear y configurar la etiqueta de largo de la estela
            lblLargoEstela = new Label();
            lblLargoEstela.Location = new Point(gridSize * cellSize + 10, 80); // Posicionar la etiqueta fuera de la malla
            lblLargoEstela.Size = new Size(160, 20);
            lblLargoEstela.Text = "Largo de Estela: " + playerMoto.Estela.Count;
            this.Controls.Add(lblLargoEstela); // Añadir la etiqueta al formulario


            // Crear y configurar la etiqueta de combustible
            lblCombustible = new Label();
            lblCombustible.Location = new Point(gridSize * cellSize + 10, 20); // Posicionar la etiqueta fuera de la malla
            lblCombustible.Size = new Size(160, 20);
            lblCombustible.Text = "Combustible: ";
            this.Controls.Add(lblCombustible); // Añadir la etiqueta al formulario


            // Crear y configurar la etiqueta de información de ítems
            lblInfoItems = new Label();
            lblInfoItems.Location = new Point(gridSize * cellSize + 10, 110); // Posicionar la etiqueta fuera de la malla, debajo de las otras etiquetas
            lblInfoItems.Size = new Size(180, 140);
            lblInfoItems.Text = "Color Negro: Bomba\n\nColor Morado: Alarga Estela\n\nColor Café: Combustible\n\nColor Azul: Escudo\n\nColor amarillo: Hiper velocidad";
            this.Controls.Add(lblInfoItems); // Añadir la etiqueta al formulario

            // Crear y configurar la etiqueta del poder actual
            lblPoderActual = new Label();
            lblPoderActual.Location = new Point(1010, 850); // Ajusta la ubicación 
            lblPoderActual.Size = new Size(200, 30); // Ajusta el tamaño 
            lblPoderActual.Text = "Poder Actual: Ninguno";
            this.Controls.Add(lblPoderActual);


            //configura un temporizador para el tiempo de juego
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 500; // Intervalo de 700 ms
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Start();


            //configura un temporizador para el movimiento de la moto
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000 / playerMoto.Velocidad;
            timer.Tick += MoverMoto;
            timer.Start();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Pen gridPen = new Pen(Color.LightGray); // Color de las líneas de la malla
            Brush estelaBrush = Brushes.LightGreen; // Color de la estela
            int cellSize = 10; // Tamaño de cada celda de la malla
            int gridSize = gameGrid?.Size ?? 0; // Tamaño de la malla

            // Dibujar la malla
            for (int i = 0; i <= gridSize; i++)
            {
                g.DrawLine(gridPen, i * cellSize, 0, i * cellSize, gridSize * cellSize); // Vertical
                g.DrawLine(gridPen, 0, i * cellSize, gridSize * cellSize, i * cellSize); // Horizontal
            }

            // Dibujar los ítems en la red
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    GridNode Celda = gameGrid.GetCelda(x, y);
                    if (Celda.Item != null)
                    {
                        Brush itemBrush;
                        switch (Celda.Item.Tipo)
                        {
                            case Item.TipoItem.Combustible:
                                itemBrush = Brushes.Brown;//color cafe el combustible
                                break;
                            case Item.TipoItem.CrecimientoEstela:
                                itemBrush = Brushes.Purple;//color morado el crecimiento de la estela
                                break;
                            case Item.TipoItem.Bomba:
                                itemBrush = Brushes.Black;//color negro la bomba
                                break;
                            default:
                                itemBrush = Brushes.White;
                                break;
                        }
                        g.FillRectangle(itemBrush, x * cellSize, y * cellSize, cellSize, cellSize);
                    }

                    // Dibujar los poderes en la red
                    if (Celda.Poder != null)
                    {
                        Brush poderBrush;
                        switch (Celda.Poder.Tipo)
                        {
                            case Power.TipoPower.Escudo:
                                poderBrush = Brushes.Blue; // Color azul para el Escudo
                                break;
                            case Power.TipoPower.HiperVelocidad:
                                poderBrush = Brushes.Yellow; // Color amarillo para la Hiper Velocidad
                                break;
                            default:
                                poderBrush = Brushes.White;
                                break;
                        }
                        g.FillRectangle(poderBrush, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }


            // Dibujar la estela
            foreach (var nodo in playerMoto.Estela)
            {
                g.FillRectangle(playerMoto.EstelaBrush, nodo.X * cellSize, nodo.Y * cellSize, cellSize, cellSize);
            }

            // Dibujar la moto
            g.FillRectangle(playerMoto.MotoBrush, playerMoto.X * cellSize, playerMoto.Y * cellSize, cellSize, cellSize);
        }



        private void DibujarGrid()
        {
            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(Color.Black); // Limpiar el panel antes de redibujar

                // Dibujar las estelas de las motos
                foreach (Moto moto in listaDeMotos)
                {
                    foreach (var nodo in moto.Estela)
                    {
                        g.FillRectangle(Brushes.Red, nodo.X * cellSize, nodo.Y * cellSize, cellSize, cellSize);
                    }

                    // Dibujar la moto
                    g.FillRectangle(Brushes.Red, moto.X * cellSize, moto.Y * cellSize, cellSize, cellSize);
                }
            }
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Si el juego ha terminado, no procesar las teclas de dirección
            if (juegoTerminado)
            {
                return true;// Evitar que las teclas sean procesadas
            }

            switch (keyData)
            {
                case Keys.Up:
                    playerMoto.CambiarDireccion(0, -1); // Mover hacia arriba
                    break;
                case Keys.Down:
                    playerMoto.CambiarDireccion(0, 1); // Mover hacia abajo
                    break;
                case Keys.Left:
                    playerMoto.CambiarDireccion(-1, 0); // Mover hacia la izquierda
                    break;
                case Keys.Right:
                    playerMoto.CambiarDireccion(1, 0); // Mover hacia la derecha
                    break;
                case Keys.D3:
                    // Mover el poder en la pila
                    if (playerMoto.PilaDePoderes.Count > 0)
                    {
                        // Mover el poder del tope al fondo de la pila
                        Power poderActual = playerMoto.PilaDePoderes.Pop();
                        List<Power> poderesRestantes = new List<Power>(playerMoto.PilaDePoderes);
                        poderesRestantes.Add(poderActual);
                        playerMoto.PilaDePoderes.Clear();
                        poderesRestantes.Reverse();
                        foreach (var poder in poderesRestantes)
                        {
                            playerMoto.PilaDePoderes.Push(poder);
                        }

                        // Actualizar la interfaz
                        ActualizarPoder();
                    }
                    break;
                case Keys.D2:
                    // Aplicar el poder en la cima de la pila
                    playerMoto.AplicarPoder();
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }





        private void MoverMoto(object? sender, EventArgs e)
        {
            playerMoto.Mover(playerMoto.deltaX, playerMoto.deltaY,gameGrid!);

            // Verificar si la moto se ha quedado sin combustible
            if (playerMoto.Combustible <= 0 && !juegoTerminado)
            {
                FinDelJuego("¡Sin combustible! Fin del juego.");
            }
            

            Invalidate();
        }


        public void ActualizarPoder()
        {
            if (playerMoto.PilaDePoderes.Count > 0)
            {
                lblPoderActual.Text = "Poder Actual: " + playerMoto.PilaDePoderes.Peek().Tipo.ToString();
            }
            else
            {
                lblPoderActual.Text = "Poder Actual: Ninguno";
            }
        }



        private void gameTimer_Tick(object? sender, EventArgs e)
        {
            playerMoto.Mover(playerMoto.deltaX, playerMoto.deltaY,gameGrid!);

            if (playerMoto.VerificarColision(gridSize))
            {
                FinDelJuego("¡Colisión! Fin del juego.");
            }

            // Actualizar la etiqueta de combustible
            lblCombustible.Text = "Combustible: " + playerMoto.Combustible;
            lblVelocidad.Text = "Velocidad: " + playerMoto.Velocidad;
            lblLargoEstela.Text = "Largo de Estela: " + playerMoto.Estela.Count;

            DibujarGrid();
            Invalidate();
        }




        //Hace que el juego termine
        public void FinDelJuego(string mensaje)
        {
            gameTimer?.Stop(); // Detener el temporizador de juego
            timer?.Stop(); // Detener el temporizador de movimiento
            juegoTerminado = true; // Indicar que el juego ha terminado
            MessageBox.Show(mensaje); // Mostrar el mensaje

            // Si la moto tenía ítems no usados, colocarlos en posiciones aleatorias en la red
            while (playerMoto.colaDeItems.Count > 0)
            {
                Item item = playerMoto.colaDeItems.Dequeue();
                gameGrid.ColocarItem(item);
            }

            // Si la moto tenía poderes no usados, colocarlos en posiciones aleatorias en la red
            while (playerMoto.PilaDePoderes.Count > 0)
            {
                Power poder = playerMoto.PilaDePoderes.Pop();
                gameGrid.ColocarPoder(poder);
            }

            listaDeMotos.Remove(playerMoto); // Eliminar la moto destruida de la lista de motos
        }


        //genera los items que aparencen el el grid
        private void GenerarItemsYPoderesAleatorios()
        {
            Random rand = new Random();

            // Generar ítems en la red con algún intervalo de tiempo
            for (int i = 0; i < 5; i++) // Generar 5 ítems aleatoriamente
            {
                Item.TipoItem tipo = (Item.TipoItem)rand.Next(0, 3); // Combustible, CrecimientoEstela, Bomba

                int valor;
                if (tipo == Item.TipoItem.Combustible)
                {
                    valor = rand.Next(50, 101); // Valor aleatorio entre 50 y 100 para el combustible
                }
                else if (tipo == Item.TipoItem.CrecimientoEstela)
                {
                    valor = rand.Next(1, 11); // Valor aleatorio entre 1 y 10 para el crecimiento de estela
                }
                else
                {
                    valor = rand.Next(10, 51); // Valor aleatorio entre 10 y 50 para otros tipos de ítems
                }

                Item nuevoItem = new Item(tipo, valor);
                gameGrid.ColocarItem(nuevoItem); // Colocar el ítem en la red
            }


            // Generar poderes en la red
            for (int i = 0; i < 4; i++)
            {
                Power.TipoPower tipoPoder = (Power.TipoPower)rand.Next(0, 2);
                int duracion = rand.Next(15, 26); 
                int valor = rand.Next(5, 11); 

                Power nuevoPoder = new Power(tipoPoder, duracion, valor);
                gameGrid.ColocarPoder(nuevoPoder);
            }
        }

   
    }
}
