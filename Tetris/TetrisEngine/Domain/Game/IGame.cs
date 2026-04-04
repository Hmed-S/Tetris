namespace TetrisEngine.Domain.Game
{
    public interface IGame
    {
        public Player Player { get; init; }
        public Player Opponent { get; set; }
        public GameMode Mode { get; init; }
        public GameState GameState { get; set; }
        public abstract static IGame Start(GameMode gameMode, Player player);
        public abstract static IGame GetGame();
        public void Quit();
    }
}
