using Engine.Extensions;

namespace Engine
{
    public class TetrisEngine
    {
        public Board Board { get; init; }
 
        private Matrix _currentTetromino = new(new [,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            );
        private int _lastYPosition;
    

        private int PutTetromino(int y)
        {
            int length = Board.Height;
            int maxYValue = length-3;
            int redrawWindow = (y>maxYValue - 3)? length-1:y+2;
            
            if (y==length) return -1;
            if (y > maxYValue && y <length)
            {
                Board.Replace(0, redrawWindow, 0, Board.Width, (i, j) => 0);
                Board.DrawTetromino(y - 1, length-1, 0, _currentTetromino.Value.GetLength(1), _currentTetromino.Value);
                _lastYPosition = y;
                return 0;
            }
            
            Board.Replace(0, redrawWindow, 0, Board.Width, (i, j) => 0);
            Board.DrawTetromino(y - 1, y + 2, 0, _currentTetromino.Value.Columns(), _currentTetromino.Value);
            
            _lastYPosition = y;
            return 0;
        }

        public Result DropTetromino()
        {
            int put = PutTetromino(_lastYPosition+1);
            int lastPosition = (_lastYPosition+2 >Board.Height) ? Board.Height : _lastYPosition + 2;
            return new()
            {
                Rows = Board[lastPosition,Board.Width],
                DropStatus = put
            };
        }

    }
}
