using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TetrisServer.Dto;

namespace TetrisServer.Hubs
{
    public class TetrisHub : Hub
    {
        public async Task DropShape(Game game)
        {
            await Clients.Others.SendAsync("Drop", game);
        }

        public async Task ReadyUp(int seed)
        {
            await Clients.Others.SendAsync("Ready", seed);
        }

        public async Task RotateShape(string direction)
        {
            await Clients.Others.SendAsync("RotateShape", direction);
        }

        public async Task MoveShape(string moveDirection)
        {
            await Clients.Others.SendAsync("MoveShape", moveDirection);
        }

    }
}
