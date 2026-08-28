using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player
{
    public class MultiPlayerPlayer(
        IPlayer player
        
        ) : IPlayer
    {
        public required IPlayer Player { get; init; }
        public required IGame Game { get; set; } = player.Game;

        // TODO: implement when implmeenting Multiplayer.
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Tetromino CurrentTetromino { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Tetromino Preview { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TetrisBoard Board { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Score Score { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Seed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsReady { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Random Random { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<int>> OnLineClear { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<Tetromino>> OnDrop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<Score>> OnScoreChange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Action<Tetromino>> OnPreviewChange { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void DropTetromino()
        {
            if (!Player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            Player.DropTetromino();
        }

        public void MoveLeft()
        {
            if (!Player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            Player.MoveLeft();
        }

        public void MoveRight()
        {
            if (!Player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            Player.MoveRight();
        }

        public void Next()
        {
            if (!Player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            Player.Next();
        }

        public void Quit()
        {
            if (!Player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            Player.Quit();
        }

        public void Ready()
        {
            int seed = new Random().Next();
            Player.Seed = seed;
        }

        public void RotateLeft()
        {
            if (!Player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");

            Player.RotateLeft();
        }

        public void RotateRight()
        {
            if (Player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            Player.RotateRight();
        }
    }
}
