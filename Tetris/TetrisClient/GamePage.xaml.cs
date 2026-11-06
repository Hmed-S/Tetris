using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace TetrisClient;

/// <summary>
/// Interaction logic for GamePage.xaml
/// </summary>
public partial class GamePage : Page
{
    private GameBoard _playerBoard;

    public GameBoard OpponentBoard { get; set; }

    public GamePage()
    {
        InitializeComponent();
        
        MatchmakingLabel.Visibility = Visibility.Collapsed;
    }

    public void SetPlayerBoard(GameBoard playerBoard)
    {
        _playerBoard = playerBoard;
        _playerBoard.GameOver += (sender, e) => NavigateToHome();
        MainGrid.Children.Add(_playerBoard);
    }

    private void NavigateToHome()
    {
        HomePage homePage = ((App)Application.Current)
            .Services
            .GetRequiredService<HomePage>();

        NavigationService.Navigate(homePage);
    }

    public void ShowOpponentBoard()
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(ShowOpponentBoard);
            return;
        }

        if (OpponentBoard == null || MainGrid.Children.Contains(OpponentBoard))
        {
            return;
        }

        MainGrid.Children.Add(OpponentBoard);

        Grid.SetColumn(OpponentBoard, 1);

        MatchmakingLabel.Visibility = Visibility.Collapsed;
    }

    public void HideOpponentBoard()
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(HideOpponentBoard);
            return;
        }

        if (OpponentBoard != null)
        {
            OpponentBoard.Visibility = Visibility.Collapsed;
        }
    }
}
