namespace TetrisEngine.Sdk;

public class Tetromino
{
    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public int[,] Shape { get; set; }
    public Color Color { get; set; }
    public List<Points> Points { get; set; }
}
