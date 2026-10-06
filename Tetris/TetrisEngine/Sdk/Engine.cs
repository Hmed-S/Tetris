using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using DomainIPlayer = TetrisEngine.Domain.Game.Player.IPlayer;
using DomainPlayer = TetrisEngine.Domain.Game.Player.Player;
using DomainInterval = TetrisEngine.Domain.Game.Interval;
using DomainIGame = TetrisEngine.Domain.Game.IGame;
using DomainGame = TetrisEngine.Domain.Game.Game;
using DomainGameState = TetrisEngine.Domain.Game.GameState;
using DomainTetromino = TetrisEngine.Domain.Board.Tetromino;
using DomainColor = TetrisEngine.Domain.Board.Color;

namespace TetrisEngine.Sdk;

public class Engine
{
    private DomainIGame _game = DomainGame.GetGame();
    public Game Game { get; private set; }

    // TODO: Make a dedicated mapper class, or similar. 
    private Color ConvertColor(DomainColor domainColor) => domainColor switch
    {
        DomainColor.Blue => Color.Blue,
        DomainColor.Purple => Color.Purple,
        DomainColor.Yellow => Color.Yellow,
        DomainColor.RED => Color.RED,
        DomainColor.Green => Color.Green,
        DomainColor.Cyan => Color.Cyan,
        _ => Color.Orange
    };

    private Tetromino ToDto(DomainTetromino domainTetromino)
    {
        return new()
        {
            XPosition = domainTetromino.XPosition,
            YPosition = domainTetromino.YPosition,
            Color = ConvertColor(domainTetromino.Color),
            Islanded = domainTetromino.DropStatus == DropStatus.Landed,
            Shape = domainTetromino.Shape.Value,
            Points = domainTetromino.Points.Select(point => new Points { Column = point.Column, Row = point.Row}).ToList(),
        };
    }

    private void SetGameEvents()
    {
        _game.Player.OnTetrominoPositionChange.Add((previous, current) => Game.Player.CurrentTetromino = ToDto(current));
        _game.Player.OnPreviewChange.Add((preview) => Game.Player.Preview = ToDto(preview));
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

    private void InitialiseGame()
    {
        Game = new Game
        {
            Player = new Player
            {
                Preview = ToDto(_game.Player.Preview),
                CurrentTetromino = ToDto(_game.Player.CurrentTetromino),
                Board = new Board
                {
                    ColumnCount = _game.Player.Board.Width,
                    RowCount = _game.Player.Board.Height,
                },
                Interval = new Interval
                {
                    FastDropSeconds = _game.Player.Interval.FastDrop.Seconds,
                    FastDropMiliSeconds = _game.Player.Interval.FastDrop.MiliSeconds,
                    Seconds = _game.Player.Interval.Seconds,
                    MiliSeconds = _game.Player.Interval.MiliSeconds,
                },
                Score = _game.Player.Score.Value,
                Level = _game.Player.Level.Value,
                LineCount = _game.Player.Lines.Value,
            },
            State = GameState.Playing,
        };

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

        InitialiseGame();
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
