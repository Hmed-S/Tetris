using TetrisEngine.Domain.Game;

namespace TetrisEngine.Domain.Game.Moves
{
    internal class GameOverMoveset : IMoveSet
    {
        public void DropTetromino(Player player)
        {
            throw new InvalidOperationException("Can not drop a tetromino when the game is over.");
        }

        public void MoveLeft(Player player)
        {
            throw new InvalidOperationException("Can not move left when the game is over.");
        }

        public void MoveRight(Player player)
        {
            throw new InvalidOperationException("Can not move right when the game is over.");
        }

        public void Next(Player player)
        {
            throw new InvalidOperationException("Can not get a new tetromino when the game is over.");
        }

        public void Quit(Player player)
        {
            throw new InvalidOperationException("Can not quit when the game is over.");
        }

        public void Ready(Player player)
        {
            throw new InvalidOperationException("Can not get ready when the game is over.");
        }

        public void RotateLeft(Player player)
        {
            throw new InvalidOperationException("Can not rotate a tetromino when the game is over.");
        }

        public void RotateRight(Player player)
        {
            throw new InvalidOperationException("Can not rotate a tetromino when the game is over.");
        }
    }
}
