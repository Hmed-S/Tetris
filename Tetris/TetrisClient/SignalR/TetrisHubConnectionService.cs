using Microsoft.AspNetCore.SignalR.Client;
using TetrisClient.Dto;
using System;
using System.Threading.Tasks;

namespace TetrisClient.SignalR
{
   
    internal class TetrisHubConnectionService
    {
        private HubConnection? _hubConnection;

        internal void MakeConnection()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl("/tetrisHub")
            .Build();
        }

        internal void ConnectOnReady(Action<int> onReady)
        {
            _hubConnection.On<int>("Ready", (seed) => onReady(seed));
        }

        internal void ConnectOnDrop(Action<Game> onDrop)
        {
            _hubConnection.On<Game>("Drop", (game) => onDrop(game));
        }

        internal async Task DropShape(Game game)
        {
            await _hubConnection.SendAsync("DropShape", game);
        }

        internal async Task Ready(int seed)
        {
            await _hubConnection.SendAsync("Ready", seed);
        }

    }
}
