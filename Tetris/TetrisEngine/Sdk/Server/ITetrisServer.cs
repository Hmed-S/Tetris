namespace TetrisEngine.Sdk.Server;

public interface ITetrisServer
{
    
    public event Action<string> OnMatchFound;
    public event Action<int> OnReady;
    public event Action OnDrop;
    public event Action OnRotateRight;
    public event Action OnRotateLeft;
    public event Action OnMoveLeft;
    public event Action OnMoveRight;

    public Task FindMatch(string name);
    public Task NotifyReady(int seed);
    public Task NotifyDrop();
    public Task NotifyRotateRight();
    public Task NotifyRotateLeft();
    public Task NotifyMoveLeft();
    public Task NotifyMoveRight();

}
