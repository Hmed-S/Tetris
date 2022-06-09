using Engine.Extensions;

namespace Engine
{
    public class Board
    {
        public int[,] Values { get; }
        public int Height { get => Values.Rows(); }
        public int Width { get => Values.Columns(); }
        
        public Board(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");

            Values = new int[rowCount, columnCount];
            Replace(0,rowCount, 0,columnCount, (i,j)=>0);
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

        public int[,] this[int from, int to] => Values.SubMatrix(from, to);
        
        private void PutTetromino(int rowFrom, int rowTo,
            int columnFrom, int columnTo, int[,] matrix)
        {
            var matrixYIndex = 0;
            var matrixXIndex = 0;

            if (rowFrom < 0) rowFrom = 0;
            
            Predicate<int[,]> zeroOnBotom = (m) =>  Array.TrueForAll(m.GetRow(m.Rows() - 1), i => i==0);
            for (int i = rowFrom; i < rowTo; i++)
            {
                for (int j = columnFrom; j != columnTo; j++)
                {
                    if (matrixXIndex > 2) matrixXIndex = 0;
                    Values[i, j] = matrix[matrixYIndex, matrixXIndex];
                    matrixXIndex += 1;
                }
                matrixYIndex += 1;
            }
            if (zeroOnBotom(matrix) && zeroOnBotom(Values) && rowFrom >= Values.Rows()-2)
            {
                Replace(rowFrom, rowTo, columnFrom, columnTo, (i,j)=> 0);
                PutTetromino(rowFrom, Values.GetLength(0), columnFrom, columnTo, matrix.SubMatrix(2,3));
            }
        }
        
        
        public int DrawTetromino(int x, int y, int[,] matrix)
        {
            
            int maxYValue = Height -3;
            int maxXValue = Width - 2;
            
            int redrawYWindow = (y>maxYValue - 3)? Height-1:y+2;
            int redrawXWindow = (x>maxXValue - 3)? Width-1:x+2;
            
            if (y==Height) return -1;
            if (x == maxXValue || x<1) return -2;
            if (x > maxXValue) x = maxXValue;
            
            if (y > maxYValue && y <Height)
            {

                Replace(0, redrawYWindow+1, 0, redrawXWindow+1, (i, j) => 0);
                PutTetromino(y - 1, Height-1, x-1, x+2, matrix);
                return 0;
            }
            
            Replace(0, redrawYWindow, 0, redrawXWindow, (i, j) => 0);
            PutTetromino(y - 1, y + 2, x-1, x+2, matrix);
            
            return 0;
        }
        
    }
}
