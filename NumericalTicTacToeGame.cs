using System;
using System.Collections.Generic;

namespace PlayerBoardGame
{
    public class NumericalTicTacToeGame : Game
    {
        private HashSet<int> usedNumbers = new HashSet<int>();
        private int boardSize;

        // Constructor to initialize game mode and board size
        public NumericalTicTacToeGame(GameMode mode, int size)
        {
            GameMode = mode;
            boardSize = size;
            CurrentBoard = CreateInitialBoard();
            Players = CreatePlayers(mode);
            CurrentPlayer = Players[0];
        }

        // Create dynamic board based on size
        protected override Board CreateInitialBoard()
        {
            return new TicTacToeBoard(boardSize);
        }

        // Create human vs human or human vs computer players
        protected override List<Player> CreatePlayers(GameMode mode)
        {
            return new List<Player>
            {
                new HumanPlayer("Player 1", new Piece("Odd")),
                mode == GameMode.HumanVsComputer 
                    ? new ComputerPlayer("Computer", new Piece("Even"))
                    : new HumanPlayer("Player 2", new Piece("Even"))
            };
        }

        // Check if move is valid: empty cell, number not used, between 1–n^2
        protected override bool IsMoveValid(Move move)
        {
            if (!CurrentBoard.IsValidPosition(move.Row, move.Col))
                return false;

            if (CurrentBoard.GetPiece(move.Row, move.Col) != null)
                return false;

            if (!int.TryParse(move.PiecePlaced.Symbol, out int number))
                return false;

            return number >= 1 && number <= boardSize * boardSize && !usedNumbers.Contains(number);
        }

        // Apply the move and add it to the set of used numbers
        protected override void ApplyMove(Move move)
        {
            usedNumbers.Add(int.Parse(move.PiecePlaced.Symbol));
            CurrentBoard.PlacePiece(move.Row, move.Col, move.PiecePlaced);
        }

        // Check for a winning combination (row, col, or diagonals sum to 15)
        protected override Player CheckWinCondition()
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (SumEquals15(GetPiece(i, 0), GetPiece(i, 1), GetPiece(i, 2)) ||
                    SumEquals15(GetPiece(0, i), GetPiece(1, i), GetPiece(2, i)))
                    return CurrentPlayer;
            }

            if (SumEquals15(GetPiece(0, 0), GetPiece(1, 1), GetPiece(2, 2)) ||
                SumEquals15(GetPiece(0, 2), GetPiece(1, 1), GetPiece(2, 0)))
                return CurrentPlayer;

            return null;
        }

        // Game is a draw if all cells are used and no winner
        protected override bool CheckDrawCondition()
        {
            return usedNumbers.Count == boardSize * boardSize && CheckWinCondition() == null;
        }

        private Piece GetPiece(int row, int col)
        {
            return CurrentBoard.GetPiece(row, col);
        }

        // Helper method to check if the sum of 3 cells is 15
        private bool SumEquals15(Piece a, Piece b, Piece c)
        {
            if (a == null || b == null || c == null)
                return false;

            return int.Parse(a.Symbol) + int.Parse(b.Symbol) + int.Parse(c.Symbol) == 15;
        }
    }
}
