namespace PlayerBoardGame
{
    public class ComputerPlayer : Player
    {
        private static readonly Random _random = new Random();
        public ComputerPlayer(string name, Piece piece) : base(name, piece) { }
        public override (Game.GameCommand command, Move? moveDetails) GetMove(Board currentBoard)
        {
            Move? chosenMove = null;
            if (currentBoard is NotaktoBoard notaktoBoard)
            {
                //Notakto AI Logic
                (int subBoardIndex, int row, int col)? strategicCoords = FindBestNotaktoMove(notaktoBoard);
                if (strategicCoords.HasValue)
                {
                    var (sbIndex, r, c) = strategicCoords.Value;
                    chosenMove = new NotaktoMove(this, sbIndex, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"ComputerPlayer ({Name}) moving in Notakto sub-board {sbIndex + 1} 的 (R:{r + 1}, C:{c + 1})");
                }
                else
                {
                    var (sbIndex, r, c) = FindRandomNotaktoMove(notaktoBoard);
                    chosenMove = new NotaktoMove(this, sbIndex, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"ComputerPlayer ({Name}) in Notakto sub-board {sbIndex + 1} 的 (R:{r + 1}, C:{c + 1}) make a random move");
                }
            }
            else // Gomoku, TicTacToe
            {
                (int row, int col)? winningCoords = TryFindWinningMoveSingleBoard(currentBoard);
                if (winningCoords.HasValue)
                {
                    var (r, c) = winningCoords.Value;
                    chosenMove = new Move(this, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"ComputerPlayer ({Name}) make a winning move at (R:{r + 1}, C:{c + 1})");
                }
                else
                {
                    var (r, c) = FindRandomMoveSingleBoard(currentBoard);
                    chosenMove = new Move(this, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"ComputerPlayer ({Name}) make a random move at (R:{r + 1}, C:{c + 1})");
                }
            }

            if (chosenMove != null)
            {
                return (Game.GameCommand.MakeMove, chosenMove);
            }
            else
            {
                // AI cannot find valid move, end
                Console.WriteLine($"Error: ComputerPlayer ({Name}) cannot find any valid move, end. ");
                return (Game.GameCommand.Invalid, null); 
            }

        }
        //Notakto Computer Make Move Method
        private (int subBoardIndex, int row, int col)? FindBestNotaktoMove(NotaktoBoard currentNotaktoBoard)
        {
            var nonLosingMoves = new List<(int sbIndex, int r, int c, bool finishesSubBoard)>();
            for (int sbIndex = 0; sbIndex < currentNotaktoBoard.SubBoardCount; sbIndex++)
            {
                if (!currentNotaktoBoard.IsSubBoardFinished(sbIndex))
                {
                    TicTacToeBoard subBoard = currentNotaktoBoard.GetSubBoard(sbIndex);
                    for (int r = 0; r < subBoard.Height; r++)
                    {
                        for (int c = 0; c < subBoard.Width; c++)
                        {
                            if (subBoard.IsCellEmpty(r, c))
                            {
                                NotaktoBoard simulatedBoard = (NotaktoBoard)currentNotaktoBoard.Clone();
                                simulatedBoard.PlacePieceOnSubBoard(sbIndex, r, c, this.PlayerPiece);
                                if (!simulatedBoard.AreAllSubBoardsFinished())
                                {
                                    bool moveFinishesThisSubBoard = simulatedBoard.IsSubBoardFinished(sbIndex);
                                    nonLosingMoves.Add((sbIndex, r, c, moveFinishesThisSubBoard));
                                }
                            }
                        }
                    }
                }
            }
            if (nonLosingMoves.Any())
            {
                var strategicFinishingMove = nonLosingMoves.FirstOrDefault(m => m.finishesSubBoard);
                if (strategicFinishingMove.finishesSubBoard)
                {
                    return (strategicFinishingMove.sbIndex, strategicFinishingMove.r, strategicFinishingMove.c);
                }
                var anyNonLosingMove = nonLosingMoves[_random.Next(nonLosingMoves.Count)];
                return (anyNonLosingMove.sbIndex, anyNonLosingMove.r, anyNonLosingMove.c);
            }
            return null;
        }

        private (int subBoardIndex, int row, int col) FindRandomNotaktoMove(NotaktoBoard notaktoBoard)
        {
            var validMoves = new List<(int sbIndex, int r, int c)>();
            for (int sbIndex = 0; sbIndex < notaktoBoard.SubBoardCount; sbIndex++)
            {
                if (!notaktoBoard.IsSubBoardFinished(sbIndex))
                {
                    TicTacToeBoard subBoard = notaktoBoard.GetSubBoard(sbIndex);
                    for (int r = 0; r < subBoard.Height; r++)
                    {
                        for (int c = 0; c < subBoard.Width; c++)
                        {
                            if (subBoard.IsCellEmpty(r, c)) { validMoves.Add((sbIndex, r, c)); }
                        }
                    }
                }
            }
            if (validMoves.Any()) { return validMoves[_random.Next(validMoves.Count)]; }
            throw new InvalidOperationException("AI: No random valid Notakto moves found, game should be over.");
        }


        //SingleBoard Computer Make Move Method
        private (int row, int col)? TryFindWinningMoveSingleBoard(Board currentBoard)
        {
            for (int r = 0; r < currentBoard.Height; r++) {
                for (int c = 0; c < currentBoard.Width; c++) {
                    if (currentBoard.IsValidPosition(r, c) && currentBoard.IsCellEmpty(r, c)) {
                        Board simulatedBoard = currentBoard.Clone();
                        simulatedBoard.PlacePiece(r, c, this.PlayerPiece);
                        if (IsSimulatedStateAWin(simulatedBoard, this.PlayerPiece)) { 
                            return (r, c);
                        }
                    }
                }
            }
            return null;
        }
        private bool IsSimulatedStateAWin(Board board, Piece playerPiece)
        {
            // TODO: implement differnt game（TicTacToe, Gomoku）check winner logic
            Console.WriteLine($"Warning: IsSimulatedStateAWin for board type {board.GetType().Name} is not fully implemented.");
            return false; 
        }

        private (int r, int c) FindRandomMoveSingleBoard(Board board)
        {
            var emptyCells = new List<(int, int)>();
            for (int r = 0; r < board.Height; r++) {
                for (int c = 0; c < board.Width; c++) {
                    if (board.IsValidPosition(r,c) && board.IsCellEmpty(r, c)) { emptyCells.Add((r, c)); }
                }
            }
            if (emptyCells.Any()) { return emptyCells[_random.Next(emptyCells.Count)]; }
            throw new InvalidOperationException("Computer: No random valid single-board moves found, game should be over.");
        }


        
    }
}