namespace Engine
{
    public class Result
    {
        public int DropStatus { get; set; }
        public int LastXPosition { get; set; }
        public int LastYPosition { get; set; }
        public int[,] Tetromino { get; set; }
    }
}
