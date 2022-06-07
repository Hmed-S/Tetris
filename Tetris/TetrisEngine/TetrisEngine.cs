using Engine.MatrixExtensions;

namespace Engine
{
    public class TetrisEngine
    {
        private Board _board;
        public Board Board
        {
            get => _board;
            init { _board = value; PutTetromino(1); }
        }
        private Matrix _currentTetromino = new(new [,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            );
        private int _lastPosition;


        private int PutTetromino(int y)
        {
            int length = Board.Values.GetLength(0);
            int maxYValue = length-3;
            int redrawWindow = (y>Board.Values.Length - 3)? length-1:y+2;
            
            
            if (y > maxYValue && y <length)
            {
                _board.Replace(0, length-1, 0, _board.Values.GetLength(1), (i, j) => 0);
                _board.Replace(y - 1, length-1, 0, _currentTetromino.Value.GetLength(1), _currentTetromino.Value);
            
                _lastPosition = y;
                return 0;
            }
            
            if (y==length) return -1;
            
            _board.Replace(0, redrawWindow, 0, _board.Values.GetLength(1), (i, j) => 0);
            _board.Replace(y - 1, y + 2, 0, _currentTetromino.Value.GetLength(1), _currentTetromino.Value);
            
            _lastPosition = y;
            return 0;
        }

        public Result DropTetromino()
        {
            int put = PutTetromino(_lastPosition+1);
            int lastPosition = (_lastPosition+2 >Board.Values.GetLength(0)) ? Board.Values.GetLength(0) : _lastPosition + 2;
            return new()
            {
                Rows = _board.Values.SubMatrix(lastPosition, _board.Values.GetLength(1)),
                DropStatus = put
            };
        }

    }
}
