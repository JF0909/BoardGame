namespace PlayerBoardGame
{
    public class ComputerPlayer : Player
    {
        private Game currentGame;
        public ComputerPlayer(string name, Piece piece, Game game) : base(name, piece)
        {
            currentGame = game;
        }
        //determines the best move
        public override Move GetMove(Board currentBoard)
        {
            // TODO: Implement FindWinningMove() in Game subclasses
            Move winningMove = currentGame.FindWinningMove(this);
            if(winningMove != null)
            {
                Console.WriteLine($"AI ({Name}) plays winning move at ({winningMove.Row}, {winningMove.Col})");
                return winningMove;
            }
            else
            {
                //If no best move found, random move
                Random random = new Random();
                int row, col;
                do
                {
                    row = random.Next(currentBoard.Height);
                    col = random.Next(currentBoard.Width);
                } while (!currentBoard.IsCellEmpty(row,col));

                Console.WriteLine($"AI ({Name}) randomly plays at ({row}, {col})");
                return new Move(this,row,col, PlayerPiece);

            }
            
        }
    }
}