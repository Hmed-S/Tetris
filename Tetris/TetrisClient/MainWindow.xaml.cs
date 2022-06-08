using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Engine;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TetrisEngine _tetrisEngine;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            _tetrisEngine = new TetrisEngine { Board = new Board(TetrisGrid.RowDefinitions.Count, TetrisGrid.ColumnDefinitions.Count)};
        }
        
        private void Init()
        {
            PreviewKeyDown += Window_PreviewKeyDown;
            
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler((object sender, EventArgs args) =>
            {
                _tetrisEngine.DropTetromino();
                
                var result = _tetrisEngine.Status(); 
                if (result.DropStatus==-1 || result.DropStatus ==-2) dispatcherTimer.Stop();
                
                TetrisGrid.Children.RemoveRange((result.LastXPosition>0)? 0:result.LastXPosition-2, result.LastYPosition+2);
                DrawTetromino(result.LastYPosition-1, result.LastXPosition-1, result.Tetromino);
            });

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }
        
        public void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)  _tetrisEngine.ShiftToRight();
            else if (e.Key == Key.Left) _tetrisEngine.ShiftToLeft();
        }

        private void DrawTetromino(int offsetY, int offsetX, int[,] values)
        {
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
                    TetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, i + offsetY); // Zet de rij
                    Grid.SetColumn(rectangle, j + offsetX); // Zet de kolom
                }
            }

        }
    }
}
