using Rogue.Pomodoro.WPF.ViewModels.Interfaces;
using Rogue.Pomodoro.WPF.Views;
using System.Threading.Tasks;

namespace Rogue.Pomodoro.WPF;

public class Bootstrapper
{
    private readonly MainWindow mainWindow;
    private readonly IMainWindowViewModel mainWindowViewModel;

    public Bootstrapper(MainWindow mainWindow, IMainWindowViewModel mainWindowViewModel)
    {
        this.mainWindow = mainWindow;
        this.mainWindowViewModel = mainWindowViewModel;
    }

    public async Task InitializeAsync()
    {
        mainWindow.DataContext = mainWindowViewModel;
        mainWindow.Show();
    }
}
