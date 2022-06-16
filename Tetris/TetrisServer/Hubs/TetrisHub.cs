using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TetrisServer.Hubs
{
    public class TetrisHub : Hub
    {
        public async Task DropShape(string game)
        {

            await Clients.Others.SendAsync("Drop", game);
        }

        public async Task ReadyUp()
        {
            await Clients.Others.SendAsync("Ready");
        }

        public async Task QuitGame()
        {
            await Clients.Others.SendAsync("Quit");
        }

    }
}
