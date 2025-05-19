
namespace PlayerBoardGame
{
    public class ComputerPlayer : Player
    {
        public ComputerPlayer(string name, Piece piece) : base(name, piece){ }
        //determines the best move
        public override Move GetMove(Board currentBoard)
        {
            Move winningMove = FindWinningMove(currentBoard);
            if(winningMove != null)
            {
                Console.WriteLine($"ComputerPlayer ({Name}) plays winning move at ({winningMove.Row}, {winningMove.Col})");
                return new Move(this, winningMove.Row, winningMove.Col, this.PlayerPiece, currentBoard.Clone());
            }
            else
            { 
                Move randomMove = FindRandomValidMove(currentBoard);
                Console.WriteLine($"ComputerPlayer ({Name}) randomly plays at ({randomMove.Row}, {randomMove.Col})");
                return new Move(this, randomMove.Row, randomMove.Col, this.PlayerPiece, currentBoard.Clone());
            }
            
        }
        //Help method: try to find a move that wins the game immediately
        private Move FindWinningMove(Board currentBoard)
        {
            //TODO: calling game specific logic in here
            //For each potential move, clone the board, place the piece on the clone
            //call CheckWinCondition() to verify the winning move
            return null;
        }

        private Move FindRandomValidMove(Board currentBoard)
        {
            Random random = new Random();
            int row, col;
            do
            {
                row = random.Next(currentBoard.Height);
                col = random.Next(currentBoard.Width);
            } while (!currentBoard.IsValidPosition(row, col) || !currentBoard.IsCellEmpty(row,col));
            return new Move(this,row,col,this.PlayerPiece, currentBoard.Clone());
            
        }
    
    }
}