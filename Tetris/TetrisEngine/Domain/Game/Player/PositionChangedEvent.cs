using TetrisEngine.Domain.Board;

namespace TetrisEngine.Domain.Game.Player;

public readonly struct PositionChangedEvent(Tetromino _previousTetromino, Tetromino _currentTetromino, Move _move)
{
    public Tetromino PreviousTetromino { get => _previousTetromino; init; }
    public Tetromino CurrentTetromino { get => _currentTetromino; init; }
    public Move Move { get => _move; init; }

}
