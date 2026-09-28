namespace TetrisEngine.Domain.Game;

public readonly struct Score
{
    public readonly int Value { get; }

    public Score()
    {
        Value = 0;
    }

    private Score(int value)
    {
        Value = value;
    }

    public static Score operator +(Score score, int lines)
    {
        int newScore = lines switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => 0
        };

        return new Score(score.Value + newScore);
    }
}
