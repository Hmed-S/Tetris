using System.Diagnostics;
using Engine.Extensions;

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

        public List<int> RowFromRange(int rowNumber)
        {
            return Values.GetRow(rowNumber)
                .ToList();
        }
        
        public List<int> RowFromRange(int rownumber, int from, int count)
        {
            return Values.GetRow(rownumber)
                .ToList()
                .GetRange(from, count);
        }

        public bool CanNotGoDown(Tetromino tetromino)
        {
            int xMax = tetromino.XPosition <Width-2? 3: 2;
            int xMin = tetromino.XPosition == 0 ? tetromino.XPosition : tetromino.XPosition - 1;
            
            var nextRow = RowFromRange(tetromino.YPosition + 3 - tetromino.NumberOfEmptyRows(), tetromino.XPosition, 
                xMax-tetromino.EmptyColumnFromEnd());
            Trace.WriteLine(string.Join(",", nextRow));
            return nextRow.Contains(1);
        }
        
        private int PutTetromino(int x, int y, Tetromino tetromino)
        {
            var matrix = tetromino.Shape.Value;
            int maxYValue = Height - 2 + tetromino.NumberOfEmptyRows();
            int maxXValue = Width - 2 + tetromino.numberOfEmptyColumns();
            
  
            if (y >=maxYValue) return -1;
            if (x >= maxXValue || x<0) return -2;
            if (CanNotGoDown(tetromino)) return -1;


            for (int i = tetromino.YPosition; i < tetromino.YPosition+3; i++)
            {
                for (int j = tetromino.XPosition; j < tetromino.XPosition+3 -tetromino.EmptyColumnFromEnd(); j++)
                {
                    Values[i, j] = 0;
                }
            }

            var matrixYIndex = 0;
            var matrixXIndex = 0;
            for (int i = y; i < y+3; i++)
            {
                for (int j = x; j < x+3; j++)
                {
                    if (matrixXIndex > 2) matrixXIndex = 0;
                    if(matrix[matrixYIndex, matrixXIndex] == 0) continue;
                    Values[i, j] = matrix[matrixYIndex, matrixXIndex];
                    
                    matrixXIndex += 1;
                }
                matrixYIndex += 1;
            }

            return 0;
        }
        
        private string toString(int[,] a)
        {
            var result = string.Empty;
            var maxI = a.GetLength(0);
            var maxJ = a.GetLength(1);
            for (var i = 0; i < maxI; i++)
            {
                result += "{";
                for (var j = 0; j < maxJ; j++)
                {
                    result += $"{a[i, j]}";
                }

                result += "}";
            }

            return result;
        }
        public int ShiftCoordinates(int x, int y, Tetromino tetromino)
        {
            int put = PutTetromino(x,y, tetromino);

            if (put == 0)
            {
                tetromino.YPosition = y;
                tetromino.XPosition = x;
                
            }
            return put;
        }
    }
}
