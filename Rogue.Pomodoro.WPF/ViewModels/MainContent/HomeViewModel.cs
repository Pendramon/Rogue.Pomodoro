using Rogue.Pomodoro.WPF.Commands;
using Rogue.Pomodoro.WPF.ViewModels.Base;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using System.Windows.Input;

namespace Rogue.Pomodoro.WPF.ViewModels.MainContent;

public class HomeViewModel : ViewModelBase, IHomeViewModel
{
    public HomeViewModel()
    {
        GoToSettings = new RelayCommand(NavigateToAppBlock);
        GoToPomodoroTimer = new RelayCommand(NavigateToPomodoroTimer);
    }

    public ICommand GoToSettings { get; }

    public ICommand GoToPomodoroTimer { get; }

    private void NavigateToAppBlock()
    {
        SendMessage(ChildMessageType.ChangeMainContentView, typeof(ISettingsViewModel));
    }

    private void NavigateToPomodoroTimer()
    {
        SendMessage(ChildMessageType.ChangeMainContentView, typeof(IPomodoroTimerViewModel));
    }
}
