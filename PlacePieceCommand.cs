namespace PlayerBoardGame
{
    /// <summary>
    /// Command to place a piece on a standard board
    /// Apply for TicTacToe and Gomoku
    /// </summary>
    public class PlacePieceCommand : IMoveCommand
    {
        private readonly Board _board;
        private readonly Move _move; 
        private Piece? _previousPiece;

        public PlacePieceCommand(Board board, Move move)
        {
            _board = board;
            _move = move;
        }

<<<<<<< HEAD
        public void Execute()
        {
            //Store what was in the cell before placing the new piece (for Undo)
            _previousPiece = _board.GetPiece(_move.Row, _move.Col);
            //Execute the move by placing the piece
            _board.PlacePiece(_move.Row, _move.Col, _move.PiecePlaced);
=======
        //public access to _move
        public Move OriginalMoveDetails => _move;

        public void Execute()
        {
            _previousPiece = _board.GetPiece(_move.Row, _move.Col);
            _board.PlacePiece(_move.Row, _move.Col, _move.PiecePlaced);

>>>>>>> 4387e5b (copy file from local)
        }

        public void Undo()
        {
<<<<<<< HEAD
           _board.PlacePiece(_move.Row, _move.Col, _previousPiece);
=======
            //Debug
            Piece? pieceCurrentThere = _board.GetPiece(_move.Row, _move.Col);
            _board.PlacePiece(_move.Row, _move.Col, _previousPiece!); 
>>>>>>> 4387e5b (copy file from local)
        }
    }
}