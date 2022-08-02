using Microsoft.Extensions.DependencyInjection;
using Rogue.Pomodoro.Core;
using Rogue.Pomodoro.WPF.ViewModels;
using Rogue.Pomodoro.WPF.ViewModels.Interfaces;
using Rogue.Pomodoro.WPF.ViewModels.MainContent;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using Rogue.Pomodoro.WPF.Views;
using System.IO.Abstractions;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Rogue.Pomodoro.WPF;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App
{
    private readonly ServiceProvider serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var bootstrapper = serviceProvider.GetRequiredService<Bootstrapper>();
        bootstrapper.Initialize();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddPomodoroCore();
        services.AddTransient<IFileSystem, FileSystem>();
        services.AddSingleton<Bootstrapper>();
        services.AddSingleton<SynchronizationContext, DispatcherSynchronizationContext>();
        services.AddScoped<IMainContentViewModel, PomodoroTimerViewModel>();
        services.AddScoped<IMainContentViewModel, SettingsViewModel>();
        services.AddScoped<IMainContentViewModel, HomeViewModel>();
        services.AddScoped<IHomeViewModel, HomeViewModel>();
        services.AddScoped<ISettingsViewModel, SettingsViewModel>();
        services.AddScoped<IPomodoroTimerViewModel, PomodoroTimerViewModel>();
        services.AddScoped<IMainWindowViewModel, MainWindowViewModel>();
        services.AddScoped<MainWindow>();
    }
}
