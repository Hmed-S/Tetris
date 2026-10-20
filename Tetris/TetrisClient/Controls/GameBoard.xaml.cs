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
    private TetrisGrid _tetrisGrid;
    private TetrisGrid _preview;
    private DispatcherTimer _timer;
    public double FastDropMilliSeconds { get; set; }
    public double DropIntervalMilliseconds { get; private set; }
    public Dictionary<int, Action> PlayerActions { get; private set; }
    public event Action ReadyAction;
    public event Action Quit;
    public event EventHandler GameOver;
    public event Action GameLoop;

    public GameBoard(TetrisGrid tetrisGrid, TetrisGrid preview, DispatcherTimer timer)
    {
        InitializeComponent();

        _tetrisGrid = tetrisGrid;
        _preview = preview;
        _timer = timer;

        TetrisGrid.Children.Add(_tetrisGrid);
        Preview.Children.Add(_preview);

        Grid.SetRow(_preview, 1);

        PauseButton.Click += PauseTimer;
        QuitButton.Click += StopGame;
        _timer.Tick += (sender, e) => GameLoop.Invoke();
    }

    public void StartGameLoop() => _timer.IsEnabled = true;

    public void DrawTetromino(Tetromino previous, Tetromino next)
    {
        if (!previous.Islanded)
            _tetrisGrid.Erase(previous);

        _tetrisGrid.Put(next);
    }

    public void Ready()
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => Ready());
            return;
        }

        if(ReadyButton.Visibility == Visibility.Collapsed)
        {
            ReadyLabel.ShowReadyIcon();
        }
        else
        {
            ReadyButton.Visibility = Visibility.Collapsed;
            ReadyLabel.Visibility = Visibility.Visible;
            ReadyLabel.ShowReadyIcon();
        }
        
    }

    public void ReadyClick(object sender, RoutedEventArgs e)
    {
        ReadyAction.Invoke();
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
        DropIntervalMilliseconds = interval;
        _timer.Interval = TimeSpan.FromMilliseconds(DropIntervalMilliseconds);
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

    private void PauseTimer(object sender, RoutedEventArgs e)
    {
        _timer.IsEnabled = !_timer.IsEnabled;
        PauseButton.Content = _timer.IsEnabled? "Pause" : "Resume" ;
    }

    private void StopGame(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        if ((string)QuitButton.Content == "Quit") Quit.Invoke();
        GameOver.Invoke(this, EventArgs.Empty);
    }

    private void FastDrop(object sender, RoutedEventArgs e)
    {

        _timer.Interval = TimeSpan.FromMilliseconds(FastDropMilliSeconds);
    }

    private void QuitHardDrop(object sender, RoutedEventArgs e)
    {
        _timer.Interval = TimeSpan.FromMilliseconds(DropIntervalMilliseconds);
    }

    public void EnableKeyControls(Dictionary<int, Action> playerActions)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => EnableKeyControls(playerActions));
            return;
        }
        PlayerActions = playerActions;

        if (IsLoaded) SetKeyEvents();
        else Loaded += (sender, e) => SetKeyEvents();
    }

    private void SetKeyEvents()
    {
        var window = Window.GetWindow(this);
        window.KeyDown += KeyDownControls;
        window.KeyUp += KeyUpControls;
    }

    private void KeyDownControls(object sender, KeyEventArgs e)
    {
        if (e.IsRepeat) return;
        
        switch (e.Key)
        {
            case Key.Up:
            case Key.X:
                PlayerActions[0](); e.Handled = true; break;
            case Key.RightShift:
            case Key.LeftCtrl:
            case Key.Z:
            case Key.Down:
                PlayerActions[1](); e.Handled = true; break;
            case Key.Escape:
            case Key.F1: PauseTimer(sender, e); e.Handled = true; break;
            case Key.Left:
                PlayerActions[2](); e.Handled = true; break;
            case Key.Right: PlayerActions[3](); e.Handled = true; break;
            case Key.Space:
                FastDrop(sender, e); e.Handled = true;
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

    public void StartOnLoaded()
    {
        Loaded += (sender, e) => _timer.IsEnabled = true;
    }

}
