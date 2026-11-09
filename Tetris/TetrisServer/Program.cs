using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;
using TetrisServer.Consumers;
using TetrisServer.Data;
using TetrisServer.Service;
using TetrisServer.Utill;
using TetrisServer.Hubs;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
var redisConfig = webApplicationBuilder.Configuration.GetSection("Redis").Get<RedisConfiguration>();
if (redisConfig is null || redisConfig.Hosts is null || !redisConfig.Hosts.Any())
{
    throw new InvalidOperationException("Redis configuration is missing a host.");
}

RedisHost redisHost = redisConfig.Hosts.Single();
if (string.IsNullOrWhiteSpace(redisHost.Host))
{
    throw new InvalidOperationException("Redis configuration is missing the hostname.");
}

ConfigurationOptions redisOptions = new()
{
    User = redisConfig.User,
    Password = redisConfig.Password,
    Ssl = redisConfig.Ssl,
    SslHost = redisHost.Host,
    AllowAdmin = redisConfig.AllowAdmin,
    ConnectTimeout = redisConfig.ConnectTimeout,
    SyncTimeout = redisConfig.SyncTimeout,
    AbortOnConnectFail = false,
    ConnectRetry = 3
};
redisOptions.EndPoints.Add(redisHost.Host, redisHost.Port);

redisConfig.AbortOnConnectFail = false;
redisConfig.ConfigurationOptionsAsyncHandler = options =>
{
    options.EndPoints.Clear();
    options.EndPoints.Add(redisHost.Host, redisHost.Port);
    options.User = redisConfig.User;
    options.Password = redisConfig.Password;
    options.Ssl = redisConfig.Ssl;
    options.SslHost = redisHost.Host;
    options.AbortOnConnectFail = false;
    return Task.FromResult(options);
};
IServiceCollection services = webApplicationBuilder.Services;

webApplicationBuilder.WebHost.UseUrls("http://0.0.0.0:5000");
webApplicationBuilder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);

services.AddTransient<IPlayerRepository, RedisPlayerRepository>();
services.AddTransient<RedisPlayerConsumer>();
services.AddSingleton<Random>();
services.AddSingleton<MatchService>();
services.AddSingleton<ISerializer, NewtonSoftCapSerializer>();
services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfig);
services.AddCap(cap =>
{
    cap.UseRedis(options => options.Configuration = redisOptions);
    cap.UseInMemoryStorage();
});
services.AddSignalR()
    .AddStackExchangeRedis(options => options.Configuration = redisOptions);

WebApplication app = webApplicationBuilder.Build();

app.UseRouting();
app.MapHub<TetrisHub>("/tetrisHub");

app.Run();

public partial class Program
{
}
