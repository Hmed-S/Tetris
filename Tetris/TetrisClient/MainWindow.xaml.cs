using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            _tetrisEngine = new(TetrisGrid.ColumnDefinitions.Count, TetrisGrid.RowDefinitions.Count);
            Init();
        }
        
        private void Init()
        {
            DrawTetromino(_tetrisEngine.Preview.Shape.Value, PreviewGrid);
            PreviewKeyDown += Window_PreviewKeyDown;
            _timer = new DispatcherTimer();
            _timer.Tick += DropTetromino;
            _timer.Interval = TimeSpan.FromSeconds(0.4);
            _timer.Start();
            Pause.Click += PauseTimer;
        }
        
        private void PauseTimer(object sender, EventArgs args) =>_timer.IsEnabled = !_timer.IsEnabled;
        public void RedrawPreview()
        {
            PreviewGrid.Children.Clear();
            DrawTetromino(_tetrisEngine.Preview.Shape.Value, PreviewGrid);
        }
        private void DropTetromino(object sender, EventArgs args)
        {
            _tetrisEngine.DropTetromino();
            if (_tetrisEngine.CurrentTetromino().DropStatus == -1) RedrawPreview();
            TetrisGrid.Children.Clear();
            DrawTetromino(_tetrisEngine.Board, TetrisGrid);
        }
        
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)  _tetrisEngine.ShiftToRight();
            else if (e.Key == Key.Left) _tetrisEngine.ShiftToLeft();
        }
        
        private void DrawTetromino(int[,] values, Grid grid)
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
                    grid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, i); // Zet de rij
                    Grid.SetColumn(rectangle, j); // Zet de kolom
                }
            }

        }
    }
}
