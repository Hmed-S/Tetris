using System.Linq;

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

            for (int i = rowFrom; i < rowTo; i++)
            {
                Console.WriteLine(i);
                for (int j = columnFrom; j < columnTo; j++)
                {
                    Values[i, j] = matrix[matrixIndex, j];
                }
                matrixIndex += 1;
            }

            bool trueForMatrix = Array.TrueForAll(GetRow(matrix, matrix.GetLength(0)-1), i => i==0);
            bool trueForBoard =  Array.TrueForAll(GetRow(Values.GetLength(0)-1), i => i==0);
            
            if (trueForMatrix && trueForBoard && rowFrom >= Values.GetLength(0)-2)
            {
                Replace(rowFrom, rowTo, columnFrom, columnTo, (i,j)=> 0);
                Replace(rowFrom, Values.GetLength(0), columnFrom, columnTo, GetRows(matrix, 0, 2));
            }
        }
        
        public int[] GetRow(int[,] array, int rowNumber)
        {
            return Enumerable.Range(0, array.GetLength(1))
                .Select(i => array[rowNumber,i])
                .ToArray();
        }

        public int[] GetRow(int rowNumber)
        {
            return Enumerable.Range(0, Values.GetLength(1))
                .Select(i => Values[rowNumber,i])
                .ToArray();
        }

        public int[,] GetRows(int[,] array , int from , int to)
        {
            int[,] jagged = new int[to,array.GetLength(1)];
            for (int i = from; i < to; i++)
                for (int j = 0; j < jagged.GetLength(1); j++)
                    jagged[i,j] = array[i,j];
            return jagged;
        }
        
        public int[,] GetRows(int from , int to)
        {
            int[,] jagged = new int[to,Values.GetLength(1)];
            for (int i = from; i < to; i++)
                for (int j = 0; j < jagged.GetLength(1); j++)
                    jagged[i,j] = Values[i,j];
            return jagged;
        }

    }
}
