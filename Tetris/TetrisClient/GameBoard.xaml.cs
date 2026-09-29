using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TetrisClient.Controls;
using TetrisEngine.Sdk;

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
        private Engine _engine;
        private Player _player;
        public event EventHandler GameOver;

        public GameBoard(Engine engine, TetrisGrid tetrisGrid, TetrisGrid preview, DispatcherTimer timer)
        {
            InitializeComponent();

            _engine = engine;
            _player = engine.Game.Player;
            _tetrisGrid = tetrisGrid;
            _preview = preview;
            _timer = timer;

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

            //_engine.Next();
            _preview.Put(_player.Preview);
            _tetrisGrid.Put(_player.CurrentTetromino);

            PauseButton.Click += PauseTimer;
            QuitButton.Click += StopGame;

            _timer.IsEnabled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += KeyDownControls;
            window.KeyUp += KeyUpControls;
        }

        private void HandleEvents()
        {

            _player.OnTetrominoPositionChange +=  (previous, next) => 
            { 
                if(!previous.Islanded)
                _tetrisGrid.Erase(previous); 
                
                _tetrisGrid.Put(next); 
            };
            _player.OnPreviewChange += (preview) => { _preview.Clear(); _preview.Put(preview); };
            _player.Board.OnLineClear +=  (row) => _tetrisGrid.ClearLine(row);
            _player.Board.OnRowSwap += (row1, row2) => _tetrisGrid.SwitchRows(row1, row2);
            _player.OnScoreChange += (score) => Score.Content = $"Score: {score}";
            _player.OnLineCountChange += (lines) => Lines.Content = $"Lines: {lines}";
            _player.OnLevelChange +=  
                (level) => {
                Level.Content = $"Level: {level}";
                _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.MiliSeconds);
                };

            _engine.Game.OnGameOver +=
                () =>
                {
                    Window.GetWindow(this).KeyDown -= KeyDownControls;
                    Window.GetWindow(this).KeyUp -= KeyUpControls;
                    PauseButton.IsEnabled = false;
                    _timer.Stop();
                    GameOverLabel.Visibility = Visibility.Visible;
                    QuitButton.Content = "Main Menu";

                };
        }

        private void GameLoop(object sender, EventArgs args)
        {
            _engine.Drop();
        }

        private void PauseTimer(object sender, RoutedEventArgs e)
        {
            _timer.IsEnabled = !_timer.IsEnabled;
            PauseButton.Content = _timer.IsEnabled? "Pause" : "Resume" ;
        }

        private void StopGame(object sender, RoutedEventArgs e)
        {
            if (_engine.Game.State == GameState.Playing)
            {
                _timer.Stop();
                _engine.Quit();

            }

            GameOver.Invoke(this, EventArgs.Empty);
        }

        private void HardDrop(object sender, RoutedEventArgs e)
        {

            _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.HardDropMiliSeconds);
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
                    _engine.RotateRight(); break;
                case Key.RightShift:
                case Key.LeftCtrl:
                case Key.Z:
                case Key.Down:
                    _engine.RotateLeft(); break;
                case Key.Escape:
                case Key.F1: PauseTimer(sender, e); break;
                case Key.Left:
                    _engine.MoveLeft(); break;
                case Key.Right: _engine.MoveRight(); break;
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




