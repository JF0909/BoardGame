namespace PlayerBoardGame
{
    /// <summary>
    /// Represents a player in the game, managing basic player attributes.
    /// </summary>
    public abstract class Player
    {
        public string Name { get; }
        public Piece PlayerPiece { get; }
        public Player(string name, Piece piece)
        {
            Name = name;
            PlayerPiece = piece;
        }
        public abstract Move GetMove(Board currentBoard);
        public override string ToString()
        {
            return $"{Name} ({PlayerPiece})";
        }

    }
}