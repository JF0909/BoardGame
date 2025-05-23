using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerBoardGame;

namespace PlayerBoardGame
{
    //Composite Pattern Applied
    public class NotaktoBoard : Board
    {
        private List<TicTacToeBoard> subBoards;
        private List<bool> subBoardFinishedStates;
        public const int NumberOfSubBoards = 3;
        //Store the piece for checking
        private readonly Piece _notaktoPiece;

        public NotaktoBoard() : base(0, 0)
        {
            _notaktoPiece = new Piece("X");
            subBoards = new List<TicTacToeBoard>();
            subBoardFinishedStates = new List<bool>();

            for (int i = 0; i< NumberOfSubBoards; i++)
            {
                // Each subBoard is 3*3
                subBoards.Add(new TicTacToeBoard(3)); 
                subBoardFinishedStates.Add(false);
            }
        }

        public int SubBoardCount => subBoards.Count;

        public TicTacToeBoard GetSubBoard(int index)
        {
            if (index < 0 || index >= subBoards.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Sub-board index is out of range.");
            }
            return subBoards[index];
        }

        public bool IsSubBoardFinished(int subBoardIndex)
        {
            if (subBoardIndex < 0 || subBoardIndex >= subBoardFinishedStates.Count)
            {
                return true;
            }
            return subBoardFinishedStates[subBoardIndex];
        }

        public bool AreAllSubBoardsFinished()
        {
            return subBoardFinishedStates.All(finished => finished);
        }

        public bool PlacePieceOnSubBoard(int subBoardIndex, int row, int col, Piece piece)
        {
            if (subBoardIndex < 0 || subBoardIndex >= subBoards.Count)
            {
<<<<<<< HEAD
                Console.WriteLine($"Error: Invalid sub-board index {subBoardIndex} in PlacePieceOnSubBoard.");
=======
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: Invalid sub-board index {subBoardIndex} in PlacePieceOnSubBoard.");
                Console.ResetColor();
>>>>>>> 4387e5b (copy file from local)
                return false;
            }
            if (IsSubBoardFinished(subBoardIndex))
            {
<<<<<<< HEAD
                Console.WriteLine("Error: Sub-board {subBoardIndex + 1} is already finished.");
=======
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError: Sub-board {subBoardIndex + 1} is already finished.");
                Console.ResetColor();
>>>>>>> 4387e5b (copy file from local)
                return false;
            }
            
            TicTacToeBoard targetSubBoard = subBoards[subBoardIndex];
            if (targetSubBoard.IsValidPosition(row,col) && targetSubBoard.IsCellEmpty(row,col))
            {
                targetSubBoard.PlacePiece(row, col, piece);
                if (!subBoardFinishedStates[subBoardIndex] && CheckThreeInARow(targetSubBoard, piece))
                {
                    subBoardFinishedStates[subBoardIndex] = true;
<<<<<<< HEAD
                    Console.WriteLine($"Sub-board {subBoardIndex + 1} is now finished!");
=======
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Sub-board {subBoardIndex + 1} is now finished!");
                    Console.ResetColor();
>>>>>>> 4387e5b (copy file from local)
                }
                return true;

            }
<<<<<<< HEAD
            Console.WriteLine($"Error: Invalid move on sub-board {subBoardIndex + 1} at ({row},{col}). Cell not empty or out of bounds.");
            return false;
        }

        //Remove piece and re-check method
=======
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: Invalid move on sub-board {subBoardIndex + 1} at ({row},{col}). Cell not empty or out of bounds.");
            Console.ResetColor();
            return false;
        }

        //Remove piece and re-check method for NotaktoGame
>>>>>>> 4387e5b (copy file from local)
        public void RemovePieceOnSubBoard (int subBoardIndex, int row, int col)
        {
            if (subBoardIndex < 0 || subBoardIndex >= subBoards.Count)
            {
<<<<<<< HEAD
                Console.WriteLine($"Error: Invalid sub-board index {subBoardIndex} in RemovePieceOnSubBoard.");
=======
                Console.WriteLine($"\nError: Invalid sub-board index {subBoardIndex} in RemovePieceOnSubBoard.");
>>>>>>> 4387e5b (copy file from local)
                return;
            }
            TicTacToeBoard targetSubBoard = subBoards[subBoardIndex];
            if (!targetSubBoard.IsValidPosition(row, col))
            {
<<<<<<< HEAD
                Console.WriteLine($"Error: Invalid coordinates ({row},{col}) for sub-board {subBoardIndex + 1} in RemovePieceOnSubBoard.");
                return;
            }

            targetSubBoard.PlacePiece(row, col, null);
=======
                Console.WriteLine($"\nError: Invalid coordinates ({row},{col}) for sub-board {subBoardIndex + 1} in RemovePieceOnSubBoard.");
                return;
            }

            targetSubBoard.PlacePiece(row, col, null!);
>>>>>>> 4387e5b (copy file from local)
            if (CheckThreeInARow(targetSubBoard, _notaktoPiece))
            {
                subBoardFinishedStates[subBoardIndex] = true;
            }
            else
            {
                subBoardFinishedStates[subBoardIndex] = false; 
            }
        }


        private bool CheckThreeInARow(TicTacToeBoard board, Piece piece)
        {
            if (piece == null) return false;
            string p = piece.Symbol;

            //Check Rows
            for (int i = 0;i < 3; i++)
            {
                if (board.GetPiece(i,0)?.Symbol == p &&
                    board.GetPiece(i,1)?.Symbol == p &&
                    board.GetPiece(i,2)?.Symbol == p)
                return true;
            }
            //Check Columns
            for (int j = 0; j < 3; j++)
            {
                if (board.GetPiece(0,j)?.Symbol == p &&
                    board.GetPiece(1,j)?.Symbol == p &&
                    board.GetPiece(2,j)?.Symbol == p)
                return true;
            }
            //Check Diagonals
            if (board.GetPiece(0,0)?.Symbol == p &&
                board.GetPiece(1,1)?.Symbol == p &&
                board.GetPiece(2,2)?.Symbol == p) 
            return true;
            if (board.GetPiece(0,2)?.Symbol == p &&
                board.GetPiece(1,1)?.Symbol == p &&
                board.GetPiece(2,0)?.Symbol == p)
            return true;

            return false;
        }

        public override Board Clone()
        {
            NotaktoBoard clone = new NotaktoBoard();
            for (int i = 0; i < this.subBoards.Count; i++)
            {
                clone.subBoards[i] = (TicTacToeBoard)this.subBoards[i].Clone();
                clone.subBoardFinishedStates[i] = this.subBoardFinishedStates[i];
            }
            return clone;
        }

        public override bool IsValidPosition(int row, int col)
        {
            return false;
        }

        public override void Display()
        {
            Console.WriteLine();
            Console.WriteLine("Notakto Board:");
            Console.WriteLine();

            for (int i = 0; i < subBoards.Count; i++)
            {
                Console.WriteLine($"Sub-Board {i + 1}:" + (IsSubBoardFinished(i) ? " (FINISHED)" : " (Active)"));
                subBoards[i].Display();
            }

            Console.WriteLine();
        }

        public override void SetupInitialBoard()
        {
            subBoards.Clear();
            subBoardFinishedStates.Clear();
            for (int i = 0; i< NumberOfSubBoards; i++)
            {
                subBoards.Add(new TicTacToeBoard(3));
                subBoardFinishedStates.Add(false);
            }
        }

        public override bool IsCellEmpty(int row, int col)
        {
            return false;
        }

        public override Piece ? GetPiece(int row, int col)
        {
            return null;
        }

<<<<<<< HEAD
        public override bool PlacePiece(int row, int col, Piece piece)
=======
        public override bool PlacePiece(int row, int col, Piece? piece)
>>>>>>> 4387e5b (copy file from local)
        {
            return false;

        }
    }
}
