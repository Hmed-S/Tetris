using Engine.Extensions;

namespace Engine
{
    public class Board
    {
        public int Height { get;}
        public int Width { get; }
        public List<Tetromino> Tetronminos { get; } = new();
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
        
        public int CountLines()
        {
            int numberOfLinesDetected = 0;
            foreach (var row in Enumerable.Range(0,Values.RowCount()))
            {
                bool full = Array.TrueForAll(Values.GetRow(row), i => i != 0);
                // Console.WriteLine(full);
                if (full)
                {
                    ResetRow(row);
                    numberOfLinesDetected += 1;
                }
            }
            Lines += numberOfLinesDetected;
            return numberOfLinesDetected;
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
            for (int i = 0; i < 3-tetromino.NumberOfEmptyRows(); i++)
            {
                for (int j = 0; j < 3-tetromino.numberOfEmptyColumns(); j++)
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
        
        public bool CanFit(Tetromino tetromino, int[,] rotatedValue)
        {
            bool canRotate = true;
            if (tetromino.YPosition == Height-3 || tetromino.XPosition == Width-3)
                return false;

            EmptySpot(tetromino);
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (rotatedValue[i, j] == 0 )continue;
                    if (Values[i + tetromino.YPosition, j + tetromino.XPosition] !=0 && rotatedValue[i, j] != 0)
                    {
                        canRotate = false;
                        break;
                    }
                }
            }
            PutTetromino(tetromino.XPosition, tetromino.YPosition, tetromino);
            return canRotate;
        }
        
        public bool CanGoLeftOrRight(int potentialXPosition, Tetromino tetromino)
        {
            int[,] matrix = tetromino.Shape.Value;
            bool canGoLeftOrRight = true;

            EmptySpot(tetromino);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == 0 )continue;
                    if (Values[i + tetromino.YPosition, j + potentialXPosition] !=0 && matrix[i, j] != 0)
                    {
                        canGoLeftOrRight = false;
                        break;
                    }
                }
            }
            PutTetromino(tetromino.XPosition, tetromino.YPosition, tetromino);
            return canGoLeftOrRight;
        }

        public void EmptySpot(Tetromino tetromino)
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
                    Values[i+y, j+x] = matrix[i, j];
                }
            }
        }

        private int MaximumYValue(Tetromino tetromino) => Height - 2 + tetromino.NumberOfEmptyRows();
        private int MaximumXValue(Tetromino tetromino) => Width - 2 + tetromino.numberOfEmptyColumns();
        
        public int ShiftCoordinates(int x, int y, Tetromino tetromino)
        {
            int maxYValue = MaximumYValue(tetromino);
            int maxXValue = MaximumXValue(tetromino);

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
