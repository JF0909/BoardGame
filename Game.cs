namespace PlayerBoardGame
{
    /// <summary>
    /// Abstract base class for all games.
    /// Define core game logic and structure using the template method pattern.
    /// </summary>
    public abstract class Game
    {
        //signal for human player commands from HumanPlayer.GetMove()
        public static readonly Move UndoSignal = new Move(null!, -1, -1, null!, null!);
        public static readonly Move RedoSignal = new Move(null!, -2, -1, null!, null!);
        public static readonly Move SaveSignal = new Move(null!, -3, -1, null!, null!);
        public static readonly Move LoadSignal = new Move(null!, -4, -1, null!, null!);
        public static readonly Move HelpSignal = new Move(null!, -5, -1, null!, null!);
        public static readonly Move QuitSignal = new Move(null!, -6, -1, null!, null!);


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

                Move ? playerInput = CurrentPlayer.GetMove(CurrentBoard);

                bool commandProcessedTurns = false;

                //process specific commands
                if (playerInput == UndoSignal)
                {
                    if (UndoMove()) commandProcessedTurns = true;
                   
                }
                else if (playerInput == RedoSignal)
                {
                    if (RedoMove()) commandProcessedTurns = true;
                }
                else if (playerInput == SaveSignal)
                {
                    SaveGame();
                    commandProcessedTurns = true; 
                }
                else if (playerInput == LoadSignal)
                {
                    LoadGame(); // LoadGame will change CurrentPlayer, IsGameOver..
                    commandProcessedTurns = true; 
                }
                else if (playerInput == HelpSignal)
                {
                    DisplayHelp();
                    commandProcessedTurns = true; // Help doesn't end the turn
                }
                else if (playerInput == QuitSignal)
                {
                    Console.WriteLine("You chose to quit the game.");
                    IsGameOver = true;
                    commandProcessedTurns = true;
                }
                else if (playerInput != null)
                {
                    if (IsMoveValid(playerInput))
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

                        if (!IsGameOver)
                        {
                            SwitchPlayer();
                        }
                    }
                    else
                    {
                        //Invalid piece placement move attempt
                        if(CurrentPlayer is HumanPlayer)
                        {
                            Console.WriteLine("That move is not valid, please try again.");
                        }
                    }
                }
                else
                {
                    //if playerinput was null
                    if (CurrentPlayer is HumanPlayer)
                    {
                        Console.WriteLine(" Unrecognized input or invalid move format. Type 'help' for commands.");
                    }
                }

                if (IsGameOver)
                {
                    break;
                }

               
            }
            Console.WriteLine("Game Over!");
        }

        //Create initial board
        protected abstract Board CreateInitialBoard();

        //Create the players based on game mode
        protected abstract List<Player> CreatePlayers(GameMode mode);

        //Validates a move
        protected abstract bool IsMoveValid(Move move);

        //Check if a player has won the game
        protected abstract Player CheckWinCondition();

        //Check for a draw
        protected abstract bool CheckDrawCondition();

        //Apply a move using Command Pattern
        protected virtual void ApplyMove(Move move)
        {
            IMoveCommand command = CreateMoveCommand(move);
            command.Execute();

            MoveHistory.Add(command);
            RedoHistory.Clear();
         
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

        //Save the game state
        public void SaveGame(string filePath)
        {
            // TODO: implement until board, game subclasses are finished

        }

        //Load a saved game
        public void LoadGame(string filePath)
        {
            // TODO: implement until board, game subclasses are finished

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
        
        

    }
}