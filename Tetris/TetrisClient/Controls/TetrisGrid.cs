using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TetrisEngine.Domain.Board;


namespace TetrisClient.Controls
{
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

        public void Put(Tetromino tetromino)
        {
            int[,] values = tetromino.Shape.Value;

            for (int i = 0; i < values.GetLength(0); i++)
            {

                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == 0) continue;

                    Label label = Children.OfType<Label>()
                    .First(label => GetRow(label) == tetromino.YPosition + i && GetColumn(label) == tetromino.XPosition + j);

                    label.Background = Brushes.Red;
                    label.BorderBrush = Brushes.White;

                }
            }

        }

        public void Erase(Tetromino tetromino)
        {
            int[,] values = tetromino.Shape.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == 0) continue;
                    Label label = Children.OfType<Label>()
                    .First(label => GetRow(label) == tetromino.YPosition + i && GetColumn(label) == tetromino.XPosition + j);
                    label.Background = Brushes.Transparent;
                    label.BorderBrush = Brushes.Transparent;
                }
            }

        }

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
    }
}
