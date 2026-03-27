using TetrisEngine.Board;
using TetrisEngine.Game.Moves;

namespace TetrisEngine.Game
{
    public class Player
    {
        public string Name { get; init; }
        public Tetromino CurrentTetromino { get; set; }
        public Tetromino Preview { get; set; }
        public TetrisBoard Board { get; init; }
        public Score Score { get; set; }
        public IMoveSet MoveSet { get; set; }
        public IGame Game { get; init; }
        public int Seed { get; set; }
        public bool IsReady { get; set; }
        public Random Random { get; init; }

        public void Domove(Move move)
        {
            switch (move)
            {
                case Move.MoveToLeft:
                    MoveSet.MoveLeft(this);
                    break;
                case Move.MoveToRight:
                    MoveSet.MoveRight(this);
                    break;
                case Move.Drop:
                    MoveSet.DropTetromino(this);
                    break;
                case Move.RotateRight:
                    MoveSet.RotateRight(this);
                    break;
                case Move.RotateLeft:
                    MoveSet.RotateLeft(this);
                    break;
                case Move.Next:
                    MoveSet.Next(this);
                    break;
                case Move.Ready:
                    MoveSet.Ready(this);
                    break;
                case Move.Quit:
                    MoveSet.Quit(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(move), move, null);
            }
        }


    }
}
