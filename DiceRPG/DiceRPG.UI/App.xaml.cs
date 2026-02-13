using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using DiceRPG.Core.Data;
using DiceRPG.UI.ViewModels;

namespace DiceRPG.UI;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Core services
        services.AddSingleton<JsonDataService>();

        // ViewModels
        services.AddTransient<AbaDadosViewModel>();
        services.AddTransient<AbaCyberpunkViewModel>();
        services.AddTransient<AbaWarhammerViewModel>();
        services.AddTransient<AbaSMTViewModel>();
        services.AddSingleton<MainViewModel>();

        // Main Window
        services.AddSingleton<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Apply Material Design theme
        var paletteHelper = new MaterialDesignThemes.Wpf.PaletteHelper();
        paletteHelper.SetTheme(MaterialDesignThemes.Wpf.Theme.Create(Theme.Dark));

        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow?.Show();
    }
}