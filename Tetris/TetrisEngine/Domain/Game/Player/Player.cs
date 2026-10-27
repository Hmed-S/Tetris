using System.Xml.Linq;
using TetrisEngine.Domain.Board;
using TetrisEngine.Utility;

namespace TetrisEngine.Domain.Game.Player;


public class Player : IPlayer
{
    public Name Name { get; set; }
    public Tetromino CurrentTetromino { get; set; }
    public Tetromino Preview { get; set; }
    public TetrisBoard Board { get; set; }
    public Score Score { get; set; }
    public IGame Game { get; set; }
    public int Seed { get; set; }
    public bool IsReady { get; protected set; }
    public Random Random { get; set; }
    public Lines Lines { get; set; }
    public Level Level { get; set; }
    public Interval Interval { get; set; }

    public List<Action<int>> OnLineClear { get; set; } = [];
    public List<Action<PositionChangedEvent>> OnTetrominoPositionChange { get; set; } = [];
    public List<Action<Score>> OnScoreChange { get; set; } = [];
    public List<Action<Tetromino>> OnPreviewChange { get; set; } = [];
    public List<Action<int, int>> OnLineSwab { get; set; } = [];
    public List<Action<Level>> OnLevelChange { get; set; } = [];
    public List<Action<Lines>> OnLineCountChange { get; set; } = [];
    public List<Action> OnReady { get; set; } = [];

    public Player(int seed, Random random, TetrisBoard board, Score score, Level level, Interval interval)
    {
        Seed = seed;
        Random = random;
        Board = board;
        Score = score;
        Level = level;
        Interval = interval;

        Init();
    }

    public Player(Name name, TetrisBoard board, Score score, Level level, Interval interval)
    {
        Name = name;
        Board = board;
        Score = score;
        Level = level;
        Interval = interval;
    }

    public void Init()
    {
        Preview = Tetromino.Random(Random);
        CurrentTetromino = Tetromino.Random(Random);
        OnPreviewChange.ForEach(onPreviewChange => onPreviewChange(Preview));

        CenterTetromino();
        OnTetrominoPositionChange.ForEach(
        onTetrominoChange => onTetrominoChange(new PositionChangedEvent(CurrentTetromino, CurrentTetromino, Move.Drop)));
    }

    public virtual void Next()
    {
        CurrentTetromino = Preview;
        Preview = Tetromino.Random(Random);
        CenterTetromino();
        OnPreviewChange.ForEach(onPreviewChange => onPreviewChange(Preview));

        OnTetrominoPositionChange.ForEach(
            onTetrominoChange => onTetrominoChange(new PositionChangedEvent(CurrentTetromino, CurrentTetromino, Move.Drop)));

        if (Board.IsToppedOut(CurrentTetromino))
        {
            Game.Over(this);
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

        if (!PutTetromino( () => new PositionChangedEvent(CurrentTetromino, tetromino, Move.Drop))) // when it can no longer be dropped it should have landed.
        {
            tetromino = CurrentTetromino.ChangeDropStatus(DropStatus.Landed);

            Board.WriteTetromino(tetromino);
            OnTetrominoPositionChange.ForEach(ondrop => ondrop(new PositionChangedEvent(CurrentTetromino, tetromino, Move.Drop)));
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

    public virtual void MoveLeft()
    {
        PutTetromino(() =>
        {
            var xPosition = CurrentTetromino.XPosition;
            var yPosition = CurrentTetromino.YPosition;
            
            return new PositionChangedEvent(CurrentTetromino, CurrentTetromino.ChangePosition(xPosition -1, yPosition), Move.MoveLeft);
        });
    }
   

    public virtual void MoveRight()
    {
        PutTetromino(() =>
        {
            var xPosition = CurrentTetromino.XPosition;
            var yPosition = CurrentTetromino.YPosition;

            return new PositionChangedEvent(CurrentTetromino, CurrentTetromino.ChangePosition(xPosition +1, yPosition), Move.MoveRight);
        });
    }

    public virtual void RotateRight() => PutTetromino(() => new PositionChangedEvent(CurrentTetromino, CurrentTetromino.RotateCounterClockWise(), Move.RotateRight));

    public virtual void RotateLeft() => PutTetromino(() => new PositionChangedEvent(CurrentTetromino, CurrentTetromino.RotateCounterClockWise(), Move.RotateLeft));

    public virtual void Quit()
    {
        Game.Quit(this);
    }

    public virtual void Ready()
    {
        throw new InvalidOperationException("You can't get ready if you aren't playing with someone else");
    }

    private bool PutTetromino(Func<PositionChangedEvent> positionChanged)
    {
        PositionChangedEvent changedTetromino = positionChanged();
        
        bool canFit = Board.CanFit(changedTetromino.CurrentTetromino);

        if (canFit)
        {
            
            OnTetrominoPositionChange.ForEach(OnTetrominoPositionChange => OnTetrominoPositionChange(changedTetromino));
            CurrentTetromino = changedTetromino.CurrentTetromino;
        }

        return canFit;
    }

}
