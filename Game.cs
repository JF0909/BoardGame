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
<<<<<<< HEAD
        protected Board CurrentBoard;
        protected List<Player> Players;
        protected Player CurrentPlayer;
=======
        protected Board? CurrentBoard;
        protected List<Player>? Players;
        protected Player? CurrentPlayer;
>>>>>>> 4387e5b (copy file from local)
        protected List<IMoveCommand> MoveHistory = new List<IMoveCommand>();
        protected List<IMoveCommand> RedoHistory = new List<IMoveCommand>();
        public GameMode GameMode { get; protected set; }
        public bool IsGameOver { get; protected set; }

<<<<<<< HEAD
        //Core Game cycle methods
=======
        //Core Game Methods
>>>>>>> 4387e5b (copy file from local)
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
<<<<<<< HEAD
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
=======
            if (CurrentBoard == null || Players == null || Players.Count == 0 || CurrentPlayer == null)
            {
                StartGame();
            }

            GameCommand intendedCommand = GameCommand.None;

            while (!IsGameOver)
            {
                DisplayBoard();
                Console.WriteLine($"\n{CurrentPlayer?.Name}'s turn ({CurrentPlayer?.PlayerPiece?.Symbol}).");
               
                var (currentTurnCommand, moveData) = CurrentPlayer!.GetMove(CurrentBoard!);
                intendedCommand = currentTurnCommand;

                switch (intendedCommand)
                {
                    case GameCommand.MakeMove:
                        if (moveData != null)
                        {
                            if (IsMoveValid(moveData))
>>>>>>> 4387e5b (copy file from local)
                            {
                                ApplyMove(moveData);
                                HandleGamestate();
                            }
                            else
                            {
                                if (CurrentPlayer is HumanPlayer)
                                {
<<<<<<< HEAD
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
=======
                                    Console.WriteLine("\nInvalid move, Please try again.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nError, no move details");
                        }
                        break;

                    case GameCommand.Invalid:
                        if (CurrentPlayer is HumanPlayer)
                        {
                            Console.WriteLine("\nUnrecognized input or invalid move format. Enter 'help' for commands.");
                        }
                        break;

                    case GameCommand.None:
                        if (CurrentPlayer is HumanPlayer)
                        {
                            Console.WriteLine("\nPlease enter a command or your move.");
                        }
                        break;

                    default:
                        ProcessSystemCommand(intendedCommand);
                        if (intendedCommand == GameCommand.Undo ||
                            intendedCommand == GameCommand.Redo ||
                            intendedCommand == GameCommand.Load)
                        {
                            //needsFullDisplayedRefresh = true;
                        }
                        break;

                }

                if (IsGameOver)
                {
                    break;
                }

            }
            
            if (intendedCommand == GameCommand.Quit)
            {
                Console.WriteLine("\nGame session exited by Player.");
            }
            else if (CheckWinCondition() != null || CheckDrawCondition())
            {
                Console.WriteLine("--- Game Concluded ---");
            }
            else if (IsGameOver)
            {
                Console.WriteLine("\nGame Over!");
            }
            
>>>>>>> 4387e5b (copy file from local)
        }

        // ProcessSystemCommand
        private void ProcessSystemCommand(GameCommand command)
        {
            switch (command)
            {
                case GameCommand.Undo:
<<<<<<< HEAD
                    if (UndoMove()) DisplayBoard(); 
                    break;
                case GameCommand.Redo:
                    if (RedoMove()) DisplayBoard(); 
                    break;
                case GameCommand.Save:
                    Console.Write("Enter the saved file name (e.g.game.sav): ");
=======
                    if (UndoMove()){}
                    break;
                case GameCommand.Redo:
                    if (RedoMove())
                    {
                        HandleGamestate();
                    }
                    break;
                case GameCommand.Save:
                    Console.Write("\nPlease enter the filename you want to use to save the game (e.g.game.sav): ");
>>>>>>> 4387e5b (copy file from local)
                    string? savePath = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(savePath)) SaveGame(savePath);
                    else Console.WriteLine("Save Cancelled");
                    break;
                case GameCommand.Load:
<<<<<<< HEAD
                    Console.Write("Please enter the file name ");
=======
                    Console.Write("\nPlease enter the file name to load the game: ");
>>>>>>> 4387e5b (copy file from local)
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
<<<<<<< HEAD
                    Console.WriteLine("You choose to exit the game.");
                    IsGameOver = true;
                    break;
                default: 
                    Console.WriteLine($"None: {command}");
=======
                    Console.WriteLine("\nYou choose to exit the current game.");
                    IsGameOver = true;
                    break;
                default: 
                    Console.WriteLine($"\nUnknown system command encountered in ProcessSystemCommand: {command}");
>>>>>>> 4387e5b (copy file from local)
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
<<<<<<< HEAD
            // NotaktoGame must override this to return a NotaktoCommand for a NotaktoMove
            return new PlacePieceCommand(CurrentBoard, move);
        }

        //Handle game state win/draw
=======
            if (move is NotaktoMove && !(this is NotaktoGame))
            {
                Console.WriteLine("\nWarning: Base Game.CreateMoveCommand received a NotaktoMove for a non-NotaktoGame type.");
            }
            return new PlacePieceCommand(CurrentBoard!, move);
        }

        //Handle game state win/draw, switch players
>>>>>>> 4387e5b (copy file from local)
        private void HandleGamestate()
        {
            Player? winner = CheckWinCondition();
            if (winner != null)
            {
                IsGameOver = true;
                DisplayBoard();
<<<<<<< HEAD
                Console.WriteLine($"\nGame Over! {winner.Name} ({winner.PlayerPiece?.Symbol}) wins!");
=======
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nGame Over! {winner.Name} ({winner.PlayerPiece?.Symbol}) wins!");
                Console.ResetColor();
>>>>>>> 4387e5b (copy file from local)
                return;
            }

            if (CheckDrawCondition())
            {
                IsGameOver = true;
                DisplayBoard();
<<<<<<< HEAD
                Console.WriteLine("\nGame Over! It's a draw!");
                return;
            }
            SwitchPlayer();
=======
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nGame Over! It's a draw!");
                Console.ResetColor();
                return;
            }
            Player? playerWhoEnded = CurrentPlayer;
            SwitchPlayer();

            //After computerPlayer make a move, prompt humanPlayer
            if (playerWhoEnded is ComputerPlayer && CurrentPlayer is HumanPlayer)
            {
                Console.WriteLine($"\n{playerWhoEnded.Name} has made its move. ");
                DisplayBoard();
                Console.WriteLine("Press ENTER to continue your turn...");
                Console.ReadLine();
            }
>>>>>>> 4387e5b (copy file from local)
        }
        
        //Switches to the next Player
        public void SwitchPlayer()
        {
            if (Players != null && Players.Count > 1)
            {
<<<<<<< HEAD
                int currentPlayerIndex = Players.IndexOf(CurrentPlayer);
=======
                int currentPlayerIndex = Players.IndexOf(CurrentPlayer!);
>>>>>>> 4387e5b (copy file from local)
                CurrentPlayer = Players[(currentPlayerIndex + 1) % Players.Count];
            }
        }

<<<<<<< HEAD
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
=======
        //Undo Move Command
        public virtual bool UndoMove()
        {
            if (!(CurrentPlayer is HumanPlayer))
            {
                Console.WriteLine("\nOnly humanPlayer can undo the move.");
                return false;
            }

            Player playerWhoUndo = CurrentPlayer;
            if (MoveHistory.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo move can be undo");
                Console.ResetColor();
                return false;
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nPlayer{playerWhoUndo.Name} ask for Undo..");
            Console.ResetColor();

            int movesRealUndo = 0;
            Player? playerFirstUndoneMove = null;
            Player? playerSecondUndoneMove = null;

            if (MoveHistory.Count >= 2)
            {
                movesRealUndo = 2;
                IMoveCommand opponentMoveCmd = MoveHistory[MoveHistory.Count - 1];
                IMoveCommand selfLastMoveCmd = MoveHistory[MoveHistory.Count - 2];

                if (opponentMoveCmd is PlacePieceCommand op_ppc && op_ppc.OriginalMoveDetails != null) playerFirstUndoneMove = op_ppc.OriginalMoveDetails.Player;
                else if (opponentMoveCmd is NotaktoCommand op_nc && op_nc.OriginalMoveDetails != null) playerFirstUndoneMove = op_nc.OriginalMoveDetails.Player;

                if (selfLastMoveCmd is PlacePieceCommand self_ppc && self_ppc.OriginalMoveDetails != null) playerSecondUndoneMove = self_ppc.OriginalMoveDetails.Player;
                else if (selfLastMoveCmd is NotaktoCommand self_nc && self_nc.OriginalMoveDetails != null) playerSecondUndoneMove = self_nc.OriginalMoveDetails.Player;

                Console.WriteLine($"\nProgram will undo player {playerFirstUndoneMove?.Name ?? "Opponent"} 's move, Then undo your ({playerSecondUndoneMove?.Name ?? "You"})'s last move");
            }
            else if (MoveHistory.Count == 1)
            {
                movesRealUndo = 1;
                IMoveCommand lastMoveCmd = MoveHistory.Last();
                if (lastMoveCmd is PlacePieceCommand ppc && ppc.OriginalMoveDetails != null) playerFirstUndoneMove = ppc.OriginalMoveDetails.Player;
                else if (lastMoveCmd is NotaktoCommand nc && nc.OriginalMoveDetails != null) playerFirstUndoneMove = nc.OriginalMoveDetails.Player;
                Console.WriteLine($"\nProgram will undo player {playerFirstUndoneMove?.Name ?? "last"}'s final piece");
            }

            //Undo execute
            for (int i = 0; i < movesRealUndo; i++)
            {
                if (MoveHistory.Any())
                {
                    new UndoCommand(MoveHistory, RedoHistory).Execute();
                }
                else
                {
                    Console.WriteLine("\nError: In processing undo, move history becomes empty.");
                    break;
                }
            }

            IsGameOver = false;

            CurrentPlayer = playerWhoUndo;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nUndo finished successfully. It's your turn! ({CurrentPlayer.Name}) To make a new move!");
            Console.ResetColor();
            return true;
>>>>>>> 4387e5b (copy file from local)
        }

        //Redo Move Command
        public virtual bool RedoMove()
        {
<<<<<<< HEAD
            if (RedoHistory.Any())
            {
                new RedoCommand(RedoHistory, MoveHistory).Execute();
                IsGameOver = false; 
                Console.WriteLine("Move Redone.");
                return true;
            }
            Console.WriteLine("No moves to redo.");
=======
            if (!(CurrentPlayer is HumanPlayer))
            {
                Console.WriteLine("\nOnly humanPlayer can redo the move.");
                return false;
            }
            if (RedoHistory.Any())
            {
                new RedoCommand(RedoHistory, MoveHistory).Execute();
                IsGameOver = false;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nMove Redo successfully.");
                Console.ResetColor();
                HandleGamestate();
                return true;
            }
            Console.WriteLine("\nNo moves to redo.");
>>>>>>> 4387e5b (copy file from local)
            return false;
        }

        //Display the game board
        public void DisplayBoard()
        {
<<<<<<< HEAD
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
=======
            Console.WriteLine($"\n--- {this.GetType().Name} ({GameMode}) ---");
            if (CurrentBoard != null) CurrentBoard.Display();
            else Console.WriteLine("Board is not initialized!");
        }

        //Save a game
        //<param name="filePath">Path to the save file</param>
        public virtual void SaveGame(string filePath)
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
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nGame saved successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to save the game: " + ex.Message);
            }
        }

        //Load a saved game
        //<param name="filePath">Path to the save file</param>
        public virtual void LoadGame(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string? gameType = reader.ReadLine();
                    int size = int.Parse(reader.ReadLine());
                    string currentPlayerName = reader.ReadLine();

                    CurrentBoard = new TicTacToeBoard(size);

                    Player? matched = Players?.FirstOrDefault(p => p.Name == currentPlayerName);
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
                    Console.WriteLine("Game loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load the game: " + ex.Message);
            }
        }

        //Save a game to Desktop
        public virtual void SaveGameToDesktop()
        {
            try
            {
                Console.Write("Enter file name to save (e.g., game1.sav): ");
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid file name. Save canceled.");
                    return;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullPath = Path.Combine(desktopPath, input);

                using (StreamWriter writer = new StreamWriter(fullPath))
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

                Console.WriteLine("Game saved successfully to Desktop: " + fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save the game: " + ex.Message);
            }
        }
        
        protected virtual void DisplayHelp()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- Help ---");
            Console.ResetColor();
            Console.WriteLine("Enter your move as row, col");
            Console.WriteLine("For Notakto, use 'boardIndex,row,col' (1-based index, row, col are 1-based for board).");
            Console.WriteLine("Available commands:");
            Console.WriteLine(" U or undo          - Undo the last turn/moves. (Undo may cancel the last move for both player. )");
            Console.WriteLine(" R or redo          - Redo the last undone move one step each time.");
            Console.WriteLine(" S or save          - Save the current game.");
            Console.WriteLine(" L or load          - Load a saved game.");
            Console.WriteLine(" Q or quit          - Exit the current game to the main menu.");
>>>>>>> 4387e5b (copy file from local)
            Console.WriteLine("--------------");
        }

        //Abstract methods for child games
<<<<<<< HEAD

=======
>>>>>>> 4387e5b (copy file from local)
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