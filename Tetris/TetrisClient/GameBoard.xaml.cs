using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TetrisEngine.Domain.Board;
using Matrix = TetrisEngine.Domain.Board.Matrix;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        public GameBoard()
        {
            InitializeComponent();

            Matrix matrix = new (new int[,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            );

            DrawTetromino(matrix.Rotate90().Value, PreviewGrid);
            DrawTetromino(matrix.Value, TetrisGrid);
        }


        private void DrawTetromino(int[,] values, Grid grid)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {

                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == 0) continue;

                    Label rectangle = new()
                    {
                        Width = 25,
                        Height = 25,
                        BorderBrush = Brushes.White,
                        BorderThickness = new Thickness(1),
                        Background = Brushes.Red
                    };

                    grid.Children.Add(rectangle);
                    Grid.SetRow(rectangle, i);
                    Grid.SetColumn(rectangle, j);
                }
            }
        }
    }
}




