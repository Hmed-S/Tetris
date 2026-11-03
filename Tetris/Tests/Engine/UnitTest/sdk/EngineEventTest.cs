using TetrisEngine.Sdk;

namespace UnitTest.sdk;

public class EngineEventTest
{

    [Fact]
    public void OnTetrominoPositionChangeFiresWhenPropertyIsSet()
    {
        Player player = new()
        {
            CurrentTetromino = new Tetromino
            {
                Color = Color.Blue
            }
        };
        Tetromino? previous = null;
        Tetromino? current = null;
        player.OnTetrominoPositionChange += (p, c) =>
        {
            previous = p;
            current = c;
        };

        player.CurrentTetromino = new Tetromino
        {
            Color = Color.Purple
        };

        Assert.Equal(Color.Blue, previous.Color);
        Assert.Equal(Color.Purple, current.Color);
    }

    [Fact]
    public void OnPreviewChangeFiresWhenPropertyIsSet()
    {
        Player player = new();
        Tetromino preview = null;
        player.OnPreviewChange += (p) => preview = p;

        player.Preview = new Tetromino
        {
            Color = Color.Blue
        };

        Assert.NotNull(preview);
        Assert.Equal(Color.Blue, preview.Color);
    }

    [Fact]
    public void OnScoreChangeFiresWhenPropertyIsSet()
    {
        Player player = new();
        int score = 393;
        player.OnScoreChange += (s) => score = s;

        player.Score = 20;

        Assert.NotEqual(393, score);
        Assert.Equal(20, score);
    }

    [Fact]
    public void OnLevelChangeFiresWhenPropertyIsSet()
    {
        Player player = new();
        int level = 393;
        player.OnLevelChange += (l) => level = l;

        player.Level = 2;

        Assert.NotEqual(393, level);
        Assert.Equal(2, level);
    }

    [Fact]
    public void OnLineCountChangeFiresWhenPropertyIsSet()
    {
        Player player = new();
        int lines = 393;
        player.OnLineCountChange += (l) => lines = l;

        player.LineCount = 2;

        Assert.NotEqual(393, lines);
        Assert.Equal(2, lines);
    }

    [Fact]
    public void OnReadyFiresWhenPropertyIsSet()
    {
        Player player = new();
        bool ready = false;
        player.OnReady += () => ready = true;

        player.IsReady = true;

        Assert.True(ready);
    }

    [Fact]
    public void OnQuitFiresWhenPropertyIsSet()
    {
        Player player = new();
        bool quit = false;
        player.OnQuit += () => quit = true;

        player.Quit = true;

        Assert.True(quit);
    }

    [Fact]
    public void OnDisConnectedFiresWhenPropertyIsSet()
    {
        Player player = new();
        bool disconnected = false;
        player.OnDisconnected += () => disconnected = true;

        player.Disconnected = true;

        Assert.True(disconnected);
    }

    [Fact]
    public void OnLineClearFiresWhenPropertyIsSet()
    {
        Board board = new();
        int clearedLine = 393;

        board.OnLineClear += (line) => clearedLine = line;

        board.LastClearedLine = 8;

        Assert.NotEqual(393, clearedLine);
        Assert.Equal(8, clearedLine);
    }

    [Fact]
    public void OnRowSwapFiresWhenPropertyIsSet()
    {
        Board board = new();
        int bottomLine = 392;
        int topLine = 391;
        board.OnRowSwap += (bl, tl) =>
        {
            bottomLine = bl; 
            topLine = tl;
        };

        board.LastSwappedLines = [10,9];

        Assert.NotEqual(392, bottomLine);
        Assert.NotEqual(391, topLine);
        Assert.Equal(10, bottomLine);
        Assert.Equal(9, topLine);
    }

    [Fact]
    public void OnMatchFoundFiresWhenPropertyIsSet()
    {
        Game game = new();
        string eventFired = string.Empty;
        game.OnMatchFound += () => eventFired = "Event is fired";

        game.Opponent = new Player();

        Assert.NotEmpty(eventFired);
    }

    [Fact]
    public void OnBothReadyFiresWhenPropertyIsSet()
    {
        Game game = new();
        string eventFired = string.Empty;
        game.OnBothReady += () => eventFired = "Event is fired";

        game.BothReady = true;

        Assert.NotEmpty(eventFired);
    }

    [Fact]
    public void OnGameOverFiresWhenPropertyIsSet()
    {
        Game  game = new();
        string eventFired = string.Empty;
        game.OnGameOver += () => eventFired = "Event is fired";

        game. IsOver = true;

        Assert.NotEmpty(eventFired);
    }

}
