namespace PlayerBoardGame
{
    /// <summary>
    /// Notakto Game Rules
    /// The game ends when all three sub-boards have a three-in-a-row.
    /// The player who makes the move that completes the third and final sub-board loses the game
    /// </summary>
    public class NotaktoGame : Game
    {
        private NotaktoBoard MainNotaktoBoard => CurrentBoard as NotaktoBoard;
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
                Console.WriteLine("Basic Move Validation Failed: null move/board/not Current Player");
                return false;
            }
            if (!(move is NotaktoMove notaktoMove))
            {
                Console.WriteLine("Error: Move is not a NotaktoMove for NotaktoGame.");
                return false;
            }
            if (notaktoMove.SubBoardIndex < 0 || notaktoMove.SubBoardIndex >= MainNotaktoBoard.SubBoardCount)
            {
                Console.WriteLine($"Error: Invalid sub-board index {notaktoMove.SubBoardIndex + 1}.");
                return false;
            }
            if (MainNotaktoBoard.IsSubBoardFinished(notaktoMove.SubBoardIndex))
            {
                Console.WriteLine($"Error: Sub-board {notaktoMove.SubBoardIndex + 1} is already finished (IsMoveValid check).");
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
                Console.WriteLine("Error: CurrentBoard is null in NotaktoGame.");
                throw new InvalidOperationException("Cannot create a move command without a valid board.");
            }

            if (!(move is NotaktoMove))
            {
                Console.WriteLine("Error: CreateMoveCommand received a non-NotaktoMove in NotaktoGame.");
                throw new ArgumentException("Move must be a NotaktoMove for NotaktoGame.", nameof(move));
            }
            // Create command for Notakto move 
            return new PlacePieceCommand(CurrentBoard, move);
        }
        
        protected override Player CheckWinCondition()
        {
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
        }

        protected override bool CheckDrawCondition()
        {
            // Draw will never happen in Notakto Game
            return false;

        }
    }
}