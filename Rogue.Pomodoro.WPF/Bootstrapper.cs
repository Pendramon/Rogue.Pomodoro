using System.Threading.Tasks;

namespace Rogue.Pomodoro.WPF;

public class Bootstrapper
{
    private readonly MainWindow mainWindow;

    public Bootstrapper(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    public async Task InitializeAsync()
    {
        mainWindow.Show();
    }
}
