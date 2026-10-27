using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using TetrisClient.Controls;
using TetrisEngine.Sdk;

namespace TetrisClient;

/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HomePage : Page
{
    private Engine _engine = new();

    public HomePage()
    {
        InitializeComponent();
    }

    public void ConfigureBoardEvents(GameBoard gameBoard, Player player)
    {
        player.OnTetrominoPositionChange += gameBoard.DrawTetromino;
        player.OnPreviewChange += gameBoard.RedrawPreview;
        player.Board.OnLineClear += gameBoard.ClearLine;
        player.Board.OnRowSwap += gameBoard.SwapRow;
        player.OnScoreChange += gameBoard.UpdateScore;
        player.OnLineCountChange += gameBoard.UpdateLineCount;
        player.OnLevelChange += gameBoard.UpdateLevel;
        player.OnLevelChange += (level) => gameBoard.UpdateInterval(player.Interval.MiliSeconds);
        player.OnReady += gameBoard.Ready;

        gameBoard.UpdateInterval(player.Interval.MiliSeconds);
    }


    private void Start_Single_player_Game(object sender, RoutedEventArgs e)
    {
        GameBoard gameboard = new (new TetrisGrid(20, 10), new TetrisGrid(4, 4), new DispatcherTimer());

        _engine.StartSinglePlayerGame(20, 10);
        _engine.Game.OnGameOver += gameboard.HandleGameover;

        ConfigureBoardEvents(gameboard, _engine.Game.Player);
        
        gameboard.ReadyButton.Visibility = Visibility.Collapsed;
        gameboard.FastDropMilliSeconds = _engine.Game.Player.Interval.FastDropMiliSeconds;
        gameboard.GameLoop += _engine.Drop;
        gameboard.StartOnLoaded();
        gameboard.Quit += _engine.Quit;
        

        gameboard.EnableKeyControls(new Dictionary<int, Action>
        {
            {  0, _engine.RotateRight },
            {  1, _engine.RotateLeft  },
            {  2, _engine.MoveLeft    },
            {  3, _engine.MoveRight   },
        });

        
        gameboard.DrawTetromino(_engine.Game.Player.CurrentTetromino,_engine.Game.Player.CurrentTetromino);
        gameboard.RedrawPreview(_engine.Game.Player.Preview);
        NavigationService.Navigate(new GamePage(gameboard));
    }

    private async void Start_MultiPlayer_Game(object sender, RoutedEventArgs e)
    {
        await _engine.StartMultiPlayerGame(20, 10, PlayerNameInput.Text);

        GameBoard playerBoard = new(new TetrisGrid(20, 10), new TetrisGrid(4, 4), new DispatcherTimer());
        GameBoard opponentBoard = new(new TetrisGrid(20, 10), new TetrisGrid(4, 4), new DispatcherTimer());

        _engine.Game.OnGameOver += playerBoard.HandleGameover;
        _engine.Game.OnGameOver += opponentBoard.HandleGameover;

        playerBoard.PauseButton.Visibility = Visibility.Collapsed;
        opponentBoard.PauseButton.Visibility = Visibility.Collapsed;
        opponentBoard.QuitButton.Visibility = Visibility.Collapsed;

        opponentBoard.ReadyButton.Visibility = Visibility.Collapsed;

        playerBoard.PlayerNameLabel.Visibility = Visibility.Visible;
        playerBoard.PlayerNameLabel.Content = PlayerNameInput.Text;

        ConfigureBoardEvents(playerBoard, _engine.Game.Player);

        playerBoard.PauseButton.Visibility = Visibility.Collapsed;
        opponentBoard.PauseButton.Visibility = Visibility.Collapsed;
        playerBoard.GameLoop += _engine.Drop;
        opponentBoard.GameLoop += _engine.OpponentDrop;
        playerBoard.Quit += _engine.Quit;

        GamePage gamePage = new (playerBoard, opponentBoard);

        _engine.Game.OnMatchFound += gamePage.ShowOpponentBoard;
        playerBoard.ReadyAction += _engine.Ready;


        _engine.Game.OnBothReady += () =>
        {
            playerBoard.EnableKeyControls(new Dictionary<int, Action>
            {
                {  0, _engine.RotateRight },
                {  1, _engine.RotateLeft  },
                {  2, _engine.MoveLeft    },
                {  3, _engine.MoveRight   },
            });


            playerBoard.DrawTetromino(_engine.Game.Player.CurrentTetromino, _engine.Game.Player.CurrentTetromino);
            playerBoard.RedrawPreview(_engine.Game.Player.Preview);
            opponentBoard.DrawTetromino(_engine.Game.Opponent.CurrentTetromino, _engine.Game.Opponent.CurrentTetromino);
            opponentBoard.RedrawPreview(_engine.Game.Opponent.Preview);
            playerBoard.StartGameLoop();
            opponentBoard.StartGameLoop();

        };

        _engine.Game.OnMatchFound += () =>
        {
            ConfigureBoardEvents(opponentBoard, _engine.Game.Opponent);

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() =>
                {
                    opponentBoard.PlayerNameLabel.Visibility = Visibility.Visible;
                    opponentBoard.PlayerNameLabel.Content = _engine.Game.Opponent.Name;
                    opponentBoard.ReadyLabel.Visibility = Visibility.Visible;
                    _engine.Game.Opponent.OnQuit += gamePage.HideOpponentBoard;

                    playerBoard.Dispatcher.Invoke(() =>
                    {
                        playerBoard.GameOverLabel.Content =
                            $"{_engine.Game.Opponent.Name} Quit";
                    });
                });
            }

        };

        NavigationService.Navigate(gamePage);
    }

    public void  ShowPlayerHandleInput(object sender, RoutedEventArgs e)
    {
        MultiplayerButton.Visibility = Visibility.Collapsed;
        PlayerNameLabel.Visibility = Visibility.Visible;
        PlayerNameInput.Visibility = Visibility.Visible;
        StartMultiPlayerGameButton.Visibility = Visibility.Visible;
    }

}
