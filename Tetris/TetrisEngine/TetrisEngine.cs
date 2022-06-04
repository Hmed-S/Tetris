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

        public int PutTetromino(int y)
        {
            int maxYvalue = Board.Values.Length - 3;
            
            if (y - 1 == maxYvalue) return -1;

            _board.Replace(0, y + 2, 0, _board.Values.GetLength(1), (i, j) => 0);

            _board.Replace(y-1, y+2,
                0, _currentTetromino.Value.GetLength(1),
                (i, j) => _currentTetromino.Value[i,j]);

            return 0;
        }

    }
}
