using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using TetrisServer.Data.Entities;
using TetrisServer.Service;

namespace TetrisServer.Hubs;

public class TetrisHub(MatchService matchService) : Hub
{

    public override async Task OnConnectedAsync()
    {
        HttpContext httpContext = Context.GetHttpContext();
        string name = httpContext.Request.Query["name"];

        await matchService.JoinQueue(Context.ConnectionId, name);

        await base.OnConnectedAsync();
    }

    public async Task Ready()
    {
        Player opponent = await matchService.GetOpponentByPlayerId(Context.ConnectionId);

        await Clients.Client(opponent.Id).SendAsync(nameof(Ready));
    }

    public async Task Drop()
    {
        Player opponent = await matchService.GetOpponentByPlayerId(Context.ConnectionId);

        await Clients.Client(opponent.Id).SendAsync("Drop");
    }

    public async Task RotateLeft()
    {
        Player opponent = await matchService.GetOpponentByPlayerId(Context.ConnectionId);

        await Clients.Client(opponent.Id).SendAsync(nameof(RotateLeft));
    }

    public async Task RotateRight()
    {
        Player opponent = await matchService.GetOpponentByPlayerId(Context.ConnectionId);

        await Clients.Client(opponent.Id).SendAsync(nameof(RotateRight));
    }

    public async Task MoveLeft()
    {
        Player opponent = await matchService.GetOpponentByPlayerId(Context.ConnectionId);

        await Clients.Client(opponent.Id).SendAsync(nameof(MoveLeft));
    }

    public async Task MoveRight()
    {
        Player opponent = await matchService.GetOpponentByPlayerId(Context.ConnectionId);

        await Clients.Client(opponent.Id).SendAsync(nameof(MoveRight));
    }

    public async Task QuitGame()
    {
        Player opponent = await matchService.GetOpponentByPlayerId(Context.ConnectionId);

        await Clients.Client(opponent.Id).SendAsync("Quit");
    }

}
