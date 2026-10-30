using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;
using TetrisEngine.Domain.Game.Player.Factory;

namespace TetrisEngine.Tests.UnitTest;

public class GameTest
{
    private readonly PlayerFactory _playerFactory = new();

    public GameTest()
    {
        IGame game = Game.GetGame();

        game?.Quit(game.Player);
    }

    private IPlayer CreatePlayer(
        GameMode gameMode,
        int seed = 123)
    {
        Name name = new("TestPlayer");
        TetrisBoard board = new(10,10);
        Score score = new();
        Level level = new();
        Interval interval = new();

        return _playerFactory.CreatePlayer(
            gameMode,
            seed,
            name,
            board,
            score,
            level,
            interval);
    }

    private IGame CreateGame(
        GameMode gameMode,
        IPlayer player,
        IPlayer? opponent = null)
    {
        IGame game = Game.Start(gameMode, player);

        if (opponent != null)
        {
            game.Opponent = opponent;
        }

        return game;
    }

    // --------------------------------------------------
    // Player and opponent
    // --------------------------------------------------

    [Fact]
    public void Player_Set_ShouldSubscribeToOnReady()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(GameMode.MultiPlayer, player);

        Assert.Single(player.OnReady);
    }

    [Fact]
    public void Opponent_Set_ShouldSetGameReference()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(
            GameMode.MultiPlayer,
            player,
            opponent);

        Assert.Same(game, opponent.Game);
    }

    [Fact]
    public void Opponent_Set_ShouldSubscribeToOnReady()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(
            GameMode.MultiPlayer,
            player,
            opponent);

        Assert.Single(opponent.OnReady);
    }

    [Fact]
    public void Opponent_GetInSinglePlayer_ShouldThrow()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);

        IGame game = CreateGame(GameMode.SinglePlayer, player);  

        Assert.Throws<InvalidOperationException>(
            () => _ = game.Opponent);
    }

    [Fact]
    public void Opponent_SetInSinglePlayer_ShouldThrow()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(GameMode.SinglePlayer, player);

        Assert.Throws<InvalidOperationException>(
            () => game.Opponent = opponent);
    }

    // --------------------------------------------------
    // Start
    // --------------------------------------------------

    [Fact]
    public void Start_WhenNoGameExists_ShouldCreateGame()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);

        IGame game = Game.Start(GameMode.SinglePlayer, player);

        Assert.NotNull(game);
        Assert.Equal(GameMode.SinglePlayer, game.Mode);
        Assert.Equal(GameState.Playing, game.GameState);
        Assert.Same(player, game.Player);
        Assert.Same(game, player.Game);
    }

    [Fact]
    public void Start_WhenGameIsPlaying_ShouldThrow()
    {
        IPlayer firstPlayer = CreatePlayer(GameMode.SinglePlayer);
        IPlayer secondPlayer = CreatePlayer(GameMode.SinglePlayer);

        Game.Start(GameMode.SinglePlayer, firstPlayer);

        Assert.Throws<InvalidOperationException>(
            () => Game.Start(GameMode.SinglePlayer, secondPlayer));
    }

    [Theory]
    [InlineData(GameState.Quit)]
    [InlineData(GameState.GameOver)]
    public void Start_WhenPreviousGameHasEnded_ShouldCreateNewGame(
        GameState previousState)
    {
        IPlayer firstPlayer = CreatePlayer(GameMode.SinglePlayer);

        IGame previousGame = Game.Start(
            GameMode.SinglePlayer,
            firstPlayer);

        previousGame.GameState = previousState;

        IPlayer secondPlayer = CreatePlayer(GameMode.SinglePlayer);

        IGame newGame = Game.Start(
            GameMode.SinglePlayer,
            secondPlayer);

        Assert.NotSame(previousGame, newGame);
        Assert.Same(secondPlayer, newGame.Player);
        Assert.Same(newGame, secondPlayer.Game);
        Assert.Equal(GameState.Playing, newGame.GameState);
    }

    [Fact]
    public void GetGame_ShouldReturnCurrentGame()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);

        IGame game = Game.Start(GameMode.SinglePlayer, player);

        Assert.Same(game, Game.GetGame());
    }

    // --------------------------------------------------
    // Ready notifications
    // --------------------------------------------------

    [Fact]
    public void BothPlayersReady_ShouldInvokeOnBothReady()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(
            GameMode.MultiPlayer,
            player,
            opponent);

        int callbackCount = 0;

        game.OnBothReady.Add(() => callbackCount++);

        player.Ready();
        opponent.Ready();

        Assert.Equal(1, callbackCount);
    }

    [Fact]
    public void OnlyOnePlayerReady_ShouldNotInvokeOnBothReady()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(
            GameMode.MultiPlayer,
            player,
            opponent);

        int callbackCount = 0;

        game.OnBothReady.Add(() => callbackCount++);

        player.Ready();

        Assert.Equal(0, callbackCount);
    }

    [Fact]
    public void BothPlayersReady_ShouldInvokeAllCallbacks()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(
            GameMode.MultiPlayer,
            player,
            opponent);

        bool firstCallbackCalled = false;
        bool secondCallbackCalled = false;

        game.OnBothReady.Add(() => firstCallbackCalled = true);
        game.OnBothReady.Add(() => secondCallbackCalled = true);

        player.Ready();
        opponent.Ready();

        Assert.True(firstCallbackCalled);
        Assert.True(secondCallbackCalled);
    }

    // --------------------------------------------------
    // Quit
    // --------------------------------------------------

    [Fact]
    public void Quit_ShouldSetGameStateToQuit()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IGame game = CreateGame(GameMode.SinglePlayer, player);

        game.Quit(player);

        Assert.Equal(GameState.Quit, game.GameState);
    }

    [Fact]
    public void Quit_ShouldInvokeOnGameOverCallbacks()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IGame game = CreateGame(GameMode.SinglePlayer, player);

        IPlayer? callbackPlayer = null;

        game.OnGameOver.Add(p => callbackPlayer = p);

        game.Quit(player);

        Assert.Same(player, callbackPlayer);
    }

    [Fact]
    public void Quit_ShouldReplacePlayerWithGameOverPlayer()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IGame game = CreateGame(GameMode.SinglePlayer, player);

        game.Quit(player);

        Assert.NotSame(player, game.Player);
        Assert.IsType<GameOverPlayer>(game.Player);
    }

    // --------------------------------------------------
    // Game over
    // --------------------------------------------------

    [Fact]
    public void Over_ShouldSetGameStateToGameOver()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IGame game = CreateGame(GameMode.SinglePlayer, player);

        game.Over(player);

        Assert.Equal(GameState.GameOver, game.GameState);
    }

    [Fact]
    public void Over_ShouldInvokeOnGameOverCallbacks()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IGame game = CreateGame(GameMode.SinglePlayer, player);

        IPlayer? callbackPlayer = null;

        game.OnGameOver.Add(p => callbackPlayer = p);

        game.Over(player);

        Assert.Same(player, callbackPlayer);
    }

    [Fact]
    public void Over_ShouldReplacePlayerWithGameOverPlayer()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IGame game = CreateGame(GameMode.SinglePlayer, player);

        game.Over(player);

        Assert.NotSame(player, game.Player);
        Assert.IsType<GameOverPlayer>(game.Player);
    }

    // --------------------------------------------------
    // Winner selection
    // --------------------------------------------------

    [Fact]
    public void Over_WhenOpponentLoses_ShouldSetPlayerAsWinner()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(
            GameMode.MultiPlayer,
            player,
            opponent);

        game.Over(opponent);

        Assert.Same(game.Player, game.Winner);
    }

    [Fact]
    public void Over_WhenPlayerLoses_ShouldSetOpponentAsWinner()
    {
        IPlayer player = CreatePlayer(GameMode.MultiPlayer);
        IPlayer opponent = CreatePlayer(GameMode.MultiPlayer);

        IGame game = CreateGame(
            GameMode.MultiPlayer,
            player,
            opponent);

        game.Over(player);

        Assert.Same(opponent, game.Winner);
    }

    [Fact]
    public void OverInSinglePlayer_ShouldNotSetWinner()
    {
        IPlayer player = CreatePlayer(GameMode.SinglePlayer);
        IGame game = CreateGame(GameMode.SinglePlayer, player);

        game.Over(player);

        Assert.Null(game.Winner);
    }

}
