namespace PlayerBoardGame
{
    public class RedoCommand : IMoveCommand
    {
        private readonly List<IMoveCommand> _redoHistory;
        private readonly List<IMoveCommand> _moveHistory;

        public RedoCommand(List<IMoveCommand> redoHistory, List<IMoveCommand> moveHistory)
        {
            _redoHistory = redoHistory;
            _moveHistory = moveHistory;
        }

        public void Execute()
        {
            if (_redoHistory.Any())
            {
                IMoveCommand commandToRedo = _redoHistory.Last();
                commandToRedo.Execute();
                _redoHistory.RemoveAt(_redoHistory.Count - 1);
                _moveHistory.Add(commandToRedo);
            }
            else
            {
                Console.WriteLine("No moves to redo.");
            }
        }

        public void Undo(){}

    }
    
}