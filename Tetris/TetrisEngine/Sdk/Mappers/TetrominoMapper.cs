using TetrisEngine.Domain.Board;
using DomainTetromino = TetrisEngine.Domain.Board.Tetromino;
using DomainColor = TetrisEngine.Domain.Board.Color;

namespace TetrisEngine.Sdk.Mappers;

internal static class TetrominoMapper
{

    private static Color ConvertColor(DomainColor domainColor) => domainColor switch
    {
        DomainColor.Blue => Color.Blue,
        DomainColor.Purple => Color.Purple,
        DomainColor.Yellow => Color.Yellow,
        DomainColor.RED => Color.RED,
        DomainColor.Green => Color.Green,
        DomainColor.Cyan => Color.Cyan,
        _ => Color.Orange
    };

    public static Tetromino ToDto(this DomainTetromino tetromino)
    {
        return new()
        {
            XPosition = tetromino.XPosition,
            YPosition = tetromino.YPosition,
            Color = ConvertColor(tetromino.Color),
            Islanded = tetromino.DropStatus == DropStatus.Landed,
            Shape = tetromino.Shape.Value,
            Points = tetromino.Points.Select(point => new Points { Column = point.Column, Row = point.Row }).ToList(),
        };

    }

}
