namespace PlayerBoardGame
{
    public class HumanPlayer: Player
    {
        public HumanPlayer(string name, Piece piece) : base( name, piece) { }
        //Validate integer input
        private int GetValidatedIntInput (string prompt, int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write($"{prompt} ({min}-{max}): ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out value) && value >= min && value <= max)
                {
                    return value;
                }
                else
                {
                    Console.WriteLine($"Invalid input. Please enter a whole number between {min} and {max}.");
                }
            }
        }
        public override IMoveCommand GetMove(Board currentBoard)
        {
            Console.WriteLine($"\n{Name} ({PlayerPiece.Symbol}), it's your turn.");
            currentBoard.Display();

            if (currentBoard is NotaktoBoard notaktoBoard)
            {
                int subBoardIndex, row, col;
                while (true)
                {
                    Console.WriteLine("Playing on Notakto Board (select an active sub-board).");
                    subBoardIndex = GetValidatedIntInput("Enter target sub-board index", 1, notaktoBoard.SubBoardCount) - 1;

                    if (notaktoBoard.IsSubBoardFinished(subBoardIndex))
                    {
                        Console.WriteLine($"Sub-board {subBoardIndex + 1} is already finished. Please choose an active board.");
                        notaktoBoard.Display();
                        continue;
                    }

                    TicTacToeBoard targetSubBoard = notaktoBoard.GetSubBoard(subBoardIndex);
                    Console.WriteLine($"Selected Sub-Board {subBoardIndex + 1}.");

                    row = GetValidatedIntInput($"Enter row for sub-board {subBoardIndex + 1}", 1, targetSubBoard.Height) - 1;
                    col = GetValidatedIntInput($"Enter column for sub-board {subBoardIndex + 1}", 1, targetSubBoard.Width) - 1;

                    if(targetSubBoard.IsValidPosition(row, col) && targetSubBoard.IsCellEmpty(row, col))
                    {
                        Move move = new NotaktoMove(this, subBoardIndex, row, col, this.PlayerPiece, currentBoard.Clone());
                        return new PlacePieceCommand(currentBoard, move);
                    }
                    else
                    {
                        Console.WriteLine("Invalid move on the sub-board (cell occupied or out of bounds). Please try again.");
                    }
                }
            }
            else
            {
                //Logic for another 2 games
                int row, col;
                while (true)
                {
                    row = GetValidatedIntInput("Enter row", 1,currentBoard.Height) - 1;
                    col = GetValidatedIntInput("Enter column",1,currentBoard.Width) - 1;

                    if (currentBoard.IsValidPosition(row, col) && currentBoard.IsCellEmpty(row, col))
                    {
                        Move move = new Move(this, row, col, this.PlayerPiece, currentBoard.Clone());
                        return new PlacePieceCommand(currentBoard, move);

                    }
                    else
                    {
                        Console.WriteLine("Invalid move (position out of bounds or cell occupied). Please try again.");
                    }
                }

            }
            
        }
    }
}