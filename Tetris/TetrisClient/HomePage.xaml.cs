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
using System.Windows.Threading;
using TetrisClient.Controls;
using TetrisEngine.Domain.Game;
using TetrisEngine;

namespace TetrisClient
{
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
            IGame game = _engine.StartSinglePlayerGame(20,10);
            GameBoard gameboard = new GameBoard(game.Player, new TetrisGrid(20, 10), new TetrisGrid(4, 4), new DispatcherTimer(), new DispatcherTimer());
            
            gameboard.ReadyButton.Visibility = Visibility.Collapsed;
            NavigationService.Navigate(new GamePage(gameboard));
        }

        private void Start_MultiPlayer_Game(object sender, RoutedEventArgs e)
        {
            //GameBoard gameeboard = new GameBoard(PlayerNameInput.Text, new TetrisGrid(18, 7), new TetrisGrid(4, 4), new DispatcherTimer());
            //GameBoard opponentBoard = new GameBoard("Some opponent", new TetrisGrid(18, 7), new TetrisGrid(4, 4), new DispatcherTimer());

            //gameeboard.PauseButton.Visibility = Visibility.Collapsed;

            //opponentBoard.PauseButton.Visibility = Visibility.Collapsed;
            //opponentBoard.ReadyButton.Visibility = Visibility.Collapsed;
            //opponentBoard.QuitButton.Visibility = Visibility.Collapsed;

            //NavigationService.Navigate(new GamePage(gameeboard,opponentBoard));
        }

        public void  ShowPlayerHandleInput(object sender, RoutedEventArgs e)
        {
            MultiplayerButton.Visibility = Visibility.Collapsed;
            PlayerNameLabel.Visibility = Visibility.Visible;
            PlayerNameInput.Visibility = Visibility.Visible;
            StartMultiPlayerGameButton.Visibility = Visibility.Visible;
        }
    }
}
