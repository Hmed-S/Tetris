using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TetrisEngine.Matrix matrix = new(new int[,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            );

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }


        private void Init()
        {
            int offsetY = 0;
            int offsetx = 0;

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler((object sender, EventArgs args) =>
            {
                if (offsetY == TetrisGrid.RowDefinitions.Count -2) dispatcherTimer.Stop();
                
                TetrisGrid.Children.RemoveRange(0, offsetY * 6);
                
                DrawTetromino(offsetY, offsetx);

                offsetY += 1;
            });

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }

        private void DrawTetromino(int offsetY, int offsetX)
        {
            int[,] values = matrix.Value;

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
                    Grid.SetRow(rectangle, i + offsetY); 
                    Grid.SetColumn(rectangle, j + offsetX); 
                }
            }

        }
    }
}
