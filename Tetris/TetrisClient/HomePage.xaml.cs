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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Start_Single_player_Game(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new GamePage(GameMode.SinglePlayer)
                );
        }

        private void Start_MultiPlayer_Game(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new GamePage(GameMode.MultiPlayer)
                );
        }
    }
}
