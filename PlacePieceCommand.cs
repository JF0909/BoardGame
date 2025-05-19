namespace PlayerBoardGame
{
    public class PlacePieceCommand : IMoveCommand
    {
        private readonly Board _targetBoard;
        private readonly Move _moveDetails; // Contains Player, Row, Col, PiecePlaced
        private Piece? _originalPieceInCell;

        public PlacePieceCommand(Board board, Move move)
        {
            _targetBoard = board;
            _moveDetails = move;
        }

        public void Execute()
        {
            _originalPieceInCell = _targetBoard.GetPiece(_moveDetails.Row, _moveDetails.Col);
            _targetBoard.PlacePiece(_moveDetails.Row, _moveDetails.Col, _moveDetails.PiecePlaced);
        }

        public void Undo()
        {
            _targetBoard.PlacePiece(_moveDetails.Row, _moveDetails.Col, _originalPieceInCell);
        }
    }
}