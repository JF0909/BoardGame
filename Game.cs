namespace PlayerBoardGame
{
    /// <summary>
    /// Abstract base class for all games.
    /// Define core game logic and structure using the template method pattern.
    /// </summary>
    public abstract class Game
    {
        protected Board CurrentBoard;
        protected List<Player> Players;
        protected Player CurrentPlayer;
        protected List<IMoveCommand> MoveHistory = new List<IMoveCommand>();
        protected List<IMoveCommand> RedoHistory = new List<IMoveCommand>();
        public GameMode GameMode { get; protected set; }
        public bool IsGameOver { get; protected set; }

        //Main Game Loop to handle turns
        public void RunGameLoop()
        {
            StartGame();
            while (!IsGameOver)
            {
                DisplayBoard();
                Move move = CurrentPlayer.GetMove(CurrentBoard);
                if(IsMoveValid(move))
                {
                    ApplyMove(move);
                    IsGameOver = CheckWinCondition() != null || CheckDrawCondition();
                    SwitchPlayer();
                }
            }
            Console.WriteLine("Game Over!");
        }

        //Initializes the game setup
        public void StartGame()
        {
            CurrentBoard = CreateInitialBoard();
            Players = CreatePlayers(GameMode);
            CurrentPlayer = Players[0];
            IsGameOver = false;
        }

        //Validates a move
        protected abstract bool IsMoveValid(Move move);

        //Apply a move for childclasses to override
        protected virtual void ApplyMove(Move move)
        {
            CurrentBoard.PlacePiece(move.Row, move.Col, move.PiecePlaced);
        }

        //Check if a player has won the game
        protected abstract Player CheckWinCondition();

        //Check for a draw
        protected abstract bool CheckDrawCondition();

        //Switches to the next Player
        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Players[0]) ? Players[1] : Players[0];
        }

        //Save the game state
        public void SaveGame(string filePath)
        {
            // TODO: implement until board, game subclasses are finished

        }

        //Load a saved game
        public void LoadGame(string filepath)
        {
            // TODO: implement until board, game subclasses are finished

        }

        //Allows undo and redo functionality
        public bool UndoMove()
        {
            return false;
        }
        public bool RedoMove()
        {
            return false;
        }

        //Display the game board
        public void DisplayBoard()
        {
            CurrentBoard.Display();
        }
        
        //Create initial board
        protected abstract Board CreateInitialBoard();

        //Create the players based on game mode
        protected abstract List<Player> CreatePlayers(GameMode mode);

    }
}