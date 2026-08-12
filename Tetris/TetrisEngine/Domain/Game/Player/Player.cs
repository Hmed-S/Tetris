using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player
{
    public class Player : IPlayer
    {
        public string Name { get; set; }
        public Tetromino CurrentTetromino { get; set; }
        public Tetromino Preview { get; set; }
        public TetrisBoard Board { get; set; }
        public Score Score { get; set; }
        public IGame Game { get; set; }
        public int Seed { get; set; }
        public bool IsReady { get; set; }
        public Random Random { get; set; }


        public void Next()
        {
            CurrentTetromino = Preview;
            Preview = Tetromino.Random(new Random(Seed));
        }

        public void DropTetromino()
        {
            DropStatus draw = Board.ShiftCoordinates(
                CurrentTetromino.XPosition,
                CurrentTetromino.YPosition + 1,
                CurrentTetromino);
            CurrentTetromino = CurrentTetromino.ChangeDropStatus(draw);
            if (CurrentTetromino.DropStatus == DropStatus.Landed) Next();
        }

        public void MoveLeft() => PutTetromino(CurrentTetromino.XPosition - 1, CurrentTetromino.YPosition);

        public void MoveRight() => PutTetromino(CurrentTetromino.XPosition + 1, CurrentTetromino.YPosition);

        public void RotateRight()
        {
            if (CurrentTetromino.XPosition < Board.Width - 2)
            {
                Tetromino rotatedTetromino = CurrentTetromino.RotateClockWise();

                Board.EraseTetromino(CurrentTetromino);
                bool fit = Board.CanFit(rotatedTetromino);


                if (fit) CurrentTetromino = rotatedTetromino;

                Board.ShiftCoordinates(CurrentTetromino.XPosition, CurrentTetromino.YPosition,  CurrentTetromino);
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
            Game.Quit();
        }

        public void Ready()
        {
            throw new InvalidOperationException("You can't get ready if you aren't playing with someone else");
        }

        private void PutTetromino(int x, int y)
        {
            DropStatus draw = Board.ShiftCoordinates(x, y, CurrentTetromino);
            CurrentTetromino = CurrentTetromino.ChangeDropStatus(draw);
        }


    }
}
