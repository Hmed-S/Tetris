
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
        private int _lastXposition =1;
        private int _lastDropStatus;
    

        private void PutTetromino(int x, int y)
        {
            int draw = Board.DrawTetromino(x, y, _currentTetromino.Value);
            if (draw == 0)
            {
                _lastXposition = x;
                _lastYPosition = y;
            }
            _lastDropStatus = draw;
        }

        public Result Status()
        {
            int lastYPosition = (_lastYPosition+2 >Board.Height) ? Board.Height : _lastYPosition;
            
            return new()
            {
                LastXPosition = _lastXposition,
                LastYPosition = lastYPosition,
                DropStatus = _lastDropStatus,
                Tetromino = _currentTetromino.Value
            };
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
