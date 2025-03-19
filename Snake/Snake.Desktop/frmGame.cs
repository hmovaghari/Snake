using Snake.Desktop.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Settings = Snake.Desktop.Properties.Settings;

namespace Snake.Desktop
{
    public partial class frmGame : Form
    {
        private List<SnakePoint> Snake = new List<SnakePoint>();

        private List<Label> Foods = new List<Label>();

        private Label Food = new Label()
        {
            ForeColor = Color.LightSalmon,
            BackColor = Color.LightSalmon,
            Size = new Size(10, 10),
        };

        private Point Move = new Point(16, 16);
        private AutoDirection AutoDirection = AutoDirection.None;

        private Point GameBoardStatLocation = new Point(6, 2);
        private Point GameBoardEndLocation = new Point(518, 226);

        public frmGame()
        {
            InitializeComponent();
        }

        private void frmGame_Load(object sender, EventArgs e)
        {
            MakeSnake();
            MakeFoods();
            MakeFood();
            AddSaneToGameGround();
            timer.Interval = 100 + Foods.Count - 1;
            timer.Start();
        }

        private void MakeSnake()
        {
            SnakePoint.Clear();
            Snake.Clear();
            for (int i = 0; i < 6; ++i)
            {
                var lastLoaction = Snake.LastOrDefault()?.Location;
                Snake.Add(new SnakePoint()
                {
                    Location = new Point((lastLoaction?.X ?? 150) - Move.X, (lastLoaction?.Y ?? 130)),
                });
            }
        }

        private void MakeFoods()
        {
            Foods.Clear();
            for(int x = GameBoardStatLocation.X; x <= GameBoardEndLocation.X; x += Move.X)
            {
                for (int y = GameBoardStatLocation.Y; y <= GameBoardEndLocation.Y; y += Move.Y)
                {
                    Foods.Add(new Label()
                    {
                        Location = new Point(x, y)
                    });
                }
            }
        }

        private void MakeFood()
        {
            Random rand = new Random();
            do
            {
                var randomIndex = rand.Next(0, Foods.Count);
                var randomFood = Foods[randomIndex];
                bool isFoodSave = true;
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (randomFood.Location.X == Snake[i].Location.X && randomFood.Location.Y == Snake[i].Location.Y)
                    {
                        isFoodSave = false;
                        break;
                    }
                }
                if (isFoodSave)
                {
                    Food.Location = new Point(randomFood.Location.X, randomFood.Location.Y);
                    break;
                }
            } while (true);
        }

        private void AddSaneToGameGround()
        {
            pnlGameGround.Controls.Clear();
            Snake.ForEach(p => pnlGameGround.Controls.Add(p));
            pnlGameGround.Controls.Add(Food);
        }

        private void frmGame_KeyDown(object sender, KeyEventArgs e)
        {
            var lastLocation = new Point()
            {
                X = Snake.Last().Location.X,
                Y = Snake.Last().Location.Y
            };

            MoveSnakeByAutoOrKey(e);

            GameState gameState = GetGameSate();
            if (gameState != GameState.Play)
            {
                ShowGameStatus(gameState);
            }
            else
            {
                IncreaseLengthSnakeIfEatsFood(lastLocation);
            }
        }

        private void MoveSnakeByAutoOrKey(KeyEventArgs e)
        {
            switch (e?.KeyCode)
            {
                case Keys.Left:
                    SnakeMoveLeft();
                    break;
                case Keys.Up:
                    SnakeMoveUp();
                    break;
                case Keys.Right:
                    SnakeMoveRight();
                    break;
                case Keys.Down:
                    SnakeMoveDown();
                    break;
                case Keys.Y:
                    if (GetGameSate() != GameState.Play)
                    {
                        AutoDirection = AutoDirection.None;
                        frmGame_Load(null, null);
                    }
                    break;
                case Keys.N:
                    if (GetGameSate() != GameState.Play)
                    {
                        this.Close();
                    }
                    break;
                default:
                    switch (AutoDirection)
                    {
                        case AutoDirection.Up:
                            SnakeMoveUp();
                            break;
                        case AutoDirection.Left:
                            SnakeMoveLeft();
                            break;
                        case AutoDirection.Down:
                            SnakeMoveDown();
                            break;
                        case AutoDirection.Right:
                            SnakeMoveRight();
                            break;
                    }
                    break;
            }
        }

        private void IncreaseLengthSnakeIfEatsFood(Point lastLocation)
        {
            if (Snake.Any(s => s.Location.X == Food.Location.X && s.Location.Y == Food.Location.Y))
            {
                Snake.Add(new SnakePoint()
                {
                    Location = new Point(lastLocation.X, lastLocation.Y),
                });
                timer.Interval -= 1;
                AddSaneToGameGround();
                MakeFood();
            }
        }

        private void ShowGameStatus(GameState gameState)
        {
            if (gameState == GameState.Win)
            {
                //You Win
                timer.Stop();
                Settings.Default.BestScore = SnakePoint.Score;
                Settings.Default.Save();
                var text = string.Empty;
                text += @" __   __  _______  __   __    _     _  ___   __    _ " + Environment.NewLine;
                text += @"|  | |  ||       ||  | |  |  | | _ | ||   | |  |  | |" + Environment.NewLine;
                text += @"|  |_|  ||   _   ||  | |  |  | || || ||   | |   |_| |" + Environment.NewLine;
                text += @"|       ||  | |  ||  |_|  |  |       ||   | |       |" + Environment.NewLine;
                text += @"|_     _||  |_|  ||       |  |       ||   | |  _    |" + Environment.NewLine;
                text += @"  |   |  |       ||       |  |   _   ||   | | | |   |" + Environment.NewLine;
                text += @"  |___|  |_______||_______|  |__| |__||___| |_|  |__|" + Environment.NewLine;
                text += Environment.NewLine;
                text += $"                   Your score is {SnakePoint.Score}" + Environment.NewLine;
                text += @"               Play new game ? (y or n)";
                ShowGameEnd(text, Color.Green, new Point(70, 35));
            }
            else if (gameState == GameState.GameOver)
            {
                //Game Over
                timer.Stop();
                if (SnakePoint.Score > Settings.Default.BestScore)
                {
                    Settings.Default.BestScore = SnakePoint.Score;
                    Settings.Default.Save();
                }
                var text = string.Empty;
                text += @"  ______  _______  __   __  ______    _______  __   __  ______  ______   " + Environment.NewLine;
                text += @" |      ||   _   ||  |_|  ||      |  |       ||  | |  ||      ||    _ |  " + Environment.NewLine;
                text += @" |   ___||  |_|  ||       ||   ___|  |   _   ||  |_|  ||   ___||   | ||  " + Environment.NewLine;
                text += @" |  | __ |       ||       ||  |___   |  | |  ||       ||  |___ |   |_||_ " + Environment.NewLine;
                text += @" |  ||  ||       ||       ||   ___|  |  |_|  ||       ||   ___||    __  |" + Environment.NewLine;
                text += @" |  |_| ||   _   || ||_|| ||  |___   |       | |     | |  |___ |   |  | |" + Environment.NewLine;
                text += @" |______||__| |__||_|   |_||______|  |_______|  |___|  |______||___|  |_|" + Environment.NewLine;
                text += Environment.NewLine;
                text += $"                            Your score is {SnakePoint.Score}" + Environment.NewLine;
                text += $"                            Best score is {Settings.Default.BestScore}" + Environment.NewLine;
                text += @"                        Play new game ? (y or n)";
                ShowGameEnd(text, Color.Red, new Point(1, 35));
            }
        }

        private GameState GetGameSate()
        {
            var gameState = GameState.Play;
            Snake.ForEach(s =>
            {
                if (Snake.Any(x
                    => s.TabIndex != x.TabIndex &&
                    s.Location.X == x.Location.X &&
                    s.Location.Y == x.Location.Y))
                {
                    gameState = GameState.GameOver;
                }
            });
            if (gameState == GameState.Play)
            {
                if (Snake.Any(s
                    => s.Location.X < GameBoardStatLocation.X ||
                    s.Location.X > GameBoardEndLocation.X ||
                    s.Location.Y < GameBoardStatLocation.Y ||
                    s.Location.Y > GameBoardEndLocation.Y
                    ))
                {
                    gameState = GameState.GameOver; ;
                }
            }
            if (Snake.Count >= Foods.Count)
            {
                gameState = GameState.Win;
            }

            return gameState;
        }

        private void ShowGameEnd(string text, Color color, Point location)
        {
            var lblGameOver = new Label()
            {
                BackColor = Color.Black,
                ForeColor = color,
                Text = text,
                AutoSize = true,
                Font = lblBoardGameBottom.Font,
                Location = location,
            };
            pnlGameGround.Controls.Clear();
            pnlGameGround.Controls.Add(lblGameOver);
        }

        private void SnakeMoveDown()
        {
            switch (AutoDirection)
            {
                case AutoDirection.None:
                case AutoDirection.Left:
                case AutoDirection.Down:
                case AutoDirection.Right:
                    SnakeMoveAllPosition();
                    Snake[0].Location = new Point()
                    {
                        X = Snake[0].Location.X,
                        Y = Snake[0].Location.Y + Move.Y
                    };
                    AutoDirection = AutoDirection.Down;
                    break;
            }
        }

        private void SnakeMoveUp()
        {
            switch (AutoDirection)
            {
                case AutoDirection.None:
                case AutoDirection.Up:
                case AutoDirection.Left:
                case AutoDirection.Right:
                    SnakeMoveAllPosition();
                    Snake[0].Location = new Point()
                    {
                        X = Snake[0].Location.X,
                        Y = Snake[0].Location.Y - Move.Y
                    };
                    AutoDirection = AutoDirection.Up;
                    break;
            }
        }

        private void SnakeMoveRight()
        {
            switch (AutoDirection)
            {
                case AutoDirection.None:
                case AutoDirection.Up:
                case AutoDirection.Right:
                case AutoDirection.Down:
                    SnakeMoveAllPosition();
                    Snake[0].Location = new Point()
                    {
                        X = Snake[0].Location.X + Move.X,
                        Y = Snake[0].Location.Y
                    };
                    AutoDirection = AutoDirection.Right;
                    break;
            }
        }

        private void SnakeMoveLeft()
        {
            switch (AutoDirection)
            {
                case AutoDirection.Up:
                case AutoDirection.Left:
                case AutoDirection.Down:
                    SnakeMoveAllPosition();
                    Snake[0].Location = new Point()
                    {
                        X = Snake[0].Location.X - Move.X,
                        Y = Snake[0].Location.Y
                    };
                    AutoDirection = AutoDirection.Left;
                    break;
            }
        }

        private void SnakeMoveAllPosition()
        {
            var points = Snake.Select(s => new Point(s.Location.X, s.Location.Y)).ToList();
            Snake.ForEach(s =>
            {
                var index = Snake.IndexOf(s);
                if (index > 0)
                {
                    s.Location = new Point()
                    {
                        X = points[index - 1].X,
                        Y = points[index - 1].Y,
                    };
                }
            });
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            frmGame_KeyDown(null, null);
        }
    }
}
