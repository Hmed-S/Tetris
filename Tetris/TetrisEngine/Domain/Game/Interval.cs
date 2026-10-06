namespace TetrisEngine.Domain.Game;

public readonly struct Interval
{
    public readonly double Seconds { get; }
    public readonly double MiliSeconds { get => Seconds * 1000; }
    public readonly Interval FastDrop { get => new(0.05); }

    public Interval()
    {
        Seconds = 1;
    }

    private Interval(Level level)
    {
        Seconds = Math.Pow(0.8 - ((level.Value - 1) * 0.007), level.Value - 1);
    }

    private Interval(double seconds)
    {
        Seconds = seconds;
    }

    public static Interval operator +(Interval interval, Level level) => new Interval(level);

}
