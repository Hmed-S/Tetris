using DomainGame = TetrisEngine.Domain.Game.IGame;

namespace TetrisEngine.Sdk.Mappers;

internal static class GameMapper
{

    public static Game ToDto(this DomainGame game)
    {
        return new Game
        {
            Player = new Player
            {
                Preview = game.Player.Preview.ToDto(),
                CurrentTetromino = game.Player.CurrentTetromino.ToDto(),
                Board = new Board
                {
                    ColumnCount = game.Player.Board.Width,
                    RowCount = game.Player.Board.Height,
                },
                Interval = new Interval
                {
                    FastDropSeconds = game.Player.Interval.FastDrop.Seconds,
                    FastDropMiliSeconds = game.Player.Interval.FastDrop.MiliSeconds,
                    Seconds = game.Player.Interval.Seconds,
                    MiliSeconds = game.Player.Interval.MiliSeconds,
                },
                Score = game.Player.Score.Value,
                Level = game.Player.Level.Value,
                LineCount = game.Player.Lines.Value,
            },
            State = GameState.Playing,
        };
    }

}
