using System.Threading.Tasks;

namespace TetrisServer.Consumers;

public interface IPlayerConsumer
{
    public Task OnPlayerJoined();
}
