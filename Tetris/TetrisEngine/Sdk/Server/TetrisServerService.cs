using Microsoft.AspNetCore.SignalR.Client;

namespace TetrisEngine.Sdk.Server;

public class TetrisServerService(string url) : ITetrisServer
{
    private HubConnection _hubConnection;
    public event Action<string, int> OnMatchFound;
    public event Action OnReady;
    public event Action OnDrop;
    public event Action OnRotateRight;
    public event Action OnRotateLeft;
    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action OnDisconnected;
    public event Action OnQuit;
    public event Action OnFastDroppedToggled;

    public async Task FindMatch(string name)
    {
        _hubConnection =
            new HubConnectionBuilder()
            .WithUrl($"{url}?name={name}")
            .Build();

        await _hubConnection.StartAsync();

        _hubConnection.On<string, int>("MatchFound", (name, seed) => OnMatchFound?.Invoke(name, seed));
        _hubConnection.On("Ready", () => OnReady?.Invoke());
        _hubConnection.On("RotateRight", () => OnRotateRight?.Invoke());
        _hubConnection.On("RotateLeft", () => OnRotateLeft?.Invoke());
        _hubConnection.On("MoveLeft", () => OnMoveLeft?.Invoke());
        _hubConnection.On("MoveRight", () => OnMoveRight?.Invoke());
        _hubConnection.On("ToggleFastDrop", () => OnFastDroppedToggled?.Invoke());
        _hubConnection.On("OpponentDisconnected", () => OnDisconnected.Invoke());
        _hubConnection.On("Quit", () => OnQuit.Invoke());
        _hubConnection.On("Quit", async () => await _hubConnection.StopAsync());

    }

    public async Task NotifyReady() => await _hubConnection.SendAsync("Ready");
    public async Task NotifyRotateRight() => await _hubConnection.SendAsync("RotateRight");
    public async Task NotifyRotateLeft() => await _hubConnection.SendAsync("RotateLeft");
    public async Task NotifyMoveLeft() => await _hubConnection.SendAsync("MoveLeft");
    public async Task NotifyMoveRight() => await _hubConnection.SendAsync("MoveRight");
    public async Task NotifyQuit() => await _hubConnection.SendAsync("QuitGame");
    public async Task NotifyFastDrop() => await _hubConnection.SendAsync("ToggleFastDrop");

}
