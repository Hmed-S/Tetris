using System.Diagnostics.Contracts;

namespace TetrisEngine.Domain.Board
{
    public class Tetromino
    {
        public DropStatus DropStatus { get; private init; }
        public Matrix Shape { get; private init; }
        public Color Color { get; private init; }
        public int XPosition { get; private init; }
        public int YPosition { get; private init; }
        public List<Point> Points => Point.Of(XPosition, YPosition, this);


        public static Tetromino FromShape(ShapeType shape)
        {
            Matrix shapeMatrix = (shape) switch
            {
                ShapeType.LSHAPE => Shapes.LShape,
                ShapeType.JSHAPE => Shapes.JShape,
                ShapeType.ISHAPE => Shapes.IShape,
                ShapeType.S_SHAPE => Shapes.SShape,
                ShapeType.TSHAPE => Shapes.TShape,
                ShapeType.ZSHAPE => Shapes.ZShape,
                _ => Shapes.OShape
            };

            Color color = shape switch
            {
                ShapeType.LSHAPE => Color.Orange,
                ShapeType.JSHAPE => Color.Cyan,
                ShapeType.ISHAPE => Color.Blue,
                ShapeType.S_SHAPE => Color.Green,
                ShapeType.TSHAPE => Color.Purple,
                ShapeType.ZSHAPE => Color.RED,
                _ => Color.Yellow,
            };

            return new Tetromino()
            {
                XPosition = 0,
                YPosition = 0,
                DropStatus = DropStatus.Falling,
                Shape = shapeMatrix,
                Color = color,
            };

        }

        [Pure]
        public Tetromino ChangePosition(int x, int y)
        {
            return new()
            {
                XPosition = x,
                YPosition = y,
                DropStatus = DropStatus,
                Shape = Shape,
                Color = Color,
            };
        }

        [Pure]
        public Tetromino RotateClockWise()
        {
            return new()
            {
                XPosition = XPosition,
                YPosition = YPosition,
                DropStatus = DropStatus,
                Shape = Shape.Rotate90(),
                Color = Color,
            };
        }

        [Pure]
        public Tetromino RotateCounterClockWise()
        {
            return new()
            {
                XPosition = XPosition,
                YPosition = YPosition,
                DropStatus = DropStatus,
                Shape = Shape.Rotate90CounterClockwise(),
                Color = Color,
            };
        }

        [Pure]
        public Tetromino ChangeDropStatus(DropStatus dropStatus)
        {
            return new()
            {
                XPosition = XPosition,
                YPosition = YPosition,
                DropStatus = dropStatus,
                Shape = Shape,
                Color = Color,
            };
        }

        public static Tetromino Random(Random random)
        {
            int randomIndex = random.Next(0, Enum.GetValues<ShapeType>().Length);
            ShapeType shape = Enum.GetValues<ShapeType>()[randomIndex];
            Tetromino tetromino = FromShape(shape);

            return tetromino;
        }

    }
   
}
