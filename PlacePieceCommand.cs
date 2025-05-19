namespace PlayerBoardGame
{
    public class PlacePieceCommand : IMoveCommand
    {
        private readonly Board _board;
        private readonly Move _move; // Contains Player, Row, Col, PiecePlaced
        private Piece? _previousPiece;

        public PlacePieceCommand(Board board, Move move)
        {
            _board = board;
            _move = move;
        }

        public void Execute()
        {
            if (_board is NotaktoBoard notaktoBoard && _move is NotaktoMove notaktoMove)
            {
                //Handle Notakto Specific game placement
                TicTacToeBoard subBoard = notaktoBoard.GetSubBoard(notaktoMove.SubBoardIndex);
                _previousPiece = subBoard.GetPiece(notaktoMove.Row, notaktoMove.Col);
                notaktoBoard.PlacePieceOnSubBoard(notaktoMove.SubBoardIndex, notaktoMove.Row, notaktoMove.Col, notaktoMove.PiecePlaced );
            }
            else
            {
                _previousPiece = _board.GetPiece(_move.Row, _move.Col);
                _board.PlacePiece(_move.Row, _move.Col, _move.PiecePlaced);

            }
            
        }

        public void Undo()
        {
            if (_board is NotaktoBoard notaktoBoard && _move is NotaktoMove notaktoMove)
            {
                //Undo Notakto move
                notaktoBoard.PlacePieceOnSubBoard(notaktoMove.SubBoardIndex, notaktoMove.Row, notaktoMove.Col, _previousPiece);
            }
            else
            {
                //Undo standard move for another two games
                _board.PlacePiece(_move.Row, _move.Col, _previousPiece);
            }
        }
    }
}