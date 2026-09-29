namespace TetrisEngine.Sdk;

public class Player
{
    public string Name { get; set; }
    public Board Board { get; set; }
    private Tetromino _currentTetromino;
    public Tetromino CurrentTetromino 
    {
        get
        {
            return _currentTetromino;
        }
        set
        {
            OnTetrominoPositionChange?.Invoke(_currentTetromino, value);
            _currentTetromino = value;
        }
    }
    private Tetromino _preview;
    public Tetromino Preview
    { get
        {
            return _preview;
        }
        set
        {
            _preview = value;
            OnPreviewChange?.Invoke(_preview);
        }
    }
    public Interval Interval { get; set; }

    private int _score;
    public int Score 
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            OnScoreChange?.Invoke(_score);
        }
    }
    private int _level;
    public int Level 
    { 
       get
       {
            return _level;
       }
       set
       {
            _level = value;
            OnLevelChange?.Invoke(_level);
       }
    }
    private int _lineCount;
    public int LineCount
    {
        get
        {
            return _lineCount;
        }
        set
        {
            _lineCount = value;
            OnLineCountChange?.Invoke(_lineCount);
        }
    }

    public event Action<Tetromino, Tetromino> OnTetrominoPositionChange;
    public event Action<Tetromino> OnPreviewChange;
    public event Action<int> OnScoreChange;
    public event Action<int> OnLevelChange;
    public event Action<int> OnLineCountChange;

}
