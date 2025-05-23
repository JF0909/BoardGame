namespace PlayerBoardGame
{
    // Gomoku Game: first to align 5 on 15x15 board
    public class GomokuGame : Game
    {
        private readonly Piece X = new Piece("X");
        private readonly Piece O = new Piece("O");
        public GomokuGame() { }
        public GomokuGame(GameMode mode)
        {
            this.GameMode = mode;
        }

        protected override Board CreateInitialBoard()
        {
            return new GomokuBoard(); // Assume default 15x15 in constructor
        }

        protected override List<Player> CreatePlayers(GameMode mode)
        {
            var players = new List<Player> { new HumanPlayer("Player 1", X) };
            if (mode == GameMode.HumanVsHuman)
                players.Add(new HumanPlayer("Player 2", O));
            else
                players.Add(new ComputerPlayer("Computer", O));
            return players;
        }

        protected override bool IsMoveValid(Move move)
        {
            return CurrentBoard!.IsValidPosition(move.Row, move.Col) &&
                   CurrentBoard.IsCellEmpty(move.Row, move.Col);
        }

        protected override Player? CheckWinCondition()
        {
            int size = (CurrentBoard as GomokuBoard)?.Width ?? 15;
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    var piece = CurrentBoard!.GetPiece(row, col);
                    if (piece == null) continue;

                    if (CheckDirection(row, col, 1, 0, piece.Symbol) ||
                        CheckDirection(row, col, 0, 1, piece.Symbol) ||
                        CheckDirection(row, col, 1, 1, piece.Symbol) ||
                        CheckDirection(row, col, 1, -1, piece.Symbol))
                        return CurrentPlayer;
                }
            }
            return null;
        }

        private bool CheckDirection(int row, int col, int dr, int dc, string symbol)
        {
            for (int i = 1; i < 5; i++)
            {
                int r = row + dr * i;
                int c = col + dc * i;
                if (!CurrentBoard!.IsValidPosition(r, c) ||
                    CurrentBoard.GetPiece(r, c)?.Symbol != symbol)
                    return false;
            }
            return true;
        }

        protected override bool CheckDrawCondition()
        {
            var board = CurrentBoard as GomokuBoard;
            if (board == null) return false;

            for (int r = 0; r < board.Height; r++)
                for (int c = 0; c < board.Width; c++)
                    if (board.IsCellEmpty(r, c))
                        return false;
            return true;
        }

        protected override void DisplayHelp()
        {
            base.DisplayHelp();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- Gomoku Game Rules & Tips ---");
            Console.WriteLine("  - Two players take turns placing two types of pieces (e.g. an X and an O ) on a 15 × 15 board");
            Console.WriteLine("  - The winner is the first player to form an unbroken line of five pieces of their colour horizontally, vertically, or diagonally.");
            Console.WriteLine("---------------------------------");
            Console.ResetColor();
        }

        public override void SaveGame(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(this.GetType().Name);
                    writer.WriteLine(CurrentBoard?.Width);
                    writer.WriteLine(CurrentPlayer?.Name);

                    for (int row = 0; row < CurrentBoard?.Height; row++)
                    {
                        for (int col = 0; col < CurrentBoard.Width; col++)
                        {
                            var piece = CurrentBoard.GetPiece(row, col);
                            writer.Write(piece != null ? piece.Symbol : ".");
                        }
                        writer.WriteLine();
                    }
                }

                Console.WriteLine("GomokuGame saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save GomokuGame: " + ex.Message);
            }
        }

        /// <summary>
        /// Loads the Gomoku game state from a file.
        /// </summary>
        /// <param name="filePath">Path to the save file</param>
        public override void LoadGame(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string gameType = reader.ReadLine();
                    int size = int.Parse(reader.ReadLine());
                    string currentPlayerName = reader.ReadLine();

                    CurrentBoard = new GomokuBoard();

                    Player? matched = Players.FirstOrDefault(p => p.Name == currentPlayerName);
                    if (matched != null) CurrentPlayer = matched;

                    for (int row = 0; row < CurrentBoard.Height; row++)
                    {
                        string? line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        for (int col = 0; col < Math.Min(line.Length, CurrentBoard.Width); col++)
                        {
                            char symbol = line[col];
                            if (symbol != '.')
                            {
                                CurrentBoard.PlacePiece(row, col, new Piece(symbol.ToString()));
                            }
                        }
                    }

                    Console.WriteLine("GomokuGame loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load GomokuGame: " + ex.Message);
            }
        }

    }
}
