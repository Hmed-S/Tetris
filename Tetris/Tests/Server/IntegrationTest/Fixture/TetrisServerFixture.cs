using Garnet;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Net;

namespace TetrisServer.IntegrationTests.Fixtures;

public class TetrisServerFixture : IAsyncLifetime
{
    private GarnetServer _redis;
    private ILoggerFactory _loggerFactory;

    private WebApplicationFactory<Program> _factory;
    private ConnectionMultiplexer _redisConnection;
    public IServiceProvider Services => _factory.Services;

    public string RedisConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        _loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
        _redis = new GarnetServer(
            ["--bind", "127.0.0.1", "--port", "6379"],
            _loggerFactory,
            cleanupDir: true,
            authenticationSettingsOverride: null);
        _redis.Start();

        RedisConnectionString = "127.0.0.1:6379";

        Dictionary<string, string> settings = new()
        {
            ["ConnectionStrings:Redis"] = RedisConnectionString,
            ["Redis:Hosts:0:Host"] = "127.0.0.1",
            ["Redis:Hosts:0:Port"] = "6379",
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

        _redis?.Dispose();
        _loggerFactory?.Dispose();
    }

}
