using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player
{
    public class GameOverPlayer(
        IPlayer player
        
        ): IPlayer
    {
        public IGame Game { get; set; } = player.Game;
        public string Name { get; set; } = player.Name;
        public Tetromino CurrentTetromino { get; set; } = player.CurrentTetromino;
        public Tetromino Preview { get; set; } = player.Preview;
        public TetrisBoard Board { get; set; } = player.Board;
        public Score Score { get; set; } = player.Score;
        public int Seed { get; set; } = player.Seed;
        public bool IsReady { get; set; } = player.IsReady;
        public Random Random { get; set; } = player.Random;
        public List<Action<int>> OnLineClear { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<Tetromino, Tetromino>> OnDrop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<Score>> OnScoreChange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<Tetromino>> OnPreviewChange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<int, int>> OnLineSwab { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void DropTetromino()
        {
            throw new InvalidOperationException("Can not drop a tetromino when the game is over.");
        }

        public void MoveLeft()
        {
            throw new InvalidOperationException("Can not move left when the game is over.");
        }

        public void MoveRight()
        {
            throw new InvalidOperationException("Can not move right when the game is over.");
        }

        public void Next()
        {
            throw new InvalidOperationException("Can not get a new tetromino when the game is over.");
        }

        public void Quit()
        {
            throw new InvalidOperationException("Can not quit when the game is over.");
        }

        public void Ready()
        {
            throw new InvalidOperationException("Can not get ready when the game is over.");
        }

        public void RotateLeft()
        {
            throw new InvalidOperationException("Can not rotate a tetromino when the game is over.");
        }

        public void RotateRight()
        {
            throw new InvalidOperationException("Can not rotate a tetromino when the game is over.");
        }
    }
}
