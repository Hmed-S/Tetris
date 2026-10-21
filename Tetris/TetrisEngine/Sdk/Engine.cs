using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using DomainIPlayer = TetrisEngine.Domain.Game.Player.IPlayer;
using DomainPlayer = TetrisEngine.Domain.Game.Player.Player;
using DomainInterval = TetrisEngine.Domain.Game.Interval;
using DomainIGame = TetrisEngine.Domain.Game.IGame;
using DomainGame = TetrisEngine.Domain.Game.Game;
using DomainGameState = TetrisEngine.Domain.Game.GameState;
using TetrisEngine.Sdk.Mappers;
using TetrisEngine.Sdk.Server;
using Microsoft.AspNetCore.SignalR.Client;
using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine.Sdk;

public class Engine
{
    private DomainIGame _game = DomainGame.GetGame();
    public ITetrisServer Server { get; set; }
    public Game Game { get; private set; }

    
    private void SetPlayerEvents(DomainIPlayer player, Player dto)
    {

        player.OnTetrominoPositionChange.Add((position) => dto.CurrentTetromino = position.CurrentTetromino.ToDto());
        player.OnPreviewChange.Add((preview) => dto.Preview = preview.ToDto());
        player.OnTetrominoPositionChange.Add((position) => dto.Board.Values = _game.Player.Board.Values);
        player.OnReady.Add(() => dto.IsReady = true);

        player.OnLevelChange.Add((level) => {
            dto.Level = level.Value;
            dto.Interval.Seconds = player.Interval.Seconds;
            dto.Interval.MiliSeconds = player.Interval.MiliSeconds;
        });
        player.OnLineClear.Add((line) => dto.Board.LastClearedLine = line);
        player.OnLineSwab.Add((bottomLine, topLine) => dto.Board.LastSwappedLines = [bottomLine, topLine]);
        player.OnScoreChange.Add((score) => dto.Score = score.Value);
        player.OnLineCountChange.Add((lines) => dto.LineCount = lines.Value);

    }
    
    private void SetGameEvents()
    {
        _game.OnGameOver.Add(() => { Game.IsOver = true; Game.State = (_game.GameState == DomainGameState.GameOver) ? GameState.GameOver : GameState.Quit; });
        _game.OnBothReady.Add(()=> { Game.BothReady = true; });
    }

    private void DoStartGame(int rowCount, int columnCount, string name = null, GameMode gameMode = GameMode.SinglePlayer)
    {
        int seed = new Random().Next();

        // TODO: Make factory, or similar.
        DomainIPlayer player = new DomainPlayer
        (
            seed,
            new Random(seed),
            new TetrisBoard(rowCount, columnCount),
            new Score(),
            new Level(),
            new DomainInterval()
        );

        if ( gameMode == GameMode.MultiPlayer)
        {
            player = new MultiPlayerPlayer
                (
                   new Name(name),
                   new TetrisBoard(rowCount, columnCount),
                   new Score(),
                   new Level(),
                   new DomainInterval()
                );

        }

        _game = DomainGame.Start(gameMode, player);

        Game = _game.ToDto();
        SetGameEvents();
        SetPlayerEvents(player, Game.Player);
    }

    public Game StartSinglePlayerGame(int rowCount, int columnCount)
    {
        DoStartGame(rowCount, columnCount);

        return Game;
    }

    private void SetServerEvents()
    {
        Server.OnReady += _game.Opponent.Ready;

        Server.OnRotateRight += _game.Opponent.RotateRight;
        Server.OnRotateLeft += _game.Opponent.RotateLeft;
        Server.OnMoveLeft += _game.Opponent.MoveLeft;
        Server.OnMoveRight += _game.Opponent.MoveRight;
    }

    private void InitServer()
    {
        IHubConnectionBuilder hubConnectionBuilder = new HubConnectionBuilder()
             .WithUrl($"http://localhost:5000/tetrisHub?name={_game.Player.Name.Value}");

        HubConnection hubConnection = hubConnectionBuilder.Build();

        Server = new TetrisServerService(hubConnection);

    }

    private void SetOpponent(string name, int seed)
    {
        DomainIPlayer player = new MultiPlayerPlayer
        (
            new Name(name),
            new TetrisBoard(_game.Player.Board.Height, _game.Player.Board.Width),
            new Score(),
            new Level(),
            new DomainInterval()
        )
        {
            Seed = seed,
            Random = new(seed)
        };

        _game.Player.Seed = seed;
        _game.Player.Random = new(seed);

        _game.Opponent = player;

        _game.OnBothReady.Insert(0, () =>
        {
            _game.Player.Init();
            _game.Opponent.Init();
        });

        Game.Opponent = new Player // todo, make player mapper
        {
            Name = _game.Opponent.Name.Value,
            Preview = _game.Opponent.Preview.ToDto(),
            CurrentTetromino = _game.Opponent.CurrentTetromino.ToDto(),
            Board = new Board
            {
                ColumnCount = _game.Opponent.Board.Width,
                RowCount = _game.Opponent.Board.Height,
            },
            Interval = new Interval
            {
                FastDropSeconds = _game.Opponent.Interval.FastDrop.Seconds,
                FastDropMiliSeconds = _game.Opponent.Interval.FastDrop.MiliSeconds,
                Seconds = _game.Opponent.Interval.Seconds,
                MiliSeconds = _game.Opponent.Interval.MiliSeconds,
            },
            Score = _game.Opponent.Score.Value,
            Level = _game.Opponent.Level.Value,
            LineCount = _game.Opponent.Lines.Value,
        };

        SetGameEvents();
        SetPlayerEvents(_game.Opponent, Game.Opponent);
        SetServerEvents();

    }

    private void NotifyServer()
    {
        _game.Player.OnTetrominoPositionChange.Insert(0, (position)=>
        {
            switch (position.Move) 
            {
                case Move.MoveLeft:
                    Server.NotifyMoveLeft();
                    break;
                case Move.MoveRight:
                    Server.NotifyMoveRight();
                    break;
                case Move.RotateLeft: 
                    Server.NotifyRotateLeft();
                    break;
                case Move.RotateRight:
                    Server.NotifyRotateRight();
                    break;
            }
        });

        _game.Player.OnReady.Insert(0, () => Server.NotifyReady());

    }

    public async Task<Game> StartMultiPlayerGame(int rowCount, int columnCount, string name)
    {
        DoStartGame(rowCount, columnCount, name, GameMode.MultiPlayer);

        InitServer();

        NotifyServer();

        Server.OnMatchFound += SetOpponent;

        Server.Listen();
        await Server.FindMatch();

        return Game;
    }

    public void Drop()
    {
        _game.Player.DropTetromino();
    }

    public void OpponentDrop()
    {
        _game.Opponent.DropTetromino();
    }

    public void RotateRight() => _game.Player.RotateRight();
    public void RotateLeft() => _game.Player.RotateLeft();
    public void MoveRight() => _game.Player.MoveRight();
    public void MoveLeft() => _game.Player.MoveLeft();
    public void Quit() => _game.Player.Quit();
    public void Ready() => _game.Player.Ready();

}
