using System;
using System.Windows;
using System.Windows.Controls;

namespace TetrisClient;

/// <summary>
/// Interaction logic for GamePage.xaml
/// </summary>
public partial class GamePage : Page
{
    private GameBoard _playerBoard;
    private GameBoard _opponentBoard;
    public GamePage(GameBoard playerBoard, GameBoard opponentBoard)
    {
        _playerBoard = playerBoard;
        _opponentBoard = opponentBoard;

        InitializeComponent();

        MainGrid.Children.Add(_playerBoard);
        MainGrid.Children.Add(_opponentBoard);

        Grid.SetColumn(playerBoard, 0);
       
    }

    public GamePage(GameBoard playerBoard)
    {
        _playerBoard = playerBoard;

        InitializeComponent();

        _playerBoard.GameOver += (object sender, EventArgs e) => NavigationService.GoBack();
        MainGrid.Children.Add(_playerBoard);

        MatchmakingLabel.Visibility = Visibility.Collapsed;
    }

    public void ShowOpponentBoard()
    {
        // Controleer of we op de verkeerde thread zitten, zo ja: stuur door naar de UI-thread
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => ShowOpponentBoard());
            return;
        }

        // --- Vanaf hier is de code gegarandeerd op de UI-thread ---
        MatchmakingLabel.Visibility = Visibility.Collapsed;
        Grid.SetColumn(_opponentBoard, 1);
    }

}
