using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Threading.Tasks;
using TetrisServer.Data;
using TetrisServer.Data.Entities;
using TetrisServer.Hubs;

namespace TetrisServer.Consumers;

public class RedisPlayerConsumer(IRedisDatabase redis, IPlayerRepository repository, IHubContext<TetrisHub> tetrisHub, ILogger<RedisPlayerConsumer> _logger) : Controller, IPlayerConsumer, ICapSubscribe
{

    public async Task NotifyMatch(Player player1, Player player2)
    {
        await tetrisHub.Clients.Client(player1.Id).SendAsync("MatchFound", player2.Name);
        await tetrisHub.Clients.Client(player2.Id).SendAsync("MatchFound", player1.Name);
    }

    [CapSubscribe("Queue")]
    public async Task OnPlayerJoined()
    {
        long queueLenght = await redis.ListLengthAsync("Players");
        _logger.LogWarning("length:");
        _logger.LogWarning($"{queueLenght}");

        if(queueLenght >= 2) 
        {
            _logger.LogWarning("Atleast 2 or more");
            Player player1 = await redis.ListGetFromRightAsync<Player>("Players");
            await redis.ListRemoveAsync("Players", player1);

            Player player2 = await redis.ListGetFromRightAsync<Player>("Players");
            await redis.ListRemoveAsync("Players", player2);

            player1.Opponent = player2;
            player2.Opponent = player1;
            
            await repository.SavePlayer(player1);
            await repository.SavePlayer(player2);

            await NotifyMatch(player1, player2);
        }
        _logger.LogWarning("less than two");
    }

}
