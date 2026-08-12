using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine.Domain.Game
{
    public interface IGame
    {
        public IPlayer Player { get; init; }
        public IPlayer Opponent { get; set; }
        public GameMode Mode { get; init; }
        public GameState GameState { get; set; }
        public abstract static IGame Start(GameMode gameMode, IPlayer player);
        public abstract static IGame GetGame();
        public void Quit();
    }
}
