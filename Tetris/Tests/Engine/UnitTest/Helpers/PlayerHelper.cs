using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;
using TetrisEngine.Domain.Game.Player.Factory;

namespace TetrisEngine.Tests.UnitTest;

internal static class PlayerHelper
{
    public static IPlayer CreateSinglePlayer(
        int seed = 123,
        int rows = 20,
        int columns = 10)
    {
        Name name = new("TestPlayer");
        TetrisBoard board = new(rows, columns);
        Score score = new();
        Level level = new();
        Interval interval = new();

        PlayerFactory factory = new();

        IPlayer player = factory.CreatePlayer(
            GameMode.SinglePlayer,
            seed,
            name,
            board,
            score,
            level,
            interval);

        IGame game = Game.Start(GameMode.SinglePlayer, player);

        return player;
    }

    public static (IPlayer Player, IPlayer Opponent, Game Game)
        CreateMultiplayerPlayers(int seed = 123, int rows = 20, int columns = 10)
    {
        Name playerName = new("Player");
        Name opponentName = new("Opponent");

        TetrisBoard playerBoard = new(rows, columns);
        TetrisBoard opponentBoard = new(rows, columns);

        Score playerScore = new();
        Score opponentScore = new();

        Level playerLevel = new();
        Level opponentLevel = new();

        Interval playerInterval = new();
        Interval opponentInterval = new();

        PlayerFactory factory = new();

        IPlayer player = factory.CreatePlayer(
            GameMode.MultiPlayer,
            seed,
            playerName,
            playerBoard,
            playerScore,
            playerLevel,
            playerInterval);

        IPlayer opponent = factory.CreatePlayer(
            GameMode.MultiPlayer,
            seed + 1,
            opponentName,
            opponentBoard,
            opponentScore,
            opponentLevel,
            opponentInterval);

        IGame game = Game.Start(GameMode.MultiPlayer, player);
        game.Opponent = opponent;

        return (player, opponent, (Game)game);
    }

    public static void FillBottomRowExcept(
        TetrisBoard board,
        params int[] emptyColumns)
    {
        for (int column = 0; column < board.Width; column++)
        {
            if (!emptyColumns.Contains(column))
            {
                board.Values[board.Height - 1, column] = 1;
            }
        }
    }

}
