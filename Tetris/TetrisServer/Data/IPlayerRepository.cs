using System;
using System.Threading.Tasks;
using TetrisServer.Data.Entities;

namespace TetrisServer.Data;

public interface IPlayerRepository
{
    public Task AddToQueue(Player player);
    public Task<Player> SavePlayer(Player player);
    public Task<long> GetQueueLength();

    public Task<Player> GetPlayerById(string id);
    public Task<Tuple<Player, Player>> Pop();
    public Task RemovePlayerFromQueue(string id);
    public Task RemovePlayer(string id);
    public Task<bool> PlayerIsQueued(string id);
}
