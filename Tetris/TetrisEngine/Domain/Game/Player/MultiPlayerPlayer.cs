using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player;

public class MultiPlayerPlayer : Player
{
    public MultiPlayerPlayer(int seed, Random random, TetrisBoard board, Score score, Level level, Interval interval) : base(seed, random, board, score, level, interval)
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
        Seed = seed;
    }

}
