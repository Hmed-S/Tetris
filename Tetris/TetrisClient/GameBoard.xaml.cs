using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TetrisClient.Controls;
using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game.Player;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        private TetrisGrid _tetrisGrid;
        private TetrisGrid _preview;
        private DispatcherTimer _timer;
        private DispatcherTimer _holdTimer;
        private IPlayer _player;
        public event EventHandler GameOver;

        public GameBoard(IPlayer player, TetrisGrid tetrisGrid, TetrisGrid preview, DispatcherTimer timer, DispatcherTimer holdTimer)
        {
            InitializeComponent();

            _player = player;
            _tetrisGrid = tetrisGrid;
            _preview = preview;
            _timer = timer;
            _holdTimer = holdTimer;

            TetrisGrid.Children.Add(_tetrisGrid);
            Preview.Children.Add(_preview);

            Grid.SetRow(_preview, 1);


            if (_player.Name != null)
            {
                PlayerNameLabel.Content = _player.Name;

            }
            else
            {
                PlayerNameLabel.Visibility = System.Windows.Visibility.Collapsed;
            }

            HandleEvents();

            _timer.Tick += GameLoop;
            _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.MiliSeconds);
            _holdTimer.Interval = TimeSpan.FromMilliseconds(50);
            _holdTimer.Tick += (object sender, EventArgs args) => _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.HardDrop.MiliSeconds);
            _timer.IsEnabled = true;
            _player.Next();
            _preview.Put(_player.Preview);
            _tetrisGrid.Put(_player.CurrentTetromino);

            PauseButton.Click += PauseTimer;
            QuitButton.Click += StopGame;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += KeyDownControls;
            window.KeyUp += KeyUpControls;
        }

        private void HandleEvents()
        {

            _player.OnDrop = [ (previous, next) => { _tetrisGrid.Erase(previous); _tetrisGrid.Put(next); } ];
            _player.OnPreviewChange = [ (preview) => { _preview.Clear(); _preview.Put(preview); _tetrisGrid.Put(_player.CurrentTetromino); } ];
            _player.OnLineClear = [ (row) => _tetrisGrid.ClearLine(row) ];
            _player.OnLineSwab = [ (row1, row2) => _tetrisGrid.SwitchRows(row1, row2) ];
            _player.OnScoreChange = [ (score) => Score.Content = $"Score: {score.Value}" ];
            _player.OnLineCountChange = [ (lines) => Lines.Content = $"Lines: {lines.Value}" ];
            _player.OnLevelChange = [ 
                (level) => {
                Level.Content = $"Level: {level.Value}";
                _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.MiliSeconds);
                } 
            ];

            _player.Game.OnGameOver.Add(
                () => {

                    _timer.Stop();
                    GameOverLabel.Visibility = Visibility.Visible;
                    QuitButton.Content = "Main Menu";

                 }
                );
        }

        private void GameLoop(object sender, EventArgs args)
        {
            _player.DropTetromino();
        }

        private void PauseTimer(object sender, RoutedEventArgs e)
        {
            _timer.IsEnabled = !_timer.IsEnabled;
            PauseButton.Content = _timer.IsEnabled? "Pause" : "Resume" ;
        }

        private void StopGame(object sender, RoutedEventArgs e)
        {
            if (_player.Game.GameState != TetrisEngine.Domain.Game.GameState.GameOver)
            {
                _timer.Stop();
                _player.Game.Quit();

            }

            GameOver.Invoke(this, EventArgs.Empty);
        }

        private void HardDrop(object sender, RoutedEventArgs e)
        {

            _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.HardDrop.MiliSeconds);
        }

        private void QuitHardDrop(object sender, RoutedEventArgs e)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.MiliSeconds);
        }

        private void KeyDownControls(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;
            switch (e.Key)
            {
                case Key.Up:
                case Key.X:
                    _player.RotateRight(); break;
                case Key.RightShift:
                case Key.LeftCtrl:
                case Key.Z:
                case Key.Down:
                    _player.RotateLeft(); break;
                case Key.Escape:
                case Key.F1: PauseTimer(sender, e); break;
                case Key.Left:
                    _player.MoveLeft(); break;
                case Key.Right: _player.MoveRight(); break;
                case Key.Space:
                    HardDrop(sender, e);
                    break;
            }
        }

        private void KeyUpControls(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            switch(e.Key)
            {
                case Key.Space:
                    QuitHardDrop(sender, e); 
                    break;

            }

        }


    }

 }




