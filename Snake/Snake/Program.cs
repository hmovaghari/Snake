using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Snake
{
    class Program
    {
        #region Disable Resize Windows

        private const int MF_BYCOMMAND = 0x00000000;
        //public const int SC_CLOSE = 0xF060; // CLOSE Button
        //public const int SC_MINIMIZE = 0xF020; // MINIMIZE Button
        public const int SC_MAXIMIZE = 0xF030; // MAXIMIZE Button
        public const int SC_SIZE = 0xF000; // Resize Windows

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        #endregion

        #region Struct Point 2D

        private struct Point2D
        {
            public int X;
            public int Y;
        }

        #endregion

        #region Main

        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Clear();
            ConsoleProperties(); // Properties of console window size, background and foreground color, title, Cursor Size, Cursor Visible, Disable Resize and MAXIMIZE Window
            SnakeASCII(); // ASCII: Snake
            LoadingBar(); // Show Loading Bar

            ConsoleKey key = ConsoleKey.NoName;
            do
            {
                Point2D[] Snake = new Point2D[5]; // First location of snake
                Snake[0].X = 9; Snake[0].Y = 17;
                Snake[1].X = Snake[0].X - 1; Snake[1].Y = Snake[0].Y;
                Snake[2].X = Snake[0].X - 2; Snake[2].Y = Snake[0].Y;
                Snake[3].X = Snake[0].X - 3; Snake[3].Y = Snake[0].Y;
                Snake[4].X = Snake[0].X - 4; Snake[4].Y = Snake[0].Y;
                SnakeMainBody(Snake);
                Console.ResetColor();
                do
                {
                    Console.SetCursorPosition(45, 20);
                    Console.Write(" Play new game ? (y or n) ");
                    key = Console.ReadKey(true).Key;
                } while (key != ConsoleKey.Y && key != ConsoleKey.N);
            } while (key == ConsoleKey.Y);

            /*Snake[0].X = 3;*//*Snake[0].X = 117;*/
            /*Snake[0].Y = 7;*//*Snake[0].Y = 27;*/

            Console.Clear();
        }

        #endregion

        #region Snake game : main body

        /// <summary>
        /// Snake game : main body
        /// </summary>
        /// <param name="Snake">Location of snake</param>
        static void SnakeMainBody(Point2D[] Snake)
        {
            byte oldDirection = 4; // Block Direction Up:1 Left:2 Down:3 Right:4
            bool isEatFood = false;
            Border();
            do
            {
                bool gameOver = false; // Is game over?
                Point2D snakeFood = MakeFood(Snake); // Make food of snake
                SnakePrint(Snake, snakeFood); // Print Snake and food of snake
                ConsoleKeyInfo keyArrow = new ConsoleKeyInfo();
                if (!isEatFood)
                {
                    keyArrow = Console.ReadKey(); // Get key arrow from user
                    isEatFood = false;
                }
                do
                {
                    if (Console.KeyAvailable) // If user press a key
                    {
                        keyArrow = Console.ReadKey(true);
                    }

                    switch (keyArrow.Key) // Up, Left, Down, Right
                    {
                        case ConsoleKey.LeftArrow:
                            if ((oldDirection == 2) | (Snake[0].X == Snake[1].X)) // if old Direction is Left | Point2D[0],[1] of snake is Vertical
                            {
                                oldDirection = SnakeMoveLeft(Snake, oldDirection);
                            }
                            else
                            {
                                oldDirection = SnakeMoveRight(Snake, oldDirection);
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            if ((oldDirection == 1) | (Snake[0].Y == Snake[1].Y)) // if old Direction is Up | Point2D[0],[1] of snake is Horizontal
                            {
                                oldDirection = SnakeMoveUp(Snake, oldDirection);
                            }
                            else
                            {
                                oldDirection = SnakeMoveDown(Snake, oldDirection);
                            }
                            break;

                        case ConsoleKey.RightArrow:
                            if ((oldDirection == 4) | (Snake[0].X == Snake[1].X)) // if old Direction is Right | Point2D[0],[1] of snake is Vertical
                            {
                                oldDirection = SnakeMoveRight(Snake, oldDirection);
                            }
                            else
                            {
                                oldDirection = SnakeMoveLeft(Snake, oldDirection);
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            if ((oldDirection == 3) | (Snake[0].Y == Snake[1].Y)) // if old Direction is Down | Point2D[0],[1] of snake is Horizontal
                            {
                                oldDirection = SnakeMoveDown(Snake, oldDirection);
                            }
                            else
                            {
                                oldDirection = SnakeMoveUp(Snake, oldDirection);
                            }
                            break;

                        default: // Other keys
                            switch (oldDirection)
                            {
                                case 1: // Up
                                    oldDirection = SnakeMoveUp(Snake, oldDirection);
                                    break;
                                case 2: // Left
                                    oldDirection = SnakeMoveLeft(Snake, oldDirection);
                                    break;
                                case 3: // Down
                                    oldDirection = SnakeMoveDown(Snake, oldDirection);
                                    break;
                                case 4: // Right
                                    oldDirection = SnakeMoveRight(Snake, oldDirection);
                                    break;
                            }
                            break;
                    }
                    if (Snake.Length >= 2414) // ((117-3+1) * (27-7+1)) - 1 = 2414 : Number of Playground points
                    {
                        YouWinASCII(Snake.Length); // ASCII: Game Over
                        gameOver = true; // Is game Over : true
                        break;
                    }

                    if (Snake[0].X < 3 | Snake[0].X > 117 | Snake[0].Y < 7 | Snake[0].Y > 27) // If the snake collision the border
                    {
                        GameOverASCII(Snake.Length); // ASCII: Game Over
                        gameOver = true; // Is game Over : true
                        break;
                    }
                    for (int i = 1; i < Snake.Length; i++)
                    {
                        if (Snake[i].X == Snake[0].X & Snake[i].Y == Snake[0].Y) // If the snake eating herself
                        {
                            GameOverASCII(Snake.Length); // ASCII: Game Over
                            gameOver = true; // Is game Over : true
                            break;
                        }
                    }
                    if (gameOver) // If the snake eating herself
                    {
                        break;
                    }
                    if (Snake[0].X == snakeFood.X & Snake[0].Y == snakeFood.Y) // If the snake eating food
                    {
                        isEatFood = true;
                        Point2D temp = Snake[Snake.Length - 1]; // Save last of point of snake
                        switch (oldDirection)
                        {
                            case 1: // Up
                                oldDirection = SnakeMoveUp(Snake, oldDirection);
                                break;
                            case 2: // Left
                                oldDirection = SnakeMoveLeft(Snake, oldDirection);
                                break;
                            case 3: // Down
                                oldDirection = SnakeMoveDown(Snake, oldDirection);
                                break;
                            case 4: // Right
                                oldDirection = SnakeMoveRight(Snake, oldDirection);
                                break;
                        }
                        Array.Resize(ref Snake, Snake.Length + 1); // The snake gets bigger
                        Snake[Snake.Length - 1] = temp; // The last point of the snake is filled
                        break;
                    }
                    SnakePrint(Snake, snakeFood); // Print snake and food of snake
                    Thread.Sleep(100);
                } while (true);
                if (gameOver)
                {
                    break;
                }
            } while (true);
        }

        #endregion

        #region Snake Move to Down

        /// <summary>
        /// Snake Move to Down
        /// </summary>
        /// <param name="Snake">Snake Location</param>
        /// <param name="oldDirection">old Direction of snake</param>
        /// <returns>New Direction</returns>
        static byte SnakeMoveDown(Point2D[] Snake, byte oldDirection)
        {
            SnakeMoveAllPosition(Snake);
            ++Snake[0].Y;
            oldDirection = 3;
            return oldDirection;
        }

        #endregion

        #region Snake Move to Up

        /// <summary>
        /// Snake Move to Up
        /// </summary>
        /// <param name="Snake">Snake Location</param>
        /// <param name="oldDirection">old Direction of snake</param>
        /// <returns>New Direction</returns>
        static byte SnakeMoveUp(Point2D[] Snake, byte oldDirection)
        {
            SnakeMoveAllPosition(Snake);
            --Snake[0].Y;
            oldDirection = 1;
            return oldDirection;
        }

        #endregion

        #region Snake Move to Right

        /// <summary>
        /// Snake Move to Right
        /// </summary>
        /// <param name="Snake">Snake Location</param>
        /// <param name="oldDirection">old Direction of snake</param>
        /// <returns>New Direction</returns>
        static byte SnakeMoveRight(Point2D[] Snake, byte oldDirection)
        {
            SnakeMoveAllPosition(Snake);
            ++Snake[0].X;
            oldDirection = 4;
            return oldDirection;
        }

        #endregion

        #region Snake Move to Left

        /// <summary>
        /// Snake Move to Left
        /// </summary>
        /// <param name="Snake">Snake Location</param>
        /// <param name="oldDirection">old Direction of snake</param>
        /// <returns>New Direction</returns>
        static byte SnakeMoveLeft(Point2D[] Snake, byte oldDirection)
        {
            SnakeMoveAllPosition(Snake);
            --Snake[0].X;
            oldDirection = 2;
            return oldDirection;
        }

        #endregion

        #region Move all point of snake

        /// <summary>
        /// Move all point of snake
        /// </summary>
        /// <param name="Snake">Snake Location</param>
        static void SnakeMoveAllPosition(Point2D[] Snake)
        {
            Point2D[] temp = new Point2D[Snake.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].X = Snake[i].X;
                temp[i].Y = Snake[i].Y;
            }
            //
            for (int i = 1; i < Snake.Length; i++)
            {
                Snake[i] = temp[i - 1];
            }
        }

        #endregion

        #region Print snake and food of snake

        /// <summary>
        /// Print snake and food of snake
        /// </summary>
        /// <param name="Snake">Snake location</param>
        /// <param name="snakeFood">Food of snake location</param>
        static void SnakePrint(Point2D[] Snake, Point2D snakeFood)
        {
            for (int i = 0; i < Snake.Length; i++)
            {
                Console.BackgroundColor = (i == 0) ? ConsoleColor.DarkGreen : (i % 2 == 0) ? ConsoleColor.Green : ConsoleColor.DarkGreen;
                Console.SetCursorPosition(Snake[i].X, Snake[i].Y);
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(snakeFood.X, snakeFood.Y);
            Console.Write(" ");

            RefreshPlayground(Snake);
        }

        private static void RefreshPlayground(Point2D[] Snake)
        {
            var last = Snake.Length - 1;
            var bLast = last - 1;
            Console.BackgroundColor = ConsoleColor.Black;
            if (Snake[last].X == Snake[bLast].X - 1)
            {
                Console.SetCursorPosition(Snake[last].X - 1, Snake[last].Y);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X - 1, Snake[last].Y - 1);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X - 1, Snake[last].Y + 1);
                Console.Write(" ");
            }
            else if (Snake[last].X == Snake[bLast].X + 1)
            {
                Console.SetCursorPosition(Snake[last].X + 1, Snake[last].Y);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X + 1, Snake[last].Y - 1);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X + 1, Snake[last].Y + 1);
                Console.Write(" ");
            }
            else if (Snake[last].Y == Snake[bLast].Y - 1)
            {
                Console.SetCursorPosition(Snake[last].X, Snake[last].Y - 1);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X - 1, Snake[last].Y - 1);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X + 1, Snake[last].Y - 1);
                Console.Write(" ");
            }
            else if (Snake[last].Y == Snake[bLast].Y + 1)
            {
                Console.SetCursorPosition(Snake[last].X, Snake[last].Y + 1);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X - 1, Snake[last].Y + 1);
                Console.Write(" ");
                Console.SetCursorPosition(Snake[last].X + 1, Snake[last].Y + 1);
                Console.Write(" ");
            }

            Console.BackgroundColor = ConsoleColor.Gray;
            for (int x = 2; x < 119; x++)
            {
                Console.SetCursorPosition(x, 6);
                Console.Write(" ");
                Console.SetCursorPosition(x, 28);
                Console.Write(" ");
            }
            for (int y = 6; y < 29; y++)
            {
                Console.SetCursorPosition(2, y);
                Console.Write(" ");
                Console.SetCursorPosition(118, y);
                Console.Write(" ");
            }
            Console.SetCursorPosition(118, 28);
            Console.Write(" ");
        }

        #endregion

        #region Make Snake Food

        /// <summary>
        /// Make snake food on the Playground
        /// </summary>
        /// <param name="snake">Snake Position</param>
        /// <returns></returns>
        static Point2D MakeFood(Point2D[] snake)
        {
            Random rand = new Random();
            Point2D result = new Point2D();
            do
            {
                result.X = rand.Next(3 + 1, 118 - 1);
                result.Y = rand.Next(7 + 1, 27 - 1);
                bool @true = true;
                for (int i = 0; i < snake.Length; i++)
                {
                    if (result.X == snake[i].X & result.Y == snake[i].Y)
                    {
                        @true = false;
                    }
                }
                if (@true)
                {
                    return result;
                }
            } while (true);
        }

        #endregion

        #region Border

        /// <summary>
        /// Border
        /// </summary>
        static void Border()
        {
            Console.ResetColor();
            Console.Clear(); // clear screen of console
            Console.WriteLine();
            Console.WriteLine("  █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█");
            Console.WriteLine("  █                                                  Welcome to Snake!                                                █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                              Programming by HMovaghari                                            █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █                                                                                                                   █");
            Console.WriteLine("  █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████");
        }

        #endregion

        #region Loading Bar

        /// <summary>
        /// Show Loading Bar
        /// </summary>
        static void LoadingBar()
        {
            Console.SetCursorPosition(46, 26);
            Console.WriteLine(" Programming by HMovaghari");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.SetCursorPosition(11, 19);
            for (int i = 0; i <= 100; i++)
            {
                Console.SetCursorPosition(11 + i, 19);
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(12, 19);
                Console.Write($"{i}%");
                System.Threading.Thread.Sleep(100);
            }
            Console.SetCursorPosition(47, 22);
            Console.ResetColor();
            Console.WriteLine("Press any key to play game");
            Console.ReadKey(true);
        }

        #endregion

        #region ASCII: Snake

        /// <summary>
        /// Show Snake by ascii codes
        /// </summary>
        static void SnakeASCII()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\t" + @"      ___           ___           ___           ___           ___     ");
            Console.WriteLine("\t\t\t" + @"     /  /\         /__/\         /  /\         /__/|         /  /\    ");
            Console.WriteLine("\t\t\t" + @"    /  /:/_        \  \:\       /  /::\       |  |:|        /  /:/_   ");
            Console.WriteLine("\t\t\t" + @"   /  /:/ /\        \  \:\     /  /:/\:\      |  |:|       /  /:/ /\  ");
            Console.WriteLine("\t\t\t" + @"  /  /:/ /::\   _____\__\:\   /  /:/~/::\   __|  |:|      /  /:/ /:/_ ");
            Console.WriteLine("\t\t\t" + @" /__/:/ /:/\:\ /__/::::::::\ /__/:/ /:/\:\ /__/\_|:|____ /__/:/ /:/ /\");
            Console.WriteLine("\t\t\t" + @" \  \:\/:/~/:/ \  \:\~~\~~\/ \  \:\/:/__\/ \  \:\/:::::/ \  \:\/:/ /:/");
            Console.WriteLine("\t\t\t" + @"  \  \::/ /:/   \  \:\  ~~~   \  \::/       \  \::/~~~~   \  \::/ /:/ ");
            Console.WriteLine("\t\t\t" + @"   \__\/ /:/     \  \:\        \  \:\        \  \:\        \  \:\/:/  ");
            Console.WriteLine("\t\t\t" + @"     /__/:/       \  \:\        \  \:\        \  \:\        \  \::/   ");
            Console.WriteLine("\t\t\t" + @"     \__\/         \__\/         \__\/         \__\/         \__\/    ");
        }

        #endregion

        #region ASCII: You Win

        /// <summary>
        /// Show You Win by ascii codes
        /// </summary>
        static void YouWinASCII(int Score)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(35, 10);
            Console.WriteLine(@" __   __  _______  __   __    _     _  ___   __    _ ");
            Console.SetCursorPosition(35, 11);
            Console.WriteLine(@"|  | |  ||       ||  | |  |  | | _ | ||   | |  |  | |");
            Console.SetCursorPosition(35, 12);
            Console.WriteLine(@"|  |_|  ||   _   ||  | |  |  | || || ||   | |   |_| |");
            Console.SetCursorPosition(35, 13);
            Console.WriteLine(@"|       ||  | |  ||  |_|  |  |       ||   | |       |");
            Console.SetCursorPosition(35, 14);
            Console.WriteLine(@"|_     _||  |_|  ||       |  |       ||   | |  _    |");
            Console.SetCursorPosition(35, 15);
            Console.WriteLine(@"  |   |  |       ||       |  |   _   ||   | | | |   |");
            Console.SetCursorPosition(35, 16);
            Console.WriteLine(@"  |___|  |_______||_______|  |__| |__||___| |_|  |__|");
            Console.SetCursorPosition(53, 18);
            Console.WriteLine($"Your score is {Score - 5}");
        }

        #endregion

        #region ASCII: Game Over

        /// <summary>
        /// Show Game Over by ascii codes
        /// </summary>
        static void GameOverASCII(int Score)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(23, 10);
            Console.WriteLine(@" _______  _______  __   __  _______    _______  __   __  _______  ______   ");
            Console.SetCursorPosition(23, 11);
            Console.WriteLine(@"|       ||   _   ||  |_|  ||       |  |       ||  | |  ||       ||    _ |  ");
            Console.SetCursorPosition(23, 12);
            Console.WriteLine(@"|    ___||  |_|  ||       ||    ___|  |   _   ||  |_|  ||    ___||   | ||  ");
            Console.SetCursorPosition(23, 13);
            Console.WriteLine(@"|   | __ |       ||       ||   |___   |  | |  ||       ||   |___ |   |_||_ ");
            Console.SetCursorPosition(23, 14);
            Console.WriteLine(@"|   ||  ||       ||       ||    ___|  |  |_|  ||       ||    ___||    __  |");
            Console.SetCursorPosition(23, 15);
            Console.WriteLine(@"|   |_| ||   _   || ||_|| ||   |___   |       | |     | |   |___ |   |  | |");
            Console.SetCursorPosition(23, 16);
            Console.WriteLine(@"|_______||__| |__||_|   |_||_______|  |_______|  |___|  |_______||___|  |_|");
            Console.SetCursorPosition(53, 18);
            Console.WriteLine($"Your score is {Score - 5}");
        }

        #endregion

        #region Console Properties

        /// <summary>
        /// Console Properties: Change window size, background and foreground color, title, Cursor Size, Cursor Visible, Disable Resize and MAXIMIZE Window
        /// </summary>
        static void ConsoleProperties()
        {
            Console.WindowHeight = 30; // Change Window Height
            Console.WindowWidth = 120; // Change Window Width
            //Console.BufferHeight = 30; //9001//30// Change Buffer Height
            Console.BufferWidth = 120; // Change Buffer Width
            Console.BackgroundColor = ConsoleColor.Black; // Change Background Color
            Console.ForegroundColor = ConsoleColor.Gray; // Change Foreground Color
            Console.Title = "Snake"; // Change Title
            Console.CursorSize = 25; // Change Cursor Size
            Console.CursorVisible = false; // Cursor Visible
            /* Disable Resize Windows */
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                //DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND); // Disable CLOSE Button
                //DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND); // Disable MINIMIZE Button
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND); // Disable MAXIMIZE Button
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND); // Disable Resize Windows
            }
            /* Disable Resize Windows */
        }

        #endregion
    }
}
