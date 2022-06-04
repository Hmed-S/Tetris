

namespace TetrisEngine
{
    public class Board
    {
        public int[,] Values { get; private set; }

        public Board(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");

            Values = new int[rowCount, columnCount];
            FillBoard(rowCount, columnCount);
        }

        private void FillBoard(int rowCount, int collumnCount)
        {
            for(int i = 0; i < rowCount; i++)
                for (int j = 0; i < collumnCount; i++)
                    Values[i, j] = 0;
        }
    }
}
