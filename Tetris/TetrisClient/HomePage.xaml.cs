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
        gameboard.PutPreview(_engine.Game.Player.Preview);
        NavigationService.Navigate(new GamePage(gameboard));
    }

    private void Start_MultiPlayer_Game(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException("This has yet to be implemented");
    }

    public void  ShowPlayerHandleInput(object sender, RoutedEventArgs e)
    {
        MultiplayerButton.Visibility = Visibility.Collapsed;
        PlayerNameLabel.Visibility = Visibility.Visible;
        PlayerNameInput.Visibility = Visibility.Visible;
        StartMultiPlayerGameButton.Visibility = Visibility.Visible;
    }

}
