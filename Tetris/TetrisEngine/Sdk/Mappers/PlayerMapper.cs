using DomainIPlayer = TetrisEngine.Domain.Game.Player.IPlayer;

namespace TetrisEngine.Sdk.Mappers;

internal static class PlayerMapper
{
    public static Player ToDto(this DomainIPlayer player)
    {
        return new Player
        {
            Name = player.Name.Value,
            Preview = player.Preview.ToDto(),
            CurrentTetromino = player.CurrentTetromino.ToDto(),
            Board = new Board
            {
                ColumnCount = player.Board.Width,
                RowCount = player.Board.Height,
                Values = player.Board.Values
            },
            Interval = new Interval
            {
                FastDropSeconds = player.Interval.FastDrop.Seconds,
                FastDropMiliSeconds = player.Interval.FastDrop.MiliSeconds,
                Seconds = player.Interval.Seconds,
                MiliSeconds = player.Interval.MiliSeconds,
            },
            Score = player.Score.Value,
            Level = player.Level.Value,
            LineCount = player.Lines.Value,
        };
    }

}
