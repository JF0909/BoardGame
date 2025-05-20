using System.Reflection;

namespace PlayerBoardGame
{
    /// <summary>
    /// Abstract base class for all games.
    /// Define core game logic and structure using the template method pattern.
    /// </summary>
    public abstract class Game
    {
        //Enum for game command
        public enum GameCommand { Undo, Redo, Save, Load, Help, Quit, None }
       
        //Property
        protected Board CurrentBoard;
        protected List<Player> Players;
        protected Player CurrentPlayer;
        protected List<IMoveCommand> MoveHistory = new List<IMoveCommand>();
        protected List<IMoveCommand> RedoHistory = new List<IMoveCommand>();
        public GameMode GameMode { get; protected set; }
        public bool IsGameOver { get; protected set; }

        //Core Game cycle methods
        public virtual void StartGame()
        {
            CurrentBoard = CreateInitialBoard();
            Players = CreatePlayers(GameMode);
            if (Players == null || !Players.Any())
            {
                throw new InvalidOperationException("Players list cannot be null or empty after CreatePlayers.");
            }
            CurrentPlayer = Players[0];
            IsGameOver = false;
            MoveHistory.Clear();
            RedoHistory.Clear();
            Console.WriteLine($"\n--- {this.GetType().Name} Game Started ({GameMode}) ---");
        }

        //Main Game Loop to handle turns
        public void RunGameLoop()
        {
            if (CurrentBoard == null || Players == null || CurrentPlayer == null )
            {
                StartGame();
            }
            
            while (!IsGameOver)
            {
                DisplayBoard();
                Console.WriteLine($"\n{CurrentPlayer.Name}'s turn ({CurrentPlayer.PlayerPiece?.Symbol}).");
                Console.WriteLine("Type 'help' for commands (undo, redo, save, load, quit) or Enter your move.");

                string? playerInput = Console.ReadLine()? .Trim().ToLower();
                GameCommand command = ParseGameCommand(playerInput);

                if (command != GameCommand.None)
                {
                    ProcessGameCommand(command);
                    continue;
                }

                Move? move = ParseMoveInput(playerInput);
                if (move != null && IsMoveValid(move))
                {
                    ApplyMove(move);
                    HandleGamestate();
                }
                else
                {
                    Console.WriteLine("Invalid move, please try again.");
                }
            }
            Console.WriteLine("Game Over!");
        }

        //Parse system commands
        private GameCommand ParseGameCommand(string input)
        {
            return input switch
            {
                "u" or "undo" => GameCommand.Undo,
                "r" or "redo" => GameCommand.Redo,
                "s" or "save" => GameCommand.Save,
                "l" or "load" => GameCommand.Load,
                "h" or "help" => GameCommand.Help,
                "q" or "quit" => GameCommand.Quit,
                _ => GameCommand.None
            };
        }

        //Parse player move input
        private Move? ParseMoveInput(string input)
        {
            string[] parts = input.Split(',');
            if (parts.Length == 2 && int.TryParse(parts[0], out int row) && int.TryParse(parts[1], out int col))
            {
                return new Move(CurrentPlayer, row, col, CurrentPlayer.PlayerPiece, CurrentBoard.Clone());
            }
            return null;
        }

        //Executes game commands
        private void ProcessGameCommand(GameCommand command)
        {
            switch (command)
            {
                case GameCommand.Undo:
                    UndoMove();
                    break;
                case GameCommand.Redo:
                    RedoMove();
                    break;
                case GameCommand.Save:
                    SaveGame();
                    break;
                case GameCommand.Load:
                    LoadGame();
                    break;
                case GameCommand.Help:
                    DisplayHelp();
                    break;
                case GameCommand.Quit:
                    Console.WriteLine("You chose to quit the game.");
                    IsGameOver = true;
                    break;
            }
        }
        

        //Apply a move 
        protected virtual void ApplyMove(Move move)
        {
            IMoveCommand command = CreateMoveCommand(move);
            if (command != null)
            {
                command.Execute();
                MoveHistory.Add(command);
                RedoHistory.Clear();
            }
            else
            {
                Console.WriteLine("Error: Invalid move command.");
            }
            
        }

        protected virtual IMoveCommand CreateMoveCommand(Move move)
        {
            return new PlacePieceCommand(CurrentBoard, move);
        }

        //Handle game state win/draw
        private void HandleGamestate()
        {
            Player? winner = CheckWinCondition();
            if (winner != null)
            {
                IsGameOver = true;
                DisplayBoard();
                Console.WriteLine($"\nGame Over! {winner.Name} ({winner.PlayerPiece?.Symbol}) wins!");
                return;
            }

            if (CheckDrawCondition())
            {
                IsGameOver = true;
                DisplayBoard();
                Console.WriteLine("\nGame Over! It's a draw!");
                return;
            }
            SwitchPlayer();

        }
        
        //Switches to the next Player
        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Players[0]) ? Players[1] : Players[0];
        }

        //Undo Move Command
        public virtual bool UndoMove()
        {
            new UndoCommand(MoveHistory, RedoHistory).Execute();
            return !IsGameOver;
        }

        //Redo Move Command
        public virtual bool RedoMove()
        {
            new RedoCommand(RedoHistory, MoveHistory).Execute();
            return !IsGameOver;
        }

        //Display the game board
        public void DisplayBoard()
        {
            CurrentBoard.Display();
        }

        public void SaveGame(string filePath)
        {
            // TODO
        }

        //Load a saved game
        public void LoadGame(string filePath)
        {
            // TODO
        }
        protected virtual void DisplayHelp()
        {
            Console.WriteLine("\n--- Help ---");
            Console.WriteLine("Enter your move as row, col");
            Console.WriteLine("For Notakto, use 'boardIndex,row,col' (1-based index for board, 0-based for row/col).");
            Console.WriteLine("Available commands:");
            Console.WriteLine(" U or undo         - Undo the last move.");
            Console.WriteLine(" R or redo          - Redo the last undone move again.");
            Console.WriteLine(" S or save          - Save the current game.");
            Console.WriteLine(" L or load          - Load a saved game.");
            Console.WriteLine(" Q or quit          - Exit the current game.");
            Console.WriteLine(" H or help          - Show this help information.");
            Console.WriteLine("--------------");
        }

        //Abstract methods for child games

        protected abstract Board CreateInitialBoard();
        protected abstract List<Player> CreatePlayers(GameMode mode);
        protected abstract bool IsMoveValid(Move move);
        protected abstract Player CheckWinCondition();
        protected abstract bool CheckDrawCondition();

        //public method for computerPlayer access
        public Player? GetWinner()
        {
            return CheckWinCondition();
        }
    }
}