namespace PlayerBoardGame
{
    public class UndoCommand : IMoveCommand
    {
        private readonly List<IMoveCommand> _moveHistory;
        private readonly List<IMoveCommand> _redoHistory;

        public UndoCommand(List<IMoveCommand> moveHistory, List<IMoveCommand> redoHistory)
        {
            _moveHistory = moveHistory;
            _redoHistory = redoHistory;
        }

        public void Execute()
        {
            if (_moveHistory.Any())
            {
                IMoveCommand commandToUndo = _moveHistory.Last();
                commandToUndo.Undo();
                _moveHistory.RemoveAt(_moveHistory.Count - 1);
                _redoHistory.Add(commandToUndo);
            }
            else
            {
                Console.WriteLine("No moves to undo.");
            }
        }

        public void Undo(){}

    }
    
}