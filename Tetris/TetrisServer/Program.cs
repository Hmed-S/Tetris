using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using TetrisServer.Consumers;
using TetrisServer.Data;
using TetrisServer.Service;
using TetrisServer.Utill;
using TetrisServer.Hubs;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
string redisConnectionString = webApplicationBuilder.Configuration.GetConnectionString("Redis");
var redisConfig = webApplicationBuilder.Configuration.GetSection("Redis").Get<RedisConfiguration>();
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
    cap.UseRedis(redisConnectionString);
    cap.UseInMemoryStorage();
});
services.AddSignalR()
    .AddStackExchangeRedis(redisConnectionString);

WebApplication app = webApplicationBuilder.Build();

app.UseRouting();
app.MapHub<TetrisHub>("/tetrisHub");

app.Run();

public partial class Program
{
}
