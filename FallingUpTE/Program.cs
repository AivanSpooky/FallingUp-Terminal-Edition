namespace FallingUpTE
{
    class Program
    {
        static int windowWidth = 120;
        static int windowHeight = 25;
        static bool soundOn = true;
        static int score = 0;
        static Random rand = new Random();
        static string[] tailFrames = { "/|\\", "|||" };
        static int tailFrameIndex = 0;

        static void Main(string[] args)
        {
            Console.Title = "FALLING UP";
            Console.CursorVisible = false;
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.SetBufferSize(windowWidth, windowHeight);
            MainMenu();
        }

        static void MainMenu()
        {
            int selectedOption = 0;
            string[] options = { "ИГРАТЬ", "НАСТРОЙКИ", "ВЫЙТИ" };
            List<int[]> obstacles = GenerateObstacles();

            while (true)
            {
                Console.Clear();
                DrawTitle();
                DrawMenu(options, selectedOption);
                /*DrawObstacles(obstacles);*/
                AnimateTail();

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.W)
                    selectedOption = (selectedOption - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.S)
                    selectedOption = (selectedOption + 1) % options.Length;
                else if (key == ConsoleKey.Spacebar)
                {
                    if (options[selectedOption] == "ИГРАТЬ")
                    {
                        PlayGame();
                    }
                    else if (options[selectedOption] == "НАСТРОЙКИ")
                    {
                        SettingsMenu();
                    }
                    else if (options[selectedOption] == "ВЫЙТИ")
                    {
                        ExitAnimation();
                        Environment.Exit(0);
                    }
                }

                MoveObstacles(obstacles);
                Thread.Sleep(150);
            }
        }

        static void DrawTitle()
        {
            // Логотип "FALLING UP" в виде мозаики
            string[] titlePixels = {
                "█████   ████   ██    ██    ██  ██   ██  █████  ",
                "█      ██  ██  ██    ██       ██ █  ██ ██      ",
                "███   ██ ██ ██ ██    ██    ██ ██ █  ██ ██  ████",
                "█     ██    ██ ██    ██    ██ ██  █ ██ █    ██ ",
                "█     ██    ██ █████ █████ ██ ██  ███  ██████  ",
                "",
                "██    ██ ██████",
                "██    ██ ██  ██",
                "██    ██ ██████",
                " ██  ██  ██    ",
                "  ████   ██    "
            };

            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < titlePixels.Length; i++)
            {
                // Убедимся, что строка не длиннее ширины окна
                string line = titlePixels[i];
                if (line.Length > windowWidth)
                {
                    line = line.Substring(0, windowWidth); // Обрезаем строку, если она слишком длинная
                }

                int x = Math.Max(0, (windowWidth - line.Length) / 2); // Убедимся, что позиция не отрицательная
                Console.SetCursorPosition(x, 2 + i);
                Console.Write(line);
            }

            // Рисуем кубик с хвостом
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(windowWidth / 2, 13);
            Console.Write("■");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(windowWidth / 2 - 1, 14);
            Console.Write(tailFrames[tailFrameIndex]);
            Console.ResetColor();
        }

        static void DrawMenu(string[] options, int selectedOption)
        {
            int startY = 17;
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition((windowWidth - options[i].Length) / 2, startY + i * 2);
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"[{options[i]}]");
                }
                else
                {
                    Console.Write($" {options[i]} ");
                }
                Console.ResetColor();
            }
        }

        static void SettingsMenu()
        {
            int selectedOption = 0;
            string[] options = { $"ЗВУК: {(soundOn ? "вкл" : "выкл")}", "ВЫЙТИ" };
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition((windowWidth - "СОЗДАТЕЛЬ: aivansp00ky".Length) / 2, 2);
                Console.Write("СОЗДАТЕЛЬ: ai");
                Console.Write("v");
                Console.Write("ansp");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("00");
                Console.ResetColor();
                Console.Write("k");
                Console.Write("y");
                Console.SetCursorPosition((windowWidth - 7) / 2, 3);
                Console.Write("          \\__/");

                DrawMenu(options, selectedOption);

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.W)
                    selectedOption = (selectedOption - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.S)
                    selectedOption = (selectedOption + 1) % options.Length;
                else if (key == ConsoleKey.Spacebar)
                {
                    if (selectedOption == 0)
                    {
                        soundOn = !soundOn;
                        options[0] = $"ЗВУК: {(soundOn ? "вкл" : "выкл")}";
                    }
                    else if (selectedOption == 1)
                    {
                        break;
                    }
                }
            }
        }

        static List<int[]> GenerateObstacles()
        {
            List<int[]> obstacles = new List<int[]>();
            for (int i = 0; i < 5; i++)
            {
                int x = rand.Next(0, windowWidth);
                int y = rand.Next(0, windowHeight / 2); // Генерируем препятствия в верхней половине окна
                obstacles.Add(new int[] { x, y });
            }
            return obstacles;
        }

        static void MoveObstacles(List<int[]> obstacles)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i][1]++; // Сдвигаем препятствие вниз

                // Если препятствие выходит за нижнюю границу экрана, перемещаем его наверх
                if (obstacles[i][1] >= windowHeight - 1)
                {
                    obstacles[i][0] = rand.Next(0, windowWidth);
                    obstacles[i][1] = 0; // Сбрасываем его на верх экрана
                }
            }
        }

        static void DrawObstacles(List<int[]> obstacles)
        {
            foreach (var obs in obstacles)
            {
                Console.SetCursorPosition(obs[0], obs[1]);
                Console.ForegroundColor = obs[2] == 0 ? ConsoleColor.Green : ConsoleColor.Magenta;
                Console.Write(obs[2] == 0 ? "█" : "■");
                Console.ResetColor();
            }
        }

        static void AnimateTail()
        {
            tailFrameIndex = (tailFrameIndex + 1) % tailFrames.Length;
        }

        static void PlayGame()
        {
            int playerX = windowWidth / 2;
            int playerY = windowHeight - 3; // Устанавливаем позицию чуть выше нижней границы
            List<int[]> obstacles = new List<int[]>();
            bool isAlive = true;
            score = 0;

            // Создаем поток для обработки ввода
            Thread inputThread = new Thread(() =>
            {
                while (isAlive)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.A)
                        playerX = (playerX - 1 + windowWidth) % windowWidth;
                    else if (key == ConsoleKey.D)
                        playerX = (playerX + 1) % windowWidth;
                }
            });

            inputThread.Start();

            // Игровой цикл
            while (isAlive)
            {
                Console.Clear();
                DrawBorders();

                // Добавляем новые препятствия случайным образом
                if (rand.Next(0, 10) > 7)
                {
                    obstacles.Add(new int[] { rand.Next(0, windowWidth), 0 });
                }

                // Обновляем положение препятствий
                for (int i = 0; i < obstacles.Count; i++)
                {
                    obstacles[i][1]++;
                    if (obstacles[i][1] >= windowHeight - 1)
                    {
                        obstacles.RemoveAt(i);
                        i--;
                        score++;
                    }
                }

                // Проверяем столкновения с препятствиями
                foreach (var obs in obstacles)
                {
                    if (obs[0] == playerX && obs[1] == playerY)
                    {
                        isAlive = false;
                        break;
                    }
                }

                // Обновляем анимацию хвоста
                AnimateTail();

                // Рисуем игрока (кубик) и его хвост
                DrawPlayer(playerX, playerY);

                // Рисуем препятствия
                foreach (var obs in obstacles)
                {
                    Console.SetCursorPosition(obs[0], obs[1]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("#");
                    Console.ResetColor();
                }

                Thread.Sleep(100);
            }

            // Ждем завершения потока перед продолжением
            isAlive = false;
            inputThread.Join();
            GameOverMenu();
        }

        static void DrawPlayer(int playerX, int playerY)
        {
            playerX = Math.Max(1, Math.Min(playerX, windowWidth - 2));
            playerY = Math.Max(0, Math.Min(playerY, windowHeight - 2));

            Console.SetCursorPosition(playerX, playerY);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("■");
            Console.ForegroundColor = ConsoleColor.Red;

            int tailX = Math.Max(0, playerX - 1);
            int tailY = Math.Min(windowHeight - 1, playerY + 1);
            Console.SetCursorPosition(tailX, tailY);
            Console.Write(tailFrames[tailFrameIndex]);
            Console.ResetColor();
        }

        static void DrawBorders()
        {
            for (int x = 0; x < windowWidth; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write("-");
                Console.SetCursorPosition(x, windowHeight - 1);
                Console.Write("-");
            }
            for (int y = 0; y < windowHeight; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("|");
                Console.SetCursorPosition(windowWidth - 1, y);
                Console.Write("|");
            }
        }

        static void GameOverMenu()
        {
            int selectedOption = 0;
            string[] options = { "ЗАНОВО", "ВЫЙТИ" };
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition((windowWidth - "ИГРА ОКОНЧЕНА!".Length) / 2, 5);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("ИГРА ОКОНЧЕНА!");
                Console.ResetColor();
                Console.SetCursorPosition((windowWidth - 25) / 2, 7);
                Console.Write($"Вы пролетели: {score} клеток.");

                DrawMenu(options, selectedOption);

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.W)
                    selectedOption = (selectedOption - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.S)
                    selectedOption = (selectedOption + 1) % options.Length;
                else if (key == ConsoleKey.Spacebar)
                {
                    if (selectedOption == 0)
                    {
                        PlayGame();
                        break;
                    }
                    else if (selectedOption == 1)
                        break;
                    
                }
            }
        }
        static void ExitAnimation()
        {
            int step = 2;
            for (int y = 0; y < windowHeight; y++)
            {
                for (int x = 0; x < windowWidth; x += step)
                {
                    for (int i = 0; i < step && x + i < windowWidth; i++)
                    {
                        Console.SetCursorPosition(x + i, y);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("█");
                    }
                }
                Thread.Sleep(1);
            }
            Environment.Exit(0);
        }
    }
}
