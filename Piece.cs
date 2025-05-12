namespace PlayerBoardGame
{
    /// <summary>
    /// Represents a piece in the game, adaptable for different board types.
    /// </summary>
    public class Piece
    {
        public string Symbol { get; }
        public Piece(string symbol)
        {
            Symbol = symbol;
        }
        public override string ToString()
        {
            return Symbol;
        }

    }

}