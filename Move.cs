namespace PlayerBoardGame
{
    /// <summary>
    /// Represents a player's move, including piece placement and position.
    /// </summary>
    public class Move 
    {
        public Player Player { get; }
        public int Row { get; }
        public int Col { get; }
        public Piece PiecePlaced { get; }
        public Move(Player player, int row, int col, Piece piecePlaced)
        {
            Player = player;
            Row = row;
            Col = col;
            PiecePlaced = piecePlaced;
        }
        public override string ToString()
        {
            return $"{Player}: Placed {PiecePlaced} at ({Row}, {Col})";
        }

    }
}