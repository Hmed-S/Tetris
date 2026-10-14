using System.Threading.Tasks;
using TetrisServer.Data.Entities;

namespace TetrisServer.Data;

public interface IPlayerRepository
{

    public Task<Player> GetPlayerById(string id);

    public Task<Player> SavePlayer(Player player);

}
