namespace TetrisEngine.Board
{

    public readonly struct Tetromino
    {
        public int DropStatus { get; init; }
        public Matrix Shape { get; init; }
        public Color Color { get; init; }
        public int XPosition { get; init; }
        public int YPosition { get; init; }
        public List<Point> Points => Point.Of(XPosition, YPosition, this);

        public static Tetromino FromShape(Matrix shape) => new() { Shape = shape };

        public Tetromino ChangePosition(int x, int y)
        {
            return new()
            {
                XPosition = x,
                YPosition = y,
                DropStatus = DropStatus,
                Shape = Shape,
            };
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

        public static Tetromino Random(Random random)
        {
            int randomIndex = random.Next(0, Shapes.AllShapes.Length);
            var tetromino = FromShape(Shapes.AllShapes[randomIndex]);

            return tetromino;
        }

    }
   
}
