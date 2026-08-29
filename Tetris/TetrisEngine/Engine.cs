using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine;

public class Engine
{
    
    public IGame StartSinglePlayerGame(string initials, int rowCount, int columnCount)
    {
        IPlayer player = new Player
        {
            Name = initials,
            Board = new TetrisBoard(rowCount, columnCount),
        };

        IGame game = Game.Start(GameMode.SinglePlayer, player);

        return game;
    }

}