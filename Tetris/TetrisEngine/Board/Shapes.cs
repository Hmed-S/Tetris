namespace TetrisEngine.Board
{
    public static class Shapes
    {
        public readonly static Matrix LShape = new(new[,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 }
                }
            );

        public readonly static Matrix JShape = new(new[,]
            {
                { 2, 0, 0 },
                { 2, 0, 0 },
                { 2, 2, 0 }
            }
        );

        public readonly static Matrix IShape = new(new[,]
            {
                { 3, 0, 0 },
                { 3, 0, 0 },
                { 3, 0, 0 }
            }
        );

        public readonly static Matrix SShape = new(new[,]
            {
                { 0, 4, 4 },
                { 4, 4, 0 },
                { 0, 0, 0 }
            }
        );

        public readonly static Matrix TShape = new(new[,]
            {
                { 5, 5, 5 },
                { 0, 5, 0 },
                { 0, 5, 0 }
            }
        );

        public readonly static Matrix ZShape = new(new[,]
            {
                { 6, 6, 0 },
                { 0, 6, 6 },
                { 0, 0, 0 }
            }
        );

        public readonly static Matrix OShape = new(new[,]
            {
                { 7, 7, 0 },
                { 7, 7, 0 },
                { 0, 0, 0 }
            }
        );

        public readonly static Matrix[] AllShapes = { LShape, JShape, IShape, SShape, TShape, ZShape, OShape };
    }
}
