using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Engine;
using TetrisClient.SignalR;
using TetrisClient.Dto;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TetrisEngine _tetrisEngine;
        private DispatcherTimer _timer;
        private string _gameMode = GameMode.GetGameMode();
        private TetrisHubConnectionService _connectionService = new();

        public MainWindow()
        {
            InitializeComponent();
            _tetrisEngine = new(new Board(TetrisGrid.RowDefinitions.Count, TetrisGrid.ColumnDefinitions.Count));
            Init();
        }
        
        private void InitMultiPlayer()
        {
            TetrisGridPlayer2.Visibility = Visibility.Visible;
            Player2Status.Visibility = Visibility.Visible;
            Pause.Visibility = Visibility.Hidden;


            _connectionService.MakeConnection();
            _connectionService.ConnectOnDrop((game) =>
            {
                Lines_ValuePlayer2.Content = game.Lines;
                ScorePlayer2.Content = game.Score;
                DrawTetromino(game.Preview, PreviewGridPlayer2);
                DrawTetromino(game.Board, TetrisGridPlayer2);
            });
            
            // ToDo: implement ready
            //_connectionService.ConnectOnReady();

        }

        private void Init()
        {
            if (_gameMode == "MultiPlayer") InitMultiPlayer();

            DrawTetromino(_tetrisEngine.Preview.Shape.Value, PreviewGrid);
            PreviewKeyDown += KeyDownControls;
            _timer = new DispatcherTimer();
            _timer.Tick += DropTetromino;
            _timer.Interval = TimeSpan.FromSeconds(0.4);
            Quit.Click += QuitGame;
            if(_gameMode != "MultiPlayer")_timer.Start();
            Pause.Click += PauseTimer;
        }

        private void QuitGame(object sender, EventArgs args)
        {
            var homepage = new HomePage();
            Close();
            homepage.Show();
        }
        
        private void PauseTimer(object sender, EventArgs args) =>_timer.IsEnabled = !_timer.IsEnabled;
        
        public void RedrawPreview()
        {
            PreviewGrid.Children.Clear();
            DrawTetromino(_tetrisEngine.Preview.Shape.Value, PreviewGrid);
        }
        private async void DropTetromino(object sender, EventArgs args)
        {
            _tetrisEngine.DropTetromino();
            Lines_Value.Content = _tetrisEngine.Lines;
            Score_Value.Content = _tetrisEngine.Score;
            RedrawPreview();
            TetrisGrid.Children.Clear();
            DrawTetromino(_tetrisEngine.Board, TetrisGrid);
            if (_gameMode == "MultiPlayer")
            {
                await _connectionService.DropShape(new Game
                {
                    Lines = _tetrisEngine.Lines,
                    Score = _tetrisEngine.Score,
                    Board = _tetrisEngine.Board,
                    Preview = _tetrisEngine.Preview.Shape.Value
                });
            }
        }
        
        private void KeyDownControls(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.X: _tetrisEngine.RotateRight();break;
                case Key.Space: Trace.WriteLine("harddrop"); break;
                case Key.RightShift:
                case Key.C: Trace.WriteLine("hold"); break;
                case Key.LeftCtrl: 
                case Key.Z: _tetrisEngine.RotateLeft(); break;
                case Key.Escape:
                case Key.F1: PauseTimer(sender,e); break;
                case Key.Left: _tetrisEngine.ShiftToLeft(); break;
                case Key.Right: _tetrisEngine.ShiftToRight(); break;
                case Key.Down: Trace.WriteLine("softdrop"); break;
            }
        }
        
        private void DrawTetromino(int[,] values, Grid grid)
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
                    grid.Children.Add(rectangle); 
                    Grid.SetRow(rectangle, i); 
                    Grid.SetColumn(rectangle, j); 
                }
            }

        }
    }
}
