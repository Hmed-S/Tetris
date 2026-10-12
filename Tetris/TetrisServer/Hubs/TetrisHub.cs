using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TetrisServer.Hubs;

public class TetrisHub : Hub
{

    public async Task ReadyUp(string clientId, int seed)
    {
        await Clients.Client(clientId).SendAsync("Ready", seed);
    }

    public async Task Drop(string clientId)
    {
        await Clients.Client(clientId).SendAsync("Drop");
    }

    public async Task RotateLeft(string clientId)
    {
        await Clients.Client(clientId).SendAsync(nameof(RotateLeft));
    }

    public async Task RotateRight(string clientId)
    {
        await Clients.Client(clientId).SendAsync(nameof(RotateRight));
    }

    public async Task MoveLeft(string clientId)
    {
        await Clients.Client(clientId).SendAsync(nameof(MoveLeft));
    }

    public async Task MoveRight(string clientId)
    {
        await Clients.Client(clientId).SendAsync(nameof(MoveRight));
    }

    public async Task QuitGame(string clientId)
    {
        await Clients.Client(clientId).SendAsync("Quit", clientId);
    }

}
