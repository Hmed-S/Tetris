using System;
using System.Collections.Generic;
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
    protected Player _player;
    private TetrisGrid _tetrisGrid;
    private TetrisGrid _preview;
    protected DispatcherTimer _timer;
    public Dictionary<int, Action> PlayerActions { get; private set; }
    public event Action OnReady;
    public event Action OnQuit;
    public event EventHandler GameOver;
    public event Action GameLoop;
    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action OnRotateLeft;
    public event Action OnRotateRight;
    public event Action OnFastDrop;

    public GameBoard(Player player, TetrisGrid tetrisGrid, TetrisGrid preview, DispatcherTimer timer)
    {
        InitializeComponent();
        _player = player;

        _tetrisGrid = tetrisGrid;
        _preview = preview;
        _timer = timer;

        TetrisGrid.Children.Add(_tetrisGrid);
        Preview.Children.Add(_preview);

        Grid.SetRow(_preview, 1);

        PauseButton.Click += PauseTimer;
        QuitButton.Click += StopGame;
        _timer.Tick += (sender, e) => GameLoop.Invoke();
        
        SetPlayerEvents();
    }

    private void SetPlayerEvents()
    {
        _player.OnTetrominoPositionChange += DrawTetromino;
        _player.OnPreviewChange += RedrawPreview;
        _player.Board.OnLineClear += ClearLine;
        _player.Board.OnRowSwap += SwapRow;
        _player.OnScoreChange += UpdateScore;
        _player.OnLineCountChange += UpdateLineCount;
        _player.OnLevelChange += UpdateLevel;
        _player.OnLevelChange += (level) => UpdateInterval(_player.Interval.MiliSeconds);
        _player.OnReady += Ready;
        _player.OnFastDropped += () =>
        {
            if(_player.FastDrop)
            {
                UpdateInterval(_player.Interval.FastDropMiliSeconds);
            }
            else
            {
                UpdateInterval(_player.Interval.MiliSeconds);
            }
        };
        UpdateInterval(_player.Interval.MiliSeconds);
    }

    public void StartGameLoop()
    {
        if (IsLoaded) SetKeyEvents();
        else Loaded += (sender, e) => SetKeyEvents();

        _tetrisGrid.Put(_player.CurrentTetromino);
        _preview.Put(_player.Preview);
        
        _timer.IsEnabled = true;
    }

    public void DrawTetromino(Tetromino previous, Tetromino next)
    {
        if (!previous.Islanded)
            _tetrisGrid.Erase(previous);

        _tetrisGrid.Put(next);
    }

    public void Ready()
    {
        Dispatcher.Invoke(()=>
        {
            if (ReadyButton.Visibility == Visibility.Collapsed)
            {
                ReadyLabel.ShowReadyIcon();
            }
            else
            {
                ReadyButton.Visibility = Visibility.Collapsed;
                ReadyLabel.Visibility = Visibility.Visible;
                ReadyLabel.ShowReadyIcon();
            }
        });
    }

    public void ReadyClick(object sender, RoutedEventArgs e)
    {
        OnReady.Invoke();
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

    public void UpdateInterval(double interval)
    {
        _timer.Interval = TimeSpan.FromMilliseconds(interval);
    }

    public void HandleGameover()
    {
        Dispatcher.Invoke(() =>
            {
                _timer.Stop();
                Window.GetWindow(this).KeyDown -= KeyDownControls;
                Window.GetWindow(this).KeyUp -= KeyUpControls;
                PauseButton.IsEnabled = false;
                GameOverLabel.Visibility = Visibility.Visible;
                QuitButton.Content = "Main Menu";
            });

    }

    private void PauseTimer(object sender, RoutedEventArgs e)
    {
        _timer.IsEnabled = !_timer.IsEnabled;
        PauseButton.Content = _timer.IsEnabled? "Pause" : "Resume" ;

        if (_timer.IsEnabled)
        {
            SetKeyEvents();
        }
        if (!_timer.IsEnabled) 
        {
            DisableControlls();
        }
    }

    private void StopGame(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        if ((string)QuitButton.Content == "Quit") OnQuit.Invoke();
        GameOver.Invoke(this, EventArgs.Empty);
    }

    private void FastDrop(object sender, RoutedEventArgs e)
    {
        OnFastDrop?.Invoke();
        _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.FastDropMiliSeconds);
    }

    private void QuitFastDrop(object sender, RoutedEventArgs e)
    {
        OnFastDrop?.Invoke();
        _timer.Interval = TimeSpan.FromMilliseconds(_player.Interval.MiliSeconds);
    }

    private void SetKeyEvents()
    {
        var window = Window.GetWindow(this);
        window.KeyDown += KeyDownControls;
        window.KeyUp += KeyUpControls;
        window.KeyDown += PauseControls;
    }

    private void DisableControlls()
    {
        var window = Window.GetWindow(this);
        window.KeyDown -= KeyDownControls;
        window.KeyUp -= KeyUpControls;
    }


    private void KeyDownControls(object sender, KeyEventArgs e)
    {
        if (e.IsRepeat) return;
        
        switch (e.Key)
        {
            case Key.Up:
            case Key.X:
                OnRotateRight.Invoke(); e.Handled = true; break;
            case Key.RightShift:
            case Key.LeftCtrl:
            case Key.Z:
            case Key.Down:
                OnRotateLeft.Invoke(); e.Handled = true; break;
            case Key.Left:
                OnMoveLeft.Invoke(); e.Handled = true; break;
            case Key.Right: OnMoveRight.Invoke(); e.Handled = true; break;
            case Key.Space:
                FastDrop(sender, e); e.Handled = true;
                break;
        }

    }

    private void PauseControls(object sender, KeyEventArgs e)
    {
        if (e.IsRepeat) return;

        switch(e.Key)
        {
            case Key.Escape:
            case Key.F1: PauseTimer(sender, e); e.Handled = true; break;
        }
    }

    private void KeyUpControls(object sender, KeyEventArgs e)
    {
        if (e.IsRepeat) return;

        switch(e.Key)
        {
            case Key.Space:
                QuitFastDrop(sender, e); 
                break;

        }

    }

    internal void HideAllButtons()
    {
        QuitButton.Visibility = Visibility.Collapsed;
        ReadyButton.Visibility = Visibility.Collapsed;
        PauseButton.Visibility = Visibility.Collapsed;
    }

}
