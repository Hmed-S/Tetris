using DotNetCore.CAP;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;
using TetrisServer.Data.Entities;

namespace TetrisServer.Producers;

public class RedisStreamPlayerProducer(IRedisDatabase redis, ICapPublisher capPublisher) : IPlayerProducer
{

    public async Task Enqueue(Player player)
    {
        player.TimeJoined = DateTime.UtcNow.ToString("o");
        await redis.ListAddToLeftAsync("Players", player);
        
        await capPublisher.PublishAsync("Queue", player);
    }

}
