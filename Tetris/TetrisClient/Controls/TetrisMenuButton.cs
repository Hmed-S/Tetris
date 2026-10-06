using System.Windows;
using System.Windows.Controls;

namespace TetrisClient.Controls;

public partial class TetrisMenuButton : Button
{
    static TetrisMenuButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(TetrisMenuButton),
            new FrameworkPropertyMetadata(typeof(TetrisMenuButton)));
    }

}
