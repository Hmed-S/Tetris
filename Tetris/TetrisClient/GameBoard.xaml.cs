using System.Windows.Controls;
using System.Windows.Threading;
using TetrisClient.Controls;
using TetrisEngine.Domain.Board;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        private TetrisGrid _tetrisGrid;
        private TetrisGrid _preview;
        private DispatcherTimer _timer;

        public GameBoard(TetrisGrid tetrisGrid, TetrisGrid preview, DispatcherTimer timer)
        {
            InitializeComponent();

            _tetrisGrid = tetrisGrid;
            _preview = preview;
            _timer = timer;

            TetrisGrid.Children.Add(_tetrisGrid);
            Preview.Children.Add(_preview);

            Grid.SetRow(_preview, 1);
            

            _preview.Put(Tetromino.FromShape(ShapeType.TSHAPE));
            _tetrisGrid.Put(Tetromino.FromShape(ShapeType.LSHAPE));

        }
    }
}




