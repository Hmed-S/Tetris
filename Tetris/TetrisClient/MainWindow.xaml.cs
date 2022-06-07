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
            GetWindow(this).PreviewKeyDown += Window_PreviewKeyDown;
            
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler((object sender, EventArgs args) =>
            {
                var result = _tetrisEngine.DropTetromino(); 
                if (result.DropStatus==-1) dispatcherTimer.Stop();

                TetrisGrid.Children.RemoveRange(0, result.Rows.GetLength(0) * 6);
                
                DrawTetromino(result.Rows);
            });

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }
        
        public void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)  _tetrisEngine.ShiftToRight();
            else if (e.Key == Key.Left) _tetrisEngine.ShiftToLeft();
        }

        private void DrawTetromino(int[,] values)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] != 1) continue;

                    Rectangle rectangle = new Rectangle()
                    {
                        Width = 25, 
                        Height = 25, 
                        Stroke = Brushes.White, 
                        StrokeThickness = 1, 
                        Fill = Brushes.Red,
                    };

                    TetrisGrid.Children.Add(rectangle); 
                    Grid.SetRow(rectangle, i); 
                    Grid.SetColumn(rectangle, j); 
                }
            }

        }
    }
}
