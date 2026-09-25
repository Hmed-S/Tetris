using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Extensions;

namespace TetrisEngine.Domain.Game.Player
{
    public class Player : IPlayer
    {
        public string Name { get; set; }
        public Tetromino CurrentTetromino { get; set; } = Tetromino.Random(new Random());
        public Tetromino Preview { get; set; } = Tetromino.Random(new Random());
        public TetrisBoard Board { get; set; }
        public Score Score { get; set; } = new Score();
        public IGame Game { get; set; }
        public int Seed { get; set; } = 0;
        public bool IsReady { get; set; }
        public Random Random { get; set; } = new Random();
        public Lines Lines { get; set; } = new Lines();
        public Level Level { get; set; } = new Level();
        public Interval Interval { get; set; } = new Interval();

        public List<Action<int>> OnLineClear { get; set; } = [];
        public List<Action<Tetromino, Tetromino>> OnDrop { get; set; } = [];
        public List<Action<Score>> OnScoreChange { get; set; } = [];
        public List<Action<Tetromino>> OnPreviewChange { get; set; } = [];
        public List<Action<int, int>> OnLineSwab { get; set; } = [];
        public List<Action<Level>> OnLevelChange { get; set; } = [];
        public List<Action<Lines>> OnLineCountChange { get; set; } = [];

        public void Next()
        {
            CurrentTetromino = Preview;
            Preview = Tetromino.Random(new Random());

            CurrentTetromino = 
                CurrentTetromino.ChangePosition((Board.Width - Preview.Shape.Value.ColumnCount()) / 2,0);
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
                int linesCleared = Board.Clearlines(OnLineClear, OnLineSwab);

                Lines += linesCleared; // this is the total lines cleared in the entire game
                OnLineCountChange.ForEach(onlineChange => onlineChange(Lines));

                Score += linesCleared; // score only increments with the total lines cleared at this exact moment
                OnScoreChange.ForEach(onScoreChange => onScoreChange(Score));

                Level += Lines;
                OnLevelChange.ForEach(OnLevelChange => OnLevelChange(Level));

                Interval += Level;

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
