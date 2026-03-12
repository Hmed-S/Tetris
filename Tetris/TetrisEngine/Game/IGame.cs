using TetrisEngine.Board;

namespace TetrisEngine.Game
{
    public interface IGame
    {
        public Player Player { get; }
        public Tetromino CurrentTetromino { get; }
        public TetrisBoard Board { get; }
        public Score Score { get; }
        public Tetromino NextTetronimo { get; }
        public int Start();// should return the seed of the game
        public void Drop();
        public void MoveLeft();
        public void MoveRight();
        public void RotateRight();
        public void RotateLeft();

    }
}
