using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;

namespace TetrisEngine.Tests.UnitTest;

public class MultiPlayerPlayerTest
{

    public MultiPlayerPlayerTest() 
    {
        IGame game = Game.GetGame();

        game?.Quit(game.Player);
    }

    [Fact]
    public void Constructor_ShouldNotInitializePlayer()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Null(player.Random);
        Assert.Null(player.CurrentTetromino.Shape.Value);
        Assert.Null(player.Preview.Shape.Value);

        Assert.Null(opponent.Random);
        Assert.Null(opponent.CurrentTetromino.Shape.Value);
        Assert.Null(opponent.Preview.Shape.Value);
    }

    [Fact]
    public void Constructor_ShouldAssignUniqueIds()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.NotEqual(Guid.Empty, player.Id);
        Assert.NotEqual(Guid.Empty, opponent.Id);
        Assert.NotEqual(player.Id, opponent.Id);
    }

    [Fact]
    public void Constructor_ShouldAssignGameToBothPlayers()
    {
        (IPlayer player, IPlayer opponent, Game game) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Same(game, player.Game);
        Assert.Same(game, opponent.Game);
    }

    [Fact]
    public void Ready_ShouldSetIsReady()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        player.Ready();

        Assert.True(player.IsReady);
    }

    [Fact]
    public void Ready_ShouldAssignSeed()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        player.Seed = -1;

        player.Ready();

        Assert.True(player.Seed >= 0);
    }

    [Fact]
    public void Ready_ShouldRaiseOnReadyEvent()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        int callbackCount = 0;

        player.OnReady.Add(() => callbackCount++);

        player.Ready();

        Assert.Equal(1, callbackCount);
    }

    [Fact]
    public void Ready_ShouldInvokeAllOnReadyCallbacks()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        int firstCallbackCount = 0;
        int secondCallbackCount = 0;

        player.OnReady.Add(() => firstCallbackCount++);
        player.OnReady.Add(() => secondCallbackCount++);

        player.Ready();

        Assert.Equal(1, firstCallbackCount);
        Assert.Equal(1, secondCallbackCount);
    }

    [Fact]
    public void MoveLeft_WhenOpponentIsNotReady_ShouldThrow()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Throws<InvalidOperationException>(
            () => player.MoveLeft());
    }

    [Fact]
    public void MoveRight_WhenOpponentIsNotReady_ShouldThrow()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Throws<InvalidOperationException>(
            () => player.MoveRight());
    }

    [Fact]
    public void DropTetromino_WhenOpponentIsNotReady_ShouldThrow()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Throws<InvalidOperationException>(
            () => player.DropTetromino());
    }

    [Fact]
    public void Next_WhenOpponentIsNotReady_ShouldThrow()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Throws<InvalidOperationException>(
            () => player.Next());
    }

    [Fact]
    public void RotateLeft_WhenOpponentIsNotReady_ShouldThrow()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Throws<InvalidOperationException>(
            () => player.RotateLeft());
    }

    [Fact]
    public void RotateRight_WhenOpponentIsNotReady_ShouldThrow()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        Assert.Throws<InvalidOperationException>(
            () => player.RotateRight());
    }

    [Fact]
    public void MoveLeft_WhenOpponentIsReady_ShouldMoveTetromino()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        opponent.Ready();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        player.MoveLeft();

        Assert.Equal(3, player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveRight_WhenOpponentIsReady_ShouldMoveTetromino()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        opponent.Ready();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        player.MoveRight();

        Assert.Equal(5, player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveLeft_WhenPlayerIsReadyButOpponentIsNot_ShouldStillThrow()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        player.Ready();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        Assert.Throws<InvalidOperationException>(
            () => player.MoveLeft());
    }

    [Fact]
    public void MoveLeft_WhenOpponentBecomesReady_ShouldWork()
    {
        (IPlayer player, IPlayer opponent, _) =
            PlayerHelper.CreateMultiplayerPlayers();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        opponent.Ready();

        player.MoveLeft();

        Assert.Equal(3, player.CurrentTetromino.XPosition);
    }
}
