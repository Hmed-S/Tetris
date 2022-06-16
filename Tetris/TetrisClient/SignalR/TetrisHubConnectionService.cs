using Microsoft.AspNetCore.SignalR.Client;
using TetrisClient.Dto;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TetrisClient.SignalR
{
   
    internal class TetrisHubConnectionService
    {
        private HubConnection? _hubConnection;

        internal async Task MakeConnection()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/tetrisHub")
            .Build();
        }

        internal async Task StartConnection()
        {
            await _hubConnection.StartAsync();
        }


        internal async Task CloseConnection()
        {
            await _hubConnection.StopAsync();
        }

        internal void ConnectOnReady(Action onReady)
        {
            _hubConnection.On("Ready", () => onReady());
        }

        internal async Task ConnectOnDrop(Action<Game> onDrop)
        {
            _hubConnection.On<string>("Drop", (game) => {
                Game deserializedGame = JsonConvert.DeserializeObject<Game>(game);
                onDrop(deserializedGame);
             });
        }

        internal async void ConnectQuit(Action onQuit)
        {
            _hubConnection.On("Quit", () => onQuit());
        }

        internal async Task DropShape(Game game)
        {
            await _hubConnection.SendAsync("DropShape", 
                JsonConvert.SerializeObject(game));
        }

        internal async Task Ready()
        {
            await _hubConnection.SendAsync("ReadyUp");
        }

        internal async Task Quit()
        {
            await _hubConnection.SendAsync("QuitGame");
        }

    }
}
