using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task AddToQueue(Player player)
    {
        await redis.SortedSetAddAsync("Players", player, player.TimeJoined.Microsecond);
    }

    public async Task<long> GetQueueLength()
    {
        return await redis.SortedSetLengthAsync("Players");
    }

    public async Task<Tuple<Player, Player>> Pop()
    {
        IEnumerable<ScoreRankResult<Player>> scoreRankResults = await redis.SortedSetPopAsync<Player>("Players", 2);
        
        Player player1 = scoreRankResults.ElementAt(0).Element;
        Player player2 = scoreRankResults.ElementAt(1).Element;

        return new Tuple<Player, Player>(player1, player2);
    }
}
