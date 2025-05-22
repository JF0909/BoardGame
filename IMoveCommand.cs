namespace PlayerBoardGame
{
    public interface IMoveCommand
    {
        void Execute();
        void undo();
    }
}