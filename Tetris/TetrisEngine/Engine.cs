using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine;

public class Engine
{
    private readonly TetrisBoard _board;
    public int[,] Board { get => _board.Values; }
    public virtual Tetromino CurrentTetromino {get; private set;}
    public Tetromino Preview { get; private set; }
    public int Lines { get=> _board.Lines; }
    
    public int Score { get; private set; }
    
    public IGame StartSinglePlayerGame(string initials)
    {
        Player player = new Player
        {
            Name = initials,
            Board = new TetrisBoard(10, 10),
        };

        IGame game = Game.Start(GameMode.SinglePlayer, player);

        return game;
    }

}