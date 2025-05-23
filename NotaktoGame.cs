namespace PlayerBoardGame
{
    /// <summary>
    /// Notakto Game Rules
    /// The game ends when all three sub-boards have a three-in-a-row.
    /// The player who makes the move that completes the third and final sub-board loses the game
    /// </summary>
    public class NotaktoGame : Game
    {
<<<<<<< HEAD
        private NotaktoBoard MainNotaktoBoard => CurrentBoard as NotaktoBoard;
=======
        private NotaktoBoard? MainNotaktoBoard => CurrentBoard as NotaktoBoard;
>>>>>>> 4387e5b (copy file from local)
        private readonly Piece SharedPiece = new Piece("X");
        public NotaktoGame(GameMode mode)
        {
            this.GameMode = mode;
        }

        protected override Board CreateInitialBoard()
        {
            return new NotaktoBoard();
        }

        protected override List<Player> CreatePlayers(GameMode mode)
        {
            var players = new List<Player>();
            //Both player use 'X'
            players.Add(new HumanPlayer("Player 1", SharedPiece));
            if (mode == GameMode.HumanVsHuman)
            {
                players.Add(new HumanPlayer("Player 2", SharedPiece));
            }
            else
            {
                //HumanVsComputer
                players.Add(new ComputerPlayer("Computer", SharedPiece));
            }
            return players;
        }

        protected override bool IsMoveValid(Move move)
        {
            if (move == null || MainNotaktoBoard == null || move.Player != CurrentPlayer)
            {
<<<<<<< HEAD
                Console.WriteLine("Basic Move Validation Failed: null move/board/not Current Player");
=======
                Console.WriteLine("\nBasic Move Validation Failed: null move/board/not Current Player");
>>>>>>> 4387e5b (copy file from local)
                return false;
            }
            if (!(move is NotaktoMove notaktoMove))
            {
<<<<<<< HEAD
                Console.WriteLine("Error: Move is not a NotaktoMove for NotaktoGame.");
=======
                Console.WriteLine("\nError: Move is not a NotaktoMove for NotaktoGame.");
>>>>>>> 4387e5b (copy file from local)
                return false;
            }
            if (notaktoMove.SubBoardIndex < 0 || notaktoMove.SubBoardIndex >= MainNotaktoBoard.SubBoardCount)
            {
<<<<<<< HEAD
                Console.WriteLine($"Error: Invalid sub-board index {notaktoMove.SubBoardIndex + 1}.");
=======
                Console.WriteLine($"\nError: Invalid sub-board index {notaktoMove.SubBoardIndex + 1}.");
>>>>>>> 4387e5b (copy file from local)
                return false;
            }
            if (MainNotaktoBoard.IsSubBoardFinished(notaktoMove.SubBoardIndex))
            {
<<<<<<< HEAD
                Console.WriteLine($"Error: Sub-board {notaktoMove.SubBoardIndex + 1} is already finished (IsMoveValid check).");
=======
                Console.WriteLine($"\nError: Sub-board {notaktoMove.SubBoardIndex + 1} is already finished.");
>>>>>>> 4387e5b (copy file from local)
                return false;
            }

            TicTacToeBoard targetSubBoard = MainNotaktoBoard.GetSubBoard(notaktoMove.SubBoardIndex);
            return targetSubBoard.IsValidPosition(notaktoMove.Row, notaktoMove.Col)
            && targetSubBoard.IsCellEmpty(notaktoMove.Row, notaktoMove.Col);

        }

        protected override IMoveCommand CreateMoveCommand(Move move)
        {
            if (CurrentBoard == null)
            {
<<<<<<< HEAD
                Console.WriteLine("Error: CurrentBoard is null in NotaktoGame.");
                throw new InvalidOperationException("Cannot create a move command without a valid board.");
=======
                Console.WriteLine("\nError: CurrentBoard is null in NotaktoGame.");
                throw new InvalidOperationException("\nCannot create a move command without a valid board.");
>>>>>>> 4387e5b (copy file from local)
            }

            if (CurrentBoard is NotaktoBoard currentNotaktoBoard)
            {
                if (move is NotaktoMove notaktoMove)
                {
                    return new NotaktoCommand(currentNotaktoBoard, notaktoMove);
                }
                else
                {
                    Console.WriteLine("Error: CreateMoveCommand in NotaktoGame received a non-NotaktoMove.");
                    throw new ArgumentException("Move must be a NotaktoMove for NotaktoGame's CreateMoveCommand.", nameof(move));
                }
            }
            else
            {
                Console.WriteLine("Error: CurrentBoard in NotaktoGame is not a NotaktoBoard instance during CreateMoveCommand.");
                throw new InvalidOperationException("CurrentBoard is not of type NotaktoBoard in NotaktoGame.");
            }
        }

        protected override Player CheckWinCondition()
        {
<<<<<<< HEAD
            if(MainNotaktoBoard == null) return null;
            if (MainNotaktoBoard.AreAllSubBoardsFinished())
            {
                //Notakto Rules: this currentPlayer loses
                Console.WriteLine($"\nGAME OVER! All sub-boards are finished.");
                Console.WriteLine($"{CurrentPlayer.Name} made the final move and LOSES!");
                //Try to find another player
                Player winner = Players.FirstOrDefault(p => p != CurrentPlayer);
                if (winner != null)
                {
                    Console.WriteLine($"{winner.Name} WINS!");
                }
                else
                {
                    Console.WriteLine($" Unexpected Outcome.");
                }
                return winner;
            }
            return null;
=======
            if (MainNotaktoBoard == null) return null!;
            if (MainNotaktoBoard.AreAllSubBoardsFinished())
            {
                //Notakto Rules: this currentPlayer loses
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nGAME OVER! All sub-boards are finished.");
                Console.WriteLine($"\n{CurrentPlayer?.Name} made the Final move and LOSES!");
                Console.ResetColor();
                //Try to find another player
                Player? winner = Players?.FirstOrDefault(p => p != CurrentPlayer);
                if (winner != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n{winner.Name} WINS!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"\n Unexpected Outcome.");
                }
                return winner!;
            }
            return null!;
>>>>>>> 4387e5b (copy file from local)
        }


        protected override bool CheckDrawCondition()
        {
            // Draw will never happen in Notakto Game
            return false;

        }
<<<<<<< HEAD
=======

        protected override void DisplayHelp()
        {
            base.DisplayHelp();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- Notakto Game Rules & Tips ---");
            Console.WriteLine("  - Players take turns placing 'X' on any of the three 3x3 sub-boards.");
            Console.WriteLine("  - A sub-board is 'finished' when a 3-in-a-row of 'X's is formed on it.");
            Console.WriteLine("  - The game ends when all three sub-boards are finished.");
            Console.WriteLine("  - The player who makes the move that finishes the THIRD sub-board LOSES the game.");
            Console.WriteLine("  - Move Format: 'boardIndex,row,col'");
            Console.WriteLine("    (e.g., '1,1,1' for Sub-board 1, Row 1, Column 1. All are 1-based for input).");
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

                Console.WriteLine("NotaktoGame saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save NotaktoGame: " + ex.Message);
            }
        }

        /// <summary>
        /// Loads the Notakto game state from a file.
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

                    CurrentBoard = new NotaktoBoard();

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

                    Console.WriteLine("NotaktoGame loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load NotaktoGame: " + ex.Message);
            }
        }
>>>>>>> 4387e5b (copy file from local)
    }
}