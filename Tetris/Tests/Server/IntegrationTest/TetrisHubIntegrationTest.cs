using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using TetrisServer.Data;
using TetrisServer.IntegrationTests.Fixtures;

namespace TetrisServer.IntegrationTests.Hubs;

public class TetrisHubIntegrationTest : IClassFixture<TetrisServerFixture>
{
    private readonly TetrisServerFixture _fixture;

    public TetrisHubIntegrationTest(TetrisServerFixture fixture)
    {
        _fixture = fixture;
    }

    private async Task<(HubConnection Player1, HubConnection Player2)>
        ConnectTwoPlayersAsync()
    {
        await _fixture.ResetRedisAsync();

        HubConnection player1 = _fixture.CreateHubConnection("Player One");
        HubConnection player2 = _fixture.CreateHubConnection("Player Two");

        TaskCompletionSource<(string Name, int Seed)> player1Match =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        TaskCompletionSource<(string Name, int Seed)> player2Match =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        player1.On<string, int>(
            "MatchFound",
            (name, seed) => player1Match.TrySetResult((name, seed)));

        player2.On<string, int>(
            "MatchFound",
            (name, seed) => player2Match.TrySetResult((name, seed)));

        await player1.StartAsync();
        await player2.StartAsync();

        (string Name, int Seed) match1 =
            await player1Match.Task.WaitAsync(TimeSpan.FromSeconds(60));

        (string Name, int Seed) match2 =
            await player2Match.Task.WaitAsync(TimeSpan.FromSeconds(60));

        Assert.True(player1Match.Task.IsCompleted);
        Assert.True(player2Match.Task.IsCompleted);

        Assert.Equal("Player Two", match1.Name);
        Assert.Equal("Player One", match2.Name);
        Assert.Equal(match1.Seed, match2.Seed);

        return (player1, player2);
    }

    [Fact]
    public async Task Connect_TwoPlayers_MatchesThemSuccessfully()
    {
        await _fixture.ResetRedisAsync();

        HubConnection player1 = _fixture.CreateHubConnection("Player One");
        HubConnection player2 = _fixture.CreateHubConnection("Player Two");

        try
        {
            TaskCompletionSource<(string Name, int Seed)> player1Match =
                new(TaskCreationOptions.RunContinuationsAsynchronously);

            TaskCompletionSource<(string Name, int Seed)> player2Match =
                new(TaskCreationOptions.RunContinuationsAsynchronously);

            player1.On<string, int>(
                "MatchFound",
                (name, seed) => player1Match.TrySetResult((name, seed)));

            player2.On<string, int>(
                "MatchFound",
                (name, seed) => player2Match.TrySetResult((name, seed)));

            await player1.StartAsync();
            await player2.StartAsync();

            (string Name, int Seed) match1 =
                await player1Match.Task.WaitAsync(TimeSpan.FromSeconds(60));

            (string Name, int Seed) match2 =
                await player2Match.Task.WaitAsync(TimeSpan.FromSeconds(60));

            Assert.Equal("Player Two", match1.Name);
            Assert.Equal("Player One", match2.Name);
            Assert.Equal(match1.Seed, match2.Seed);

            IPlayerRepository repository =
                _fixture.Services.GetRequiredService<IPlayerRepository>();

            Assert.False(await repository.PlayerIsQueued(player1.ConnectionId));
            Assert.False(await repository.PlayerIsQueued(player2.ConnectionId));
        }
        finally
        {
            await player1.DisposeAsync();
            await player2.DisposeAsync();
        }
    }

    [Theory]
    [InlineData("Ready")]
    [InlineData("FastDrop")]
    [InlineData("RotateLeft")]
    [InlineData("RotateRight")]
    [InlineData("MoveLeft")]
    [InlineData("MoveRight")]
    public async Task HubMethod_ForwardsMessageToOpponent(string methodName)
    {
        (HubConnection player1, HubConnection player2) =
            await ConnectTwoPlayersAsync();

        try
        {
            TaskCompletionSource receivedMessage =
                new(TaskCreationOptions.RunContinuationsAsynchronously);

            player2.On(methodName, () =>
                receivedMessage.TrySetResult());

            await player1.InvokeAsync(methodName);

            await receivedMessage.Task.WaitAsync(
                TimeSpan.FromSeconds(10));
        }
        finally
        {
            await player1.DisposeAsync();
            await player2.DisposeAsync();
        }
    }

    [Fact]
    public async Task QuitGame_RemovesBothPlayersAndNotifiesOpponent()
    {
        (HubConnection player1, HubConnection player2) =
            await ConnectTwoPlayersAsync();

        try
        {
            TaskCompletionSource receivedQuit =
                new(TaskCreationOptions.RunContinuationsAsynchronously);

            player2.On("Quit", () => receivedQuit.TrySetResult());

            await player1.InvokeAsync("QuitGame");

            await receivedQuit.Task.WaitAsync(
                TimeSpan.FromSeconds(10));

            IPlayerRepository repository =
                _fixture.Services.GetRequiredService<IPlayerRepository>();

            Assert.Null(await repository.GetPlayerById(player1.ConnectionId));
            Assert.Null(await repository.GetPlayerById(player2.ConnectionId));
        }
        finally
        {
            await player1.DisposeAsync();
            await player2.DisposeAsync();
        }
    }

    [Fact]
    public async Task Disconnect_RemovesBothPlayersAndNotifiesOpponent()
    {
        (HubConnection player1, HubConnection player2) =
            await ConnectTwoPlayersAsync();

        try
        {
            string player1ConnectionId = player1.ConnectionId;
            string player2ConnectionId = player2.ConnectionId;

            TaskCompletionSource receivedDisconnect =
                new(TaskCreationOptions.RunContinuationsAsynchronously);

            player2.On(
                "OpponentDisconnected",
                () => receivedDisconnect.TrySetResult());

            await player1.StopAsync();

            await receivedDisconnect.Task.WaitAsync(
                TimeSpan.FromSeconds(10));

            IPlayerRepository repository =
                _fixture.Services.GetRequiredService<IPlayerRepository>();

            Assert.Null(await repository.GetPlayerById(player1ConnectionId));
            Assert.Null(await repository.GetPlayerById(player2ConnectionId));
        }
        finally
        {
            await player1.DisposeAsync();
            await player2.DisposeAsync();
        }
    }

}
