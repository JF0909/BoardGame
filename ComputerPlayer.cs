<<<<<<< HEAD
=======

using System.ComponentModel;

>>>>>>> 4387e5b (copy file from local)
namespace PlayerBoardGame
{
    public class ComputerPlayer : Player
    {
        private static readonly Random _random = new Random();
        public ComputerPlayer(string name, Piece piece) : base(name, piece) { }
        public override (Game.GameCommand command, Move? moveDetails) GetMove(Board currentBoard)
        {
<<<<<<< HEAD
=======
            Console.WriteLine($"ComputerPlayer ({Name} - {PlayerPiece.Symbol}) is thinking...");
>>>>>>> 4387e5b (copy file from local)
            Move? chosenMove = null;
            if (currentBoard is NotaktoBoard notaktoBoard)
            {
                //Notakto AI Logic
<<<<<<< HEAD
                (int subBoardIndex, int row, int col)? strategicCoords = FindBestNotaktoMove(notaktoBoard);
                if (strategicCoords.HasValue)
                {
                    var (sbIndex, r, c) = strategicCoords.Value;
                    chosenMove = new NotaktoMove(this, sbIndex, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"ComputerPlayer ({Name}) moving in Notakto sub-board {sbIndex + 1} 的 (R:{r + 1}, C:{c + 1})");
=======
                (int subBoardIndex, int row, int col)? bestMove = FindBestNotaktoMove(notaktoBoard);
                if (bestMove.HasValue)
                {
                    var (sbIndex, r, c) = bestMove.Value;
                    chosenMove = new NotaktoMove(this, sbIndex, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"\nComputerPlayer ({Name}) made a best move on Notakto sub-board {sbIndex + 1}'s (R:{r + 1}, C:{c + 1})");
>>>>>>> 4387e5b (copy file from local)
                }
                else
                {
                    var (sbIndex, r, c) = FindRandomNotaktoMove(notaktoBoard);
                    chosenMove = new NotaktoMove(this, sbIndex, r, c, this.PlayerPiece, currentBoard.Clone());
<<<<<<< HEAD
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

=======
                    Console.WriteLine($"\nComputerPlayer ({Name}) in Notakto sub-board {sbIndex + 1}'s (R:{r + 1}, C:{c + 1}) make a random move");
                }
            }
            else if (currentBoard is GomokuBoard gomokuBoard)
            {
                var winningMove = bestMoveSingleBoard(gomokuBoard, 5);
                if (winningMove.HasValue)
                {
                    var (r, c) = winningMove.Value;
                    chosenMove = new Move(this, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"\nComputer ({Name}) makes a winning move for Gomoku at (Row:{r}, Col:{c}).");
                }
                else
                {
                    try
                    {
                        var (r, c) = FindRandomMoveSingleBoard(gomokuBoard);
                        chosenMove = new Move(this, r, c, this.PlayerPiece, currentBoard.Clone());
                        Console.WriteLine($"\nComputer ({Name}) makes a random move for Gomoku at (Row:{r}, Col:{c}).");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Error for {Name} (Gomoku Computer - Random): {ex.Message}");
                    }
                }
            }
            else if (currentBoard is TicTacToeBoard numericalBoard)
            {
                int boardSizeN = numericalBoard.Width;
                int targetSum = boardSizeN * (boardSizeN * boardSizeN + 1) / 2;

                var winningNumericalMove = FindWinningNumericalMove(numericalBoard, boardSizeN, targetSum);
                if (winningNumericalMove.HasValue)
                {
                    var (r, c, num) = winningNumericalMove.Value;
                    chosenMove = new Move(this, r, c, new Piece(num.ToString()), numericalBoard.Clone());
                    Console.WriteLine($"Computer ({Name}) makes a WINNING move for Numerical TicTacToe at (Row:{r + 1}, Col:{c + 1}) with number {num}!");
                }
                else
                {
                    // Otherwise, make a random move
                    try
                    {
                        var (r, c, num) = FindRandomNumericalMove(numericalBoard, boardSizeN);
                        chosenMove = new Move(this, r, c, new Piece(num.ToString()), numericalBoard.Clone());
                        Console.WriteLine($"\nComputer ({Name}) makes a RANDOM move for Numerical TicTacToe at (Row:{r + 1}, Col:{c + 1}) with number {num}.");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"\nComputer Error (Numerical TTT Random): {ex.Message}");

                    }
                }
            }
            else
            {
                try
                {
                    var (r, c) = FindRandomMoveSingleBoard(currentBoard);
                    chosenMove = new Move(this, r, c, this.PlayerPiece, currentBoard.Clone());
                    Console.WriteLine($"\nComputer ({Name}) makes a random move for TicTacToe at (Row:{r}, Col:{c}).");
                }
                catch (InvalidOperationException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nError for {Name} (TicTacToe Computer - Random): {ex.Message}");
                    Console.ResetColor();
                }
            }

            if (chosenMove != null)
            {
                return (Game.GameCommand.MakeMove, chosenMove);
            }
            else
            {
                // AI cannot find valid move, end
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: ComputerPlayer ({Name}) cannot find any valid move, end. ");
                Console.ResetColor();
                return (Game.GameCommand.Invalid, null);
            }

>>>>>>> 4387e5b (copy file from local)
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
<<<<<<< HEAD
                var strategicFinishingMove = nonLosingMoves.FirstOrDefault(m => m.finishesSubBoard);
                if (strategicFinishingMove.finishesSubBoard)
                {
                    return (strategicFinishingMove.sbIndex, strategicFinishingMove.r, strategicFinishingMove.c);
=======
                var bestFinishingMove = nonLosingMoves.FirstOrDefault(m => m.finishesSubBoard);
                if (bestFinishingMove.finishesSubBoard)
                {
                    return (bestFinishingMove.sbIndex, bestFinishingMove.r, bestFinishingMove.c);
>>>>>>> 4387e5b (copy file from local)
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
<<<<<<< HEAD
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


        
=======
            throw new InvalidOperationException("\nComputer: No random valid Notakto moves found, game should be over.");
        }


        //SingleBoard Computer Make Move Method: TicTacToe , Gomoku
        private (int row, int col)? bestMoveSingleBoard(Board currentBoard, int lineLengthToWin)
        {
            for (int r = 0; r < currentBoard.Height; r++)
            {
                for (int c = 0; c < currentBoard.Width; c++)
                {
                    if (currentBoard.IsValidPosition(r, c) && currentBoard.IsCellEmpty(r, c))
                    {
                        Board simulatedBoard = currentBoard.Clone();
                        if (simulatedBoard.PlacePiece(r, c, this.PlayerPiece))
                        {
                            if (IsSimulatedStateAWin(simulatedBoard, this.PlayerPiece, lineLengthToWin))
                            {
                                return (r, c);//Found winning move
                            }
                        }

                    }
                }
            }
            return null;
        }

        private bool IsSimulatedStateAWin(Board board, Piece playerPiece, int lineLengthToWin)
        {
            if (playerPiece == null || string.IsNullOrEmpty(playerPiece.Symbol) || board == null)
            {
                return false;
            }
            string symbol = playerPiece.Symbol;

            //Gomoku win check
            if (board is GomokuBoard gBoard)
            {
                for (int r = 0; r < gBoard.Height; r++)
                {
                    for (int c = 0; c < gBoard.Width; c++)
                    {
                        if (gBoard.GetPiece(r, c)?.Symbol == symbol)
                        {
                            int[][] directions = {
                                new[] {0,1}, //Horizontal
                                new[] {1,0}, //Virtical
                                new[] {1,1}, // Diagonal
                                new[] {1,-1}, 
                            };

                            foreach (var dir in directions)
                            {
                                if (CheckLineHelper(gBoard, r, c, dir[0], dir[1], symbol) >= lineLengthToWin)
                                {
                                    return true; // Found a winning line
                                }

                            }
                        }
                    }

                }
                return false; // No Gomoku win found
            }


            else if (board is TicTacToeBoard tBoard && !(this.PlayerPiece.Symbol == "Odd" || this.PlayerPiece.Symbol == "Even"))
            {
         
                string currentSymbol = playerPiece.Symbol;

                for (int r = 0; r < tBoard.Height; r++)
                {
                    for (int c = 0; c < tBoard.Width; c++)
                    {
                        if (tBoard.GetPiece(r, c)?.Symbol == currentSymbol)
                        {
                            int[][] directions = {
                                new[] {0, 1},  
                                new[] {1, 0}, 
                                new[] {1, 1},  
                                new[] {1, -1}  
                            };

                            foreach (var dir in directions)
                            {
                               
                                if (CheckLineHelper(tBoard, r, c, dir[0], dir[1], currentSymbol) >= lineLengthToWin)
                                {
                                    return true; 
                                }
                            }
                        }
                    }
                }
                return false;
            }
            return false;

        }

        //For singleboard games: find random valid move
        private (int r, int c) FindRandomMoveSingleBoard(Board board)
        {
            var emptyCells = new List<(int, int)>();
            for (int r = 0; r < board.Height; r++)
            {
                for (int c = 0; c < board.Width; c++)
                {
                    if (board.IsValidPosition(r, c) && board.IsCellEmpty(r, c)) { emptyCells.Add((r, c)); }
                }
            }
            if (emptyCells.Any()) { return emptyCells[_random.Next(emptyCells.Count)]; }
            throw new InvalidOperationException("\nComputer: No random valid single-board moves found, game should be over.");
        }

        //Helper function for Gomoku
        private int CheckLineHelper(Board board, int r, int c, int dr, int dc, string symbol)
        {
            int count = 1;
            for (int i = 1; ; i++)
            {
                int nextR = r + dr * i;
                int nextC = c + dc * i;
                if (board.IsValidPosition(nextR, nextC) && board.GetPiece(nextR, nextC)?.Symbol == symbol)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            for (int i = 1; ; i++)
            {
                int prevR = r - dr * i;
                int prevC = c - dc * i;
                if (board.IsValidPosition(prevR, prevC) && board.GetPiece(prevR, prevC)?.Symbol == symbol)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;

        }
        
        //The following is method for check winning place in TicTacToe

        /// <summary>
        /// Gets a list of numbers the computer can currently play for Numerical TicTacToe.
        /// </summary>
        private List<int> GetAvailableNumbersForComputer(TicTacToeBoard board, int boardSizeN)
        {
            var allPossibleNumbers = Enumerable.Range(1, boardSizeN * boardSizeN).ToList();
            var currentlyUsedNumbers = new HashSet<int>();
            for (int r = 0; r < board.Height; r++)
            {
                for (int c = 0; c < board.Width; c++)
                {
                    Piece? p = board.GetPiece(r, c);
                    if (p != null && int.TryParse(p.Symbol, out int num))
                    {
                        currentlyUsedNumbers.Add(num);
                    }
                }
            }

            bool amIOddPlayer = this.PlayerPiece.Symbol == "Odd";
            List<int> availableNumbers = new List<int>();
            foreach (int num in allPossibleNumbers)
            {
                if (!currentlyUsedNumbers.Contains(num))
                {
                    if (amIOddPlayer && num % 2 != 0) availableNumbers.Add(num);
                    else if (!amIOddPlayer && num % 2 == 0) availableNumbers.Add(num);
                }
            }
            return availableNumbers;
        }

        /// <summary>
        /// For Numerical TicTacToe: Tries to find a number and position for an immediate win.
        /// </summary>
        private (int row, int col, int number)? FindWinningNumericalMove(TicTacToeBoard currentBoard, int boardSizeN, int targetSumS)
        {
            List<int> availableNumbers = GetAvailableNumbersForComputer(currentBoard, boardSizeN);
            if (!availableNumbers.Any()) return null;

            for (int r = 0; r < currentBoard.Height; r++)
            {
                for (int c = 0; c < currentBoard.Width; c++)
                {
                    if (currentBoard.IsValidPosition(r, c) && currentBoard.IsCellEmpty(r, c))
                    {
                        foreach (int numToTry in availableNumbers)
                        {
                            TicTacToeBoard simulatedBoard = (TicTacToeBoard)currentBoard.Clone();
                            Piece numPiece = new Piece(numToTry.ToString());
                            if (simulatedBoard.PlacePiece(r, c, numPiece))
                            {
                                // Check if this simulated state is a win
                                if (IsNumericalTTTWin(simulatedBoard, boardSizeN, targetSumS))
                                {
                                    return (r, c, numToTry);
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Checks win condition for Numerical TicTacToe on a given board state.
        /// </summary>
        private bool IsNumericalTTTWin(TicTacToeBoard board, int n, int targetSumS)
        {
            // Check rows 
            for (int r = 0; r < n; r++)
            {
                int currentLineSum = 0;
                int piecesInLine = 0;
                for (int c = 0; c < n; c++)
                {
                    Piece? p = board.GetPiece(r, c);
                    if (p != null && int.TryParse(p.Symbol, out int val)) { currentLineSum += val; piecesInLine++; }
                    else break;
                }
                if (piecesInLine == n && currentLineSum == targetSumS) return true;
            }

            // Check columns 
            for (int c = 0; c < n; c++)
            {
                int currentLineSum = 0;
                int piecesInLine = 0;
                for (int r = 0; r < n; r++)
                {
                    Piece? p = board.GetPiece(r, c);
                    if (p != null && int.TryParse(p.Symbol, out int val)) { currentLineSum += val; piecesInLine++; }
                    else break;
                }
                if (piecesInLine == n && currentLineSum == targetSumS) return true;
            }

            int diag1Sum = 0;
            int diag1Pieces = 0;
            for (int i = 0; i < n; i++)
            {
                Piece? p = board.GetPiece(i, i);
                if (p != null && int.TryParse(p.Symbol, out int val)) { diag1Sum += val; diag1Pieces++; }
                else break;
            }
            if (diag1Pieces == n && diag1Sum == targetSumS) return true;

            int diag2Sum = 0;
            int diag2Pieces = 0;
            for (int i = 0; i < n; i++)
            {
                Piece? p = board.GetPiece(i, n - 1 - i);
                if (p != null && int.TryParse(p.Symbol, out int val)) { diag2Sum += val; diag2Pieces++; }
                else break;
            }
            if (diag2Pieces == n && diag2Sum == targetSumS) return true;

            return false; // No win found 
        }
        
        /// <summary>
        /// For Numerical TicTacToe: Finds a random valid number and a random valid empty cell.
        /// </summary>
        private (int r, int c, int number) FindRandomNumericalMove(TicTacToeBoard board, int boardSizeN)
        {
            List<int> availableNumbers = GetAvailableNumbersForComputer(board, boardSizeN);
            var emptyCells = new List<(int, int)>();
            for (int r = 0; r < board.Height; r++)
            {
                for (int c = 0; c < board.Width; c++)
                {
                    if (board.IsValidPosition(r, c) && board.IsCellEmpty(r, c))
                    {
                        emptyCells.Add((r, c));
                    }
                }
            }

            if (availableNumbers.Any() && emptyCells.Any())
            {
                int chosenNumber = availableNumbers[_random.Next(availableNumbers.Count)];
                var (r, c) = emptyCells[_random.Next(emptyCells.Count)];
                return (r, c, chosenNumber);
            }
           
            throw new InvalidOperationException($"Computer ({Name}): No random valid numerical moves found, game should be over.");
        }
    
   
>>>>>>> 4387e5b (copy file from local)
    }
}