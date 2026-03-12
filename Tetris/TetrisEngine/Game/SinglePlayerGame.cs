using TetrisEngine.Board;

namespace TetrisEngine.Game
{
    internal class SinglePlayerGame
        (
            Player player,
            Tetromino currentTetromino,
            Board.TetrisBoard board,
            Score score,
            Tetromino nextTetronimo
        ) : IGame
    {
        public Player Player { get; }

        public Tetromino CurrentTetromino { get; private set; }

        public Board.TetrisBoard Board {  get; }

        public Score Score { get; }

        public Tetromino NextTetronimo { get; private set; }

        private void PutTetromino(int x, int y)
        {
            //int draw = _board.ShiftCoordinates(x, y, CurrentTetromino); ;
            //CurrentTetromino.DropStatus = draw;
        }

        private void Next()
        {
            //Score += GetIncrement(_board.CountLines());
            //CurrentTetromino = Preview;
            //Preview = Tetromino.Random();
        }

        public void Drop()
        {
            int draw = Board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition + 1, CurrentTetromino);
            CurrentTetromino.DropStatus = draw;
            if (CurrentTetromino.DropStatus == -1) Next();
        }

        public void MoveLeft() => PutTetromino(CurrentTetromino.XPosition - 1, CurrentTetromino.YPosition);

        public void MoveRight() => PutTetromino(CurrentTetromino.XPosition + 1, CurrentTetromino.YPosition);
        public void RotateLeft()
        {
            if (CurrentTetromino.XPosition < Board.Width - 2)
            {
                Tetromino RotatedTetromino = CurrentTetromino.RotateCounterClockWise();
                var currentPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
                var rotatedPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, RotatedTetromino);

                Board.EraseTetromino(currentPoints);
                bool fit = Board.CanFit(rotatedPoints);


                if (fit) CurrentTetromino = RotatedTetromino;

                Board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
            }
        }
    
        public void RotateRight()
        {
            if (CurrentTetromino.XPosition < Board.Width - 2)
            {
                Tetromino RotatedTetromino = CurrentTetromino.RotateClockWise();
                var currentPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
                var rotatedPoints = Point.Of(CurrentTetromino.XPosition, CurrentTetromino.YPosition, RotatedTetromino);


                Board.EraseTetromino(currentPoints);
                bool fit = Board.CanFit(rotatedPoints);


                if (fit) CurrentTetromino = RotatedTetromino;

                Board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
            }
        }

        public int Start()
        {
            throw new NotImplementedException();
        }
    }
}
