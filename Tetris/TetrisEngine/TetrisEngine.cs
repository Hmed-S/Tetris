
namespace Engine
{
    public class TetrisEngine
    {
        public Board Board { get; init; }

        private Tetronmino _currentTetromino = Tetronmino.Random();
        public Tetronmino Preview { get; private set; } = Tetronmino.Random();
        private int _lastYPosition;
        private int _lastXposition =1;
        private int _lastDropStatus;
    

        private void PutTetromino(int x, int y)
        {
            int draw = Board.DrawTetromino(x, y, _currentTetromino.Shape.Value);
            if (draw == 0)
            {
                _lastXposition = x;
                _lastYPosition = y;
            }
            _lastDropStatus = draw;
        }

        public void Next()
        {
            _currentTetromino = Preview;
            Preview = Tetronmino.Random();
            _lastYPosition = 0;
            _lastXposition = 1;
            _lastDropStatus = 0;
            
        }

        public Tetronmino Status()
        {
            int lastYPosition = (_lastYPosition+2 >Board.Height) ? Board.Height : _lastYPosition;
            _currentTetromino.LastXPosition = _lastXposition;
            _currentTetromino.LastYPosition = lastYPosition;
            _currentTetromino. DropStatus = _lastDropStatus;
            return _currentTetromino;
        }
        
        public void ShiftToLeft()
        {
            if(_lastYPosition != Board.Height-3) PutTetromino(_lastXposition-1, _lastYPosition);
        }

        public void ShiftToRight()
        {
            if(_lastYPosition != Board.Height-3) PutTetromino(_lastXposition+1, _lastYPosition);
        }
        
        public void DropTetromino() => PutTetromino(_lastXposition, _lastYPosition+1);

    }
}
