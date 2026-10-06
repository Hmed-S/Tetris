using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player;

public interface IPlayer
{
    public IGame Game { get; set; }
    public string Name { get; set; }
    public Tetromino CurrentTetromino { get; set; }
    public Tetromino Preview { get; set; }
    public TetrisBoard Board { get; set; }
    public Score Score { get; set; }
    public int Seed { get; set; }
    public bool IsReady { get; }
    public Random Random { get; set; }
    public Level Level { get; set; }
    public Lines Lines { get; set; }
    public Interval Interval { get; set; }

    public List<Action<int>> OnLineClear { get; set; }
    public List<Action<Tetromino, Tetromino>> OnTetrominoPositionChange { get; set; }
    public List<Action<Score>> OnScoreChange { get; set; }
    public List<Action<Tetromino>> OnPreviewChange { get; set; }
    public List<Action<int, int>> OnLineSwab { get; set; }
    public List<Action<Level>> OnLevelChange { get; set; }
    public List<Action<Lines>> OnLineCountChange { get; set; }

    public void Next();
    public void DropTetromino();
    public void MoveLeft();
    public void MoveRight();
    public void RotateRight();
    public void RotateLeft();
    public void Ready();
    public void Quit();

}
