using System.Threading.Tasks;
using TetrisServer.Data.Entities;

namespace TetrisServer.Producers;

public interface IPlayerProducer
{
    public Task Enqueue(Player player);
}
