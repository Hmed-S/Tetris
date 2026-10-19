using Microsoft.AspNetCore.SignalR.Client;

namespace TetrisEngine.Sdk.Server;

public class TetrisServerService(HubConnection _hubConnection) : ITetrisServer
{
    
    public event Action<string> OnMatchFound;
    public event Action<int> OnReady;
    public event Action OnDrop;
    public event Action OnRotateRight;
    public event Action OnRotateLeft;
    public event Action OnMoveLeft;
    public event Action OnMoveRight;

    private bool isConnected => _hubConnection.State == HubConnectionState.Connected;

    public async Task FindMatch()
    {
        if (!isConnected)
        {
            await _hubConnection.StartAsync();
        }

        
    }

    public void Listen()
    {
        _hubConnection.On<string>("MatchFound", (name) => OnMatchFound?.Invoke(name));
        _hubConnection.On<int>("Ready", (seed) => OnReady?.Invoke(seed));
        _hubConnection.On("RotateRight", () => OnRotateRight?.Invoke());
        _hubConnection.On("RotateLeft", () => OnRotateLeft?.Invoke());
        _hubConnection.On("MoveLeft", () => OnMoveLeft?.Invoke());
        _hubConnection.On("MoveRight", () => OnMoveRight?.Invoke());
    }

    public async Task NotifyReady(int seed) => await _hubConnection.SendAsync("ReadyUp", seed);
    public async Task NotifyDrop() => await _hubConnection.SendAsync("Drop");
    public async Task NotifyRotateRight() => await _hubConnection.SendAsync("RotateRight");
    public async Task NotifyRotateLeft() => await _hubConnection.SendAsync("RotateLeft");
    public async Task NotifyMoveLeft() => await _hubConnection.SendAsync("MoveLeft");
    public async Task NotifyMoveRight() => await _hubConnection.SendAsync("MoveRight");

}
