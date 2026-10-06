using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TetrisClient.Controls;
using TetrisEngine.Sdk;

namespace TetrisClient;

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

    public void DrawTetromino(Tetromino previous, Tetromino next)
    {
        if (!previous.Islanded)
            _tetrisGrid.Erase(previous);

        _tetrisGrid.Put(next);
    }

    public void RedrawPreview(Tetromino preview)
    {
        _preview.Clear();
        _preview.Put(preview);
    }

    public void ClearLine(int row) => _tetrisGrid.ClearLine(row);

    public void SwapRow(int row1, int row2) => _tetrisGrid.SwitchRows(row1, row2);

    public void UpdateScore(int score) => Score.Content = $"Score: {score}";

    public void UpdateLineCount(int lineCount) => Lines.Content = $"Lines: {lineCount}";

    public void UpdateLevel(int level)
    {
        Level.Content = $"Level: {level}";
        
    }

    public void UpdateInterval(int interval)
    {
        _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.MiliSeconds);
    }

    public void HandleGameover()
    {
        Window.GetWindow(this).KeyDown -= KeyDownControls;
        Window.GetWindow(this).KeyUp -= KeyUpControls;
        PauseButton.IsEnabled = false;
        _timer.Stop();
        GameOverLabel.Visibility = Visibility.Visible;
        QuitButton.Content = "Main Menu";
    }

    private void HandleEvents()
    {

        _player.OnTetrominoPositionChange += DrawTetromino;
        _player.OnPreviewChange += RedrawPreview;
        _player.Board.OnLineClear += ClearLine;
        _player.Board.OnRowSwap += SwapRow;
        _player.OnScoreChange += UpdateScore;
        _player.OnLineCountChange += UpdateLineCount;
        _player.OnLevelChange += UpdateLevel;
        _player.OnLevelChange -= UpdateInterval;

        _engine.Game.OnGameOver += HandleGameover;
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

        _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.FastDropMiliSeconds);
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
