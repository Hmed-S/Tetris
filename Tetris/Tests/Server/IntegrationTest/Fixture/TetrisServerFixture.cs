using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Net;
using Testcontainers.Redis;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.CAP;

namespace TetrisServer.IntegrationTests.Fixtures;

public class TetrisServerFixture : IAsyncLifetime
{
    private readonly RedisContainer _redis =
        new RedisBuilder()
            .WithImage("redis:7-alpine")
            .Build();

    private WebApplicationFactory<Program> _factory;
    private ConnectionMultiplexer _redisConnection;
    public IServiceProvider Services => _factory.Services;

    public string RedisConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        await _redis.StartAsync();

        RedisConnectionString = _redis.GetConnectionString();

        Dictionary<string, string> settings = new()
        {
            ["ConnectionStrings:Redis"] = RedisConnectionString,
            ["Redis:Hosts:0:Host"] = _redis.Hostname,
            ["Redis:Hosts:0:Port"] =
                _redis.GetMappedPublicPort(6379).ToString(),
            ["Redis:Password"] = "",
            ["Redis:AllowAdmin"] = "true",
            ["Redis:Ssl"] = "false",
            ["Redis:ConnectTimeout"] = "5000",
            ["Redis:SyncTimeout"] = "5000",
            ["Redis:Database"] = "0",
            ["Redis:PoolSize"] = "5",
            ["Redis:IsDefault"] = "true",
            ["Redis:AllowAdmin"] = "true"
        };

        ConfigurationOptions redisOptions = ConfigurationOptions.Parse(RedisConnectionString);

        redisOptions.AllowAdmin = true;

        _redisConnection =
            await ConnectionMultiplexer.ConnectAsync(redisOptions);

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(webHost =>
            {
                webHost.ConfigureAppConfiguration(
                    (context, configuration) =>
                    {
                        configuration.AddInMemoryCollection(settings);
                    });
            });

        _ = _factory.Server;
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
