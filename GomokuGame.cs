using System;

namespace PlayerBoardGame
{
    public class GomokuGame : Game
    {
        public GomokuGame()
        {
            CurrentBoard = new GomokuBoard(); // 15x15 board
            SetupPlayers();
        }

        private void SetupPlayers()
        {
            // Two players with different symbols
            Players = new Player[]
            {
                new HumanPlayer("Player 1", new Piece("X")),
                new HumanPlayer("Player 2", new Piece("O"))
            };
            CurrentPlayer = Players[0];
        }

        protected override bool IsMoveValid(Move move)
        {
            // Position must be inside board and empty
            return CurrentBoard.IsValidPosition(move.Row, move.Col) &&
                   CurrentBoard.GetPiece(move.Row, move.Col) == null;
        }

        protected override void ApplyMove(Move move)
        {
            CurrentBoard.PlacePiece(move.Row, move.Col, move.Piece);
        }

        protected override Player CheckWinCondition()
        {
            // Check all directions for 5 in a row
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    Piece piece = CurrentBoard.GetPiece(row, col);
                    if (piece != null && (
                        CountSamePieces(row, col, 0, 1, piece) >= 5 ||
                        CountSamePieces(row, col, 1, 0, piece) >= 5 ||
                        CountSamePieces(row, col, 1, 1, piece) >= 5 ||
                        CountSamePieces(row, col, 1, -1, piece) >= 5))
                    {
                        return CurrentPlayer;
                    }
                }
            }
            return null;
        }

        protected override bool CheckDrawCondition()
        {
            // Board full, no winner
            for (int row = 0; row < 15; row++)
                for (int col = 0; col < 15; col++)
                    if (CurrentBoard.GetPiece(row, col) == null)
                        return false;

            return CheckWinCondition() == null;
        }

        private int CountSamePieces(int row, int col, int dr, int dc, Piece piece)
        {
            int count = 0;
            while (row >= 0 && row < 15 && col >= 0 && col < 15 &&
                   CurrentBoard.GetPiece(row, col)?.ToString() == piece.ToString())
            {
                count++;
                row += dr;
                col += dc;
            }
            return count;
        }
    }
}
