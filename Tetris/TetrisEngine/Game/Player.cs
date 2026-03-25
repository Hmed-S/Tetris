using TetrisEngine.Board;

namespace TetrisEngine.Game
{
    public class Player
    {
        public string Name { get; init; }
        public Tetromino CurrentTetromino { get; set; }
        public Tetromino Preview { get; set; }
        public TetrisBoard Board { get; init; }
        public Score Score { get; set; }
        private IGame? Game { get; init; }
        public Random Random { get; set; }


        private void PutTetromino(int x, int y)
        {
            DropStatus draw = Board.ShiftCoordinates(x, y, CurrentTetromino);
            CurrentTetromino = CurrentTetromino.ChangeDropStatus(draw);
        }

        public void Next()
        {
            UpdateScore(Board.CountLines());
            CurrentTetromino = Preview;
            Preview = Tetromino.Random(Random);
        }

        public void ShiftToLeft() => PutTetromino(CurrentTetromino.XPosition - 1, CurrentTetromino.YPosition);

        public void ShiftToRight() => PutTetromino(CurrentTetromino.XPosition + 1, CurrentTetromino.YPosition);

        public void DropTetromino()
        {
            DropStatus draw = Board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition + 1, CurrentTetromino);
            CurrentTetromino = CurrentTetromino.ChangeDropStatus(draw);
            if (CurrentTetromino.DropStatus == DropStatus.Landed) Next();
        }

        private void UpdateScore(int linesGained)
        {
            Score = new Score(linesGained);
        }

        public void RotateRight()
        {
            if (CurrentTetromino.XPosition < Board.Width - 2)
            {
                Tetromino rotatedTetromino = CurrentTetromino.RotateClockWise();

                Board.EraseTetromino(CurrentTetromino);
                bool fit = Board.CanFit(rotatedTetromino);


                if (fit) CurrentTetromino = rotatedTetromino;

                Board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
            }

        }

        public void RotateLeft()
        {
            if (CurrentTetromino.XPosition < Board.Width - 2)
            {
                Tetromino rotatedTetromino = CurrentTetromino.RotateCounterClockWise();

                Board.EraseTetromino(CurrentTetromino);
                bool fit = Board.CanFit(rotatedTetromino);


                if (fit) CurrentTetromino = rotatedTetromino;

                Board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition, CurrentTetromino);
            }

        }

        public void Quit()
        {

        }

        public void Ready()
        {

        }

    }
}
