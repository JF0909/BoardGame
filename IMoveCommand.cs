namespace PlayerBoardGame
{
    public interface IMoveCommand
    {
        void Execute();
        void Undo();
        Move MoveData { get; } // used in command and game class
    }
}