using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using DomainIPlayer = TetrisEngine.Domain.Game.Player.IPlayer;
using DomainPlayer = TetrisEngine.Domain.Game.Player.Player;
using DomainInterval = TetrisEngine.Domain.Game.Interval;
using DomainIGame = TetrisEngine.Domain.Game.IGame;
using DomainGame = TetrisEngine.Domain.Game.Game;

namespace TetrisEngine.Sdk;

public class Engine
{
    
    public DomainIGame StartSinglePlayerGame(int rowCount, int columnCount)
    {
        int seed = new Random().Next();
        DomainIPlayer player = new DomainPlayer
        (
            seed,
            new Random(seed),
            new TetrisBoard(rowCount, columnCount),
            new Score(),
            new Level(),
            new DomainInterval()
        );

        DomainIGame game = DomainGame.Start(GameMode.SinglePlayer, player);

        return game;
    }

}
