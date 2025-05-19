namespace PlayerBoardGame
{
    public class NotaktoMove: Move
    {
        public int SubBoardIndex { get; }
        public NotaktoMove (Player player, int subBoardIndex, int row, int col, Piece piecePlaced, Board boardStateBeforeMove)
        : base (player, row, col, piecePlaced, boardStateBeforeMove)
        {
            SubBoardIndex = subBoardIndex;
        }
        public override string ToString()
        {
            return $"{Player}: Placed {PiecePlaced} on Board {SubBoardIndex + 1} at ({Row + 1}, {Col + 1})";
        }

    }

}