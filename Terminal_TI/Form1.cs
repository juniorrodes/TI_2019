using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Terminal_TI {
    public partial class Form1 : Form {

        private List<Square> Snake = new List<Square>();
        private Square food = new Square();
        int MaxHeight;
        int MaxWidht;

        public Form1() {
            InitializeComponent();

            // inicializa as configurações
            new Setings();
            MaxHeight = pbCanvas.Size.Height / Setings.Height;
            MaxWidht = pbCanvas.Size.Width / Setings.Width;

            // seta a velocidade do jogo  e inicia o timer
            GameTimer.Interval = 1500 / Setings.Speed;
            GameTimer.Tick += UpdateScreen;
            GameTimer.Start();

            StartGame();

        }

        private void StartGame() {
            new Setings();

            lbGameOver.Visible = false;
            Snake.Clear();
            Square head = new Square();
            head.x = 10;
            head.y = 5;
            Snake.Add(head);
            GenerateFood();
        }

        private void GenerateFood() {
            Random random = new Random();

            food = new Square();
            food.x = random.Next(0, MaxWidht);
            food.y = random.Next(0, MaxHeight);
            for(int i = 0; i < Snake.Count; i++) {
                if((food.x == Snake[i].x) && (food.y == Snake[i].y)) {
                    food.x = random.Next(0, MaxWidht);
                    food.y = random.Next(0, MaxHeight);
                }
            }
        }

        private void UpdateScreen(object sender, EventArgs e) {
            if (Setings.GameOver) {
                if (Input.KeyPressed(Keys.Enter)) {
                    StartGame();
                }
            }else {
                if(Snake.Count - 1 == 0) {
                    if ((Input.KeyPressed(Keys.Right)))
                        Setings.directions = Directions.Right;
                    else if ((Input.KeyPressed(Keys.Left)))
                        Setings.directions = Directions.Left;
                    else if ((Input.KeyPressed(Keys.Up)))
                        Setings.directions = Directions.Up;
                    else if ((Input.KeyPressed(Keys.Down)))
                        Setings.directions = Directions.Down;
                }
                if ((Input.KeyPressed(Keys.Right)) && (Setings.directions != Directions.Left))
                    Setings.directions = Directions.Right;
                else if ((Input.KeyPressed(Keys.Left)) && (Setings.directions != Directions.Right))
                    Setings.directions = Directions.Left;
                else if ((Input.KeyPressed(Keys.Up)) && (Setings.directions != Directions.Down))
                    Setings.directions = Directions.Up;
                else if ((Input.KeyPressed(Keys.Down)) && (Setings.directions != Directions.Up))
                    Setings.directions = Directions.Down;

                MovePlayer();
                Score.Text = "Score: " + Setings.Score;
            }
            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e) {

            Graphics canvas = e.Graphics;
            Brush SnakeCollor;
            if (!Setings.GameOver) {
                SnakeCollor = Brushes.ForestGreen;
                for (int i = 0; i < Snake.Count; i++) {
                    canvas.FillRectangle(SnakeCollor, new Rectangle(Snake[i].x * Setings.Width, 
                        Snake[i].y * Setings.Height, Setings.Width, Setings.Height));
                    canvas.FillRectangle(Brushes.Red,
                        new Rectangle(food.x * Setings.Width, food.y * Setings.Height, Setings.Width, Setings.Height));

                }
            }else {
                string GameOver = "Game over \nYour final score is: " + Setings.Score + "\nPress enter to continue.";
                lbGameOver.Text = GameOver;
                lbGameOver.Visible = true;
            }

        }

        private void MovePlayer() {
            for(int i = Snake.Count - 1; i >= 0; i--) {
                if(i == 0) {
                    switch (Setings.directions) {
                        case Directions.Right:
                            Snake[i].x++;
                            break;
                        case Directions.Left:
                            Snake[i].x--;
                            break;
                        case Directions.Down:
                            Snake[i].y++;
                            break;
                        case Directions.Up:
                            Snake[i].y--;
                            break;
                    }
                    //detecta a colisão com as paredes
                    if((Snake[i].x < 0) || (Snake[i].y < 0) || 
                        (Snake[i].x >= MaxWidht) || (Snake[i].y >= MaxHeight)) {
                        Setings.GameOver = true;
                    }

                    //detecta a colisão com o corpo
                    for(int j = 1; j < Snake.Count; j++) {
                        if((Snake[i].x == Snake[j].x) && (Snake[i].y == Snake[j].y)) {
                            Setings.GameOver = true;
                        }
                    }
                    //detecta a colisão com a comida
                    if((Snake[i].x == food.x) && (Snake[i].y == food.y)) {
                        Eat();
                    }

                }else {
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }
            }
        }

        private void Eat() {
            //cria um novo bloco no corpo da cobra
            Square square = new Square();
            square.x = Snake[Snake.Count - 1].x;
            square.y= Snake[Snake.Count - 1].y;
            Snake.Add(square);

            Setings.Score += Setings.Points;
            //gera um novo alimento
            GenerateFood();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}
