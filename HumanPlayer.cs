namespace PlayerBoardGame
{
    public class HumanPlayer: Player
    {
        public HumanPlayer(string name, Piece piece) : base( name, piece) { }
        public override (Game.GameCommand command, Move? moveDetails) GetMove(Board currentBoard)
        {
            //HumanPlayer will get message until get a valid piece move
            while (true)
            {
                Console.Write($"{Name},Please enter your move ( '0,0' or 'undo', 'help'): ");
                string? rawInput = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrEmpty(rawInput))
                {
                    Console.WriteLine("Invalid input, cannot be empty.");
                    continue; 
                }

                Game.GameCommand systemCommand = ParseGameCommand(rawInput);
                if (systemCommand != Game.GameCommand.None) 
                {
                    return (systemCommand, null); 
                }

                Move? placementMove = ParsePlacementMove(rawInput, currentBoard);
                if (placementMove != null)
                {
                    return (Game.GameCommand.MakeMove, placementMove); 
                }
                Console.WriteLine("Invalid move, please enter 'row, col'(e.g.(0, 1))");
                if (currentBoard is NotaktoBoard)
                {
                    Console.WriteLine("For Notakto Game, Please enter boardIndex, row, col(e.g. '1,0,0')");
                }
                Console.WriteLine("Or enter 'help can check command info");
            }
        }

        //Parse system commands
        private Game.GameCommand ParseGameCommand(string input)
        {
            return input switch
            {
                "u" or "undo" => Game.GameCommand.Undo,
                "r" or "redo" => Game.GameCommand.Redo,
                "s" or "save" => Game.GameCommand.Save,
                "l" or "load" => Game.GameCommand.Load,
                "h" or "help" => Game.GameCommand.Help,
                "q" or "quit" => Game.GameCommand.Quit,
                _ => Game.GameCommand.None
            };
        }

        //parse user input as move placement(Move / NotaktoMove)
        private Move? ParsePlacementMove(string input, Board currentBoard)
        {
            string[] parts = input.Split(',');

            if (currentBoard is NotaktoBoard notaktoBoard) 
            {
                if (parts.Length == 3 &&
                    int.TryParse(parts[0].Trim(), out int boardIdxOneBased) && 
                    int.TryParse(parts[1].Trim(), out int row) &&         
                    int.TryParse(parts[2].Trim(), out int col))           
                {
                    int subBoardIndex = boardIdxOneBased - 1; 
                    return new NotaktoMove(this, subBoardIndex, row, col, this.PlayerPiece, currentBoard.Clone());
                }
            }
            else 
            {
                //For Tictactoe, Gomoku
                if (parts.Length == 2 &&
                    int.TryParse(parts[0].Trim(), out int row) && 
                    int.TryParse(parts[1].Trim(), out int col))   
                {
                    return new Move(this, row, col, this.PlayerPiece, currentBoard.Clone());
                }
            }
            return null; 
        } 
    }
}