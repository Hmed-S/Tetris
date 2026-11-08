using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player.Factory;

public interface IPlayerFactory
{
    public IPlayer CreatePlayer(GameMode gameMode, int seed, Name name, TetrisBoard board, Score score, Level level, Interval interval);
}