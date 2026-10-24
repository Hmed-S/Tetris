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
        await redis.AddAsync(player.Id, player);

        return await Task.FromResult(player);
    }

    public async Task AddToQueue(Player player)
    {
        await SavePlayer(player);
        await redis.SortedSetAddAsync("Players", player.Id, player.TimeJoined.Microsecond);
    }

    public async Task<long> GetQueueLength()
    {
        return await redis.SortedSetLengthAsync("Players");
    }

    public async Task<Tuple<Player, Player>> Pop()
    {
        IEnumerable<ScoreRankResult<string>> scoreRankResults = await redis.SortedSetPopAsync<string>("Players", 2);
        
        string player1Id = scoreRankResults.ElementAt(0).Element;
        string player2Id = scoreRankResults.ElementAt(1).Element;

        Player player1 = await GetPlayerById(player1Id);
        Player player2 = await GetPlayerById(player2Id);

        return new Tuple<Player, Player>(player1, player2);
    }

    public async Task RemovePlayerFromQueue(string id)
    {
        await RemovePlayer(id);
        await redis.SortedSetRemoveAsync("Players", id);
    }

    public async Task RemovePlayer(string id)
    {
       await redis.RemoveAsync(id);
    }

    public async Task<bool> PlayerIsQueued(string id)
    {
        double? score = await redis.SortedSetScoreAsync("Players", id);

        return score.HasValue;
    }

}
