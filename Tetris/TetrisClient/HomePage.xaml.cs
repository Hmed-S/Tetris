using System;
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

    private void Start_Single_player_Game(object sender, RoutedEventArgs e)
    {
        Game game = _engine.StartSinglePlayerGame(20,10);
        GameBoard gameboard = new (_engine, new TetrisGrid(20, 10), new TetrisGrid(4, 4), new DispatcherTimer());
        
        gameboard.ReadyButton.Visibility = Visibility.Collapsed;
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
