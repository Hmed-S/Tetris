using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TetrisEngine.Sdk;
using TetrisEngine.Sdk.Server;

namespace TetrisClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    public IServiceProvider Services => _host?.Services
        ?? throw new InvalidOperationException("The application host has not started.");

    protected override void OnStartup(StartupEventArgs e)
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                string serverUrl = context.Configuration["Server:Url"]
                    ?? throw new InvalidOperationException("The Server:Url setting is required.");

                services.AddTransient<ITetrisServer>(_ =>
                    new TetrisServerService(serverUrl));
                services.AddTransient<Engine>();
                services.AddTransient<HomePage>();
                services.AddTransient<GamePage>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        _host.Start();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            _host.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
        }

        base.OnExit(e);
    }

}
