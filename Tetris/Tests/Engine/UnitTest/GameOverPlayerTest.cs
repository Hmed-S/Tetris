using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine.Tests.UnitTest;

public class GameOverPlayerTest
{

    public GameOverPlayerTest()
    {
        IGame game = Game.GetGame();

        game?.Quit(game.Player);
    }

    [Fact]
    public void Constructor_ShouldPreservePlayerId()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        GameOverPlayer gameOverPlayer = new(player);

        Assert.Equal(player.Id, gameOverPlayer.Id);
    }

    [Fact]
    public void Constructor_ShouldPreservePlayerProperties()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        GameOverPlayer gameOverPlayer = new(player);

        Assert.Same(player.Game, gameOverPlayer.Game);
        Assert.Equal(player.Name.Value, gameOverPlayer.Name.Value);
        Assert.Equal(player.Seed, gameOverPlayer.Seed);
        Assert.Same(player.Board, gameOverPlayer.Board);
        Assert.Equal(player.Score.Value, gameOverPlayer.Score.Value);
        Assert.Equal(player.Level.Value, gameOverPlayer.Level.Value);
        Assert.Equal(player.Lines.Value, gameOverPlayer.Lines.Value);
        Assert.Equal(player.Interval.Seconds, gameOverPlayer.Interval.Seconds);
        Assert.Equal(player.IsReady, gameOverPlayer.IsReady);
        Assert.Same(player.Random, gameOverPlayer.Random);
    }

    [Fact]
    public void Constructor_ShouldPreserveTetrominoes()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        GameOverPlayer gameOverPlayer = new(player);

        Assert.Equal(
            player.CurrentTetromino.Shape.Value.Cast<int>(),
            gameOverPlayer.CurrentTetromino.Shape.Value.Cast<int>());

        Assert.Equal(
            player.Preview.Shape.Value.Cast<int>(),
            gameOverPlayer.Preview.Shape.Value.Cast<int>());
    }

    [Fact]
    public void Constructor_ShouldPreserveEventLists()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        GameOverPlayer gameOverPlayer = new(player);

        Assert.Same(player.OnReady, gameOverPlayer.OnReady);
        Assert.Same(player.OnLineClear, gameOverPlayer.OnLineClear);
        Assert.Same(
            player.OnTetrominoPositionChange,
            gameOverPlayer.OnTetrominoPositionChange);
        Assert.Same(player.OnScoreChange, gameOverPlayer.OnScoreChange);
        Assert.Same(player.OnPreviewChange, gameOverPlayer.OnPreviewChange);
        Assert.Same(player.OnLineSwab, gameOverPlayer.OnLineSwab);
        Assert.Same(player.OnLevelChange, gameOverPlayer.OnLevelChange);
        Assert.Same(player.OnLineCountChange, gameOverPlayer.OnLineCountChange);
    }

    [Fact]
    public void Init_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.Init());
    }

    [Fact]
    public void Next_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.Next());
    }

    [Fact]
    public void DropTetromino_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.DropTetromino());
    }

    [Fact]
    public void MoveLeft_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.MoveLeft());
    }

    [Fact]
    public void MoveRight_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.MoveRight());
    }

    [Fact]
    public void RotateLeft_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.RotateLeft());
    }

    [Fact]
    public void RotateRight_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.RotateRight());
    }

    [Fact]
    public void Ready_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.Ready());
    }

    [Fact]
    public void Quit_ShouldThrow()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();
        GameOverPlayer gameOverPlayer = new(player);

        Assert.Throws<InvalidOperationException>(
            () => gameOverPlayer.Quit());
    }

}
