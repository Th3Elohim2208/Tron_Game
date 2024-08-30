using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;

namespace Tron_Game
{
    public partial class Form1 : Form
    {
        private Grid? gameGrid;// La malla del juego
        private System.Windows.Forms.Timer? timer;//temporizador para el movimiento de la moto
        private System.Windows.Forms.Timer? gameTimer;//temporizador del tiempo de juego
        private Moto playerMoto; // Variable para la moto del jugador
        private List<Moto> listaDeMotos; // Lista para contener todas las motos en el juego
        private int gridSize = 100; // Tamaño del grid
        private int cellSize = 10; // Tamaño de cada celda en píxeles (puedes ajustar esto según necesites)
        private bool juegoTerminado = false; // Indica si el juego ha terminado

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }


        private void InitializeGame()
        {
            int xInicial = 10;
            int yInicial = 10;


            //Inicializa los campos
            gameGrid = new Grid(gridSize);
            listaDeMotos = new List<Moto>();
            playerMoto = new Moto(xInicial, yInicial);
            listaDeMotos.Add(playerMoto);


            // Ajustar el tamaño del formulario
            this.ClientSize = new Size(gridSize * cellSize, gridSize * cellSize);


            //configura un temporizador para el tiempo de juego
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 700; // Intervalo de 100 ms
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
            Brush motoBrush = Brushes.Blue; // Color de la moto
            Brush estelaBrush = Brushes.Red; // Color de la estela

            // Dibujar la malla
            for (int i = 0; i <= gridSize; i++)
            {
                g.DrawLine(gridPen, i * cellSize, 0, i * cellSize, gridSize * cellSize); // Vertical
                g.DrawLine(gridPen, 0, i * cellSize, gridSize * cellSize, i * cellSize); // Horizontal
            }

            // Dibujar la estela
            foreach (var nodo in playerMoto.Estela)
            {
                g.FillRectangle(estelaBrush, nodo.X * cellSize, nodo.Y * cellSize, cellSize, cellSize);
            }

            // Dibujar la moto
            g.FillRectangle(motoBrush, playerMoto.X * cellSize, playerMoto.Y * cellSize, cellSize, cellSize);
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
                return true; // Evitar que las teclas sean procesadas
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
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void MoverMoto(object? sender, EventArgs e)
        {
            playerMoto.Mover(playerMoto.deltaX, playerMoto.deltaY);

            // Verificar si la moto se ha quedado sin combustible
            if (playerMoto.Combustible <= 0 && !juegoTerminado)
            {
                FinDelJuego("¡Sin combustible! Fin del juego.");
            }

            Invalidate();
        }

        private void gameTimer_Tick(object? sender, EventArgs e)
        {
            playerMoto.Mover(playerMoto.deltaX, playerMoto.deltaY);

            if (playerMoto.VerificarColision(gridSize))
            {
                FinDelJuego("¡Colisión! Fin del juego.");
            }

            DibujarGrid();
            Invalidate();
        }

        private void FinDelJuego(string mensaje)
        {
            gameTimer?.Stop(); // Detener el temporizador de juego
            timer?.Stop(); // Detener el temporizador de movimiento
            juegoTerminado = true; // Indicar que el juego ha terminado
            MessageBox.Show(mensaje); // Mostrar el mensaje
        }
    }
}
