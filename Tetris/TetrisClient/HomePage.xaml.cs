using System;
using System.Net.Http;
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
    private readonly Engine _engine;
    private readonly GamePage _gamePage;

    public HomePage(Engine engine, GamePage gamePage)
    {       
        InitializeComponent();
        _engine = engine;
        _gamePage = gamePage;
    }

    private void SetBoardEvents(GameBoard gameBoard)
    {
        _engine.Game.OnGameOver += gameBoard.HandleGameover;
        gameBoard.GameLoop += _engine.Drop;
        gameBoard.OnQuit += _engine.Quit;
        gameBoard.OnRotateRight += _engine.RotateRight;
        gameBoard.OnRotateLeft += _engine.RotateLeft;
        gameBoard.OnMoveLeft += _engine.MoveLeft;
        gameBoard.OnMoveRight += _engine.MoveRight;
        gameBoard.OnReady += _engine.Ready;
    }

    private void Start_Single_player_Game(object sender, RoutedEventArgs e)
    {
        _engine.StartSinglePlayerGame(20, 10);
        GameBoard gameboard = new(_engine.Game.Player, new TetrisGrid(20, 10), new TetrisGrid(4, 4), new DispatcherTimer());

        SetBoardEvents(gameboard);  
        gameboard.StartGameLoop();

        _gamePage.SetPlayerBoard(gameboard);
        NavigationService.Navigate(_gamePage);
    }

    private async void Start_MultiPlayer_Game(object sender, RoutedEventArgs e)
    {
        SinglePlayerButton.IsEnabled = false;

        try
        {
            StartMultiPlayerGameButton.IsEnabled = false;
            ErrorLabel.Visibility = Visibility.Collapsed;
            await _engine.StartMultiPlayerGame(20, 10, PlayerNameInput.Text);

        }
        catch (HttpRequestException)
        {
            ErrorLabel.Content ="Failed to connect to the server, try again later.";

            ErrorLabel.Visibility = Visibility.Visible;
            _engine.Quit();
            SinglePlayerButton.IsEnabled = true;
            StartMultiPlayerGameButton.IsEnabled = true;
            return;
        }
        catch (ArgumentException ae)
        {
            ErrorLabel.Content = ae.Message;
            ErrorLabel.Visibility = Visibility.Visible;
            SinglePlayerButton.IsEnabled = true;
            StartMultiPlayerGameButton.IsEnabled = true;
            return;
        }

        GameBoard playerBoard = new MultiPlayerBoard(
            _engine.Game.Player,
            new TetrisGrid(20, 10),
            new TetrisGrid(4, 4),
            new DispatcherTimer());

        SetBoardEvents(playerBoard);
        playerBoard.OnFastDrop += _engine.NotifyFastDrop;
        _gamePage.SetPlayerBoard(playerBoard);
        _gamePage.MatchmakingLabel.Visibility = Visibility.Visible;

        _engine.Game.OnMatchFound += () =>
        {
            _gamePage.Dispatcher.Invoke(() =>
            {
                SetupOpponentBoard(_gamePage, playerBoard);
            });
        };

        _engine.Game.OnBothReady += () =>
        {
            _gamePage.Dispatcher.Invoke(() =>
            {
                playerBoard.StartGameLoop();
                _gamePage.OpponentBoard?.StartGameLoop();
            });
        };

        NavigationService.Navigate(_gamePage);
    }

    private void SetupOpponentBoard(GamePage gamePage, GameBoard playerBoard)
    {
        GameBoard opponentBoard = new MultiPlayerBoard(
            _engine.Game.Opponent,
            new TetrisGrid(20, 10),
            new TetrisGrid(4, 4),
            new DispatcherTimer());

        gamePage.OpponentBoard = opponentBoard;

        opponentBoard.GameLoop += _engine.OpponentDrop;
        _engine.Game.OnGameOver += opponentBoard.HandleGameover;

        opponentBoard.HideAllButtons();

        gamePage.ShowOpponentBoard();

        opponentBoard.ReadyLabel.Visibility = Visibility.Visible;
        playerBoard.ReadyButton.Visibility = Visibility.Visible;

        _engine.Game.Opponent.OnQuit += () =>
        {
            playerBoard.Dispatcher.Invoke(() =>
            {
                playerBoard.GameOverLabel.Text =
                    $"{_engine.Game.Opponent.Name} Quit";

                playerBoard.ReadyButton.Visibility = Visibility.Collapsed;
            });

            gamePage.Dispatcher.Invoke(
                gamePage.HideOpponentBoard);
        };

        _engine.Game.Opponent.OnDisconnected += () =>
        {
            playerBoard.Dispatcher.Invoke(() =>
            {
                playerBoard.GameOverLabel.Text =
                    $"Connection lost to {_engine.Game.Opponent.Name}";

                playerBoard.ReadyButton.Visibility = Visibility.Collapsed;
            });

            gamePage.Dispatcher.Invoke(
                gamePage.HideOpponentBoard);
        };

        _engine.Game.OnGameOver += () =>
        {
            playerBoard.Dispatcher.Invoke(() =>
            {
                if (_engine.Game.Player.Won)
                {
                    playerBoard.GameOverLabel.Text = "You won!";
                }
                else if (_engine.Game.Opponent.Won)
                {
                    playerBoard.GameOverLabel.Text = "You lose!";
                }
            });
        };
    }

    public void ShowPlayerHandleInput(object sender, RoutedEventArgs e)
    {
        MultiplayerButton.Visibility = Visibility.Collapsed;
        PlayerNameLabel.Visibility = Visibility.Visible;
        PlayerNameInput.Visibility = Visibility.Visible;
        StartMultiPlayerGameButton.Visibility = Visibility.Visible;
    }

}
