namespace PlayerBoardGame
{
    /// <summary>
    /// Setup GameFactory class for creating different game instances
    /// </summary>
    public static class GameFactory
    {
        public static Game CreateGame(GameType type, GameMode mode, int? optionalSize = null)
        {
            switch (type)
            {
                case GameType.TicTacToe:
                    int sizeN = optionalSize ?? 3;
                    return new TicTacToeGame(mode, sizeN);
                case GameType.Notakto:
                    return new NotaktoGame(mode);
                case GameType.Gomoku:
                    return new GomokuGame(mode);
                default:
                    throw new  ArgumentException("Invalid Game Type.");
            }
        }
    }

}