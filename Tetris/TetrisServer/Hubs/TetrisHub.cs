using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using TetrisServer.Data;
using TetrisServer.Data.Entities;
using TetrisServer.Producers;

namespace TetrisServer.Hubs;

public class TetrisHub
    (
     IPlayerProducer playerProducer,
     IPlayerRepository playerRepository
    ) : Hub
{

    public override Task OnConnectedAsync()
    {
        HttpContext httpContext = Context.GetHttpContext();
        string name = httpContext.Request.Query["name"];

        Player player = new()
        {
            Id = Context.ConnectionId,
            Name = name,
        };

        playerProducer.Enqueue(player);

        return base.OnConnectedAsync();
    }

    public async Task ReadyUp(int seed)
    {
        Player player = await playerRepository.GetPlayerById(Context.ConnectionId);

        await Clients.Client(player.Opponent.Id).SendAsync("Ready", seed);
    }

    public async Task Drop()
    {
        Console.WriteLine(Context.ConnectionId);
        Player player = await playerRepository.GetPlayerById(Context.ConnectionId);

        await Clients.Client(player.Opponent.Id).SendAsync("Drop");
    }

    public async Task RotateLeft()
    {
        Player player = await playerRepository.GetPlayerById(Context.ConnectionId);

        await Clients.Client(player.Opponent.Id).SendAsync(nameof(RotateLeft));
    }

    public async Task RotateRight()
    {
        Player player = await playerRepository.GetPlayerById(Context.ConnectionId);

        await Clients.Client(player.Opponent.Id).SendAsync(nameof(RotateRight));
    }

    public async Task MoveLeft()
    {
        Player player = await playerRepository.GetPlayerById(Context.ConnectionId);

        await Clients.Client(player.Opponent.Id).SendAsync(nameof(MoveLeft));
    }

    public async Task MoveRight()
    {
        Player player = await playerRepository.GetPlayerById(Context.ConnectionId);

        await Clients.Client(player.Opponent.Id).SendAsync(nameof(MoveRight));
    }

    public async Task QuitGame()
    {
        Player player = await playerRepository.GetPlayerById(Context.ConnectionId);

        await Clients.Client(player.Opponent.Id).SendAsync("Quit");
    }

}
