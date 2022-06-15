using System;
using System.Windows.Controls;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage
    {
        private MainWindow _mainwindow = new();
        public HomePage()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            SinglePlayer.Click += SwitchScreens;
            Multiplayer.Click += SwitchScreens;
            
        }

        private void SwitchScreens(object o, EventArgs e)
        {
            var b = o as Button;
            if (b.Name == "Multiplayer") _mainwindow.GameMode = "Multiplayer";
            Hide();
            _mainwindow.Show();
        }
    }
}
