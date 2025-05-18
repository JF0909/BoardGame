using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerBoardGame;

namespace PlayerBoardGame
{
    internal class NotaktoBoard : Board
    {
        private List<TicTacToeBoard> subboards;

        public NotaktoBoard() : base(3, 3)
        {
            subboards = new List<TicTacToeBoard>
            {
                new TicTacToeBoard(3),
                new TicTacToeBoard(3),
                new TicTacToeBoard(3)
            };
        }

        public TicTacToeBoard GetSubBoard(int index)
        {
            if (index < 0 || index >= subboards.Count)
                throw new ArgumentOutOfRangeException("Invalid board index.");
            return subboards[index];
        }

        public override Board Clone()
        {
            var clone = new NotaktoBoard();
            for (int i = 0; i < subboards.Count; i++)
            {
                clone.subboards[i] = (TicTacToeBoard)subboards[i].Clone();
            }
            return clone;
        }

        public override bool IsValidPosition(int row, int col)
        {
            return true;
        }

        public override void Display()
        {
            Console.WriteLine();
            Console.WriteLine("Notato Board:");
            Console.WriteLine();

            for (int i = 0; i < subboards.Count; i++)
            {
                Console.WriteLine($"Board {i + 1}:");
                subboards[i].Display();
            }

            Console.WriteLine();
        }

        public override void SetupInitialBoard()
        {
            foreach (var board in subboards)
            {
                board.SetupInitialBoard();
            }
        }

        public bool CheckBoardFinish(int index)
        {
            return CheckThreeInARow(subboards[index]);
        }

        public bool CheckAllFinish()
        {
            foreach(var board in subboards)
            {
                if (!CheckThreeInARow(board))
                    return false;
            }
            return true;
        }

        private bool CheckThreeInARow(Board board)
        {
            for (int i = 0;i < 3; i++)
            {
                //Row
                if (Line(board,i,0,0,1))
                    return true;

                //Column
                if (Line(board,0,i,1,0))
                    return true;
            }
            //Diagonal
            if (Line(board,0,0,1,1))
                return true;

            //Reverse diagonal
            if (Line(board,0,2,1,-1))
                return true;

            return false;
        }

        private bool Line(Board board, int r1, int c1, int r2, int c2)
        {
            var first = board.GetPiece(r1, c1);
            if (first == null)
                return false;

            for (int i = 1; i < 3; i++)
            {
                var next = board.GetPiece(r1 + i * r2, c1 + i * c2);
                if (next == null || next.ToString() != first.ToString())
                    return false;
            }
            return true;
        }
    }
}
