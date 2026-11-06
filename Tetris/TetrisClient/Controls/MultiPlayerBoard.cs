using System.Windows.Threading;
using TetrisEngine.Sdk;
using System.Windows;

namespace TetrisClient.Controls;

public class MultiPlayerBoard : GameBoard
{
    public MultiPlayerBoard(Player player, TetrisGrid tetrisGrid, TetrisGrid preview, DispatcherTimer timer) : base(player, tetrisGrid, preview, timer)
    {
        PlayerNameLabel.Content = _player.Name;

        PauseButton.Visibility = Visibility.Collapsed;
        PlayerNameLabel.Visibility = Visibility.Visible;
        ReadyButton.Visibility = Visibility.Collapsed;
        tetrisGrid.Clear();
        preview.Clear();
    }

}
