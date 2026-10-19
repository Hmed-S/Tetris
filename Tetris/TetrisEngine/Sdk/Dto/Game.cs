namespace TetrisEngine.Sdk;

public class Game
{
   public Player Player { get; set; }

    private Player _opponent;
   public Player Opponent 
    {
        get
        {
            return _opponent;
        }
        set
        {
            _opponent = value;
            OnMatchFound();
        }
    }

   public GameState State { get; set; }

   private bool _isOver = false;
   private bool _bothReady = false;
   
   public bool BothReady
    {
        get 
        {
            return _bothReady;
        }
        set
        {
            _bothReady = value;
            OnBothReady.Invoke();
        }
    }

   public event Action OnBothReady;
   public event Action OnGameOver;
   public event Action OnMatchFound;
   public bool IsOver { 
       get 
        {
            return _isOver;
        }
       set
       {
            _isOver = value;
            OnGameOver?.Invoke();
       } }

}
