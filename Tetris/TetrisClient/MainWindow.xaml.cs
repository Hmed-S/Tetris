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
        private Matrix matrix = new(new int[,]
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
                DrawTetromino(offsetY, offsetx);
                TetrisGrid.Children.RemoveRange(0, offsetY * 6);

                if (offsetY == TetrisGrid.Children.Count - 1) dispatcherTimer.Stop();
                offsetY += 1;

            });
            dispatcherTimer.Interval = TimeSpan.FromSeconds(5);
            dispatcherTimer.Start();

        }

        private void DrawTetromino(int offsetY, int offsetX)
        {
            int[,] values = matrix.Value;
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
