namespace PlayerBoardGame
{
    /// <summary>
    /// Represents a player in the game, managing basic player attributes.
    /// </summary>
    public abstract class Player
    {
        public string Name { get; }
        public Piece PlayerPiece { get; }
        protected Player(string name, Piece piece)
        {
            Name = name;
            PlayerPiece = piece;
        }
        //public abstract IMoveCommand GetMove(Board currentBoard);
        public abstract (Game.GameCommand command, Move? moveDetails) GetMove(Board currentBoard);
        public override string ToString()
        {
            return $"{Name} ({PlayerPiece?.Symbol})";
        }

    }
}