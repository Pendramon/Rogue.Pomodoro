using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Rogue.Pomodoro.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        this.serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var bootstrapper = serviceProvider.GetRequiredService<Bootstrapper>();
        await bootstrapper.InitializeAsync();
    }
}
