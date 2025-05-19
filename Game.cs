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
                Console.WriteLine("Type 'help' for commands (undo, redo, save, load, quit) or enter your move.");

                IMoveCommand moveCommand = CurrentPlayer.GetMove(CurrentBoard);
                Move ? playerInput = moveCommand?.MoveData;
                
                GameCommand command = ParseGameCommand(playerInput);

                switch (command)
                {
                    case GameCommand.Undo:
                        if (UndoMove()) continue;
                        break;
                    case GameCommand.Redo:
                        if (RedoMove()) continue;
                        break;
                    case GameCommand.Save:
                        SaveGame();
                        continue;
                    case GameCommand.Load:
                        LoadGame();
                        continue;
                    case GameCommand.Help:
                        DisplayHelp();
                        continue;
                    case GameCommand.Quit:
                        Console.WriteLine("You chose to quit the game.");
                        IsGameOver = true;
                        break;
                    case GameCommand.None:
                        //process regular move
                        if (playerInput != null && IsMoveValid(playerInput))
                        {
                            ApplyMove(playerInput);

                            Player ? winner = CheckWinCondition();
                            if (winner != null)
                            {
                                IsGameOver = true;
                                DisplayBoard();
                                Console.WriteLine($"\nGame Over! {winner.Name} ({winner.PlayerPiece?.Symbol}) wins!");
                            }
                            else if (CheckDrawCondition())
                            {
                                IsGameOver = true;
                                DisplayBoard();
                                Console.WriteLine("\nGame Over! It's a draw!");
                            }
                            if (!IsGameOver) SwitchPlayer();
                        }
                        else
                        {
                            Console.WriteLine("Invalid move, please try again.");
                        }
                        break;

                }
   
            }
            Console.WriteLine("Game Over!");
        }

        private GameCommand ParseGameCommand(Move? move)
        {
            if (move == null) return GameCommand.None;
            return move.Row switch
            {
                -1 => GameCommand.Undo,
                -2 => GameCommand.Redo,
                -3 => GameCommand.Save,
                -4 => GameCommand.Load,
                -5 => GameCommand.Help,
                -6 => GameCommand.Quit,
                _ => GameCommand.None

            };
        }

        protected abstract Board CreateInitialBoard();
        protected abstract List<Player> CreatePlayers(GameMode mode);
        protected abstract bool IsMoveValid(Move move);
        protected abstract Player CheckWinCondition();
        protected abstract bool CheckDrawCondition();

        //Apply a move using Command Pattern
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
            return new PlacePieceCommand(this.CurrentBoard, move);
        }

        //Switches to the next Player
        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Players[0]) ? Players[1] : Players[0];
        }

        //Allows undo and redo functionality
        public virtual bool UndoMove()
        {
            if (MoveHistory.Any())
            {
                IMoveCommand commandToUndo = MoveHistory.Last();
                MoveHistory.RemoveAt(MoveHistory.Count - 1);

                commandToUndo.Undo();

                RedoHistory.Add(commandToUndo);
                IsGameOver = false;
                Console.WriteLine("Move undone.");
                return true;
            }
            Console.WriteLine("No moves to undo");
            return false;
        }
        public virtual bool RedoMove()
        {
            if (RedoHistory.Any())
            {
                IMoveCommand commandToRedo = RedoHistory.Last();
                RedoHistory.RemoveAt(RedoHistory.Count - 1);

                commandToRedo.Execute(); 
                         
                MoveHistory.Add(commandToRedo);
                IsGameOver = false; 
                Console.WriteLine("Move redone.");
                return true;
            }
            Console.WriteLine("No moves to redo.");
            return false;
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
            Console.WriteLine("Enter your move based on game prompts (e.g., 'row,col').");
            Console.WriteLine("For Notakto, use 'boardIndex,row,col' (1-based index for board, 0-based for row/col).");
            Console.WriteLine("Available commands:");
            Console.WriteLine("  undo          - Revert the last move.");
            Console.WriteLine("  redo          - Perform the last undone move again.");
            Console.WriteLine("  save          - Save the current game.");
            Console.WriteLine("  load          - Load a previously saved game (current game will end).");
            Console.WriteLine("  quit          - Exit the current game.");
            Console.WriteLine("  help          - Show this help information.");
            Console.WriteLine("--------------");
        }

        //public method for computerPlayer access
        public Player? GetWinner()
        {
            return CheckWinCondition();
        }
    }
}