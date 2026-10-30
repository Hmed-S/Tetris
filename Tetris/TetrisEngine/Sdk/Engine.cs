using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using DomainIPlayer = TetrisEngine.Domain.Game.Player.IPlayer;
using DomainInterval = TetrisEngine.Domain.Game.Interval;
using DomainIGame = TetrisEngine.Domain.Game.IGame;
using DomainGame = TetrisEngine.Domain.Game.Game;
using DomainGameState = TetrisEngine.Domain.Game.GameState;
using TetrisEngine.Sdk.Mappers;
using TetrisEngine.Sdk.Server;
using TetrisEngine.Domain.Game.Player;
using TetrisEngine.Domain.Game.Player.Factory;

namespace TetrisEngine.Sdk;

public class Engine
{
    private DomainIGame _game = DomainGame.GetGame();
    public Game Game { get; private set; }
    private ITetrisServer Server { get; }

    public Engine(ITetrisServer tetrisServer)
    {
        Server = tetrisServer;
    }

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
        _game.OnGameOver.Add((player) =>
        {
            Game.State = (_game.GameState == DomainGameState.GameOver) ? GameState.GameOver : GameState.Quit;
            
            Game.Player.Quit = player.Id == _game.Player.Id && _game.GameState == DomainGameState.Quit;


            if (_game.Mode == GameMode.MultiPlayer && _game.Opponent != null)
            {
                Game.Opponent.Quit = player.Id == _game.Opponent.Id && _game.GameState == DomainGameState.Quit;
                
                if(_game.GameState == DomainGameState.GameOver)
                {
                    Game.Opponent.Won = _game.Winner.Id == _game.Opponent.Id;
                    Game.Player.Won = _game.Winner.Id == _game.Player.Id;
                }

            }
            Game.IsOver = true;
        });
        _game.OnBothReady.Add(()=> { Game.BothReady = true; });
    }

    private void DoStartGame(int rowCount, int columnCount, string name = "JohnDoe", GameMode gameMode = GameMode.SinglePlayer)
    {
        int seed = new Random().Next();

        // TODO: Make factory, or similar.
        IPlayerFactory factory = new PlayerFactory();

        DomainIPlayer player = factory.CreatePlayer(
            gameMode, seed, new Name(name),
            new TetrisBoard(rowCount, columnCount),
            new Score(), new Level(),
            new DomainInterval());

        _game = DomainGame.Start(gameMode, player);

        Game = _game.ToDto();
        SetGameEvents();
        SetPlayerEvents(player, Game.Player);
    }

    private void SetServerEvents()
    {
        Server.OnReady += _game.Opponent.Ready;
        Server.OnRotateRight += _game.Opponent.RotateRight;
        Server.OnRotateLeft += _game.Opponent.RotateLeft;
        Server.OnMoveLeft += _game.Opponent.MoveLeft;
        Server.OnMoveRight += _game.Opponent.MoveRight;
        Server.OnQuit += _game.Opponent.Quit;
        Server.OnDisconnected += () =>_game.Over(_game.Opponent);
        Server.OnDisconnected += () => Game.Opponent.Disconnected = true;
    }

    private void SetOpponent(string name, int seed)
    {
        IPlayerFactory factory = new PlayerFactory();

        DomainIPlayer player = factory.CreatePlayer(
            _game.Mode, seed, new Name(name),
            new TetrisBoard(_game.Player.Board.Height, _game.Player.Board.Width),
            new Score(), new Level(),
            new DomainInterval());

        player.Seed = seed;
        player.Random = new Random(seed);

        _game.Player.Seed = seed;
        _game.Player.Random = new Random(seed);

        _game.Opponent = player;

        _game.OnBothReady.Insert(0, () =>
        {
            _game.Player.Init();
            _game.Opponent.Init();
        });

        Game.Opponent = _game.Opponent.ToDto();

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

        _game.Player.OnReady.Insert(0, ()=> Server.NotifyReady());

        _game.OnGameOver.Insert(0, (player) =>
        {
            if (player.Id == _game.Player.Id && _game.GameState == DomainGameState.Quit) Server.NotifyQuit();  
        });
    }

    public Game StartSinglePlayerGame(int rowCount, int columnCount)
    {
        DoStartGame(rowCount, columnCount);

        return Game;
    }

    public async Task<Game> StartMultiPlayerGame(int rowCount, int columnCount, string name)
    {
        DoStartGame(rowCount, columnCount, name, GameMode.MultiPlayer);
        NotifyServer();

        Server.OnMatchFound += SetOpponent;
        await Server.FindMatch(_game.Player.Name.Value);

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
