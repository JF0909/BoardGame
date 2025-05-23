namespace PlayerBoardGame
{
    /// <summary>
    /// Numerical TicTacToe logic
    /// Players use odd/even numbers on an N*N board.
    /// The first to get N numbers in a line summing to a target S wins
    /// </summary>
    public class TicTacToeGame : Game
    {
        private int _boardSize;
        private int _targetSum;

        // Tracks numbers already played
        private HashSet<int> _usedNumbers;


        public TicTacToeGame(GameMode mode, int size)
        {
            //validation for size, N must be at least 3
            if (size < 3)
            {
                Console.WriteLine($"\nWarning: Board size {size} is too small for standard Numerical Tic-Tac-Toe. Defaulting to 3.");
                size = 3;
            }
            GameMode = mode;
            _boardSize = size;
            _targetSum = _boardSize * (_boardSize * _boardSize + 1) / 2;
            _usedNumbers = new HashSet<int>();
        }

        protected override Board CreateInitialBoard()
        {
            return new TicTacToeBoard(_boardSize);
        }

        //Creates players, assigning them "Odd" or "Even" roles
        protected override List<Player> CreatePlayers(GameMode mode)
        {
            var players = new List<Player>();
            // Player 1 uses "Odd" numbers, Player 2 (or Computer) uses "Even" numbers.
            players.Add(new HumanPlayer("Player 1 (Odd)", new Piece("Odd")));
            if (mode == GameMode.HumanVsHuman)
            {
                players.Add(new HumanPlayer("Player 2 (Even)", new Piece("Even")));
            }
            else // HvC
            {
                players.Add(new ComputerPlayer("Computer (Even)", new Piece("Even")));
            }
            return players;
        }

        //Validate move for numerical TTT game
        protected override bool IsMoveValid(Move move)
        {
            if (CurrentBoard == null || move == null || move.Player == null || move.PiecePlaced == null) return false;

            //Check if the position is valid on the board.
            if (!CurrentBoard.IsValidPosition(move.Row, move.Col))
            {
                Console.WriteLine("\nError: Position is outside the board.");
                return false;
            }

            // Check if the cell is empty.
            if (!CurrentBoard.IsCellEmpty(move.Row, move.Col))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError: Cell is already occupied.");
                Console.ResetColor();
                return false;
            }

            //Check if the placed piece's symbol can be parsed as a number.
            if (!int.TryParse(move.PiecePlaced.Symbol, out int number))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: '{move.PiecePlaced.Symbol}' is not a valid number.");
                Console.ResetColor();
                return false;
            }

            //Check if the number is within the allowed range (1 to N*N).
            int maxNumber = _boardSize * _boardSize;
            if (number < 1 || number > maxNumber)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n--Error: Number must be between 1 and {maxNumber}.--");
                Console.ResetColor();
                return false;
            }

            //Check if the number has already been used
            if (_usedNumbers.Contains(number))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n--Error: Number {number} has already been used.--");
                Console.ResetColor();
                return false;
            }

            bool isPlayerOdd = move.Player.PlayerPiece.Symbol == "Odd";
            bool numberIsOdd = (number % 2 != 0);

            if (isPlayerOdd && !numberIsOdd)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {move.Player.Name} (Odd) cannot play an even number ({number}).");
                Console.ResetColor();
                return false;
            }
            if (!isPlayerOdd && numberIsOdd)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {move.Player.Name} (Even) cannot play an odd number ({number}).");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a specific command for placing a numerical piece, to handle _usedNumbers.
        /// </summary>
        protected override IMoveCommand CreateMoveCommand(Move move)
        {
            if (int.TryParse(move.PiecePlaced.Symbol, out int numberValue))
            {
                return new TicTacToeCommand(CurrentBoard!, move, _usedNumbers, numberValue);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: Could not parse piece symbol '{move.PiecePlaced.Symbol}' as number in CreateMoveCommand.");
            Console.ResetColor();
            throw new InvalidOperationException("Cannot create command for non-numerical piece in NumericalTicTacToe.");
        }

        protected override Player? CheckWinCondition()
        {
            if (CurrentBoard == null || CurrentPlayer == null) return null;

            for (int r = 0; r < _boardSize; r++)
            {
                for (int c = 0; c < _boardSize; c++)
                {
                    if (c <= _boardSize - _boardSize)
                        if (CheckLineSum(r, c, 0, 1)) return CurrentPlayer;

                    if (r <= _boardSize - _boardSize)
                        if (CheckLineSum(r, c, 1, 0)) return CurrentPlayer;

                    if (r == 0 && c == 0)
                        if (CheckLineSum(r, c, 1, 1)) return CurrentPlayer;

                    if (r == 0 && c == _boardSize - 1)
                        if (CheckLineSum(r, c, 1, -1)) return CurrentPlayer;
                }
            }
            return null;
        }

        //helper method check line sum
        private bool CheckLineSum(int startRow, int startCol, int dr, int dc)
        {
            int currentSum = 0;
            int numbersInLine = 0;
            List<Piece?> piecesInLine = new List<Piece?>();

            for (int i = 0; i < _boardSize; i++)
            {
                int r = startRow + i * dr;
                int c = startCol + i * dc;

                if (!CurrentBoard!.IsValidPosition(r, c))
                {
                    return false;
                }

                Piece? piece = CurrentBoard.GetPiece(r, c);
                if (piece != null && int.TryParse(piece.Symbol, out int numberValue))
                {
                    piecesInLine.Add(piece);
                    currentSum += numberValue;
                    numbersInLine++;
                }
                else
                {
                    return false;
                }
            }

            // After checking N cells in the line:
            if (numbersInLine == _boardSize && currentSum == _targetSum)
            {
                return true;
            }
            return false;
        }

        protected override bool CheckDrawCondition()
        {
            return _usedNumbers.Count == _boardSize * _boardSize && CheckWinCondition() == null;
        }

        // Save method override
        public override void SaveGame(string fileName)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullPath = Path.Combine(desktopPath, fileName);

                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.WriteLine(this.GetType().Name);
                    writer.WriteLine(_boardSize);
                    writer.WriteLine(CurrentPlayer?.Name);
                    writer.WriteLine(string.Join(",", _usedNumbers)); // Store the used numbers

                    for (int row = 0; row < CurrentBoard.Height; row++)
                    {
                        for (int col = 0; col < CurrentBoard.Width; col++)
                        {
                            var piece = CurrentBoard.GetPiece(row, col);
                            writer.Write(piece != null ? piece.Symbol : ".");
                        }
                        writer.WriteLine();
                    }
                }

                Console.WriteLine("TicTacToeGame saved to Desktop.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save TicTacToeGame: " + ex.Message);
            }
        }
        
        // Load method override
        /// <summary>
        /// Loads the game state for Numerical TicTacToe from a file.
        /// </summary>
        /// <param name="filePath">Path to the save file</param>
        public override void LoadGame(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string gameType = reader.ReadLine(); // Read game type (not used)
                    int size = int.Parse(reader.ReadLine()); // Read board size
                    string currentPlayerName = reader.ReadLine(); // Read current player's name

                    _boardSize = size;
                    _targetSum = _boardSize * (_boardSize * _boardSize + 1) / 2;
                    _usedNumbers.Clear();

                    CurrentBoard = new TicTacToeBoard(_boardSize);

                    // Set current player from loaded name
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
                                if (int.TryParse(symbol.ToString(), out int num))
                                {
                                    _usedNumbers.Add(num);
                                }
                            }
                        }
                    }

                    Console.WriteLine("TicTacToeGame loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load TicTacToeGame: " + ex.Message);
            }
        }
    }
}