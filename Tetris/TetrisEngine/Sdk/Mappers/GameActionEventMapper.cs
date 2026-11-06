using DomainIGame = TetrisEngine.Domain.Game.IGame;
using DomainGameState = TetrisEngine.Domain.Game.GameState;
using TetrisEngine.Domain.Game;
namespace TetrisEngine.Sdk.Mappers;

internal static class GameActionEventMapper
{
    public static void MapGameActionEvents(this DomainIGame game, Game dto)
    {
        game.OnGameOver.Add((player) =>
        {
            dto.State = (game.GameState == DomainGameState.GameOver) ? GameState.GameOver : GameState.Quit;

            dto.Player.Quit = player.Id == game.Player.Id && game.GameState == DomainGameState.Quit;


            if (game.Mode == GameMode.MultiPlayer && game.Opponent != null)
            {
                dto.Opponent.Quit = player.Id == game.Opponent.Id && game.GameState == DomainGameState.Quit;

                if (game.GameState == DomainGameState.GameOver)
                {
                    dto.Opponent.Won = game.Winner.Id == game.Opponent.Id;
                    dto.Player.Won = game.Winner.Id == game.Player.Id;
                }

            }
            dto.IsOver = true;
        });
        game.OnBothReady.Add(() => { dto.BothReady = true; });
    }

}
