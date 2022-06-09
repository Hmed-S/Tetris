namespace Engine
{
    public class Tetronmino
    {
        public int DropStatus { get; set; }
        public int LastXPosition { get; set; }
        public int LastYPosition { get; set; }
        public Matrix Shape { get; set; }

        public static Tetronmino Random()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, Shapes.allShapes.Length);

            return new (){Shape = Shapes.allShapes[randomIndex]};
        }
        public static Tetronmino FromShape(Matrix shape) => new(){Shape = shape};
    }
    
    public static class Shapes
    {
        public readonly static Matrix Lshape = new(new [,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            );

        public readonly static Matrix Jshape = new(new [,]
            {
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 1, 0 },
            }
        );
        
        public readonly static Matrix Ishape = new(new [,]
            {
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 0, 0 },
            }
        );
        
        public readonly static Matrix Sshape = new(new [,]
            {
                { 0, 1, 1 },
                { 1, 1, 0 },
                { 0, 0, 0 },
            }
        );
        
        public readonly static Matrix Tshape = new(new [,]
            {
                { 1, 1, 1 },
                { 0, 1, 0 },
                { 0, 1, 0 },
            }
        );
        
        public readonly static Matrix Zshape = new(new [,]
            {
                { 1, 1, 0 },
                { 0, 1, 1 },
                { 0, 0, 0 },
            }
        );
        
        public readonly static Matrix Oshape = new(new [,]
            {
                { 1, 1, 0 },
                { 1, 1, 0 },
                { 0, 0, 0 },
            }
        );


        public readonly static Matrix[] allShapes = new[] {Lshape, Jshape, Ishape, Sshape, Tshape, Zshape, Oshape};
    }
}
