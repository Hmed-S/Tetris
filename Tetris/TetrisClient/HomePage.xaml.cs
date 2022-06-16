using System;
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
            Close();
            gameWindow.Show();
            if (b.Name == "MultiPlayer")
            {
                var gameWindowPlayer2 = new MainWindow();
                gameWindowPlayer2.Show();
            }
        }
    }
}
