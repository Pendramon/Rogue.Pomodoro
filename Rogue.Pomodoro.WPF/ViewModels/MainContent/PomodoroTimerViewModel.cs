using Rogue.Pomodoro.Core.Interfaces;
using Rogue.Pomodoro.WPF.Commands;
using Rogue.Pomodoro.WPF.ViewModels.Base;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using System.Windows.Input;

namespace Rogue.Pomodoro.WPF.ViewModels.MainContent;

public class PomodoroTimerViewModel : ViewModelBase, IMainContentViewModel
{
    public PomodoroTimerViewModel(IPomodoroTimer pomodoroTimer)
    {
        Timer = pomodoroTimer;
        TogglePomodoroTimer = new RelayCommand(ToggleTimer);
    }

    public IPomodoroTimer Timer { get; }

    public ICommand TogglePomodoroTimer { get; set; }

    public void ToggleTimer()
    {
        if (Timer.IsRunning)
        {
            Timer.Stop();
            return;
        }

        Timer.Start();
    }
}
