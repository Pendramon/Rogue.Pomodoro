using Microsoft.Extensions.DependencyInjection;
using Rogue.Pomodoro.WPF.ViewModels;
using Rogue.Pomodoro.WPF.ViewModels.Interfaces;
using Rogue.Pomodoro.WPF.ViewModels.MainContent;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using Rogue.Pomodoro.WPF.Views;
using System.Windows;

namespace Rogue.Pomodoro.WPF;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        serviceProvider = services.BuildServiceProvider();
    }

    protected async override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var bootstrapper = serviceProvider.GetRequiredService<Bootstrapper>();
        await bootstrapper.InitializeAsync();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<Bootstrapper>();
        services.AddSingleton<IMainContentViewModel, MainContentViewModel>();
        services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
    }
}
