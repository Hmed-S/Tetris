namespace Engine
{
    public class Board
    {
        public int Height { get;}
        public int Width { get; }
        public List<Tetromino> Tetronminos { get; } = new();
        public int[,] Values { get; }

        public Board(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");
            Height = rowCount;
            Width = columnCount;
            Values = new int[rowCount, columnCount];
            Replace(0,rowCount, 0,columnCount, (i,j)=>0);
        }

        public void AddTetromino(Tetromino tetromino) => Tetronminos.Add(tetromino);
        
        public void Replace(int rowFrom, int rowTo,
            int columnFrom, int columnTo,  Func<int, int, int> action)
        {
            var m = 0;
            for (int i = rowFrom; i < rowTo; i++)
            for (int j = columnFrom; j < columnTo; j++)
            {
                Values[i, j] = action(m, j);
            }
        }

        public bool CanGoDown(int potentialYPosition, Tetromino tetromino)
        {
            int[,] matrix = tetromino.Shape.Value;
            bool canGoDown = true;
            
            EmptySpot(tetromino);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == 0 )continue;
                    if (Values[i + potentialYPosition, j + tetromino.XPosition] !=0 && matrix[i, j] != 0)
                    {
                        canGoDown = false;
                        break;
                    }
                }
            }
            PutTetromino(tetromino.XPosition, tetromino.YPosition, tetromino);
            return canGoDown;
        }
        
        public bool CanGoLeftOrRight(int potentialXPosition, Tetromino tetromino)
        {
            int[,] matrix = tetromino.Shape.Value;
            bool canGoDown = true;
            
            EmptySpot(tetromino);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == 0 )continue;
                    if (Values[i + tetromino.YPosition, j + potentialXPosition] !=0 && matrix[i, j] != 0)
                    {
                        canGoDown = false;
                        break;
                    }
                }
            }
            PutTetromino(tetromino.XPosition, tetromino.YPosition, tetromino);
            return canGoDown;
        }

        private void EmptySpot(Tetromino tetromino)
        {
            var matrix = tetromino.Shape.Value;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == 0 )continue;
                    Values[i+tetromino.YPosition, j+tetromino.XPosition] = 0;
                }
            }
        }
        
        private void PutTetromino(int x, int y, Tetromino tetromino)
        {
            var matrix = tetromino.Shape.Value;
            EmptySpot(tetromino);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i,j] == 0) continue;
                    if(Values[i+y, j+x] ==1) continue;
                    if(matrix[i, j] == 1 && Values[i+y, j+x] ==0) Values[i+y, j+x] = matrix[i, j];
                }
            }
        }

        public int ShiftCoordinates(int x, int y, Tetromino tetromino)
        {            
            int maxYValue = Height - 2 + tetromino.NumberOfEmptyRows();
            int maxXValue = Width - 2 + tetromino.numberOfEmptyColumns();

            if (y >= maxYValue || !CanGoDown(y, tetromino))
            {
                tetromino.DropStatus = -1;
                return -1;
            }
            if (x >= maxXValue || x<0 || !CanGoLeftOrRight(x,tetromino)) return -2;
            
            PutTetromino(x,y, tetromino);

            tetromino.YPosition = y;
            tetromino.XPosition = x;
            return 0;
        }
    }
}
