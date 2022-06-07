
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
    

        private int PutTetromino(int y, int x)
        {
            int length = Board.Height;
            int width = Board.Width;
            
            int maxYValue = length-3;
            int maxXValue = width - 3;
            
            int redrawYWindow = (y>maxYValue - 3)? length-1:y+2;
            int redrawXWindow = (x>maxXValue - 3)? width-1:x+2;
            if (y==length) return -1;
            if (x == width) return -2;
            
            if (y > maxYValue && y <length || x>maxXValue)
            {
                
                Board.Replace(0, redrawYWindow, 0, redrawXWindow, (i, j) => 0);
                Board.DrawTetromino(y - 1, (x>maxXValue &&x<width)?y+2:length-1, (x>maxXValue+1)?x-2:x-1, width, _currentTetromino.Value);
                _lastXposition = x;
                _lastYPosition = y;
                return 0;
            }
            
            Board.Replace(0, redrawYWindow, 0, redrawXWindow, (i, j) => 0);
            Board.DrawTetromino(y - 1, y + 2, x-1, x+2, _currentTetromino.Value);

             _lastXposition = x;
            _lastYPosition = y;
            return 0;
        }

        public Result ShiftToLeft()
        {
            int put = PutTetromino(_lastYPosition, _lastXposition);
            if(_lastXposition!= 1) put = PutTetromino(_lastYPosition, _lastXposition-1);
            
            int lastYPosition = (_lastYPosition+2 >Board.Height) ? Board.Height : _lastYPosition + 2;
            int lastXPosition = (_lastXposition+2 >Board.Width) ? Board.Width : _lastXposition + 2;
            return new()
            {
                Rows = Board[lastYPosition, lastXPosition],
                DropStatus = put
            };
        }

        public Result ShiftToRight()
        {
            int put = PutTetromino(_lastYPosition, _lastXposition);
            if(_lastXposition!= Board.Width-3) put = PutTetromino(_lastYPosition, _lastXposition+1);

            int lastYPosition = (_lastYPosition+2 >Board.Height) ? Board.Height : _lastYPosition + 2;
            int lastXPosition = (_lastXposition+2 >Board.Width) ? Board.Width : _lastXposition + 2;
            return new()
            {
                Rows = Board[lastYPosition, lastXPosition],
                DropStatus = put
            };
        }

        public Result DropTetromino()
        {
            int put = PutTetromino(_lastYPosition+1, _lastXposition);
            int lastYPosition = (_lastYPosition+2 >Board.Height) ? Board.Height : _lastYPosition + 2;
            int lastXPosition = (_lastXposition+2 >Board.Width) ? Board.Width : _lastXposition + 2;
            return new()
            {
                Rows = Board[lastYPosition, lastXPosition],
                DropStatus = put
            };
        }

    }
}
