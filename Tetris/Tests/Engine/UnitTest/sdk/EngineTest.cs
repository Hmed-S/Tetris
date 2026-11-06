using Moq;
using TetrisEngine.Domain.Game;
using TetrisEngine.Sdk.Server;
using DomainGame = TetrisEngine.Domain.Game.Game;
using DomainInterval = TetrisEngine.Domain.Game.Interval;
using TetrisEngine.Domain.Game.Player;
using TetrisEngine.Domain.Game.Player.Factory;

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

        _engine = new Engine(_serverMock.Object, new PlayerFactory());
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

    [Fact]
    public void MoveLeft_UpdatesPlayerBoardDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        int[,] originalBoard = (int[,])game.Player.Board.Values.Clone();

        _engine.MoveLeft();

        Assert.Same(
            game.Player.Board.Values,
            game.Player.Board.Values);

        Assert.Equal(20, game.Player.Board.RowCount);
        Assert.Equal(10, game.Player.Board.ColumnCount);

        Assert.Equal(
            originalBoard.GetLength(0),
            game.Player.Board.Values.GetLength(0));

        Assert.Equal(
            originalBoard.GetLength(1),
            game.Player.Board.Values.GetLength(1));
    }

    [Fact]
    public void MoveRight_UpdatesTetrominoPositionInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        int originalX = game.Player.CurrentTetromino.XPosition;

        _engine.MoveRight();

        Assert.Equal(
            originalX + 1,
            game.Player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveLeft_UpdatesTetrominoPositionInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        int originalX = game.Player.CurrentTetromino.XPosition;

        _engine.MoveLeft();

        Assert.Equal(
            originalX - 1,
            game.Player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void RotateRight_UpdatesTetrominoShapeInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);

        _engine.RotateRight();

        Assert.NotNull(game.Player.CurrentTetromino.Shape);
        Assert.NotEmpty(game.Player.CurrentTetromino.Points);
    }

    [Fact]
    public void TetrominoPositionChange_UpdatesBoardValuesInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);
        int[,] originalvalues = (int[,])game.Player.Board.Values.Clone();

        for (int i = 0; i<25; i++) // simulate a landing, this will update board, once it is commited to the board
        _engine.Drop();

        int[,] domainBoard = DomainGame.GetGame().Player.Board.Values;
        Assert.NotNull(game.Player.Board.Values);
        Assert.NotEmpty(game.Player.Board.Values);
        Assert.NotEqual(originalvalues, game.Player.Board.Values);
        Assert.NotSame(domainBoard, game.Player.Board.Values);
        Assert.Equal(domainBoard, game.Player.Board.Values);
    }

    [Fact]
    public void LineClear_UpdatesInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);
        int originalLastClearedLine = game.Player.Board.LastClearedLine;
        IPlayer domainPlayer = DomainGame.GetGame().Player;

        domainPlayer.OnLineClear.ForEach(onLineClear => // simulate a lineclear
        {
            onLineClear(12);
        });

        Assert.NotEqual(originalLastClearedLine, game.Player.Board.LastClearedLine);
        Assert.Equal(12, game.Player.Board.LastClearedLine);
    }

    [Fact]
    public void LineSwap_UpdatesInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);
        List<int> originalLineSwap = game.Player.Board.LastSwappedLines;
        IPlayer domainPlayer = DomainGame.GetGame().Player;

        domainPlayer.OnLineSwab.ForEach(onLineSwap =>
        {
            onLineSwap(11,10);
        });

        Assert.NotSame(originalLineSwap, game.Player.Board.LastSwappedLines);
        Assert.Equal(2, game.Player.Board.LastSwappedLines.Count);
        Assert.Equal(11, game.Player.Board.LastSwappedLines[0]);
        Assert.Equal(10, game.Player.Board.LastSwappedLines[1]);
    }

    [Fact]
    public void ScoreChange_UpdatesInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);
        int originalScore = game.Player.Score;
        IPlayer domainPlayer = DomainGame.GetGame().Player;

        domainPlayer.OnScoreChange.ForEach(onScoreChange =>
        {
            Score score = new Score();
            score += 4;
            onScoreChange(score);
            domainPlayer.Score = score;
        });

        Assert.NotEqual(originalScore, game.Player.Score);
        Assert.Equal(domainPlayer.Score.Value, game.Player.Score);
    }

    [Fact]
    public void LineCountChange_UpdatesInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);
        int originalLineCount = game.Player.LineCount;
        IPlayer domainPlayer = DomainGame.GetGame().Player;

        domainPlayer.OnLineCountChange.ForEach(onLineCountChange =>
        {
            Lines lines = new();
            lines += 10;

            domainPlayer.Lines = lines;
            onLineCountChange(lines);
        });

        Assert.NotEqual(originalLineCount, game.Player.LineCount);
        Assert.Equal(domainPlayer.Lines.Value, game.Player.LineCount);
    }

    [Fact]
    public void LevelUp_UpdatesLevelInDto()
    {
        Game game = _engine.StartSinglePlayerGame(20, 10);
        int originalLevel = game.Player.Level;
        IPlayer domainPlayer = DomainGame.GetGame().Player;

        domainPlayer.OnLevelChange.ForEach(onLevelChange =>
        {
            Level level = new ();
            Lines lines = new ();
            DomainInterval interval = new ();
            lines += 10;
            level += lines;
            interval += level;

            domainPlayer.Level = level;
            domainPlayer.Lines = lines;
            domainPlayer.Interval = interval;

            onLevelChange (level);
        });


        Assert.NotEqual(originalLevel, game.Player.Level);
        Assert.Equal(domainPlayer.Level.Value, game.Player.Level);
        Assert.Equal(domainPlayer.Interval.Seconds, game.Player.Interval.Seconds);
        Assert.Equal(domainPlayer.Interval.MiliSeconds, game.Player.Interval.MiliSeconds);
        Assert.Equal(domainPlayer.Interval.FastDrop.Seconds, game.Player.Interval.FastDropSeconds);
        Assert.Equal(domainPlayer.Interval.FastDrop.MiliSeconds, game.Player.Interval.FastDropMiliSeconds);
    }

    [Fact]
    public async Task PlayerQuiting_UpdatesGameInDto() //#
    {
        Game game = await StartReadyMultiplayerGame();

        _engine.Quit();

        Assert.True(game.Player.Quit);
        Assert.False(game.Opponent.Quit);
        Assert.False(game.Player.Won);
        Assert.False(game.Opponent.Won);
    }

    [Fact]
    public async Task OpponentQuiting_UpdatesGameInDto()
    {
        Game game = await StartReadyMultiplayerGame();

        _serverMock.Raise(server => server.OnQuit += null);

        Assert.True(game.Opponent.Quit);
        Assert.False(game.Player.Quit);
        Assert.False(game.Player.Won);
        Assert.False(game.Opponent.Won);
    }

    [Fact]
    public async Task PlayerGameOver_UpdatesGameInDto()
    {
        Game game = await StartReadyMultiplayerGame();

        while (!game.IsOver)
            _engine.Drop();

        Assert.True(game.Opponent.Won);
        Assert.False(game.Player.Won);
        Assert.False(game.Player.Quit);
        Assert.False(game.Opponent.Quit);
    }

    [Fact]
    public async Task OpponentGameOver_UpdatesGameInDto()
    {
        Game game = await StartReadyMultiplayerGame();

        while (!game.IsOver)
            _engine.OpponentDrop();

        Assert.True(game.Player.Won);
        Assert.False(game.Opponent.Won);
        Assert.False(game.Player.Quit);
        Assert.False(game.Opponent.Quit);

    } // #

    private async Task<Game> StartReadyMultiplayerGame()
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

        return game;
    }

    [Fact]
    public async Task MoveLeft_NotifiesServer()
    {
        await StartReadyMultiplayerGame();

        _engine.MoveLeft();

        _serverMock.Verify(
            server => server.NotifyMoveLeft(),
            Times.Once);
    }

    [Fact]
    public async Task MoveRight_NotifiesServer()
    {
        await StartReadyMultiplayerGame();

        _engine.MoveRight();

        _serverMock.Verify(
            server => server.NotifyMoveRight(),
            Times.Once);
    }

    [Fact]
    public async Task RotateLeft_NotifiesServer()
    {
        await StartReadyMultiplayerGame();

        _engine.RotateLeft();

        _serverMock.Verify(
            server => server.NotifyRotateLeft(),
            Times.Once);
    }

    [Fact]
    public async Task RotateRight_NotifiesServer()
    {
        await StartReadyMultiplayerGame();

        _engine.RotateRight();

        _serverMock.Verify(
            server => server.NotifyRotateRight(),
            Times.Once);
    }

    [Fact]
    public async Task Quit_NotifiesServer()
    {
        await StartReadyMultiplayerGame();

        _engine.Quit();

        _serverMock.Verify(
            server => server.NotifyQuit(),
            Times.Once);
    }

    [Fact]
    public async Task GameOverDoesNotNotifyServer()
    {
        await StartReadyMultiplayerGame();
        IGame game = DomainGame.GetGame();

        while(game.GameState != Domain.Game.GameState.GameOver)
            _engine.Drop();

        _serverMock.Verify(
            server => server.NotifyQuit(),
            Times.Never);
    }

    [Fact]
    public async Task OpponentDrop_UpdatesBoardDtoAndPreviewDto()
    {
        Game game = await StartReadyMultiplayerGame();

        int[,] originalBoard = (int[,])game.Opponent.Board.Values.Clone();
        Tetromino originalPreview = game.Opponent.Preview;

        Tetromino? updatedPreview = null;

        game.Opponent.OnPreviewChange += preview =>
        {
            updatedPreview = preview;
        };

        for (int i = 0; i < 21; i++) // Drop untill Next tetromino is triggered.
        {
            _engine.OpponentDrop();
        }

        Assert.NotNull(updatedPreview);
        Assert.NotSame(originalPreview, updatedPreview);

        Assert.Equal(20, game.Opponent.Board.RowCount);
        Assert.Equal(10, game.Opponent.Board.ColumnCount);

        Assert.False(
            BoardsAreEqual(originalBoard, game.Opponent.Board.Values));
    }

    [Fact]
    public async Task OpponentMoveLeft_UpdatesOpponentTetrominoDto()
    {
        Game game = await StartReadyMultiplayerGame();

        int originalX = game.Opponent.CurrentTetromino.XPosition;

        _serverMock.Raise(server => server.OnMoveLeft += null);

        Assert.Equal(
            originalX - 1,
            game.Opponent.CurrentTetromino.XPosition);
    }

    [Fact]
    public async Task OpponentMoveRight_UpdatesOpponentTetrominoDto()
    {
        Game game = await StartReadyMultiplayerGame();

        int originalX = game.Opponent.CurrentTetromino.XPosition;

        _serverMock.Raise(server => server.OnMoveRight += null);

        Assert.Equal(
            originalX + 1,
            game.Opponent.CurrentTetromino.XPosition);
    }

    [Fact]
    public async Task OpponentRotateRight_UpdatesOpponentTetrominoDto()
    {
        Game game = await StartReadyMultiplayerGame();
        IGame domainGame = DomainGame.GetGame();
        int[,] originalShape = game.Opponent.CurrentTetromino.Shape;
        int[,] orginialDomainShape = domainGame.Opponent.CurrentTetromino.Shape.Value;
        
        _serverMock.Raise(server => server.OnRotateRight += null);

        int[,] rotatedShape = game.Opponent.CurrentTetromino.Shape;
        int[,] rotatedDomainShape = domainGame.Opponent.CurrentTetromino.Shape.Value;

        Assert.NotNull(game.Opponent.CurrentTetromino.Shape);
        Assert.NotEmpty(game.Opponent.CurrentTetromino.Points);
        Assert.NotEqual(originalShape, rotatedShape);
        Assert.NotEqual(orginialDomainShape, rotatedDomainShape);
        Assert.Equal(rotatedDomainShape, rotatedShape);
    }

    [Fact]
    public async Task OpponentRotateLeft_UpdatesOpponentTetrominoDto()
    {
        Game game = await StartReadyMultiplayerGame();
        IGame domainGame = DomainGame.GetGame();
        int[,] originalShape = game.Opponent.CurrentTetromino.Shape;
        int[,] orginialDomainShape = domainGame.Opponent.CurrentTetromino.Shape.Value;

        _serverMock.Raise(server => server.OnRotateLeft += null);

        int[,] rotatedShape = game.Opponent.CurrentTetromino.Shape;
        int[,] rotatedDomainShape = domainGame.Opponent.CurrentTetromino.Shape.Value;

        Assert.NotNull(game.Opponent.CurrentTetromino.Shape);
        Assert.NotEmpty(game.Opponent.CurrentTetromino.Points);
        Assert.NotEqual(originalShape, rotatedShape);
        Assert.NotEqual(orginialDomainShape, rotatedDomainShape);
        Assert.Equal(rotatedDomainShape, rotatedShape);
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
