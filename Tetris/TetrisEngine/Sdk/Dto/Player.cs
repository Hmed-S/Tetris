namespace TetrisEngine.Sdk;

public class Player
{
    public string Name { get; set; }
    public Board Board { get; set; }
    public Tetromino CurrentTetromino 
    {
        get
        {
            return field;
        }
        set
        {
            OnTetrominoPositionChange?.Invoke(field, value);
            field = value;
        }
    }
    public bool FastDrop 
    { 
        get
        {
            return field;
        }
        internal set
        {
            field = value;
            OnFastDropped?.Invoke();
        }
    }
    public Tetromino Preview
    { 
        get
        {
            return field;
        }
        set
        {
            field = value;
            OnPreviewChange?.Invoke(field);
        }
    }
    public Interval Interval { get; set; }
    public int Score 
    {
        get
        {
            return field;
        }
        set
        {
            field = value;
            OnScoreChange?.Invoke(field);
        }
    }
    public int Level 
    { 
       get
       {
            return field;
       }
       set
       {
            field = value;
            OnLevelChange?.Invoke(field);
       }
    }
    public int LineCount
    {
        get
        {
            return field;
        }
        set
        {
            field = value;
            OnLineCountChange?.Invoke(field);
        }
    }
    public bool IsReady {
        get
        {
            return field; 
        }
        set 
        {
            field = value;
            if(IsReady)
            OnReady?.Invoke();
        }
    }
    public bool Quit 
    { 
        get
        {
            return field;
        }
        set
        {
            field = value;
            if (field)
            OnQuit?.Invoke();
        }
    }
    public bool Disconnected 
    { 
        get
        {
            return field;
        }
        set
        {
            field = value;
            if(field)
            OnDisconnected?.Invoke();
        }
    }

    public bool Won { get; set; }

    public event Action OnReady;
    public event Action<Tetromino, Tetromino> OnTetrominoPositionChange;
    public event Action<Tetromino> OnPreviewChange;
    public event Action<int> OnScoreChange;
    public event Action<int> OnLevelChange;
    public event Action<int> OnLineCountChange;
    public event Action OnQuit;
    public event Action OnDisconnected;
    public event Action OnFastDropped;

}
