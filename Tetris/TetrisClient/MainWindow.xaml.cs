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
        private string _readyUpState = "notReady";
        private int _ocupiedCells;

        public MainWindow()
        {
            Trace.WriteLine("GameMode = " + _gameMode);
            InitializeComponent();
            _tetrisEngine = new(new Board(TetrisGrid.RowDefinitions.Count, TetrisGrid.ColumnDefinitions.Count));
            Init();
        }

        private async void InitMultiPlayer()
        {
            TetrisGridPlayer2.Visibility = Visibility.Visible;
            Player2Status.Visibility = Visibility.Visible;
            Pause.Visibility = Visibility.Hidden;
            ReadyUpMessage.Visibility = Visibility.Hidden;

            LoadMessage.Visibility = Visibility.Visible;
            await _connectionService.MakeConnection();

            _connectionService.ConnectQuit(async () =>
            {
                _timer.Stop();
                _gameMode = "SinglePlayer";
                await _connectionService.CloseConnection();

                MessageBox.Show("Player 2 has quit");

                TetrisGridPlayer2.Visibility = Visibility.Hidden;
                Player2Status.Visibility = Visibility.Hidden;
                Pause.Visibility = Visibility.Visible;

            });

            await _connectionService.ConnectOnDrop((game) =>
            {
                TetrisGridPlayer2.Children.Clear();
                PreviewGridPlayer2.Children.Clear();

                Lines_ValuePlayer2.Content = game.Lines;
                ScorePlayer2.Content = game.Score;
                DrawTetromino(1,1 game.Preview, PreviewGridPlayer2);
                DrawTetromino(1,1,game.Board, TetrisGridPlayer2);
            });

            _connectionService.ConnectOnReady(() =>
            {
                ReadyUpMessage.Content = "Ready!";
                if (_readyUpState == ReadyUpMessage.Content) _timer.Start();
            });

            await _connectionService.StartConnection();
            LoadMessage.Visibility = Visibility.Hidden;
            ReadyUpMessage.Visibility = Visibility.Visible;

            ReadyUp.Visibility = Visibility.Visible;
            ReadyUp.Click += Ready;

            Closing += WindowClose;
        }

        private void Init()
        {

            DrawTetromino(1,1,_tetrisEngine.Preview.Shape.Value, PreviewGrid);
            
            PreviewKeyDown += KeyDownControls;
            _timer = new DispatcherTimer();
            _timer.Tick += DropTetromino;
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            Quit.Click += QuitGame;
            Pause.Click += PauseTimer;
            
            if (_gameMode == "MultiPlayer") InitMultiPlayer();
            if (_gameMode == "SinglePlayer")_timer.Start();
        }

        private async void WindowClose(object o, EventArgs e)
        {
            if(_gameMode == "MultiPlayer")
            {
                await _connectionService.Quit();
                await _connectionService.CloseConnection();
                _gameMode = "SinglePlayer";
            }
        }

        private async void Ready(object o, EventArgs e)
        {
            _readyUpState = "Ready!";
            ReadyUp.Visibility = Visibility.Hidden;
            await _connectionService.Ready();
            if (_readyUpState == ReadyUpMessage.Content) _timer.Start();
        }



        private async void QuitGame(object o, EventArgs e)
        {
            if (_gameMode == "MultiPlayer")
            {
                await _connectionService.Quit();
                await _connectionService.CloseConnection();
                _gameMode = "SinglePlayer";
            }

            var homepage = new HomePage();
            Close();
            homepage.Show();
        }
        
        private void PauseTimer(object sender, EventArgs args) =>_timer.IsEnabled = !_timer.IsEnabled;
        
        public void RedrawPreview()
        {
            PreviewGrid.Children.Clear();
            DrawTetromino(1,1,_tetrisEngine.Preview.Shape.Value, PreviewGrid);
        }

        private void SmartRemove(int amount)
        {
            for (int i = TetrisGrid.Children.Count - 1; i >= amount; i--)
            {
                TetrisGrid.Children.RemoveAt(i);
            }
        }

        private async void DropTetromino(object sender, EventArgs args)
        {
            _ocupiedCells = 0;
            _tetrisEngine.DropTetromino();
            Lines_Value.Content = _tetrisEngine.Lines;
            Score_Value.Content = _tetrisEngine.Score;
            RedrawPreview();
            // TetrisGrid.Children.Clear();
            SmartRemove(_ocupiedCells);
            DrawTetromino(
                _tetrisEngine.CurrentTetromino().YPosition,
                _tetrisEngine.CurrentTetromino().XPosition,
                _tetrisEngine.CurrentTetromino().Shape.Value, TetrisGrid); 
            
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
                case Key.X:_tetrisEngine.RotateRight();break;
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
        
        private void DrawTetromino(int offsetY, int offsetX, int[,] values, Grid grid)
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
                    _ocupiedCells++;
                    grid.Children.Add(rectangle); 
                    Grid.SetRow(rectangle, i+offsetY); 
                    Grid.SetColumn(rectangle, j+offsetX); 
                }
            }

        }
    }
}
