using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;
using TetrisServer.Data.Entities;

namespace TetrisServer.Data;

public class RedisPlayerRepository(IRedisDatabase redis) : IPlayerRepository
{

    public async Task<Player> GetPlayerById(string id)
    {
        return await redis.GetAsync<Player>(id);
    }

    public async Task<Player> SavePlayer(Player player)
    {
        var stored = await redis.AddAsync(player.Id, player);

        Console.WriteLine($"is stored {stored}");
        return await Task.FromResult(player);
    }

}
