using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player;

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
    public bool IsReady { get; } = player.IsReady;
    public Random Random { get; set; } = player.Random;
    public Level Level { get; set; } = player.Level;
    public Lines Lines { get; set; } = player.Lines;
    public Interval Interval { get; set; } = player.Interval;

    public List<Action<int>> OnLineClear { get; set; } = player.OnLineClear;
    public List<Action<Tetromino, Tetromino>> OnTetrominoPositionChange { get; set; } = player.OnTetrominoPositionChange;
    public List<Action<Score>> OnScoreChange { get; set; } = player.OnScoreChange;
    public List<Action<Tetromino>> OnPreviewChange { get; set; } = player.OnPreviewChange;
    public List<Action<int, int>> OnLineSwab { get; set; } = player.OnLineSwab;
    public List<Action<Level>> OnLevelChange { get; set; } = player.OnLevelChange;
    public List<Action<Lines>> OnLineCountChange { get; set; } = player.OnLineCountChange;
    
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
