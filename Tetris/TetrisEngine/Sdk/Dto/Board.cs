namespace TetrisEngine.Sdk;

public class Board
{
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public int[,] Values { get; set; }
    public event Action<int, int> OnRowSwap;
    public event Action<int> OnLineClear;

}
