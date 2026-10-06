using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using DomainIPlayer = TetrisEngine.Domain.Game.Player.IPlayer;
using DomainPlayer = TetrisEngine.Domain.Game.Player.Player;
using DomainInterval = TetrisEngine.Domain.Game.Interval;
using DomainIGame = TetrisEngine.Domain.Game.IGame;
using DomainGame = TetrisEngine.Domain.Game.Game;
using DomainGameState = TetrisEngine.Domain.Game.GameState;
using TetrisEngine.Sdk.Mappers;

namespace TetrisEngine.Sdk;

public class Engine
{
    private DomainIGame _game = DomainGame.GetGame();
    public Game Game { get; private set; }

    private void SetGameEvents()
    {
        _game.Player.OnTetrominoPositionChange.Add((previous, current) => Game.Player.CurrentTetromino = current.ToDto());
        _game.Player.OnPreviewChange.Add((preview) => Game.Player.Preview = preview.ToDto());
        _game.Player.OnTetrominoPositionChange.Add((previous, current) => Game.Player.Board.Values = _game.Player.Board.Values);
        

        _game.Player.OnLevelChange.Add((level) => {
            Game.Player.Level = level.Value;
            Game.Player.Interval.Seconds = _game.Player.Interval.Seconds;
            Game.Player.Interval.MiliSeconds = _game.Player.Interval.MiliSeconds;
            });
        _game.Player.OnLineClear.Add((line) => Game.Player.Board.LastClearedLine = line);
        _game.Player.OnLineSwab.Add((bottomLine, topLine) => Game.Player.Board.LastSwappedLines = [bottomLine, topLine]);
        _game.Player.OnScoreChange.Add((score) => Game.Player.Score = score.Value);
        _game.Player.OnLineCountChange.Add((lines) => Game.Player.LineCount = lines.Value);

        _game.OnGameOver.Add(() => { Game.IsOver = true; Game.State = (_game.GameState == DomainGameState.GameOver) ? GameState.GameOver : GameState.Quit; });
    }

    public Game StartSinglePlayerGame(int rowCount, int columnCount)
    {
        int seed = new Random().Next();
        DomainIPlayer player = new DomainPlayer
        (
            seed,
            new Random(seed),
            new TetrisBoard(rowCount, columnCount),
            new Score(),
            new Level(),
            new DomainInterval()
        );

        _game = DomainGame.Start(GameMode.SinglePlayer, player);

        Game = _game.ToDto();
        SetGameEvents();
        
        return Game;
    }

    public void Drop() => _game.Player.DropTetromino();
    public void Next() => _game.Player.Next();
    public void RotateRight() => _game.Player.RotateRight();
    public void RotateLeft() => _game.Player.RotateLeft();
    public void MoveRight() => _game.Player.MoveRight();
    public void MoveLeft() => _game.Player.MoveLeft();
    public void Quit() => _game.Player.Quit();
    public void Ready() => _game.Player.Ready();

}
