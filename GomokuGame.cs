using System;
using System.Collections.Generic;

namespace PlayerBoardGame
{
    /// <summary>
    /// Implements the Gomoku game logic on a 15x15 board.
    /// Two players place pieces in turns, aiming to align 5 pieces in a row, column, or diagonal.
    /// </summary>
    public class GomokuGame : Game
    {
        private const int BoardSize = 15;

        // Constructor sets up game mode, board, and players
        public GomokuGame(GameMode mode)
        {
            GameMode = mode;
            CurrentBoard = CreateInitialBoard();
            Players = CreatePlayers(mode);
            CurrentPlayer = Players[0];
        }

        // Creates a 15x15 Gomoku board
        protected override Board CreateInitialBoard()
        {
            return new GomokuBoard(BoardSize, BoardSize);
        }

        // Creates players (human vs human or human vs computer)
        protected override List<Player> CreatePlayers(GameMode mode)
        {
            return new List<Player>
            {
                new HumanPlayer("Player 1", new Piece("X")),
                mode == GameMode.HumanVsHuman
                    ? new HumanPlayer("Player 2", new Piece("O"))
                    : new ComputerPlayer("Computer", new Piece("O"), this)
            };
        }

        // Validates a move (must be within bounds and empty)
        protected override bool IsMoveValid(Move move)
        {
            return CurrentBoard.IsValidPosition(move.Row, move.Col) &&
                   CurrentBoard.GetPiece(move.Row, move.Col) == null;
        }

        // Places the player’s piece on the board
        protected override void ApplyMove(Move move)
        {
            CurrentBoard.PlacePiece(move.Row, move.Col, move.Piece);
        }

        // Checks if current player has aligned 5 pieces in any direction
        protected override Player CheckWinCondition()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Piece piece = CurrentBoard.GetPiece(row, col);
                    if (piece != null && (
                        CountSamePieces(row, col, 0, 1, piece) >= 5 ||   // Horizontal
                        CountSamePieces(row, col, 1, 0, piece) >= 5 ||   // Vertical
                        CountSamePieces(row, col, 1, 1, piece) >= 5 ||   // Diagonal down
                        CountSamePieces(row, col, 1, -1, piece) >= 5))   // Diagonal up
                    {
                        return CurrentPlayer;
                    }
                }
            }
            return null;
        }

        // Returns true if board is full and no winner (i.e., draw)
        protected override bool CheckDrawCondition()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (CurrentBoard.GetPiece(row, col) == null)
                        return false;
                }
            }
            return CheckWinCondition() == null;
        }

        // Counts consecutive pieces of the same type in a direction
        private int CountSamePieces(int row, int col, int dr, int dc, Piece piece)
        {
            int count = 0;
            while (row >= 0 && row < BoardSize &&
                   col >= 0 && col < BoardSize &&
                   CurrentBoard.GetPiece(row, col)?.Symbol == piece.Symbol)
            {
                count++;
                row += dr;
                col += dc;
            }
            return count;
        }
    }
}
