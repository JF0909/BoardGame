namespace PlayerBoardGame
{
    public class NotaktoCommand: IMoveCommand
    {
        private readonly NotaktoBoard _notaktoBoard;
        private readonly NotaktoMove _notaktoMove;

        public NotaktoCommand(NotaktoBoard board, NotaktoMove move)
        {
            _notaktoBoard = board ?? throw new ArgumentNullException(nameof(board));
            _notaktoMove = move ?? throw new ArgumentNullException(nameof(move));
        }
        public void Execute()
        {
            // NotaktoBoard handle the PlacePieceOnSubBoard method
            // Placing the piece and updating the sub-board's finished state.
            _notaktoBoard.PlacePieceOnSubBoard(
                _notaktoMove.SubBoardIndex,
                _notaktoMove.Row,
                _notaktoMove.Col,
                _notaktoMove.PiecePlaced
            );
        }
        public void Undo()
        {
            // TODO: NotaktoBoard needs a method to remove the piece AND correctly
            // re-evaluate the finished state of that sub-board.
            _notaktoBoard.RemovePieceOnSubBoard(
                _notaktoMove.SubBoardIndex,
                _notaktoMove.Row,
                _notaktoMove.Col
            );
        }

    }
}