using Rogue.Pomodoro.WPF.Commands;
using Rogue.Pomodoro.WPF.ViewModels.Base;
using Rogue.Pomodoro.WPF.ViewModels.Interfaces;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Rogue.Pomodoro.WPF.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    public MainWindowViewModel(IEnumerable<IMainContentViewModel> mainContentViewModels)
    {
        Title = "Rogue Pomodoro";
        MainContentViewModels = mainContentViewModels;
        RegisterChildren(MainContentViewModels);
        GoToHome = new RelayCommand(NavigateToHome);
        SetDefaultViewModel();
    }

    public string Title
    {
        get => GetProperty<string>() ?? string.Empty;
        private set => SetProperty(value);
    }

    public IMainContentViewModel CurrentMainContentViewModel
    {
        get => GetProperty<IMainContentViewModel>()!;
        private set => SetProperty(value);
    }

    public ICommand GoToHome { get; }

    private IEnumerable<IMainContentViewModel> MainContentViewModels { get; }

    protected override void OnChildMessage(object? sender, ChildMessageArgs e)
    {
        base.OnChildMessage(sender, e);
        switch (e.Key)
        {
            case ChildMessageType.ChangeMainContentView:
                ChangeMainContentView((Type)e.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e));
        }
    }

    private void NavigateToHome()
    {
        CurrentMainContentViewModel = MainContentViewModels.First(x => x is IHomeViewModel);
    }

    private void SetDefaultViewModel()
    {
        CurrentMainContentViewModel = MainContentViewModels.First(x => x is IHomeViewModel);
    }

    private void ChangeMainContentView(Type type)
    {
        CurrentMainContentViewModel = MainContentViewModels.First(type.IsInstanceOfType);
    }
}
