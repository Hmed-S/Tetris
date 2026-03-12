using TetrisEngine.Extensions;

namespace TetrisEngine.Board
{
    
    public class Point
    {
        public int Row { get; init; }
        public int Column { get; init; }
        public int Value { get; init; }
        

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
