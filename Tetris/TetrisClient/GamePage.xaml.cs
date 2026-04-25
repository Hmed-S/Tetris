using System.Windows.Controls;
using TetrisEngine.Domain.Game;
using TetrisClient.Controls;
using System.Windows.Threading;

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

            MainGrid.Children.Add(gameboard);
        }
    }
}
