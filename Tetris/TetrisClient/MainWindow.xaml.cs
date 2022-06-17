using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        public MainWindow()
        {
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
                MessageBox.Show("Player 2 has quit");
                await _connectionService.CloseConnection();

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
                DrawTetromino(game.Preview, PreviewGridPlayer2);
                DrawTetromino(game.Board, TetrisGridPlayer2);
            });

            _connectionService.ConnectOnReady(() =>
            {
                ReadyUpMessage.Content = "Ready!";
                if (_readyUpState == ReadyUpMessage.Content) _timer.Start();
            });

            try
            {
                await _connectionService.StartConnection();
            }
            catch
            {
                _gameMode = "SinglePlayer";
                MessageBox.Show("Unable to Connect to the server");
                QuitGame(null, null);
            }
            
            LoadMessage.Visibility = Visibility.Hidden;
            ReadyUpMessage.Visibility = Visibility.Visible;

            ReadyUp.Visibility = Visibility.Visible;
            ReadyUp.Click += Ready;

            Closing += WindowClose;
        }

        private void Init()
        {
            DrawTetromino(_tetrisEngine.Preview.Shape.Value, PreviewGrid);
            
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



        private async void QuitGame(object? o, EventArgs? e)
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

        private void PauseTimer(object sender, EventArgs args) 
        {
            
            _timer.IsEnabled = !_timer.IsEnabled;
            Pause.Content = _timer.IsEnabled ? "Pause" : "Resume"; 
        }
        
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
            DrawTetromino(
                _tetrisEngine.Board, TetrisGrid); 
            
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
                case Key.RightShift:
                case Key.LeftCtrl: 
                case Key.Z: _tetrisEngine.RotateLeft(); break;
                case Key.Escape:
                case Key.F1: PauseTimer(sender,e); break;
                case Key.Left: _tetrisEngine.ShiftToLeft(); break;
                case Key.Right: _tetrisEngine.ShiftToRight(); break;
            }
        }

        private SolidColorBrush ColorConverter(int value)
        {
            return value switch
            {
                1 => Brushes.Orange,
                2 => Brushes.Purple,
                3 => Brushes.LightBlue,
                4 => Brushes.Green,
                5 => Brushes.MediumPurple,
                6 => Brushes.Red,
                7 => Brushes.Yellow,
            };
        }

        private void DrawTetromino(int[,] values, Grid grid)
        {
            
            for (int i = 0; i < values.GetLength(0); i++)
            {
                
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == 0) continue;
                    
                    Label rectangle = new ()
                    {
                        Width = 25, 
                        Height = 25, 
                        BorderBrush = Brushes.White, 
                        BorderThickness = new Thickness(1), 
                        Background = ColorConverter(values[i,j]), 
                    };

                    grid.Children.Add(rectangle); 
                    Grid.SetRow(rectangle, i); 
                    Grid.SetColumn(rectangle, j); 
                }
            }

        }
    }
}
