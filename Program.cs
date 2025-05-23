using PlayerBoardGame;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("\n Welcome to the Board Game System!");

        bool keepPlaying = true;
        while (keepPlaying)
        {
            Console.WriteLine("\nChoose a game:");
            Console.WriteLine("1. Tic Tac Toe (Numerical N*N)");
            Console.WriteLine("2. Notakto");
            Console.WriteLine("3. Gomoku");
            Console.Write("Enter your choice: (or type 'exit' to quit program ): ");
            string? choice = Console.ReadLine()?.Trim().ToLower();

            if (choice == "exit" || choice == "q")
            {
                keepPlaying = false;
                continue;
            }

            Game? selectedGame = null;
            GameMode mode = GameMode.HumanVsHuman;
            int boardSize = 3;

            Console.WriteLine("\nChoose Game Mode:");
            Console.WriteLine("1. Human vs Human");
            Console.WriteLine("2. Human vs Computer");
            Console.Write("Enter your choice: ");
            string? modeInput = Console.ReadLine()?.Trim();
            mode = modeInput == "2" ? GameMode.HumanVsComputer : GameMode.HumanVsHuman;

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("\nEnter board size (>= 3): ");
                        if (!int.TryParse(Console.ReadLine(), out boardSize) || boardSize < 3)
                        {
                            Console.WriteLine("Invalid size. Defaulting to 3x3.");
                            boardSize = 3;
                        }
                        selectedGame = new TicTacToeGame(mode, boardSize);
                        break;

                    case "2":
                        selectedGame = new NotaktoGame(mode);
                        break;
                    case "3":
                        selectedGame = new GomokuGame(mode);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid game choice. Please try again.");
                        Console.ResetColor();
                        continue;
                }
                /*
                if (selectedGame != null)
                {
                    Console.WriteLine($"\n--- Starting {selectedGame?.GetType().Name} ({mode}) ---");
                    selectedGame?.StartGame();
                    selectedGame?.RunGameLoop();
                }
                */

                //Step 2: Ask if user wants to load a saved game for this type
                Console.Write("\nDo you want to load a saved game for this type? (yes/no): ");
                string? loadResponse = Console.ReadLine()?.Trim().ToLower();

                if (loadResponse == "yes" || loadResponse == "y")
                {
                    Console.Write("Enter the file name (e.g.game.sav): ");
                    string? fileName = Console.ReadLine()?.Trim();

                    if (fileName != null)
                    {
                        string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                        if (File.Exists(fullPath))
                        {
                            selectedGame.StartGame(); // Ensure players are initialized before loading
                            selectedGame.LoadGame(fullPath);
                            selectedGame.RunGameLoop();
                        }
                        else
                        {
                            Console.WriteLine("File not found. Starting new game.");
                            selectedGame.StartGame();
                            selectedGame.RunGameLoop();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No file name entered. Starting new game.");
                        selectedGame.StartGame();
                        selectedGame.RunGameLoop();
                    }
                }
                else
                {
                    selectedGame.StartGame();
                    selectedGame.RunGameLoop();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error details: {ex.Message}");
            }

            Console.Write("\nPlay another game? (yes/no): ");
            string? response = Console.ReadLine()?.Trim().ToLower();
            keepPlaying = (response == "yes" || response == "y");

            try
            {
                if (keepPlaying)
                {
                    Console.Clear();
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Console.Clear() failed (debug mode), skipping clear.");
            } 
        }
        Console.WriteLine("Thank you for playing!");
    }  
}


