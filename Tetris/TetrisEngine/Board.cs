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
                    int columnFrom, int columnTo, Func<int, int, int> action)
        {
            for (int i = rowFrom; i < rowTo; i++)
                for (int j =columnFrom; j < columnTo; j++)
                    Values.SetValue(action(i, j), i, j);
        }

    }
}
