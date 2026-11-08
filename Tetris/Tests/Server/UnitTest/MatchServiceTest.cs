using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using Moq;
using TetrisServer.Model;
using TetrisServer.Data;
using TetrisServer.Service;
using TetrisServer.Data.Entities;

namespace TetrisServer.Tests.Services;

public class MatchServiceTest
{
    private readonly Mock<IPlayerRepository> _repositoryMock;
    private readonly Mock<ICapPublisher> _publisherMock;
    private readonly Mock<ILogger<MatchService>> _loggerMock;
    private readonly Mock<Random> _randomMock;

    private readonly MatchService _matchService;

    public MatchServiceTest()
    {
        _repositoryMock = new Mock<IPlayerRepository>();
        _publisherMock = new Mock<ICapPublisher>();
        _loggerMock = new Mock<ILogger<MatchService>>();
        _randomMock = new Mock<Random>();

        _matchService = new MatchService(
            _repositoryMock.Object,
            _randomMock.Object,
            _publisherMock.Object,
            _loggerMock.Object);
    }

    private static Player CreatePlayer(string id, string name)
    {
        return new Player
        {
            Id = id,
            Name = name,
            TimeJoined = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task JoinQueue_AddsPlayerToRepository()
    {
        string id = "player1";
        string name = "Player One";

        await _matchService.JoinQueue(id, name);

        _repositoryMock.Verify(
            repository => repository.AddToQueue(
                It.Is<Player>(player =>
                    player.Id == id &&
                    player.Name == name)),
            Times.Once);
    }

    [Fact]
    public async Task JoinQueue_PublishesQueueEvent()
    {
        string id = "player1";
        string name = "Player One";

        await _matchService.JoinQueue(id, name);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                "Queue",
                It.Is<Player>(player =>
                    player.Id == id &&
                    player.Name == name),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task MakeMatch_WhenLessThanTwoPlayers_DoesNotCreateMatch()
    {
        _repositoryMock
            .Setup(repository => repository.GetQueueLength())
            .ReturnsAsync(1);

        await _matchService.MakeMatch();

        _repositoryMock.Verify(
            repository => repository.Pop(),
            Times.Never);

        _repositoryMock.Verify(
            repository => repository.SavePlayer(It.IsAny<Player>()),
            Times.Never);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                "MatchMade",
                It.IsAny<MatchEvent>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task MakeMatch_WhenTwoPlayersExist_AssignsOpponents()
    {
        Player player1 = CreatePlayer("1", "Player One");
        Player player2 = CreatePlayer("2", "Player Two");

        _repositoryMock
            .Setup(repository => repository.GetQueueLength())
            .ReturnsAsync(2);

        _repositoryMock
            .Setup(repository => repository.Pop())
            .ReturnsAsync(Tuple.Create(player1, player2));

        _randomMock
            .Setup(random => random.Next())
            .Returns(12345);

        await _matchService.MakeMatch();

        Assert.Same(player2, player1.Opponent);
        Assert.Same(player1, player2.Opponent);

        _repositoryMock.Verify(
            repository => repository.SavePlayer(player1),
            Times.Once);

        _repositoryMock.Verify(
            repository => repository.SavePlayer(player2),
            Times.Once);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                "MatchMade",
                It.IsAny<MatchEvent>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetOpponentByPlayerId_ReturnsPlayerOpponent()
    {
        Player player = CreatePlayer("1", "Player One");
        Player opponent = CreatePlayer("2", "Player Two");

        player.Opponent = opponent;

        _repositoryMock
            .Setup(repository => repository.GetPlayerById(player.Id))
            .ReturnsAsync(player);

        Player result = await _matchService.GetOpponentByPlayerId(player.Id);

        Assert.Same(opponent, result);

        _repositoryMock.Verify(
            repository => repository.GetPlayerById(player.Id),
            Times.Once);
    }

    [Fact]
    public async Task DisconnectPlayer_WhenPlayerDoesNotExist_DoesNothing()
    {
        string id = "unknown";

        _repositoryMock
            .Setup(repository => repository.GetPlayerById(id))
            .ReturnsAsync((Player)null);

        await _matchService.DisconnectPlayer(id);

        _repositoryMock.Verify(
            repository => repository.RemovePlayerFromQueue(It.IsAny<string>()),
            Times.Never);

        _repositoryMock.Verify(
            repository => repository.RemovePlayer(It.IsAny<string>()),
            Times.Never);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DisconnectPlayer_WhenPlayerIsQueued_RemovesPlayerFromQueue()
    {
        Player player = CreatePlayer("1", "Player One");

        _repositoryMock
            .Setup(repository => repository.GetPlayerById(player.Id))
            .ReturnsAsync(player);

        await _matchService.DisconnectPlayer(player.Id);

        _repositoryMock.Verify(
            repository => repository.RemovePlayerFromQueue(player.Id),
            Times.Once);

        _repositoryMock.Verify(
            repository => repository.RemovePlayer(It.IsAny<string>()),
            Times.Never);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DisconnectPlayer_WhenPlayerHasOpponent_RemovesBothPlayers()
    {
        Player player = CreatePlayer("1", "Player One");
        Player opponent = CreatePlayer("2", "Player Two");

        player.Opponent = opponent;

        _repositoryMock
            .Setup(repository => repository.GetPlayerById(player.Id))
            .ReturnsAsync(player);

        await _matchService.DisconnectPlayer(player.Id);

        _repositoryMock.Verify(
            repository => repository.RemovePlayer(player.Id),
            Times.Once);

        _repositoryMock.Verify(
            repository => repository.RemovePlayer(opponent.Id),
            Times.Once);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                "PlayerDisconnected",
                opponent.Id,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Quit_WhenPlayerIsQueued_RemovesPlayerFromQueue()
    {
        Player player = CreatePlayer("1", "Player One");

        _repositoryMock
            .Setup(repository => repository.GetPlayerById(player.Id))
            .ReturnsAsync(player);

        await _matchService.Quit(player.Id);

        _repositoryMock.Verify(
            repository => repository.RemovePlayerFromQueue(player.Id),
            Times.Once);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                "Quit",
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Quit_WhenPlayerHasOpponent_RemovesBothAndPublishesQuit()
    {
        Player player = CreatePlayer("1", "Player One");
        Player opponent = CreatePlayer("2", "Player Two");

        player.Opponent = opponent;

        _repositoryMock
            .Setup(repository => repository.GetPlayerById(player.Id))
            .ReturnsAsync(player);

        await _matchService.Quit(player.Id);

        _repositoryMock.Verify(
            repository => repository.RemovePlayer(player.Id),
            Times.Once);

        _repositoryMock.Verify(
            repository => repository.RemovePlayer(opponent.Id),
            Times.Once);

        _publisherMock.Verify(
            publisher => publisher.PublishAsync(
                "Quit",
                opponent.Id,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

}
