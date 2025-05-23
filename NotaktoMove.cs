namespace PlayerBoardGame
{
<<<<<<< HEAD
    //SubBoardIndex as a new property
=======
>>>>>>> 4387e5b (copy file from local)
    //To specify which of the three Notakto sub-boards the move applies to.
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