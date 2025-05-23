namespace PlayerBoardGame
{
    public class HumanPlayer : Player
    {
<<<<<<< HEAD
        public HumanPlayer(string name, Piece piece) : base( name, piece) { }
        public override (Game.GameCommand command, Move? moveDetails) GetMove(Board currentBoard)
        {
            //HumanPlayer will get message until get a valid piece move
            while (true)
            {
                Console.Write($"{Name},Please enter your move ( '0,0' or 'undo', 'help'): ");
                string? rawInput = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrEmpty(rawInput))
                {
                    Console.WriteLine("Invalid input, cannot be empty.");
                    continue; 
                }

                Game.GameCommand systemCommand = ParseGameCommand(rawInput);
                if (systemCommand != Game.GameCommand.None) 
                {
                    return (systemCommand, null); 
=======
        public HumanPlayer(string name, Piece piece) : base(name, piece) { }
        public override (Game.GameCommand command, Move? moveDetails) GetMove(Board currentBoard)
        {
            Console.WriteLine();
            if (currentBoard is NotaktoBoard notaktoBoard)
            {
                Console.WriteLine("\n--Enter Notakto move:");
                Console.WriteLine($"\nFormat: 'boardIndex,row,col' (e.g., '1,1,1' for board 1, row 1, col 1)");
                Console.WriteLine($"\nRange: Board [1-{notaktoBoard.SubBoardCount}], Row [1-{notaktoBoard.GetSubBoard(0).Height}], Col [1-{notaktoBoard.GetSubBoard(0).Width}]");
            }

            else if (currentBoard is TicTacToeBoard numericalBoard)
            {
                int boardN = currentBoard.Width;
                int maxNumber = boardN * boardN;
                int targetSum = boardN * (boardN * boardN + 1) / 2;
                Console.WriteLine("Enter Numerical Tic-Tac-Toe move:");
                Console.WriteLine($"\nFormat: 'row,col,number' (e.g., '1,1,5')");
                Console.WriteLine($"\nRange: Row [1-{numericalBoard.Height}], Col [1-{numericalBoard.Width}], Number [1 - {maxNumber}]");
                Console.WriteLine($"\n To Win: Get {boardN} numbers in a line (row, column, or diagonal) that sum up to {targetSum}.");

            }
            else // For Gomoku
            {
                Console.WriteLine("Enter your move with correct format and range.");
                Console.WriteLine($"\nFormat: 'row,col' (e.g., '1,1' for row 1, col 1)");
                Console.WriteLine($"\nRange: Row [1-{currentBoard.Height}], Col [1-{currentBoard.Width}]");
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n---Available commands: u(undo), r(redo), s(save), l(load), q(quit), h(help)---");
            Console.WriteLine("\n---Enter 'help' to learn more about each commands used for---");
            Console.ResetColor();

            //HumanPlayer will get message until get a valid piece move
            while (true)
            {
                Console.Write($"\n{Name}, please enter your action: ");
                string? rawInput = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine($"\nYou entered: {rawInput}");//DEBUG

                if (string.IsNullOrEmpty(rawInput))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid input, cannot be empty.");
                    Console.ResetColor();
                    continue;
                }

                Game.GameCommand systemCommand = ParseGameCommand(rawInput);
                if (systemCommand != Game.GameCommand.None)
                {
                    return (systemCommand, null);
>>>>>>> 4387e5b (copy file from local)
                }

                Move? placementMove = ParsePlacementMove(rawInput, currentBoard);
                if (placementMove != null)
                {
<<<<<<< HEAD
                    return (Game.GameCommand.MakeMove, placementMove); 
                }
                Console.WriteLine("Invalid move, please enter 'row, col'(e.g.(0, 1))");
                if (currentBoard is NotaktoBoard)
                {
                    Console.WriteLine("For Notakto Game, Please enter boardIndex, row, col(e.g. '1,0,0')");
                }
                Console.WriteLine("Or enter 'help can check command info");
            }
=======
                    return (Game.GameCommand.MakeMove, placementMove);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid move, please enter Correct formation.");
                Console.ResetColor();
                Console.WriteLine("\nPlease refer to the format examples above, or type 'h' (help) for detailed command information.");
                if (currentBoard is NotaktoBoard)
                {
                    Console.WriteLine("\nFor Notakto Game, Ensure the format is ' boardIndex, row, col '(e.g., '1,1,1'). Board index is 1-based.");
                }
                Console.WriteLine("\nOr Enter 'help can check command info");
            }
        }

        //Parse system commands
        private Game.GameCommand ParseGameCommand(string? input)
        {
            if (string.IsNullOrEmpty(input)) return Game.GameCommand.None;
            return input switch
            {
                "u" or "undo" => Game.GameCommand.Undo,
                "r" or "redo" => Game.GameCommand.Redo,
                "s" or "save" => Game.GameCommand.Save,
                "l" or "load" => Game.GameCommand.Load,
                "h" or "help" => Game.GameCommand.Help,
                "q" or "quit" => Game.GameCommand.Quit,
                _ => Game.GameCommand.None
            };
        }

       
        //parse user input , seperate logic for 3 games
        private Move? ParsePlacementMove(string? input, Board currentBoard)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string[] parts = input.Split(',');

            if (currentBoard is NotaktoBoard notaktoBoard)
            {
                if (parts.Length == 3 &&
                    int.TryParse(parts[0].Trim(), out int boardIdxOneBased) &&
                    int.TryParse(parts[1].Trim(), out int rowOneBased) &&
                    int.TryParse(parts[2].Trim(), out int colOneBased))
                {
                    int subBoardIndex = boardIdxOneBased - 1;
                    int rowZeroBased = rowOneBased - 1;
                    int colZeroBased = colOneBased - 1;
                    return new NotaktoMove(this, subBoardIndex, rowZeroBased, colZeroBased, this.PlayerPiece, currentBoard.Clone());
                }
            }
            else if (currentBoard is TicTacToeBoard && IsTicTacToeGame())
            {
                if (parts.Length == 3 &&
                    int.TryParse(parts[0].Trim(), out int rowOneBased) &&
                    int.TryParse(parts[1].Trim(), out int colOneBased) &&
                    int.TryParse(parts[2].Trim(), out int numberChosen))
                {
                    int rowZeroBased = rowOneBased - 1;
                    int colZeroBased = colOneBased - 1;

                    Piece pieceWithNumber = new Piece(numberChosen.ToString());
                    return new Move(this, rowZeroBased, colZeroBased, pieceWithNumber, currentBoard.Clone());
                }
            }
            else
            {
                //For Gomoku
                if (parts.Length == 2 &&
                    int.TryParse(parts[0].Trim(), out int rowOneBased) &&
                    int.TryParse(parts[1].Trim(), out int colOneBased))
                {
                    int rowZeroBased = rowOneBased - 1;
                    int colZeroBased = colOneBased - 1;
                    return new Move(this, rowZeroBased, colZeroBased, this.PlayerPiece, currentBoard.Clone());
                }
            }
            return null;
        }

        private bool IsTicTacToeGame()
        {
            return this.PlayerPiece.Symbol == "Odd" || this.PlayerPiece.Symbol == "Even";
>>>>>>> 4387e5b (copy file from local)
        }

        //Parse system commands
        private Game.GameCommand ParseGameCommand(string input)
        {
            return input switch
            {
                "u" or "undo" => Game.GameCommand.Undo,
                "r" or "redo" => Game.GameCommand.Redo,
                "s" or "save" => Game.GameCommand.Save,
                "l" or "load" => Game.GameCommand.Load,
                "h" or "help" => Game.GameCommand.Help,
                "q" or "quit" => Game.GameCommand.Quit,
                _ => Game.GameCommand.None
            };
        }

        //parse user input as move placement(Move / NotaktoMove)
        private Move? ParsePlacementMove(string input, Board currentBoard)
        {
            string[] parts = input.Split(',');

            if (currentBoard is NotaktoBoard notaktoBoard) 
            {
                if (parts.Length == 3 &&
                    int.TryParse(parts[0].Trim(), out int boardIdxOneBased) && 
                    int.TryParse(parts[1].Trim(), out int row) &&         
                    int.TryParse(parts[2].Trim(), out int col))           
                {
                    int subBoardIndex = boardIdxOneBased - 1; 
                    return new NotaktoMove(this, subBoardIndex, row, col, this.PlayerPiece, currentBoard.Clone());
                }
            }
            else 
            {
                //For Tictactoe, Gomoku
                if (parts.Length == 2 &&
                    int.TryParse(parts[0].Trim(), out int row) && 
                    int.TryParse(parts[1].Trim(), out int col))   
                {
                    return new Move(this, row, col, this.PlayerPiece, currentBoard.Clone());
                }
            }
            return null; 
        } 
    }
}