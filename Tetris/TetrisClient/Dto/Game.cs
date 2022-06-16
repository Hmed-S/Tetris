namespace TetrisClient.Dto
{
    internal class Game
    {
        public int Score { get; set; }  
        public int Lines { get; set; }
        public int[,] Board { get; set; }
        public int[,] Preview { get; set; }
    }
}
