using DomainGame = TetrisEngine.Domain.Game.IGame;
using DomainGameState = TetrisEngine.Domain.Game.GameState;

namespace TetrisEngine.Sdk.Mappers;

internal static class GameMapper
{

    private static GameState ConvertGameState(DomainGameState gameState) => gameState switch
    {
        DomainGameState.Playing => GameState.Playing,
        DomainGameState.GameOver => GameState.GameOver,
        _ => GameState.Quit
    };

    public static Game ToDto(this DomainGame game)
    {
        return new Game
        {
            Player = game.Player.ToDto(),
            State = ConvertGameState(game.GameState),
        };
    }

}
