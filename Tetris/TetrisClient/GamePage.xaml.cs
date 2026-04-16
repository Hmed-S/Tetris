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
        public GamePage(GameMode GameMode)
        {
            InitializeComponent();

            if (GameMode == GameMode.SinglePlayer)
            {
                MainGrid.Children.Add(
                    new GameBoard(new TetrisGrid(18, 7), new TetrisGrid(4,4),  new DispatcherTimer())
                    );
            }
            if(GameMode == GameMode.MultiPlayer)
            {
                GameBoard board1 = new GameBoard(new TetrisGrid(18, 7), new TetrisGrid(3,3),  new DispatcherTimer());
                Grid.SetColumn(board1, 0);

                GameBoard board2 = new GameBoard(new TetrisGrid(18,7), new TetrisGrid(3,3),  new DispatcherTimer());
                Grid.SetColumn(board2, 1);

                MainGrid.Children.Add(board1);
                MainGrid.Children.Add(board2);
            }
        }
    }
}
