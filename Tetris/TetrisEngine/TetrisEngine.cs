namespace Engine;

public class TetrisEngine
{
    private Board Board { get;}
    public Tetromino Preview { get; private set; }
    public TetrisEngine(int width, int height)
    {
        Board = new Board(height, width);
        Board.AddTetromino(Tetromino.Random());
        Preview = Tetromino.Random();
    }

    public virtual Tetromino CurrentTetromino() => Board.Tetronminos[Board.Tetronminos.Count - 1];

    private void PutTetromino(int x, int y)
    {
        int draw = Board.ShiftCoordinates(x, y, CurrentTetromino());
        CurrentTetromino().DropStatus = draw;
        if(CurrentTetromino().DropStatus == -1) Next();
    }

    public void Next()
    {
        Board.AddTetromino(Preview);
        Preview = Tetromino.Random();
    }

    public List<Tetromino> AllTetrominos() => Board.Tetronminos;
        
    public void ShiftToLeft() => PutTetromino(CurrentTetromino().XPosition-1, CurrentTetromino().YPosition);
        
    public void ShiftToRight() => PutTetromino(CurrentTetromino().XPosition+1, CurrentTetromino().YPosition);
        
    public void DropTetromino() => PutTetromino(CurrentTetromino().XPosition, CurrentTetromino().YPosition+1);

}