using System;
using System.Windows;
using System.Windows.Controls;

namespace TetrisClient;

/// <summary>
/// Interaction logic for GamePage.xaml
/// </summary>
public partial class GamePage : Page
{
    private readonly GameBoard _playerBoard;

    public GameBoard OpponentBoard { get; set; }

    public GamePage(GameBoard playerBoard)
    {
        InitializeComponent();

        _playerBoard = playerBoard;

        _playerBoard.GameOver += (sender, e) =>
            NavigationService.GoBack();

        MainGrid.Children.Add(_playerBoard);

        MatchmakingLabel.Visibility = Visibility.Collapsed;
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
