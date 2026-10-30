using Moq;
using System.ComponentModel;
using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Sdk.Server;
using DomainGame = TetrisEngine.Domain.Game.Game;
namespace TetrisEngine.Sdk.Tests;

public class EngineTest
{
    private readonly Mock<ITetrisServer> _serverMock;
    private readonly Engine _engine;

    public EngineTest()
    {
        IGame game = DomainGame.GetGame();

        game?.Quit(game.Player);

        _serverMock = new Mock<ITetrisServer>();

        _serverMock
            .Setup(server => server.FindMatch(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _serverMock
            .Setup(server => server.NotifyReady())
            .Returns(Task.CompletedTask);

        _serverMock
            .Setup(server => server.NotifyRotateRight())
            .Returns(Task.CompletedTask);

        _serverMock
            .Setup(server => server.NotifyRotateLeft())
            .Returns(Task.CompletedTask);

        _serverMock
            .Setup(server => server.NotifyMoveLeft())
            .Returns(Task.CompletedTask);

        _serverMock
            .Setup(server => server.NotifyMoveRight())
            .Returns(Task.CompletedTask);

        _serverMock
            .Setup(server => server.NotifyQuit())
            .Returns(Task.CompletedTask);

        _serverMock
            .Setup(server => server.NotifyFastDrop())
            .Returns(Task.CompletedTask);

        _engine = new Engine(_serverMock.Object);
    }

    [Fact]
    public void StartSinglePlayerGame_CreatesGameWithCorrectBoardDimensions()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        Assert.NotNull(game);
        Assert.NotNull(game.Player);
        Assert.Equal(20, game.Player.Board.RowCount);
        Assert.Equal(10, game.Player.Board.ColumnCount);
        Assert.Equal(GameState.Playing, game.State);
        Assert.Null(game.Opponent);
        Assert.False(game.IsOver);
    }

    [Fact]
    public void StartSinglePlayerGame_ShouldSetNoName()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        Assert.Null(game.Player.Name);
    }

    [Fact]
    public void StartSinglePlayerGame_InitializesPlayerDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        Assert.NotNull(game.Player.Board);
        Assert.NotNull(game.Player.Interval);
        Assert.NotNull(game.Player.CurrentTetromino);
        Assert.NotNull(game.Player.Preview);
        Assert.Equal(0, game.Player.Score);
        Assert.Equal(1, game.Player.Level);
        Assert.Equal(0, game.Player.LineCount);
    }

    [Fact]
    public void Quit_SetsGameStateToQuit()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        _engine.Quit();

        Assert.Equal(GameState.Quit, game.State);
        Assert.True(game.IsOver);
        Assert.True(game.Player.Quit);
    }

    [Fact]
    public void StartMultiPlayerGame_FindsMatchUsingProvidedPlayerName()
    {
        _serverMock
            .Setup(server => server.FindMatch(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        Task<Game> startTask = _engine.StartMultiPlayerGame(20, 10, "TestPlayer");

        // The matchmaking callback is raised separately in the next test.
        Assert.NotNull(startTask);
    }

    [Fact]
    public async Task StartMultiPlayerGame_CreatesOpponentWhenMatchIsFound()
    {
        _serverMock
            .Setup(server => server.FindMatch(It.IsAny<string>()))
            .Callback<string>(name =>
            {
                _serverMock.Raise(
                    server => server.OnMatchFound += null,
                    "Opponent",
                    12345);
            })
            .Returns(Task.CompletedTask);

        Game game = await _engine.StartMultiPlayerGame(20, 10, "TestPlayer");

        Assert.NotNull(game.Opponent);
        Assert.Equal("Opponent", game.Opponent.Name);
        Assert.Equal(20, game.Opponent.Board.RowCount);
        Assert.Equal(10, game.Opponent.Board.ColumnCount);

        _serverMock.Verify(
            server => server.FindMatch("TestPlayer"),
            Times.Once);
    }

    [Fact]
    public async Task StartMultiPlayerGame_NotifiesServerWhenLocalPlayerIsReady()
    {
        _serverMock
            .Setup(server => server.FindMatch(It.IsAny<string>()))
            .Callback<string>(name =>
            {
                _serverMock.Raise(
                    server => server.OnMatchFound += null,
                    "Opponent",
                    12345);
            })
            .Returns(Task.CompletedTask);

        await _engine.StartMultiPlayerGame(20, 10, "TestPlayer");

        _engine.Ready();

        _serverMock.Verify(
            server => server.NotifyReady(),
            Times.Once);
    }

    [Fact]
    public async Task StartMultiPlayerGame_BothPlayersReady_SetsBothReady()
    {
        _serverMock
            .Setup(server => server.FindMatch(It.IsAny<string>()))
            .Callback<string>(name =>
            {
                _serverMock.Raise(
                    server => server.OnMatchFound += null,
                    "Opponent",
                    12345);
            })
            .Returns(Task.CompletedTask);

        Game game = await _engine.StartMultiPlayerGame(20, 10, "TestPlayer");

        _engine.Ready();

        _serverMock.Raise(server => server.OnReady += null);

        Assert.True(game.BothReady);
        Assert.True(game.Player.IsReady);
        Assert.True(game.Opponent.IsReady);
    }

    [Fact]
    public async Task StartMultiPlayerGame_ServerQuitEvent_QuitsOpponent()
    {
        _serverMock
            .Setup(server => server.FindMatch(It.IsAny<string>()))
            .Callback<string>(name =>
            {
                _serverMock.Raise(
                    server => server.OnMatchFound += null,
                    "Opponent",
                    12345);
            })
            .Returns(Task.CompletedTask);

        Game game = await _engine.StartMultiPlayerGame(20, 10, "TestPlayer");

        _engine.Ready();
        _serverMock.Raise(server => server.OnReady += null);

        _serverMock.Raise(server => server.OnQuit += null);

        Assert.True(game.IsOver);
        Assert.Equal(GameState.Quit, game.State);
        Assert.True(game.Opponent.Quit);
    }

    [Fact]
    public async Task StartMultiPlayerGame_ServerDisconnectEvent_MarksOpponentDisconnected()
    {
        _serverMock
            .Setup(server => server.FindMatch(It.IsAny<string>()))
            .Callback<string>(name =>
            {
                _serverMock.Raise(
                    server => server.OnMatchFound += null,
                    "Opponent",
                    12345);
            })
            .Returns(Task.CompletedTask);

        Game game = await _engine.StartMultiPlayerGame(20, 10, "TestPlayer");

        _engine.Ready();
        _serverMock.Raise(server => server.OnReady += null);

        _serverMock.Raise(server => server.OnDisconnected += null);

        Assert.True(game.Opponent.Disconnected);
        Assert.True(game.IsOver);
        Assert.Equal(GameState.GameOver, game.State);
    }


    [Fact]
    public void MoveLeft_UpdatesCurrentTetrominoDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        int originalX = game.Player.CurrentTetromino.XPosition;
        List<Points> originalPoints = game.Player.CurrentTetromino.Points
            .Select(point => new Points
            {
                Row = point.Row,
                Column = point.Column
            })
            .ToList();

        Tetromino? previousTetromino = null;
        Tetromino? updatedTetromino = null;

        game.Player.OnTetrominoPositionChange += (previous, updated) =>
        {
            previousTetromino = previous;
            updatedTetromino = updated;
        };

        _engine.MoveLeft();

        Assert.NotNull(previousTetromino);
        Assert.NotNull(updatedTetromino);
        Assert.Equal(originalX, previousTetromino.XPosition);
        Assert.Equal(originalX - 1, updatedTetromino.XPosition);

        Assert.Equal(updatedTetromino.XPosition, game.Player.CurrentTetromino.XPosition);
        Assert.Equal(updatedTetromino.YPosition, game.Player.CurrentTetromino.YPosition);

        Assert.NotEqual(
            originalPoints.Select(point => (point.Row, point.Column)),
            game.Player.CurrentTetromino.Points.Select(point => (point.Row, point.Column)));
    }

    [Fact]
    public void RotateRight_UpdatesCurrentTetrominoDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        Tetromino? updatedTetromino = null;

        game.Player.OnTetrominoPositionChange += (previous, updated) =>
        {
            updatedTetromino = updated;
        };

        _engine.RotateRight();

        Assert.NotNull(updatedTetromino);
        Assert.Equal(
            updatedTetromino.XPosition,
            game.Player.CurrentTetromino.XPosition);
        Assert.Equal(
            updatedTetromino.YPosition,
            game.Player.CurrentTetromino.YPosition);

        Assert.Equal(
            updatedTetromino.Shape,
            game.Player.CurrentTetromino.Shape);
        Assert.Equal(
            updatedTetromino.Points.Select(point => (point.Row, point.Column)),
            game.Player.CurrentTetromino.Points.Select(point => (point.Row, point.Column)));
    }

    [Fact]
    public void Drop_UpdatesBoardDtoAndPreviewDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        int[,] originalBoard = (int[,])game.Player.Board.Values.Clone();
        Tetromino originalPreview = game.Player.Preview;

        Tetromino? updatedPreview = null;

        game.Player.OnPreviewChange += preview =>
        {
            updatedPreview = preview;
        };

        for(int i=0; i<21; i++) // Drop untill Next tetromino is triggered.
        {
            _engine.Drop();
        }

        Assert.NotNull(updatedPreview);
        Assert.NotSame(originalPreview, updatedPreview);

        Assert.Equal(20, game.Player.Board.RowCount);
        Assert.Equal(10, game.Player.Board.ColumnCount);

        Assert.False(
            BoardsAreEqual(originalBoard, game.Player.Board.Values));
    }

    private static bool BoardsAreEqual(int[,] first, int[,] second)
    {
        if (first.GetLength(0) != second.GetLength(0) ||
            first.GetLength(1) != second.GetLength(1))
        {
            return false;
        }

        for (int row = 0; row < first.GetLength(0); row++)
        {
            for (int column = 0; column < first.GetLength(1); column++)
            {
                if (first[row, column] != second[row, column])
                {
                    return false;
                }
            }
        }

        return true;
    }

}
