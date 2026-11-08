using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player;

public class MultiPlayerPlayer : Player
{
    public MultiPlayerPlayer(Name name, TetrisBoard board, Score score, Level level, Interval interval) : base(name, board, score, level, interval)
    {

    }

    public override void Next()
    {
        if (!Game.Opponent.IsReady)
            throw new InvalidOperationException("Opponent is not ready.");

        base.Next();
    }

    public override void DropTetromino()
    {
        if (!Game.Opponent.IsReady)
            throw new InvalidOperationException("Opponent is not ready.");

        base.DropTetromino();
    }

    public override void MoveLeft()
    {
        if (!Game.Opponent.IsReady)
            throw new InvalidOperationException("Opponent is not ready.");
        
        base.MoveLeft();
    }

    public override void MoveRight()
    {
        if (!Game.Opponent.IsReady)
            throw new InvalidOperationException("Opponent is not ready.");
        
        base.MoveRight();
    }
    public override void RotateRight()
    {
        if (!Game.Opponent.IsReady)
            throw new InvalidOperationException("Opponent is not ready.");

        base.RotateRight();
    }

    public override void RotateLeft()
    {
        if (!Game.Opponent.IsReady)
            throw new InvalidOperationException("Opponent is not ready.");

        base.RotateLeft();
    }

    public override void Ready()
    {
        int seed = new Random().Next();
        IsReady = true;
        Seed = seed;
        OnReady.ForEach(onReady => onReady());
    }

}
