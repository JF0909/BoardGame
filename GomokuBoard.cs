using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerBoardGame;

namespace PlayerBoardGame
{
    public class GomokuBoard : Board
    {
        public GomokuBoard() : base(15, 15)
        {
            SetupInitialBoard();
        }

        public override Board Clone()
        {
            var clone = new GomokuBoard();
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    clone.Cells[row, col] = this.Cells[row, col];
                }
            }
            return clone;
        }

        public override bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < Height && col >= 0 && col < Width;
        }

        public override void Display()
        {
            Console.WriteLine();
            Console.WriteLine("Gomoku Board:");
            Console.WriteLine();

            // Column header
            Console.WriteLine("   ");
            for (int col = 0; col < Width; col++)
            {
                Console.Write($"{col + 1,4}");
            }
            Console.WriteLine();

            for (int row = 0; row < Height; row++)
            {
                Console.WriteLine();
                Console.Write($"{row + 1,-4}");

                for (int col = 0; col < Width; col++)
                {
                    var piece = Cells[row, col];
                    Console.Write(piece == null ? "." : piece.ToString(), -3);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public override void SetupInitialBoard()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Cells[row, col] = null;
                }
            }
        }
    }
}
