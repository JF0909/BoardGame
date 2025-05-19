namespace PlayerBoardGame
{
    public class ComputerPlayer : Player
    {
        //Store game instance for calling checkwincondition() in order to find winning move
        private readonly Game _game;
        public ComputerPlayer(string name, Piece piece, Game game) : base(name, piece)
        { 
            _game = game;
        }
   
        public override IMoveCommand GetMove(Board currentBoard)
        {
            Move winningMove = FindWinningMove(currentBoard);
            if(winningMove != null)
            {
                Console.WriteLine($"ComputerPlayer ({Name}) plays winning move at ({winningMove.Row}, {winningMove.Col})");
                //ComputerPlayer move immediately, but won't be stored in Redo/Undo history
                currentBoard.PlacePiece(winningMove.Row, winningMove.Col, this.PlayerPiece);
                return null;
            }
            else
            { 
                Move randomMove = FindRandomValidMove(currentBoard);
                Console.WriteLine($"ComputerPlayer ({Name}) randomly plays at ({randomMove.Row}, {randomMove.Col})");
                currentBoard.PlacePiece(randomMove.Row, randomMove.Col, this.PlayerPiece);
                return null;
            }
            
        }
        //Help method: try to find a move that wins the game immediately
        private Move FindWinningMove(Board currentBoard)
        {
            for (int row = 0; row < currentBoard.Height; row++)
            {
                for (int col = 0; col < currentBoard.Width; col++)
                {
                    if (currentBoard.IsValidPosition(row, col) && currentBoard.IsCellEmpty(row, col))
                    {
                        // Clone the board and simulate the move
                        Board clonedBoard = currentBoard.Clone();
                        clonedBoard.PlacePiece(row, col, this.PlayerPiece);

                        Player? winner = _game.GetWinner();

                        //If it can results a win, then return
                        if (winner == this) 
                        {
                            return new Move(this, row, col, this.PlayerPiece, currentBoard.Clone());
                        }

                    }
                }
            }
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