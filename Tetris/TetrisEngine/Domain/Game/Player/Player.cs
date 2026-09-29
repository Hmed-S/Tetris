using TetrisEngine.Domain.Board;
using TetrisEngine.Utility;

namespace TetrisEngine.Domain.Game.Player;


public class Player : IPlayer
{
    public string Name { get; set; }
    public Tetromino CurrentTetromino { get; set; }
    public Tetromino Preview { get; set; }
    public TetrisBoard Board { get; set; }
    public Score Score { get; set; }
    public IGame Game { get; set; }
    public int Seed { get; set; }
    public bool IsReady { get; }
    public Random Random { get; set; }
    public Lines Lines { get; set; }
    public Level Level { get; set; }
    public Interval Interval { get; set; }

    public List<Action<int>> OnLineClear { get; set; } = [];
    public List<Action<Tetromino, Tetromino>> OnTetrominoPositionChange { get; set; } = [];
    public List<Action<Score>> OnScoreChange { get; set; } = [];
    public List<Action<Tetromino>> OnPreviewChange { get; set; } = [];
    public List<Action<int, int>> OnLineSwab { get; set; } = [];
    public List<Action<Level>> OnLevelChange { get; set; } = [];
    public List<Action<Lines>> OnLineCountChange { get; set; } = [];

    public Player(int seed, Random random, TetrisBoard board, Score score, Level level, Interval interval)
    {
        Seed = seed;
        Random = random;
        Board = board;
        Score = score;
        Level = level;
        Interval = interval;

        Preview = Tetromino.Random(Random);
        CurrentTetromino = Tetromino.Random(Random);
        OnPreviewChange.ForEach(onPreviewChange => onPreviewChange(Preview));
        
        CenterTetromino();
        OnTetrominoPositionChange.ForEach(
        onTetrominoChange => onTetrominoChange(CurrentTetromino, CurrentTetromino));
    }

    public virtual void Next()
    {
        CurrentTetromino = Preview;
        Preview = Tetromino.Random(Random);
        CenterTetromino();
        OnPreviewChange.ForEach(onPreviewChange => onPreviewChange(Preview));

        OnTetrominoPositionChange.ForEach(
            onTetrominoChange => onTetrominoChange(CurrentTetromino, CurrentTetromino));

        if (Board.IsToppedOut(CurrentTetromino))
        {
            Game.Over();
        }
    }

    private void CenterTetromino()
    {
        CurrentTetromino =
                    CurrentTetromino.ChangePosition((Board.Width - CurrentTetromino.Shape.Value.ColumnCount()) / 2, 0);
    }

    public virtual void DropTetromino()
    {
        Tetromino tetromino = CurrentTetromino
            .ChangePosition(CurrentTetromino.XPosition, CurrentTetromino.YPosition +1);

        if (!PutTetromino( () => tetromino)) // when it can no longer be dropped it should have landed.
        {
            tetromino = CurrentTetromino.ChangeDropStatus(DropStatus.Landed);

            Board.WriteTetromino(tetromino);
            OnTetrominoPositionChange.ForEach(ondrop => ondrop(tetromino, tetromino));
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

    public virtual void MoveLeft() => PutTetromino(() => CurrentTetromino.ChangePosition(CurrentTetromino.XPosition - 1, CurrentTetromino.YPosition));

    public virtual void MoveRight() => PutTetromino(() => CurrentTetromino.ChangePosition(CurrentTetromino.XPosition + 1, CurrentTetromino.YPosition));

    public virtual void RotateRight() => PutTetromino(() => CurrentTetromino.RotateClockWise());

    public virtual void RotateLeft() => PutTetromino(() => CurrentTetromino.RotateCounterClockWise());

    public virtual void Quit()
    {
        Game.Quit();
    }

    public virtual void Ready()
    {
        throw new InvalidOperationException("You can't get ready if you aren't playing with someone else");
    }

    private bool PutTetromino(Func<Tetromino> getTetromino)
    {
        Tetromino changedTetromino = getTetromino();
        bool canFit = Board.CanFit(changedTetromino);

        if (canFit)
        {
            
            OnTetrominoPositionChange.ForEach(OnTetrominoPositionChange => OnTetrominoPositionChange(CurrentTetromino, changedTetromino));
            CurrentTetromino = changedTetromino;
        }

        return canFit;
    }

}
