using TetrisEngine.Domain.Extensions;

namespace TetrisEngine.Domain.Board
{
    public class TetrisBoard
    {
        public int Height { get; }
        public int Width { get; }
        public int Lines { get; private set; }
        public virtual int[,] Values { get; set; }


        public TetrisBoard(int rowCount, int columnCount)
        {
            if (rowCount < 10 || columnCount < 10)
                throw new ArgumentException("row and column count must at least be ten");
            Height = rowCount;
            Width = columnCount;
            Values = new int[rowCount, columnCount];

            Replace(0, rowCount, 0, columnCount, (i, j) => 0);
        }

        private void ResetRow(int rowNumber)
        {
            Replace(rowNumber, rowNumber + 1, 0, Width, (i, j) => 0);
            foreach (var row in Enumerable.Range(0, rowNumber).Reverse())
            {
                foreach (var column in Enumerable.Range(0, Width))
                {
                    Values[row + 1, column] = Values[row, column];
                }
            }
        }

        public virtual int CountLines()
        {
            int numberOfLinesDetected = 0;

            foreach (var row in Enumerable.Range(0, Values.RowCount()))
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

        private void Replace(int rowFrom, int rowTo,
            int columnFrom, int columnTo, Func<int, int, int> action)
        {
            var m = 0;
            for (int i = rowFrom; i < rowTo; i++)
                for (int j = columnFrom; j < columnTo; j++)
                {
                    Values[i, j] = action(m, j);
                }
        }

        public bool CanFit(Tetromino tetromino)
        {
            bool canFit = true;

            foreach (var point in tetromino.Points)
            {
                if(point.Column<0 || point.Column > Width - 1)
                {
                    canFit = false;
                    break;
                }

                if (Values[point.Row, point.Column] != 0)
                {
                    canFit = false;
                    break;
                }
            }
            return canFit;
        }


        public void EraseTetromino(Tetromino tetromino)
        {
            tetromino.Points.ForEach(point =>
            {
                Values[point.Row, point.Column] = 0;
            });
        }
        
        private void PutTetromino(List<Point> points)
        {
            points.ForEach(point => Values[point.Row, point.Column] = point.Value);
        }


        public DropStatus ShiftCoordinates(int x, int y, Tetromino tetromino)
        {
            int maxYValue = Height - 1;
            var previousPoints = tetromino.Points;
            var desiredPoints = tetromino.ChangePosition(x, y).Points;

            var rows = desiredPoints.Select(i => i.Row);

            EraseTetromino(tetromino);


            if (rows.Last() >=maxYValue || !CanFit(tetromino))
            {
                PutTetromino(previousPoints);
                return DropStatus.Landed;
            };

            PutTetromino(desiredPoints);

            return DropStatus.Falling;
        }
    }
}
