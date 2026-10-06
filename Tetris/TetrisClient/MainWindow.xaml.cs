using System.Windows;

namespace TetrisClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow: Window
{
    public MainWindow()
    {
        InitializeComponent();
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
