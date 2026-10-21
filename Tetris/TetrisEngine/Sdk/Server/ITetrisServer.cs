namespace TetrisEngine.Sdk.Server;

public interface ITetrisServer
{
    
    public event Action<string, int> OnMatchFound;
    public event Action OnReady;
    public event Action OnDrop;
    public event Action OnRotateRight;
    public event Action OnRotateLeft;
    public event Action OnMoveLeft;
    public event Action OnMoveRight;

    public void Listen();
    public Task FindMatch();
    public Task NotifyReady();
    public Task NotifyDrop();
    public Task NotifyRotateRight();
    public Task NotifyRotateLeft();
    public Task NotifyMoveLeft();
    public Task NotifyMoveRight();

}
