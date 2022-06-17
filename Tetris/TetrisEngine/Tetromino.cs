namespace Engine
{

    public class Tetromino
    {
        public int DropStatus { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Matrix Shape { get; set; }

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
        
        public static Tetromino Random()
        {
            Random random = new();
            int randomIndex = random.Next(0, Shapes.AllShapes.Length);
            var tetromino = FromShape(Shapes.AllShapes[randomIndex]);

            return tetromino;
        }
        public static Tetromino FromShape(Matrix shape) => new(){Shape = shape};
    }
   
}
