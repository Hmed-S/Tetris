using System;
using System.Windows.Controls;
using System.Windows.Threading;
using TetrisClient.Controls;
using TetrisEngine.Domain.Game;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage(GameBoard gameboard, GameBoard opponentGameBoard)
        {
            InitializeComponent();

            MainGrid.Children.Add(
                gameboard
                );

            MainGrid.Children.Add(opponentGameBoard);

            Grid.SetColumn(gameboard, 0);
            Grid.SetColumn(opponentGameBoard, 1);

        }

        public GamePage(GameBoard gameboard)
        {
            InitializeComponent();
            gameboard.GameOver += (object sender, EventArgs e) => NavigationService.GoBack();
            MainGrid.Children.Add(gameboard);
        }
    }
}
