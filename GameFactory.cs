namespace PlayerBoardGame
{
    /// <summary>
    /// Setup GameFactory class for creating different game instances
    /// </summary>
    public static class GameFactory
    {
        public static GameFactory CreateGame(GameType type, GameMode mode)
        {
            switch (type)
            {
                case GameType.NumericalTicTacToe:
                    return new NumericalTicTacToeGame(mode);
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