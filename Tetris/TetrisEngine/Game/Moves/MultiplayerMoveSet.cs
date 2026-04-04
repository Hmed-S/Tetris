namespace TetrisEngine.Game.Moves
{
    public class MultiplayerMoveSet : IMoveSet
    {
        public required IMoveSet SinglePlayerMoveSet { get; init; }

        public void DropTetromino(Player player)
        {
            if (!player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            SinglePlayerMoveSet.DropTetromino(player);
        }

        public void MoveLeft(Player player)
        {
            if (!player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            SinglePlayerMoveSet.MoveLeft(player);
        }

        public void MoveRight(Player player)
        {
            if (!player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            SinglePlayerMoveSet.MoveRight(player);
        }

        public void Next(Player player)
        {
            if (!player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            SinglePlayerMoveSet.Next(player);
        }

        public void Quit(Player player)
        {
           if (!player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            SinglePlayerMoveSet.Quit(player);
        }

        public void Ready(Player player)
        {
            int seed = new Random().Next();
            player.Seed = seed;
        }

        public void RotateLeft(Player player)
        {
            if (!player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");

            SinglePlayerMoveSet.RotateLeft(player);
        }

        public void RotateRight(Player player)
        {
            if (!player.Game.Opponent.IsReady)
                throw new InvalidOperationException("Opponent is not ready.");
            SinglePlayerMoveSet.RotateRight(player);
        }
    }
}
