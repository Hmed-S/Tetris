using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine;

public class Engine
{
    
    public IGame StartSinglePlayerGame(int rowCount, int columnCount)
    {
        int seed = new Random().Next();
        IPlayer player = new Player
        (
            seed,
            new Random(seed),
            new TetrisBoard(rowCount, columnCount),
            new Score(),
            new Level(),
            new Interval()
        );

        IGame game = Game.Start(GameMode.SinglePlayer, player);

        return game;
    }

}
