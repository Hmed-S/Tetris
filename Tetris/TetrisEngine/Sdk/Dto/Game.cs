namespace TetrisEngine.Sdk;

public class Game
{
   public Player Player { get; set; }
   public Player Opponent 
    {
        get
        {
            return field;
        }
        set
        {
            field = value;
            OnMatchFound?.Invoke();
        }
    }

   public GameState State { get; set; }
   
   public bool BothReady
    {
        get 
        {
            return field;
        }
        set
        {
            field = value;
            OnBothReady?.Invoke();
        }
    }

   public event Action OnBothReady;
   public event Action OnGameOver;
   public event Action OnMatchFound;
   public bool IsOver { 
       get 
        {
            return field;
        }
       set
       {
            field = value;
            OnGameOver?.Invoke();
       } 
    }

}
