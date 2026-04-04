using System;
using System.Windows;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SwitchScreens(object o, EventArgs e)
        {
            //var b = o as Button;
            //GameMode.SetGameMode(b.Name);

            //var gameWindow = new GamePage();
            //var gameWindowPlayer2 = new GamePage();
            //try
            //{
            //    Close();
            //    gameWindow.Show();
            //    if (b.Name == "MultiPlayer") gameWindowPlayer2.Show();
            //}
            //catch
            //{
            //    gameWindow.Close();
            //    gameWindowPlayer2.Close();
            //    MessageBox.Show("Could not connect to the server please try again later");
            //}
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize_button_Click(object sender, RoutedEventArgs e)
        {            
            WindowState = WindowState.Minimized;
        }
    }
}
