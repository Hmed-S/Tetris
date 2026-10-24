using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TetrisServer.Data.Entities;
using TetrisServer.Hubs;
using TetrisServer.Model;
using TetrisServer.Service;

namespace TetrisServer.Consumers;

public class RedisPlayerConsumer(MatchService matchservice, IHubContext<TetrisHub> tetrisHub, ILogger<RedisPlayerConsumer> _logger) : Controller, ICapSubscribe
{

    private async Task NotifyMatch(Player player1, Player player2, int seed)
    {
        await tetrisHub.Clients.Client(player1.Id).SendAsync("MatchFound", player2.Name, seed);
        await tetrisHub.Clients.Client(player2.Id).SendAsync("MatchFound", player1.Name, seed);
    }

    [CapSubscribe("Queue")]
    public async Task OnPlayerJoined()
    {
        await matchservice.MakeMatch();
    }

    [CapSubscribe("MatchMade")]
    public async Task OnMatchMade(MatchEvent match)
    {
        await NotifyMatch(match.Player1, match.Player2, match.seed);
    }

    [CapSubscribe("PlayerDisconnected")]
    public async void OnPlayerDisconnected(string opponent)
    {
        await tetrisHub.Clients.Client(opponent).SendAsync("OpponentDisconnected");
    }

}
