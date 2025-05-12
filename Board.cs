namespace PlayerBoardGame
{
    ///<summary>
    ///Abstarct base Board class
    ///manage different size of board, piece management, basic operations
    ///</summary>
    public abstract class Board
    {
        public int Width { get; }
        public int Height { get; }
        protected Piece[,] Cells;

        //Initialize the Board with the given size
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Piece[height, width];
        }

        //Place a piece on the board
        public virtual bool PlacePiece(int row, int col, Piece piece)
        {
            if (IsValidPosition(row, col) && IsCellEmpty(row,col))
            {
                Cells[row,col] = piece;
                return true;
            }
            return false;
        }

        //Retrieves the piece at the given position
        public virtual Piece GetPiece(int row, int col)
        {
            return IsValidPosition(row,col)? Cells[row,col]:null;
        }

        //Check if a cell is empty
        public bool IsCellEmpty(int row, int col)
        {
            return IsValidPosition(row,col) && Cells[row, col] == null;
        }

        //Make sure piece was placed in valid position
        //implement game-specific rules in child Classes
        public abstract bool IsValidPosition(int row, int col);

        //Clone the board for Undo and Redo functionality
        public abstract Board Clone();

        //Display the board
        public abstract void Display();

        //Allow child classes to setup an initial board config
        //Gomoku starts with an empty grid
        //Natakto starts with three board
        public abstract void SetupInitialBoard();

    }
}