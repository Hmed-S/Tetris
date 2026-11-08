using DomainIPlayer = TetrisEngine.Domain.Game.Player.IPlayer;

namespace TetrisEngine.Sdk.Mappers;

internal static class PlayerActionEventMapper
{
    public static void MapPlayerActionEvents(this DomainIPlayer player, Player dto)
    {
        player.OnTetrominoPositionChange.Add((position) => dto.CurrentTetromino = position.CurrentTetromino.ToDto());
        player.OnPreviewChange.Add((preview) => dto.Preview = preview.ToDto());
        player.OnTetrominoPositionChange.Add((position) => dto.Board.Values = (int[,])player.Board.Values.Clone());
        player.OnReady.Add(() => dto.IsReady = true);

        player.OnLevelChange.Add((level) => {
            dto.Level = level.Value;
            dto.Interval.Seconds = player.Interval.Seconds;
            dto.Interval.MiliSeconds = player.Interval.MiliSeconds;
        });
        player.OnLineClear.Add((line) => dto.Board.LastClearedLine = line);
        player.OnLineSwab.Add((bottomLine, topLine) => dto.Board.LastSwappedLines = [bottomLine, topLine]);
        player.OnScoreChange.Add((score) => dto.Score = score.Value);
        player.OnLineCountChange.Add((lines) => dto.LineCount = lines.Value);
    }

}
