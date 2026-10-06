using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TetrisEngine.Sdk;
using Color = TetrisEngine.Sdk.Color;

namespace TetrisClient.Controls;

public class TetrisGrid : Grid
{
    private int _rowCount;
    private int _columnCount;
    public TetrisGrid(int rowCount, int columnCount)
    {
        _rowCount = rowCount;
        _columnCount = columnCount;

        for (int i = 0; i < _rowCount; i++)
        {
            RowDefinitions.Add(new RowDefinition());
        }
        for (int j = 0; j < _columnCount; j++)
        {
            ColumnDefinitions.Add(new ColumnDefinition());
        }

        for (int i = 0; i < _rowCount; i++)
        {
            for (int j = 0; j < _columnCount; j++)
            {
                Label cell = new ()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent,
                    Width = 30,
                    Height = 30,
                };

                SetRow(cell, i);
                SetColumn(cell, j);
                Children.Add(cell);
            }
        }

    }

    private Brush GetColor(Tetromino tetromino) => tetromino.Color switch
    {
        Color.Blue => Brushes.Blue,
        Color.Cyan => Brushes.Cyan,
        Color.Purple => Brushes.Purple,
        Color.Orange => Brushes.Orange,
        Color.Yellow => Brushes.Yellow,
        Color.Green => Brushes.Green,
        Color.RED => Brushes.Red,
        _ => Brushes.Black,
    };

    private void DoPut(Tetromino tetromino, Brush background, Brush foreGround)
    {
        foreach (Points point in tetromino.Points)
        {
            Label label = Children.OfType<Label>()
            .First(label => GetRow(label) == point.Row && GetColumn(label) == point.Column);

            label.Background = background;
            label.BorderBrush = foreGround;
        }
    }

    public void Put(Tetromino tetromino) => DoPut(tetromino, GetColor(tetromino), Brushes.White);

    public void Erase(Tetromino tetromino) => DoPut(tetromino, Brushes.Transparent, Brushes.Transparent);

    public void Clear()
    {
        for (int i = 0; i < _rowCount; i++)
        {
            for (int j = 0; j < _columnCount; j++)
            {
                Label label = Children.OfType<Label>()
                .First(label => GetRow(label) ==  i && GetColumn(label) == j);
                label.Background = Brushes.Transparent;
                label.BorderBrush = Brushes.Transparent;
            }
        }
    }

    internal void ClearLine(int row)
    {
        for (int j = 0; j < _columnCount; j++)
        {
            Label label = Children.OfType<Label>()
            .First(label => GetRow(label) == row && GetColumn(label) == j);
            label.Background = Brushes.Transparent;
            label.BorderBrush = Brushes.Transparent;
        }
    }

    internal void SwitchRows(int row1, int row2)
    {

        for (int j = 0; j < _columnCount; j++)
        {
            Label label1 = Children.OfType<Label>()
            .First(label => GetRow(label) == row1 && GetColumn(label) == j);

            Label label2 = Children.OfType<Label>()
                .First(label => GetRow(label) == row2 && GetColumn(label) == j);

            SetRow(label1, row2);
            SetRow(label2, row1);
        }
        
    }

}
