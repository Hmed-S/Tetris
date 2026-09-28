namespace TetrisEngine.Domain.Game;

public readonly struct Lines
{
    public readonly int Value { get; }

    public Lines()
    {
        Value = 0;
    }

    private Lines(int value)
    {
        Value = value;
    }

    public static Lines operator +(Lines line, int lines) => new (line.Value + lines);
}
