namespace TetrisEngine.Game.Moves
{
    public interface IMoveSet
    {
        public void Next(Player player);
        public void DropTetromino(Player player);
        public void MoveLeft(Player player);
        public void MoveRight(Player player);
        public void RotateRight(Player player);
        public void RotateLeft(Player player);
        public void Ready(Player player);
        public void Quit(Player player);
    }
}
