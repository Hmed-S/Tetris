using TetrisEngine.Domain.Extensions;

namespace TetrisEngine.Domain.Board
{
    public readonly struct Point
    {
        public int Row { get; private init; }
        public int Column { get; private init; }
        public int Value { get; private init; }
        

        public static List<Point> Of(int x, int y, Tetromino tetromino)
        {
            List<Point> points = new();

            foreach (var i in Enumerable.Range(0, tetromino.Shape.Value.RowCount()))
            {
                foreach (var j in Enumerable.Range(0, tetromino.Shape.Value.ColumnCount()))
                {
                    if (tetromino.Shape.Value[i, j] == 0) continue;
                    points.Add(new Point { Row = i + y, Column = j + x, Value = tetromino.Shape.Value[i, j] });
                }
            }
            return points;
        }
    }
}
