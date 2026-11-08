using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TetrisServer.Data;
using TetrisServer.Data.Entities;
using TetrisServer.Model;

namespace TetrisServer.Service;

public class MatchService(IPlayerRepository repository, Random random, ICapPublisher publisher, ILogger<MatchService> _logger)
{

    public async Task JoinQueue(string id, string name)
    {
        Player player = new()
        {
            Id = id,
            Name = name,
            TimeJoined = DateTime.UtcNow
        };

        await repository.AddToQueue(player);
        await publisher.PublishAsync("Queue", player);
    }

    public async Task MakeMatch()
    {
        long getQueueLength = await repository.GetQueueLength();

        if (!(getQueueLength >= 2))
        {
            _logger.LogInformation("Not enough players for a match.");

           return;
        }

        int seed = random.Next();
        
        Tuple<Player, Player> players = await repository.Pop();

        Player player1 = players.Item1;
        Player player2 = players.Item2;

        player1.Opponent = player2;
        player2.Opponent = player1;

        await repository.SavePlayer(player1);
        await repository.SavePlayer(player2);

        await publisher.PublishAsync("MatchMade", new MatchEvent(player1, player2, seed));
    }

    public async Task<Player> GetOpponentByPlayerId(string id)
    {
        Player player = await repository.GetPlayerById(id);

        return player.Opponent;
    }

    private async Task DoDisconnect(string id, string message)
    {
        Player player = await repository.GetPlayerById(id);

        if (player == null) return;

        if (player.Opponent == null)
        {
            await repository.RemovePlayerFromQueue(player.Id);
        }
        else
        {
            await repository.RemovePlayer(player.Id);
            await repository.RemovePlayer(player.Opponent.Id);
            await publisher.PublishAsync(message, player.Opponent.Id);
        }
    }

    public async Task DisconnectPlayer(string id)
    {
        await DoDisconnect(id, "PlayerDisconnected");
    }

    public async Task Quit(string id)
    {
        await DoDisconnect(id, "Quit");
    }

}
