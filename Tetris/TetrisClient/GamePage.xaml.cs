using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TetrisEngine.Domain.Game;

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
                    new GameBoard()
                    );
            }
            if(GameMode == GameMode.MultiPlayer)
            {
                GameBoard board1 = new GameBoard();
                Grid.SetColumn(board1, 0);

                GameBoard board2 = new GameBoard();
                Grid.SetColumn(board2, 1);

                MainGrid.Children.Add(board1);
                MainGrid.Children.Add(board2);
            }
        }
    }
}
