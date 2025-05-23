namespace PlayerBoardGame
{
    /// <summary>
    /// Handle Undo/Redo for TicTacToeGame _usedNumbers
    /// </summary>
    public class TicTacToeCommand : IMoveCommand
    {
        private readonly Board _board;
        private readonly Move _move;
        private readonly HashSet<int> _usedNumbers;

        private readonly int _numberPlaced;
        private Piece? _originalPieceInCell;

        public TicTacToeCommand(Board board, Move move, HashSet<int> usedNumbersSet, int numberValue)
        {
            _board = board;
            _move = move;
            _usedNumbers = usedNumbersSet;
            _numberPlaced = numberValue;
        }

        public void Execute()
        {
            _originalPieceInCell = _board.GetPiece(_move.Row, _move.Col);
            _board.PlacePiece(_move.Row, _move.Col, _move.PiecePlaced);
            _usedNumbers.Add(_numberPlaced);
        }

        public void Undo()
        {
            _board.PlacePiece(_move.Row, _move.Col, _originalPieceInCell);
            _usedNumbers.Remove(_numberPlaced);
        }
    }
}        