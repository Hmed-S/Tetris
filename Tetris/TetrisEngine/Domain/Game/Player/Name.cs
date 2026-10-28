namespace TetrisEngine.Domain.Game.Player;

public readonly struct Name
{
    public readonly string Value { get; }

    public Name(string value)
    {
        if(string.IsNullOrEmpty(value))
            throw new ArgumentException("Name can not be null or emtpy.");
        Value = value;
    }
}
