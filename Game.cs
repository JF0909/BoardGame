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
        public enum GameCommand { Undo, Redo, Save, Load, Help, Quit, MakeMove, Invalid, None }
       
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
            if (Players == null || !Players.Any() || Players.Count == 0)
            {
                throw new InvalidOperationException("Players list cannot be null or empty after CreatePlayers.");
            }
            CurrentPlayer = Players[0];
            IsGameOver = false;
            MoveHistory.Clear();
            RedoHistory.Clear();
            Console.WriteLine($"\n--- {this.GetType().Name} Game Started ({GameMode}) ---");
        }

        //Main Game Loop
        public void RunGameLoop()
        {
            if (CurrentBoard == null || Players == null || Players.Count == 0 || CurrentPlayer == null )
            {
                StartGame();
            }
            
            while (!IsGameOver)
            {
                DisplayBoard();
                Console.WriteLine($"\n{CurrentPlayer.Name}'s turn ({CurrentPlayer.PlayerPiece?.Symbol}).");
                //Console.WriteLine("Type 'help' for commands (undo, redo, save, load, quit) or Enter your move.");
                var (intendedCommand, moveData) = CurrentPlayer.GetMove(CurrentBoard);
                
                if (IsGameOver)
                {
                    Console.WriteLine("Game Over inaccidently.");
                    break;
                }

                switch(intendedCommand)
                {
                    case GameCommand.MakeMove: 
                        if (moveData != null)
                        {
                            if (IsMoveValid(moveData)) 
                            {
                                ApplyMove(moveData);
                                HandleGamestate();
                            }
                            else
                            {
                                if (CurrentPlayer is HumanPlayer)
                                {
                                    Console.WriteLine("Invalid move, Please try again.");
                                }
                            }
                        }
                        else 
                        {
                             Console.WriteLine("Error, no move details");
                        }
                        break;

                    case GameCommand.Invalid: 
                        if (CurrentPlayer is HumanPlayer)
                        {
                            Console.WriteLine("Input Cannot be recgnized. Enter 'help' to check solution。");
                        }
                        break;

                    case GameCommand.None: 
                                         
                        if (CurrentPlayer is HumanPlayer) {
                            Console.WriteLine("Please enter a valid command");
                        }
                        break;

                    default: 
                        ProcessSystemCommand(intendedCommand);
                        break;
                }

                if (IsGameOver && intendedCommand != GameCommand.Quit) { 
                    Console.WriteLine("Game Session Over");
                } else if (!IsGameOver) {
                    Console.WriteLine("Exit game loop inaccidently.");
                } else if (intendedCommand == GameCommand.Quit) {
                    Console.WriteLine("Thanks for playing. Bye.");
                } else {
                    Console.WriteLine("Game Over!"); 
                } 
            }
        }

        // ProcessSystemCommand
        private void ProcessSystemCommand(GameCommand command)
        {
            switch (command)
            {
                case GameCommand.Undo:
                    if (UndoMove()) DisplayBoard(); 
                    break;
                case GameCommand.Redo:
                    if (RedoMove()) DisplayBoard(); 
                    break;
                case GameCommand.Save:
                    Console.Write("Enter the saved file name (e.g.game.sav): ");
                    string? savePath = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(savePath)) SaveGame(savePath);
                    else Console.WriteLine("Save Cancelled");
                    break;
                case GameCommand.Load:
                    Console.Write("Please enter the file name ");
                    string? loadPath = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(loadPath))
                    {
                        LoadGame(loadPath);
                        DisplayBoard(); 
                        Console.WriteLine($"Load game successfully, it's {CurrentPlayer?.Name}'s turn");
                    }
                    else Console.WriteLine(" Load cancelled..");
                    break;
                case GameCommand.Help:
                    DisplayHelp();
                    break;
                case GameCommand.Quit:
                    Console.WriteLine("You choose to exit the game.");
                    IsGameOver = true;
                    break;
                default: 
                    Console.WriteLine($"None: {command}");
                    break;
            }
        }
        
        //Apply a move 
        protected virtual void ApplyMove(Move move)
        {
            IMoveCommand command = CreateMoveCommand(move);
            command.Execute();
            MoveHistory.Add(command);
            RedoHistory.Clear();
        }

        protected virtual IMoveCommand CreateMoveCommand(Move move)
        {
            // NotaktoGame must override this to return a NotaktoCommand for a NotaktoMove
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
            if (Players != null && Players.Count > 1)
            {
                int currentPlayerIndex = Players.IndexOf(CurrentPlayer);
                CurrentPlayer = Players[(currentPlayerIndex + 1) % Players.Count];
            }
        }

        //Undo Move Command: TODO
        public virtual bool UndoMove()
        {
            if (MoveHistory.Any())
            {
                new UndoCommand(MoveHistory, RedoHistory).Execute();
                IsGameOver = false; 
                if (Players.Count > 1) SwitchPlayer();                                 
                Console.WriteLine("Move Undone.");
                return true;
            }
            Console.WriteLine("No moves to undo.");
            return false;
        }

        //Redo Move Command
        public virtual bool RedoMove()
        {
            if (RedoHistory.Any())
            {
                new RedoCommand(RedoHistory, MoveHistory).Execute();
                IsGameOver = false; 
                Console.WriteLine("Move Redone.");
                return true;
            }
            Console.WriteLine("No moves to redo.");
            return false;
        }

        //Display the game board
        public void DisplayBoard()
        {
            Console.Clear();
            Console.WriteLine($"\n--- {this.GetType().Name} ({GameMode}) ---");
            if (CurrentBoard != null) CurrentBoard.Display();
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
        protected abstract Player? CheckWinCondition();
        protected abstract bool CheckDrawCondition();

        //public method for computerPlayer access
        public Player? GetWinner()
        {
            return CheckWinCondition();
        }
    }
}