using Engine.Extensions;

namespace Engine
{
    public class Board
    {
        public int Height { get;}
        public int Width { get; }
        public int Lines { get; private set; }
        public virtual int[,] Values { get; set; }

        
        public Board(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");
            Height = rowCount;
            Width = columnCount;
            Values = new int[rowCount, columnCount];

            Replace(0,rowCount, 0,columnCount, (i,j)=>0);
        }
        
        private void ResetRow(int rowNumber)
        {
            Replace(rowNumber, rowNumber+1, 0, Width, (i, j) =>0);
            foreach (var row in Enumerable.Range(0, rowNumber).Reverse()) 
            {
                foreach (var column in Enumerable.Range(0, Width))
                {
                    Values[row+1, column] = Values[row, column];
                }
            }
        }
        
        public virtual int CountLines()
        {
            int numberOfLinesDetected = 0;
            foreach (var row in Enumerable.Range(0,Values.RowCount()))
            {
                bool full = Array.TrueForAll(Values.GetRow(row), i => i != 0);
                if (full)
                {
                    ResetRow(row);
                    numberOfLinesDetected += 1;
                }
            }
            Lines += numberOfLinesDetected;
            return numberOfLinesDetected;
        }
        
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

        private bool Contains(int x, int y)
        {
            return Values.RowCount() >y && y>=0 && Values.ColumnCount() >x && x>=0;
        }
        
        public bool CanFit(int x, int y, Tetromino tetromino)
        {
            bool canFit = true;

            int[,] matrix = tetromino.Shape.Value;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == 0 )continue;
                    if (Values[i + y, j + x] !=0 && matrix[i, j] != 0)
                    {
                        canFit = false;
                        break;
                    }
                }
            }

            return canFit;
        }

        public void EraseTetromino(Tetromino tetromino)
        {
            var matrix = tetromino.Shape.Value;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3 -tetromino.numberOfEmptyColumns(); j++)
                {
                    
                    if (matrix[i, j] == 0 )continue;
                    Values[i+tetromino.YPosition, j+tetromino.XPosition] = 0;
                }
            }
        }
        
        private void PutTetromino(int x, int y, Tetromino tetromino)
        {
            var matrix = tetromino.Shape.Value;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i,j] == 0) continue;
                    Values[i+y, j+x] = matrix[i, j];
                }
            }
        }

        private int MaximumYValue(Tetromino tetromino) => Height - 1;
        private int MaximumXValue(Tetromino tetromino) => Width - 1;
        
        public int ShiftCoordinates(int x, int y, Tetromino tetromino)
        {
            int maxYValue = MaximumYValue(tetromino);
            int maxXValue = MaximumXValue(tetromino);
            
            EraseTetromino(tetromino);

            if (y >= maxYValue || !CanFit(tetromino.XPosition, y, tetromino)) 
            {
                PutTetromino(tetromino.XPosition, tetromino.YPosition, tetromino);
                return -1; 
            };
            
            if (x >= maxXValue || x<0 || !CanFit(x, tetromino.YPosition,tetromino))
            {
                PutTetromino(tetromino.XPosition, tetromino.YPosition, tetromino);
                return -2;
            }

            PutTetromino(x, y, tetromino);

            tetromino.YPosition = y;
            tetromino.XPosition = x;
            return 0;
        }
    }
}
