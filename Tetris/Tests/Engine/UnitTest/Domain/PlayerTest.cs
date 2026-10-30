using TetrisEngine.Domain.Board;
using TetrisEngine.Domain.Game;
using TetrisEngine.Domain.Game.Player;
using TetrisEngine.Tests.UnitTest;

namespace UnitTest.Domain;

public class PlayerTest
{

    public PlayerTest() 
    {
        IGame game = Game.GetGame();

        game?.Quit(game.Player);
    }

    [Fact]
    public void Constructor_ShouldInitializePlayer()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        Assert.NotEqual(Guid.Empty, player.Id);
        Assert.Equal(123, player.Seed);
        Assert.NotNull(player.Random);
        Assert.NotNull(player.Board);
        Assert.Equal(0, player.Score.Value);
        Assert.Equal(0, player.Lines.Value);
        Assert.Equal(1, player.Level.Value);
        Assert.Equal(1, player.Interval.Seconds);
        Assert.NotNull(player.CurrentTetromino.Shape.Value);
        Assert.NotNull(player.Preview.Shape.Value);
    }

    [Fact]
    public void Constructor_ShouldCenterCurrentTetromino()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        int expectedX =
            (player.Board.Width -
             player.CurrentTetromino.Shape.Value.GetLength(1)) / 2;

        Assert.Equal(expectedX, player.CurrentTetromino.XPosition);
        Assert.Equal(0, player.CurrentTetromino.YPosition);
    }

    [Fact]
    public void Constructor_ShouldGenerateSameTetrominoesForSameSeed()
    {
        (IPlayer, IPlayer, Game) players = PlayerHelper.CreateMultiplayerPlayers(123);
        IPlayer firstPlayer = players.Item1;
        IPlayer secondPlayer = players.Item2;

        Assert.Equal(
            firstPlayer.CurrentTetromino.Shape.Value,
            secondPlayer.CurrentTetromino.Shape.Value);

        Assert.Equal(
            firstPlayer.Preview.Shape.Value,
            secondPlayer.Preview.Shape.Value);
    }

    [Fact]
    public void Constructor_ShouldGenerateDifferentIdsForDifferentPlayers()
    {
        (IPlayer, IPlayer, Game) players = PlayerHelper.CreateMultiplayerPlayers();
        IPlayer firstPlayer = players.Item1;
        IPlayer secondPlayer = players.Item2;

        Assert.NotEqual(firstPlayer.Id, secondPlayer.Id);
    }

    [Fact]
    public void Init_ShouldGenerateNewTetrominoes()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.Init();

        Assert.True(player.Board.CanFit(player.CurrentTetromino));
        Assert.True(player.Board.CanFit(player.Preview));
    }

    [Fact]
    public void Init_ShouldRaisePreviewChangedEvent()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        int callbackCount = 0;

        player.OnPreviewChange.Add(
            preview => callbackCount++);

        player.Init();

        Assert.Equal(1, callbackCount);
    }

    [Fact]
    public void Init_ShouldRaisePositionChangedEvent()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        int callbackCount = 0;

        player.OnTetrominoPositionChange.Add(
            position => callbackCount++);

        player.Init();

        Assert.Equal(1, callbackCount);
    }

    [Fact]
    public void MoveLeft_ShouldMoveTetrominoLeft()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        player.MoveLeft();

        Assert.Equal(3, player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveRight_ShouldMoveTetrominoRight()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        player.MoveRight();

        Assert.Equal(5, player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveLeft_WhenAtLeftBoundary_ShouldNotMove()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(0, 0);

        player.MoveLeft();

        Assert.Equal(0, player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveRight_WhenAtRightBoundary_ShouldNotMove()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                player.Board.Width - 2, 0);

        player.MoveRight();

        Assert.Equal(
            player.Board.Width - 2,
            player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveLeft_WhenBlockedByAnotherTetromino_ShouldNotMove()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        player.Board.Values[0, 3] = 1;
        player.Board.Values[1, 3] = 1;

        player.MoveLeft();

        Assert.Equal(4, player.CurrentTetromino.XPosition);
    }

    [Fact]
    public void MoveRight_WhenBlockedByAnotherTetromino_ShouldNotMove()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        player.Board.Values[0, 6] = 1;
        player.Board.Values[1, 6] = 1;

        player.MoveRight();

        Assert.Equal(4, player.CurrentTetromino.XPosition);
    }

    [Theory]
    [InlineData(true, Move.MoveLeft)]
    [InlineData(false, Move.MoveRight)]
    public void Movement_ShouldRaisePositionChangedEvent(
        bool moveLeft,
        Move expectedMove)
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        Move? actualMove = null;

        player.OnTetrominoPositionChange.Add(
            position => actualMove = position.Move);

        if (moveLeft)
        {
            player.MoveLeft();
        }
        else
        {
            player.MoveRight();
        }

        Assert.Equal(expectedMove, actualMove);
    }

    [Fact]
    public void RotateLeft_ShouldRaisePositionChangedEvent()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        Move? actualMove = null;

        player.OnTetrominoPositionChange.Add(
            position => actualMove = position.Move);

        player.RotateLeft();

        Assert.Equal(Move.RotateLeft, actualMove);
    }

    [Fact]
    public void RotateRight_ShouldRaisePositionChangedEvent()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        Move? actualMove = null;

        player.OnTetrominoPositionChange.Add(
            position => actualMove = position.Move);

        player.RotateRight();

        Assert.Equal(Move.RotateRight, actualMove);
    }

    [Fact]
    public void DropTetromino_ShouldMoveTetrominoDown()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(4, 0);

        player.DropTetromino();

        Assert.Equal(1, player.CurrentTetromino.YPosition);
    }

    [Fact]
    public void DropTetromino_WhenPieceLands_ShouldWritePieceToBoard()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                4,
                player.Board.Height - 2);

        player.DropTetromino();

        Assert.Equal(1, player.Board.Values[18, 4]);
        Assert.Equal(1, player.Board.Values[18, 5]);
        Assert.Equal(1, player.Board.Values[19, 4]);
        Assert.Equal(1, player.Board.Values[19, 5]);
    }

    [Fact]
    public void DropTetromino_WhenPieceLands_ShouldRaiseLineCountEvent()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                4,
                player.Board.Height - 2);

        int callbackCount = 0;

        player.OnLineCountChange.Add(
            lines => callbackCount++);

        player.DropTetromino();

        Assert.Equal(1, callbackCount);
    }

    [Fact]
    public void DropTetromino_WhenPieceLands_ShouldRaiseScoreEvent()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                4,
                player.Board.Height - 2);

        int callbackCount = 0;

        player.OnScoreChange.Add(
            score => callbackCount++);

        player.DropTetromino();

        Assert.Equal(1, callbackCount);
    }

    [Fact]
    public void DropTetromino_WhenOneLineIsCleared_ShouldUpdateAggregateValues()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        PlayerHelper.FillBottomRowExcept(
            player.Board,
            0,
            1);

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                0,
                player.Board.Height - 2);

        int clearedLine = -1;

        player.OnLineClear.Add(
            row => clearedLine = row);

        player.DropTetromino();

        Assert.Equal(1, player.Lines.Value);
        Assert.Equal(100, player.Score.Value);
        Assert.Equal(1, player.Level.Value);
        Assert.Equal(1, player.Interval.Seconds);
        Assert.Equal(player.Board.Height - 1, clearedLine);
    }

    [Fact]
    public void DropTetromino_WhenTenLinesAreCleared_ShouldAdvanceLevel()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        for (int i = 0; i < 10; i++)
        {
            PlayerHelper.FillBottomRowExcept(
                player.Board,
                0,
                1);

            player.CurrentTetromino =
                Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                    0,
                    player.Board.Height - 2);

            player.DropTetromino();
        }

        Assert.Equal(10, player.Lines.Value);
        Assert.Equal(1000, player.Score.Value);
        Assert.Equal(2, player.Level.Value);
        Assert.Equal(Math.Pow(0.8 - 0.007, 1), player.Interval.Seconds);
    }

    [Fact]
    public void DropTetromino_WhenNoLinesAreCleared_ShouldNotIncreaseScore()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                4,
                player.Board.Height - 2);

        player.DropTetromino();

        Assert.Equal(0, player.Score.Value);
        Assert.Equal(0, player.Lines.Value);
    }

    [Fact]
    public void DropTetromino_WhenNoLinesAreCleared_ShouldKeepLevel()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.CurrentTetromino =
            Tetromino.FromShape(ShapeType.OSHAPE).ChangePosition(
                4,
                player.Board.Height - 2);

        player.DropTetromino();

        Assert.Equal(1, player.Level.Value);
    }

    [Fact]
    public void Ready_ShouldThrowForSinglePlayer()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        Assert.Throws<InvalidOperationException>(
            () => player.Ready());
    }

    [Fact]
    public void Quit_ShouldSetGameStateToQuit()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        player.Quit();

        Assert.Equal(GameState.Quit, player.Game.GameState);
        Assert.IsType<GameOverPlayer>(player.Game.Player);
    }

    [Fact]
    public void Quit_ShouldRaiseGameOverEvent()
    {
        IPlayer player = PlayerHelper.CreateSinglePlayer();

        IPlayer? callbackPlayer = null;

        player.Game.OnGameOver.Add(
            losingPlayer => callbackPlayer = losingPlayer);

        player.Quit();

        Assert.Same(player, callbackPlayer);
    }

}
