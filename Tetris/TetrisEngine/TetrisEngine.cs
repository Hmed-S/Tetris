namespace Engine;

public class TetrisEngine
{
    private Board _board;
    public int[,] Board { get => _board.Values; }
    public Tetromino Preview { get; private set; }
    public int Lines { get=> _board.Lines; }
    
    public int Score { get; private set; }
    
    public TetrisEngine(Board board)
    {
        _board = board;
        _board.AddTetromino(Tetromino.Random());
        Preview = Tetromino.Random();
    }

    public virtual Tetromino CurrentTetromino() => _board.Tetronminos[_board.Tetronminos.Count - 1];

    private void PutTetromino(int x, int y)
    {
        int draw = _board.ShiftCoordinates(x, y, CurrentTetromino());
        CurrentTetromino().DropStatus = draw;
        if(CurrentTetromino().DropStatus == -1) Next();
    }

    public void Next()
    {
        Score+=GetIncrement(_board.CountLines());
        _board.AddTetromino(Preview);
        Preview = Tetromino.Random();
    }

    public List<Tetromino> AllTetrominos() => _board.Tetronminos;
        
    public void ShiftToLeft() => PutTetromino(CurrentTetromino().XPosition-1, CurrentTetromino().YPosition);
        
    public void ShiftToRight() => PutTetromino(CurrentTetromino().XPosition+1, CurrentTetromino().YPosition);

    private int GetIncrement(int linesGained)
    {
        return linesGained switch
        {
            1 => Score += 40,
            2 => 100,
            3 => 300,
            4 => 1200,
            _ => 0
        };
    }

    public void RotateRight()
    {
        var current = CurrentTetromino();
        Matrix rotated = current.Shape.Rotate90();
        
        if (_board.CanFit(current, rotated.Value))
        {         
            _board.EmptySpot(current);
            current.Shape = rotated;
            _board.ShiftCoordinates(current.XPosition, current.YPosition, current);
        }
    }

    public void RotateLeft()
    {
        var current = CurrentTetromino();
        Matrix rotated = current.Shape.Rotate90CounterClockwise();
        
        if (_board.CanFit(current, rotated.Value))
        {
            _board.EmptySpot(current);
            current.Shape = rotated;
            _board.ShiftCoordinates(current.XPosition, current.YPosition, current);
        }
    }
        
    public void DropTetromino() => PutTetromino(CurrentTetromino().XPosition, CurrentTetromino().YPosition+1);

}