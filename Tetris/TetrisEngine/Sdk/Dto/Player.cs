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
    private bool _isReady = false;
    public bool IsReady {
        get
        {
            return _isReady; 
        }
        set 
        {
            _isReady = value;
            if(IsReady)
            OnReady?.Invoke();
        }
    }
    private bool _quit;
    public bool Quit 
    { 
        get
        {
            return _quit;
        }
        set
        {
            _quit = value;
            if (_quit)
            OnQuit?.Invoke();
        }
    }
    private bool _disconnected;
    public bool Disconnected 
    { 
        get
        {
            return _disconnected;
        }
        set
        {
            _disconnected = value;
            if(_disconnected)
            OnDisconnected?.Invoke();
        }
    }

    public event Action OnReady;
    public event Action<Tetromino, Tetromino> OnTetrominoPositionChange;
    public event Action<Tetromino> OnPreviewChange;
    public event Action<int> OnScoreChange;
    public event Action<int> OnLevelChange;
    public event Action<int> OnLineCountChange;
    public event Action OnQuit;
    public event Action OnDisconnected;

}
