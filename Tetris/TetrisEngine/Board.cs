using Engine.Extensions;

namespace Engine
{
    public class Board
    {
        public int Height { get;}
        public int Width { get; }
        public List<Tetronmino> Tetronminos { get; } = new();

        public Board(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");
            Height = rowCount;
            Width = columnCount;
        }

        public void AddTetromino(Tetronmino tetronmino) => Tetronminos.Add(tetronmino);

        private bool isEmpty(int[] array) => Array.TrueForAll(array, i => i==0);

        private int EmptyCount(int[,] matrix, Predicate<int> predicate)
        {
            int x = 0;
            foreach (int i in Enumerable.Range(0, matrix.ColumnCount()))
                if (predicate(i))
                    x += 1;
            return x;
        }

        private Tetronmino? FindByXCoordinates(int x, int index) =>
            Tetronminos.Find(tetronmino => tetronmino.XPosition == x && index != Tetronminos.IndexOf(tetronmino));
        
        private Tetronmino? FindByYCoordinates(int y,int index) =>
            Tetronminos.Find(tetronmino => tetronmino.XPosition == y && index !=Tetronminos.IndexOf(tetronmino));


        public int ShiftCoordinates(int x, int y, Tetronmino tetromino)
        {
            int maxYValue = Height-2 + EmptyCount(tetromino.Shape.Value, i => isEmpty(tetromino.Shape.Value.GetRow(i)));
            int maxXValue = Width-2 + EmptyCount(tetromino.Shape.Value, i => isEmpty(tetromino.Shape.Value.GetColumn(i)));
            int index = Tetronminos.IndexOf(tetromino);
            
            if (y >=maxYValue) return -1;
            if (x >= maxXValue || x<0) return -2;
            
            tetromino.YPosition = y;
            tetromino.XPosition = x;
            return 0;
        }
    }
}
