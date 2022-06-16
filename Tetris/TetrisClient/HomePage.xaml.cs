using System;
using System.Windows;
using System.Windows.Controls;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
            SinglePlayer.Click += SwitchScreens;
            MultiPlayer.Click += SwitchScreens;
        }

        private void SwitchScreens(object o, EventArgs e)
        {
            var b = o as Button;
            GameMode.SetGameMode(b.Name);

            var gameWindow = new MainWindow();
            var gameWindowPlayer2 = new MainWindow();
            try
            {
                Close();
                gameWindow.Show();
                if (b.Name == "MultiPlayer") gameWindowPlayer2.Show();
            }
            catch
            {
                gameWindow.Close();
                gameWindowPlayer2.Close();
                MessageBox.Show("Could not connect to the server please try again later");
            }
        }
    }
}
