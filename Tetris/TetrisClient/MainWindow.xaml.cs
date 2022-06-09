using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Engine;
using Matrix = Engine.Matrix;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TetrisEngine _tetrisEngine;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _tetrisEngine = new TetrisEngine { Board = new Board(TetrisGrid.RowDefinitions.Count, TetrisGrid.ColumnDefinitions.Count)};
            Init();

        }
        
        private void Init()
        {
            DrawTetromino(0, 0, _tetrisEngine.Preview.Shape, PreviewGrid);
            PreviewKeyDown += Window_PreviewKeyDown;
            _timer = new DispatcherTimer();
            _timer.Tick += DropTetromino;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
        }

        private string toString(int[,] a)
        {
            var result = string.Empty;
            var maxI = a.GetLength(0);
            var maxJ = a.GetLength(1);
            for (var i = 0; i < maxI; i++)
            {
                result += ",{";
                for (var j = 0; j < maxJ; j++)
                {
                    result += $"{a[i, j]},";
                }

                result += "}";
            }

            return result;
        }

        private void DropTetromino(object sender, EventArgs args)
        {
            _tetrisEngine.DropTetromino();
            var result = _tetrisEngine.Status();
            TetrisGrid.Children.Clear();

            Trace.WriteLine($"{result.LastYPosition+2}");
            TetrisGrid.Children.RemoveRange(0, result.LastYPosition+2);
            DrawTetromino(result.LastYPosition-1, result.LastXPosition-1, result.Shape, TetrisGrid);
            _timer.IsEnabled = !(result.LastYPosition == TetrisGrid.RowDefinitions.Count -2 || result.DropStatus == -1);
        }
        
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)  _tetrisEngine.ShiftToRight();
            else if (e.Key == Key.Left) _tetrisEngine.ShiftToLeft();
        }


        private void PrintChildren()
        {
            Trace.WriteLine(TetrisGrid.Children.Count);
            foreach (var v in TetrisGrid.Children)
            {
                Trace.WriteLine(v);
            }
        }
        private void SmartRemove(int from , int to)
        {
            for(int i = from; i <to; i++ )
            {
                TetrisGrid.Children.RemoveAt(i);
            }
        }
        private void DrawTetromino(int offsetY, int offsetX, Matrix matrix, Grid grid)
        {
            
            var values = matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    // Als de waarde niet gelijk is aan 1,
                    // dan hoeft die niet getekent te worden:
                    if (values[i, j] != 1) continue;
                    
                    Rectangle rectangle = new Rectangle()
                    {
                        Width = 25, // Breedte van een 'cell' in de Grid
                        Height = 25, // Hoogte van een 'cell' in de Grid
                        Stroke = Brushes.White, // De rand
                        StrokeThickness = 1, // Dikte van de rand
                        Fill = Brushes.Red, // Achtergrondkleur
                    };
                    grid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, i + offsetY); // Zet de rij
                    Grid.SetColumn(rectangle, j + offsetX); // Zet de kolom
                }
            }

        }
    }
}
