namespace PlayerBoardGame
{
    public class HumanPlayer: Player
    {
        public HumanPlayer(string name, Piece piece) : base( name, piece) { }
        public override Move GetMove(Board currentBoard)
        {
            int row, col;
            while (true)
            {
                Console.Write($"Enter row (0-{currentBoard.Height - 1}): ");
                row = Convert.ToInt32(Console.ReadLine());

                Console.Write($"Enter column (0-{currentBoard.Width - 1}): ");
                col = Convert.ToInt32(Console.ReadLine());

                if (currentBoard.IsValidPosition(row, col) && currentBoard.IsCellEmpty(row, col))
                {
                    return new Move(this, row, col, PlayerPiece);
                }
                else
                {
                    Console.WriteLine("Invalid move, please try again.");
                }
           }
        }
    }
}