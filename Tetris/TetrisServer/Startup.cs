using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Configuration;
using TetrisServer.Consumers;
using TetrisServer.Data;
using TetrisServer.Hubs;
using StackExchange.Redis.Extensions.Newtonsoft;
using TetrisServer.Service;
using DotNetCore.CAP.Serialization;
using TetrisServer.Utill;
using System;

namespace TetrisServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPlayerRepository, RedisPlayerRepository>();
            services.AddTransient<RedisPlayerConsumer>();
            services.AddSingleton<Random>();
            services.AddSingleton<MatchService>();

            services.AddSingleton<ISerializer, NewtonSoftCapSerializer>();

            var redisConfig = Configuration.GetSection("Redis").Get<RedisConfiguration>();

            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfig);

            services.AddCap(cap =>
            {
                

                cap.UseRedis("localhost:6379");

                cap.UseInMemoryStorage();
            });

            services.AddSignalR(config =>
            {
                config.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TetrisHub>("/tetrisHub");
            });
        }
    }
}
