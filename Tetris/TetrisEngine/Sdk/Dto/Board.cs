namespace TetrisEngine.Sdk;

public class Board
{
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public int[,] Values { get; set; }
    public int LastClearedLine 
    { 
        get
        {
            return field;
        }
        set
        {
            field = value;
            OnLineClear?.Invoke(field);
        } 
    }
    public List<int> LastSwappedLines 
    { 
        get
        {
            return field;
        }
        set
        {
            field = value;
            OnRowSwap?.Invoke(field[0], field[1]);
        }
    }

    public event Action<int, int> OnRowSwap;
    public event Action<int> OnLineClear;

}
