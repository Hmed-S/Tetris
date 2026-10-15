namespace TetrisEngine.Sdk;

public class Game
{
   public Player Player { get; set; }

    private Player _opponent;
   public Player Oponent 
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
            OnGameOver.Invoke();
       } }

}
