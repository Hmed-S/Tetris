namespace TetrisEngine.Sdk;

public class Board
{
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public int[,] Values { get; set; }

    private int _lastClearedLine;
    public int LastClearedLine 
    { 
        get
        {
            return _lastClearedLine;
        }
        set
        {
            _lastClearedLine = value;
            OnLineClear.Invoke(_lastClearedLine);
        } 
    }
    private List<int> _lastSwappedLines = [];
    public List<int> LastSwappedLines 
    { 
        get
        {
            return _lastSwappedLines;
        }
        set
        {
            _lastSwappedLines = value;
            OnRowSwap.Invoke(_lastSwappedLines[0], _lastSwappedLines[1]);
        }
    }

    public event Action<int, int> OnRowSwap;
    public event Action<int> OnLineClear;

}
