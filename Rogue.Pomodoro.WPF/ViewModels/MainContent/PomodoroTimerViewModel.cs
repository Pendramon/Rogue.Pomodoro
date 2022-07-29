using Rogue.Pomodoro.Core.Interfaces;
using Rogue.Pomodoro.WPF.Commands;
using Rogue.Pomodoro.WPF.ViewModels.Base;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using System.Windows.Input;

namespace Rogue.Pomodoro.WPF.ViewModels.MainContent;

public class PomodoroTimerViewModel : ViewModelBase, IPomodoroTimerViewModel
{
    private readonly IAppBlockService appBlockService;

    public PomodoroTimerViewModel(IPomodoroTimer pomodoroTimer, IAppBlockService appBlockService)
    {
        this.appBlockService = appBlockService;
        Timer = pomodoroTimer;
        TogglePomodoroTimer = new RelayCommand(ToggleTimer);
        Timer.Finished += Timer_Finished;
    }

    public IPomodoroTimer Timer { get; }

    public ICommand TogglePomodoroTimer { get; }

    private void ToggleTimer()
    {
        if (Timer.IsRunning)
        {
            Timer.Stop();
            appBlockService.Unblock();
            return;
        }

        Timer.Start();
        appBlockService.Block();
    }

    private void Timer_Finished(object? sender, System.EventArgs e)
    {
        appBlockService.Unblock();
    }
}
