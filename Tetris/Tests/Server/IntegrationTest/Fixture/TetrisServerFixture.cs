using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using StackExchange.Redis;
using System.Net;
using Testcontainers.Redis;

namespace TetrisServer.IntegrationTests.Fixtures;

public class TetrisServerFixture : IAsyncLifetime
{
    private readonly RedisContainer _redis =
        new RedisBuilder()
            .WithImage("redis:7-alpine")
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilInternalTcpPortIsAvailable(6379))
            .Build();

    private WebApplicationFactory<Program> _factory;
    private ConnectionMultiplexer _redisConnection;
    public IServiceProvider Services => _factory.Services;

    public string RedisConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        await _redis.StartAsync();

        RedisConnectionString = _redis.GetConnectionString();
        string dynamicPort = _redis.GetMappedPublicPort(6379).ToString();
        string hostname = _redis.Hostname;

        Environment.SetEnvironmentVariable("ConnectionStrings__Redis", RedisConnectionString);
        Environment.SetEnvironmentVariable("Redis__Hosts__0__Host", hostname);
        Environment.SetEnvironmentVariable("Redis__Hosts__0__Port", dynamicPort);
        Environment.SetEnvironmentVariable("Redis__Password", "");
        Environment.SetEnvironmentVariable("Redis__AllowAdmin", "true");
        Environment.SetEnvironmentVariable("Redis__Ssl", "false");

        ConfigurationOptions redisOptions = ConfigurationOptions.Parse(RedisConnectionString);
        redisOptions.AllowAdmin = true;
        _redisConnection = await ConnectionMultiplexer.ConnectAsync(redisOptions);

        _factory = new WebApplicationFactory<Program>();

    }

    public HubConnection CreateHubConnection(string name)
    {
        return new HubConnectionBuilder()
            .WithUrl($"http://localhost:5000/tetrisHub?name={name}", options =>
            {
                options.HttpMessageHandlerFactory =
                    handler => _factory.Server.CreateHandler();
            })
            .Build();
    }

    public async Task ResetRedisAsync()
    {
        EndPoint[] endpoints = _redisConnection.GetEndPoints();

        foreach (EndPoint endpoint in endpoints)
        {
            IServer server = _redisConnection.GetServer(endpoint);
            await server.FlushDatabaseAsync(0);
        }
    }

    public async Task DisposeAsync()
    {
        if (_factory != null)
        {
            _factory.Dispose();
        }

        if (_redisConnection != null)
        {
            await _redisConnection.DisposeAsync();
        }

        await _redis.DisposeAsync();
    }

}
