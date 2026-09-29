namespace TetrisEngine.Sdk;

public class Game
{
   public Player Player { get; set; }
   public Player Oponent { get; set; }

   public GameState State { get; set; }

   private bool _isOver = false;
   public event Action OnGameOver;
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
