using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine.Domain.Game;

public interface IGame
{
    public IPlayer Player { get; set; }
    public IPlayer Opponent { get; set; }
    public IPlayer Winner { get; set; }
    public GameMode Mode { get; init; }
    public GameState GameState { get; set; }

    public List<Action<IPlayer>> OnGameOver { get; }
    public List<Action> OnBothReady { get; }

    public abstract static IGame Start(GameMode gameMode, IPlayer player);
    public abstract static IGame GetGame();
    public void Quit(IPlayer player);
    public void Over(IPlayer player);

}
