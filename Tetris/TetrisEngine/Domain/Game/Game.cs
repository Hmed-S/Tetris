using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine.Domain.Game
{
    public class Game : IGame
    {
        public required IPlayer Player { get; init; }
        public required GameMode Mode { get; init; }
        public IPlayer Opponent
        {
            get
            {               
                if (Mode == GameMode.SinglePlayer)
                {
                    throw new InvalidOperationException("Cannot get opponent in single player mode.");
                }
                else
                {
                    return field;
                }
            }
            set
            {
                if (Mode == GameMode.SinglePlayer)
                {
                    throw new InvalidOperationException("Cannot set opponent in single player mode.");
                }
                else
                {
                    field = value;
                }
            }
        }
        public GameState GameState
        {
            get;
            set
            {
                if (value != GameState.Playing)
                {
                    throw new InvalidOperationException("You can only change the GameState when the Game is playing");
                }
            }
        }
        private static IGame _game;

        public static IGame Start(GameMode gameMode, IPlayer player)
        {
            if(_game == null)
            {
                _game = new Game
                {
                    Mode = gameMode,
                    GameState = GameState.Playing,
                    Player = player
                };

            }
            else if (_game.GameState == GameState.Quit | _game.GameState == GameState.GameOver)
            {
                _game = new Game
                {
                    Mode = gameMode,
                    GameState = GameState.Playing,
                    Player = player
                };
            }
            else if(_game.GameState == GameState.Playing)
            {
                throw new InvalidOperationException("A game is already in progress.");
            }

            player.Game = _game;

            return _game;
        }

        public static IGame GetGame() => _game;

        public void Quit() => GameState = GameState.Quit;
    }
}
