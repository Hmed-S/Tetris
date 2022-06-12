using Engine.Extensions;

namespace Engine
{
    public class Tetromino
    {
        public int DropStatus { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Matrix Shape { get; set; }

        private bool isEmpty(int[] array) => Array.TrueForAll(array, i => i==0);

        private int EmptyCount(int[,] matrix, Predicate<int> predicate)
        {
            int x = 0;
            foreach (int i in Enumerable.Range(0, matrix.ColumnCount()))
                if (predicate(i))
                    x += 1;
            return x;
        }

        public int EmptyColumnFromStart()
        {
            int x = 0;
            foreach (int i in Enumerable.Range(0, Shape.Value.ColumnCount()))
                if (isEmpty(Shape.Value.GetColumn(i)))
                    x += 1;
            return x;
        }
        
        public int EmptyColumnFromEnd()
        {
            int x = 0;
            for (int i = Shape.Value.ColumnCount() - 1; i > 0;i--)
                if (isEmpty(Shape.Value.GetColumn(i)))
                    x += 1;
            return x;
        }

        public int NumberOfEmptyRows() => EmptyCount(Shape.Value, i => isEmpty(Shape.Value.GetRow(i)));

        public int numberOfEmptyColumns() => EmptyCount(Shape.Value, i => isEmpty(Shape.Value.GetColumn(i)));
        
        public static Tetromino Random()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, Shapes.AllShapes.Length);

            return new (){Shape = Shapes.AllShapes[randomIndex]};
        }
        public static Tetromino FromShape(Matrix shape) => new(){Shape = shape};
    }
    
    public static class Shapes
    {
        public readonly static Matrix LShape = new(new [,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            );

        public readonly static Matrix JShape = new(new [,]
            {
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 1, 0 },
            }
        );
        
        public readonly static Matrix IShape = new(new [,]
            {
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 0, 0 },
            }
        );
        
        public readonly static Matrix SShape = new(new [,]
            {
                { 0, 1, 1 },
                { 1, 1, 0 },
                { 0, 0, 0 },
            }
        );
        
        public readonly static Matrix TShape = new(new [,]
            {
                { 1, 1, 1 },
                { 0, 1, 0 },
                { 0, 1, 0 },
            }
        );
        
        public readonly static Matrix ZShape = new(new [,]
            {
                { 1, 1, 0 },
                { 0, 1, 1 },
                { 0, 0, 0 },
            }
        );
        
        public readonly static Matrix OShape = new(new [,]
            {
                { 1, 1, 0 },
                { 1, 1, 0 },
                { 0, 0, 0 },
            }
        );
        
        public readonly static Matrix[] AllShapes = {LShape, JShape, IShape, SShape, TShape, ZShape, OShape};
    }
}
