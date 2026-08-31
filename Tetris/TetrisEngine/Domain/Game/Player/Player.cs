using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player
{
    public class Player : IPlayer
    {
        public string Name { get; set; }
        public Tetromino CurrentTetromino { get; set; } = Tetromino.Random(new Random());
        public Tetromino Preview { get; set; } = Tetromino.Random(new Random());
        public TetrisBoard Board { get; set; }
        public Score Score { get; set; }
        public IGame Game { get; set; }
        public int Seed { get; set; } = 0;
        public bool IsReady { get; set; }
        public Random Random { get; set; } = new Random();
        public List<Action<int>> OnLineClear { get; set; } = [];
        public List<Action<Tetromino, Tetromino>> OnDrop { get; set; } = [];
        public List<Action<Score>> OnScoreChange { get; set; } = [];
        public List<Action<Tetromino>> OnPreviewChange { get; set; } = [];

        public void Next()
        {
            CurrentTetromino = Preview;
            Preview = Tetromino.Random(new Random());
            OnPreviewChange.ForEach(onPreviewChange => onPreviewChange(Preview));
        }

        public void DropTetromino()
        {
            Tetromino tetromino = CurrentTetromino
                .ChangePosition(CurrentTetromino.XPosition, CurrentTetromino.YPosition +1);

            if (Board.CanFit(tetromino))
            {
                
                OnDrop.ForEach(ondrop => ondrop(CurrentTetromino, tetromino));
                CurrentTetromino = tetromino;
            }
            else // when it can no longer be dropped it should have landed.
            {
                tetromino = CurrentTetromino.ChangeDropStatus(DropStatus.Landed);

                Board.WriteTetromino(tetromino);
                OnDrop.ForEach(ondrop => ondrop(tetromino, tetromino));
                Board.Clearlines(OnLineClear);
                Next();
            }

        }

        public void MoveLeft() => PutTetromino(CurrentTetromino.XPosition - 1, CurrentTetromino.YPosition);

        public void MoveRight() => PutTetromino(CurrentTetromino.XPosition + 1, CurrentTetromino.YPosition);

        public void RotateRight()
        {
            Tetromino rotatedTetromino = CurrentTetromino.RotateClockWise();

            if (Board.CanFit(rotatedTetromino))
            {
                OnDrop.ForEach(ondrop => ondrop(CurrentTetromino, rotatedTetromino));
                CurrentTetromino = rotatedTetromino;
            }

        }

        public void RotateLeft()
        {
            Tetromino rotatedTetromino = CurrentTetromino.RotateCounterClockWise();

            if (Board.CanFit(rotatedTetromino))
            {
                OnDrop.ForEach(ondrop => ondrop(CurrentTetromino, rotatedTetromino));
                CurrentTetromino = rotatedTetromino;
            }
        }

        public void Quit()
        {
            Game.Quit();
        }

        public void Ready()
        {
            throw new InvalidOperationException("You can't get ready if you aren't playing with someone else");
        }

        private void PutTetromino(int x, int y)
        {
            Tetromino tetromino = CurrentTetromino.ChangePosition(x, y);

            if (Board.CanFit(tetromino))
            {
                OnDrop.ForEach(ondrop => ondrop(CurrentTetromino, tetromino));
                CurrentTetromino = tetromino;
            }
        }


    }
}
