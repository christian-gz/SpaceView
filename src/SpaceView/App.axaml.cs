using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SpaceView.ViewModels;
using SpaceView.Views;
using Microsoft.Extensions.Configuration;
using SpaceView.Configuration;

namespace SpaceView;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            IConfig config;
            try
            {
                config = LoadConfig();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                config = new Config();
            }

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(config),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private Config LoadConfig()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Config config = builder.Get<Config>() ?? throw new InvalidOperationException("Configuration cannot be loaded.");

        if (config.ConnectionStrings == null || string.IsNullOrEmpty(config.ConnectionStrings.Default))
        {
            throw new InvalidOperationException("The required 'Default' connection string is missing or empty.");
        }

        if (config.ApiSettings == null || string.IsNullOrEmpty(config.ApiSettings.NasaApiKey))
        {
            throw new InvalidOperationException("The required 'NasaApiKey' string is missing or empty.");
        }

        return config;
    }
}