namespace TetrisEngine.Domain.Game;

public readonly struct Level
{
    public readonly int Value { get; }
    
    public Level()
    {
        Value = 1;
    }

    private Level(int level)
    {
        Value = level;
    }

    public static Level operator +(Level level, Lines lines) => new(lines.Value/10+1);

}
