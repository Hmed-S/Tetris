using System.Linq;
using Engine.MatrixExtensions;

namespace Engine
{
    public class Board
    {
        public int[,] Values { get; private set; }

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
        
        public void Replace(int rowFrom, int rowTo,
            int columnFrom, int columnTo, int[,] matrix)
        {
            var matrixIndex = 0;
            Func<int[,], int> horizontalLength = (m) => m.GetLength(0);
            
            bool trueForMatrix = Array.TrueForAll(matrix.GetRow(horizontalLength(matrix) - 1), i => i==0);
            bool trueForBoard =  Array.TrueForAll(Values.GetRow(horizontalLength(Values)-1), i => i==0);
            
            for (int i = rowFrom; i < rowTo; i++)
            {
                for (int j = columnFrom; j < columnTo; j++)
                {
                    Values[i, j] = matrix[matrixIndex, j];
                }
                matrixIndex += 1;
            }
            if (trueForMatrix && trueForBoard && rowFrom >= horizontalLength(Values)-2)
            {
                Replace(rowFrom, rowTo, columnFrom, columnTo, (i,j)=> 0);
                Replace(rowFrom, Values.GetLength(0), columnFrom, columnTo, matrix.SubMatrix(2,3));
            }
        }
    }
}
