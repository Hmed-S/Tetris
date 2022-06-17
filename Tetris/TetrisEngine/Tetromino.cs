using Engine.Extensions;

namespace Engine
{

    public class Tetromino
    {
        public int DropStatus { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Matrix Shape { get; set; }

        private bool isEmpty(int[] array) => Array.TrueForAll(array, i => i == 0);

        private int EmptyCount(int[,] matrix, Predicate<int> predicate)
        {
            int x = 0;
            foreach (int i in Enumerable.Range(0, matrix.ColumnCount()))
                if (predicate(i))
                    x += 1;
            return x;
        }


        public Tetromino RotateClockWise()
        {
            Tetromino rotatedTetromino = new()
            {
                XPosition = XPosition,
                YPosition = YPosition,
                DropStatus = DropStatus,
                Shape = Shape.Rotate90(),
            };
            return rotatedTetromino;
        }

        public Tetromino RotateCounterClockWise()
        {
            Tetromino rotatedTetromino = new()
            {
                XPosition = XPosition,
                YPosition = YPosition,
                DropStatus = DropStatus,
                Shape = Shape.Rotate90CounterClockwise(),
            };
            return rotatedTetromino;
        }

        public int NumberOfEmptyRows() => EmptyCount(Shape.Value, i => isEmpty(Shape.Value.GetRow(i)));

        public int numberOfEmptyColumns() => EmptyCount(Shape.Value, i => isEmpty(Shape.Value.GetColumn(i)));
        
        public static Tetromino Random()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, Shapes.AllShapes.Length);
            var tetromino = FromShape(Shapes.AllShapes[randomIndex]);

            return tetromino;
        }
        public static Tetromino FromShape(Matrix shape) => new(){Shape = shape};
    }
   
}
