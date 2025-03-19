using System.Runtime.InteropServices;

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

        private static readonly Point2D PlaygroundPoint1 = new Point2D() { X = 2, Y = 6 };
        private static readonly Point2D PlaygroundPoint2 = new Point2D() { X = 77, Y = 22 };
        private static readonly int mean = (PlaygroundPoint1.X + PlaygroundPoint2.X) / 2;
        private static readonly int maxLenSnake = (PlaygroundPoint2.X - PlaygroundPoint1.X - 1) * (PlaygroundPoint2.Y - PlaygroundPoint1.Y - 1);
        private static readonly string by = "Programming by HMovaghari";

        #region Struct Point 2D

        struct Point2D
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
            //Test
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
                ConsoleResetColor();
                do
                {
                    //Console.SetCursorPosition(45, 20);
                    SetTextCenter("Play new game ? (y or n)", PlaygroundPoint1.Y + 13);
                    key = Console.ReadKey(true).Key;
                } while (key != ConsoleKey.Y && key != ConsoleKey.N);
            } while (key == ConsoleKey.Y);

            Console.Clear();
        }

        #endregion

        #region Console Reset Colour
        static void ConsoleResetColor()
        {
            //Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
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
            Border(isClearScreen: true);
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
                    if (Snake.Length >= maxLenSnake) // Number of Playground points
                    {
                        //maxLenSnake
                        YouWinASCII(Snake.Length); // ASCII: Game Over
                        gameOver = true; // Is game Over : true
                        break;
                    }

                    if (Snake[0].X <= PlaygroundPoint1.X || Snake[0].X >= PlaygroundPoint2.X || Snake[0].Y <= PlaygroundPoint1.Y - 1 || Snake[0].Y >= PlaygroundPoint2.Y - 1) // If the snake collision the border
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

            Border(isClearScreen: false);
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
            Point2D result;
            do
            {
                result.X = rand.Next(PlaygroundPoint1.X + 1, PlaygroundPoint2.X);
                result.Y = rand.Next(PlaygroundPoint1.Y + 1, PlaygroundPoint2.Y - 1);
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
        static void Border(bool isClearScreen)
        {
            ConsoleResetColor();

            if (isClearScreen)
            {
                Console.Clear(); // clear screen of console
            }

            var welcome = "Welcome to Snake!";
            SetTextCenter(welcome, yLocation: PlaygroundPoint1.Y - 4);
            SetTextCenter(by, yLocation: PlaygroundPoint1.Y - 3);

            for (int x = PlaygroundPoint1.X; x <= PlaygroundPoint2.X; x++)
            {
                Console.SetCursorPosition(x, 1);
                Console.WriteLine("▀");
            }

            for (int y = 1; y <= PlaygroundPoint1.Y; y++)
            {
                Console.SetCursorPosition(PlaygroundPoint1.X, y);
                Console.WriteLine("█");

                Console.SetCursorPosition(PlaygroundPoint2.X, y);
                Console.WriteLine("█");
            }

            for (int x = PlaygroundPoint1.X; x <= PlaygroundPoint2.X; x++)
            {
                Console.SetCursorPosition(x, PlaygroundPoint1.Y - 1);
                Console.WriteLine("█");

                Console.SetCursorPosition(x, PlaygroundPoint2.Y - 1);
                Console.WriteLine("█");
            }

            for (int y = PlaygroundPoint1.Y; y < PlaygroundPoint2.Y; y++)
            {
                Console.SetCursorPosition(PlaygroundPoint1.X, y);
                Console.WriteLine("█");

                Console.SetCursorPosition(PlaygroundPoint2.X, y);
                Console.WriteLine("█");
            }
        }

        private static void SetTextCenter(string text, int yLocation)
        {
            Console.SetCursorPosition(mean - (text.Length / 2), yLocation);
            Console.Write(text);
        }

        #endregion

        #region Loading Bar

        /// <summary>
        /// Show Loading Bar
        /// </summary>
        static void LoadingBar()
        {
            Console.SetCursorPosition(mean - (by.Length / 2), PlaygroundPoint1.Y + 9);
            Console.Write(by);

            var xFrom = PlaygroundPoint1.X + 2;
            var xTo = PlaygroundPoint2.X - 3;
            for (int x = xFrom; x <= xTo; x++)
            {
                Console.SetCursorPosition(x, PlaygroundPoint1.Y + 12);
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(" ");
                Console.SetCursorPosition(xFrom + 1, PlaygroundPoint1.Y + 12);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write((((x - xFrom) * 100) / (xTo - xFrom)).ToString("00") + "%");
                System.Threading.Thread.Sleep(200);
            }
            var pressKey = "Press any key to play game";
            Console.SetCursorPosition(mean - (pressKey.Length / 2), PlaygroundPoint1.Y + 15);
            ConsoleResetColor();
            Console.Write(pressKey);
            Console.ReadKey(true);
        }

        #endregion

        #region ASCII: Snake

        /// <summary>
        /// Show Snake by ascii codes
        /// </summary>
        static void SnakeASCII()
        {
            int y = PlaygroundPoint1.Y - 5;
            Console.ForegroundColor = ConsoleColor.White;
            SetTextCenter(@"      ___           ___           ___           ___           ___     ", ++y);
            SetTextCenter(@"     /  /\         /__/\         /  /\         /__/|         /  /\    ", ++y);
            SetTextCenter(@"    /  /:/_        \  \:\       /  /::\       |  |:|        /  /:/_   ", ++y);
            SetTextCenter(@"   /  /:/ /\        \  \:\     /  /:/\:\      |  |:|       /  /:/ /\  ", ++y);
            SetTextCenter(@"  /  /:/ /::\   _____\__\:\   /  /:/~/::\   __|  |:|      /  /:/ /:/_ ", ++y);
            SetTextCenter(@" /__/:/ /:/\:\ /__/::::::::\ /__/:/ /:/\:\ /__/\_|:|____ /__/:/ /:/ /\", ++y);
            SetTextCenter(@" \  \:\/:/~/:/ \  \:\~~\~~\/ \  \:\/:/__\/ \  \:\/:::::/ \  \:\/:/ /:/", ++y);
            SetTextCenter(@"  \  \::/ /:/   \  \:\  ~~~   \  \::/       \  \::/~~~~   \  \::/ /:/ ", ++y);
            SetTextCenter(@"   \__\/ /:/     \  \:\        \  \:\        \  \:\        \  \:\/:/  ", ++y);
            SetTextCenter(@"     /__/:/       \  \:\        \  \:\        \  \:\        \  \::/   ", ++y);
            SetTextCenter(@"     \__\/         \__\/         \__\/         \__\/         \__\/    ", ++y);
        }

        #endregion

        #region ASCII: You Win

        /// <summary>
        /// Show You Win by ascii codes
        /// </summary>
        static void YouWinASCII(int Score)
        {
            ConsoleResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            int y = PlaygroundPoint1.Y + 2;
            SetTextCenter(@" __   __  _______  __   __    _     _  ___   __    _ ", ++y);
            SetTextCenter(@"|  | |  ||       ||  | |  |  | | _ | ||   | |  |  | |", ++y);
            SetTextCenter(@"|  |_|  ||   _   ||  | |  |  | || || ||   | |   |_| |", ++y);
            SetTextCenter(@"|       ||  | |  ||  |_|  |  |       ||   | |       |", ++y);
            SetTextCenter(@"|_     _||  |_|  ||       |  |       ||   | |  _    |", ++y);
            SetTextCenter(@"  |   |  |       ||       |  |   _   ||   | | | |   |", ++y);
            SetTextCenter(@"  |___|  |_______||_______|  |__| |__||___| |_|  |__|", ++y);
            y += 2;
            SetTextCenter($"Your score is {Score - 5}", y);
        }

        #endregion

        #region ASCII: Game Over

        /// <summary>
        /// Show Game Over by ascii codes
        /// </summary>
        static void GameOverASCII(int Score)
        {
            ConsoleResetColor();
            int y = PlaygroundPoint1.Y + 2;
            Console.ForegroundColor = ConsoleColor.Red;
            SetTextCenter(@"  ______  _______  __   __  ______    _______  __   __  ______  ______   ", ++y);
            SetTextCenter(@" |      ||   _   ||  |_|  ||      |  |       ||  | |  ||      ||    _ |  ", ++y);
            SetTextCenter(@" |   ___||  |_|  ||       ||   ___|  |   _   ||  |_|  ||   ___||   | ||  ", ++y);
            SetTextCenter(@" |  | __ |       ||       ||  |___   |  | |  ||       ||  |___ |   |_||_ ", ++y);
            SetTextCenter(@" |  ||  ||       ||       ||   ___|  |  |_|  ||       ||   ___||    __  |", ++y);
            SetTextCenter(@" |  |_| ||   _   || ||_|| ||  |___   |       | |     | |  |___ |   |  | |", ++y);
            SetTextCenter(@" |______||__| |__||_|   |_||______|  |_______|  |___|  |______||___|  |_|", ++y);
            y += 2;
            SetTextCenter($"Your score is {Score - 5}", y);
        }

        #endregion

        #region Console Properties

        /// <summary>
        /// Console Properties: Change window size, background and foreground color, title, Cursor Size, Cursor Visible, Disable Resize and MAXIMIZE Window
        /// </summary>
        static void ConsoleProperties()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WindowHeight = PlaygroundPoint2.Y + 1; //30; // Change Window Height
                Console.WindowWidth = PlaygroundPoint2.X + 1; //120; // Change Window Width
                //Console.BufferHeight = Console.WindowHeight + 1; //30; //9001//30// Change Buffer Height
                //Console.BufferWidth = Console.WindowWidth + 1; //120; // Change Buffer Width
                Console.CursorSize = 25; // Change Cursor Size

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

            Console.BackgroundColor = ConsoleColor.Black; // Change Background Color
            Console.ForegroundColor = ConsoleColor.Gray; // Change Foreground Color
            Console.Title = "Snake"; // Change Title
            Console.CursorVisible = false; // Cursor Visible
        }

        #endregion
    }
}

