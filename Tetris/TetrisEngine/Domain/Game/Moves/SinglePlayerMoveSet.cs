using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Moves
{
    public class SinglePlayerMoveSet : IMoveSet
    {
        public void Next(Player player)
        {
            player.CurrentTetromino = player.Preview;
            player.Preview = Tetromino.Random(new Random(player.Seed));
        }

        public void DropTetromino(Player player)
        {
            DropStatus draw = player.Board.ShiftCoordinates(
                player.CurrentTetromino.XPosition,
                player.CurrentTetromino.YPosition + 1,
                player.CurrentTetromino);
            player.CurrentTetromino = player.CurrentTetromino.ChangeDropStatus(draw);
            if (player.CurrentTetromino.DropStatus == DropStatus.Landed) Next(player);
        }

        public void MoveLeft(Player player) => PutTetromino(player, player.CurrentTetromino.XPosition - 1, player.CurrentTetromino.YPosition);

        public void MoveRight(Player player) => PutTetromino(player, player.CurrentTetromino.XPosition + 1, player.CurrentTetromino.YPosition);

        public void RotateRight(Player player)
        {
            if (player.CurrentTetromino.XPosition < player.Board.Width - 2)
            {
                Tetromino rotatedTetromino = player.CurrentTetromino.RotateClockWise();

                player.Board.EraseTetromino(player.CurrentTetromino);
                bool fit = player.Board.CanFit(rotatedTetromino);


                if (fit) player.CurrentTetromino = rotatedTetromino;

                player.Board.ShiftCoordinates(player.CurrentTetromino.XPosition, player.CurrentTetromino.YPosition, player.CurrentTetromino);
            }

        }

        public void RotateLeft(Player player)
        {
            if (player.CurrentTetromino.XPosition < player.Board.Width - 2)
            {
                Tetromino rotatedTetromino = player.CurrentTetromino.RotateCounterClockWise();

                player.Board.EraseTetromino(player.CurrentTetromino);
                bool fit = player.Board.CanFit(rotatedTetromino);


                if (fit) player.CurrentTetromino = rotatedTetromino;

                player.Board.ShiftCoordinates(player.CurrentTetromino.XPosition, player.CurrentTetromino.YPosition, player.CurrentTetromino);
            }

        }

        public void Quit(Player player)
        {
            player.Game.Quit();
        }

        public void Ready(Player player)
        {
            throw new InvalidOperationException("You can't get ready if you aren't playing with someone else");
        }

        private void PutTetromino(Player player, int x, int y)
        {
            DropStatus draw = player.Board.ShiftCoordinates(x, y, player.CurrentTetromino);
            player.CurrentTetromino = player.CurrentTetromino.ChangeDropStatus(draw);
        }

    }
}
