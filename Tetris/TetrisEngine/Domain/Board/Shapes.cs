namespace TetrisEngine.Domain.Board
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
                { 1, 0, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 }
            }
        );

        public readonly static Matrix IShape = new(new[,]
            {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 }
            }
        );

        public readonly static Matrix SShape = new(new[,]
            {
                { 0, 1, 1 },
                { 1, 1, 0 },
                { 0, 0, 0 }
            }
        );

        public readonly static Matrix TShape = new(new[,]
            {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 }
            }
        );

        public readonly static Matrix ZShape = new(new[,]
            {
                { 1, 1, 0 },
                { 0, 1, 1 },
                { 0, 0, 0 }
            }
        );

        public readonly static Matrix OShape = new(new[,]
            {
                { 1, 1 },
                { 1, 1 },
            }
        );

    }
}
