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
        private Matrix _currentTetromino = new(new int[,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            );

        private int PutTetromino(int y)
        {
            int maxYvalue = Board.Values.Length - 3;
            
            if (y - 1 == maxYvalue) return -1;

            for (int i = y + 2; i > 0; i--)
                for (int j = 0; j < _board.Values.GetLength(1); j++)
                    _board.Values[i, j] = 0;

            for (int i = y - 1; i < y + 2; i++)
            {
                for (int j = 0; j < _currentTetromino.Value.GetLength(1); j++)
                {
                    _board.Values[i, j] = _currentTetromino.Value[i, j];
                }
            }

            return 0;
        }

    }
}
