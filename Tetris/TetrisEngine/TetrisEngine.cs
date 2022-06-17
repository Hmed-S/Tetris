namespace Engine;

public class TetrisEngine
{
    private Board _board;
    public int[,] Board { get => _board.Values; }
    public virtual Tetromino CurrentTetromino {get; private set;}
    public Tetromino Preview { get; private set; }
    public int Lines { get=> _board.Lines; }
    
    public int Score { get; private set; }
    
    public TetrisEngine(Board board)
    {
        _board = board;
        CurrentTetromino = Tetromino.Random();
        Preview = Tetromino.Random();
    }


    private void PutTetromino(int x, int y)
    {
        int draw = _board.ShiftCoordinates(x, y, CurrentTetromino);;
        CurrentTetromino.DropStatus = draw;
    }

    public void Next()
    {
        Score+=GetIncrement(_board.CountLines());
        CurrentTetromino = Preview;
        Preview = Tetromino.Random();
    }


    public void ShiftToLeft() => PutTetromino(CurrentTetromino.XPosition - 1, CurrentTetromino.YPosition);

    public void ShiftToRight() => PutTetromino(CurrentTetromino.XPosition + 1, CurrentTetromino.YPosition);

    
    public void DropTetromino()
    {
        int draw = _board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition + 1, CurrentTetromino);
        CurrentTetromino.DropStatus = draw;
        if (CurrentTetromino.DropStatus == -1) Next();
    }



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
        if(CurrentTetromino.XPosition< _board.Width - 2)
        {
            Tetromino RotatedTetromino = CurrentTetromino.RotateClockWise();
            var currentPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
            var rotatedPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);


            _board.EraseTetromino(currentPoints);
            bool fit = _board.CanFit(rotatedPoints);


            if (fit) CurrentTetromino = RotatedTetromino;

            _board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
        }
 
    }

    public void RotateLeft()
    {
        if (CurrentTetromino.XPosition< _board.Width - 2)
        {
            Tetromino RotatedTetromino = CurrentTetromino.RotateCounterClockWise();
            var currentPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
            var rotatedPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);

            _board.EraseTetromino(currentPoints);
            bool fit = _board.CanFit(rotatedPoints);


            if (fit) CurrentTetromino = RotatedTetromino;

            _board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
        }

    }

}