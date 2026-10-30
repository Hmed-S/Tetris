using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player.Factory;

public class PlayerFactory : IPlayerFactory
{
    public IPlayer CreatePlayer(GameMode gameMode, int seed, Name name, TetrisBoard board, Score score, Level level, Interval interval)
        => gameMode switch
        {
            GameMode.MultiPlayer => new MultiPlayerPlayer(name, board, score, level, interval),
            _ => new Player(seed, new Random(seed), board, score, level, interval)
        };

}
