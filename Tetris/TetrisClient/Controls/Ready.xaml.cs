using System.Windows.Controls;
using System.Windows;

namespace TetrisClient.Controls;

/// <summary>
/// Interaction logic for Ready.xaml
/// </summary>
public partial class Ready : UserControl
{

    public Ready()
    {
        InitializeComponent();


    }

    public void ShowReadyIcon()
    {
        
        NotReadyIcon.Visibility = Visibility.Collapsed;
        ReadyIcon.Visibility = Visibility.Visible;
    }
}
