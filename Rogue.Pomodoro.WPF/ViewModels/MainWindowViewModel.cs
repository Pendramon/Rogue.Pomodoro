using Rogue.Pomodoro.WPF.ViewModels.Base;
using Rogue.Pomodoro.WPF.ViewModels.Base.Interfaces;
using Rogue.Pomodoro.WPF.ViewModels.Interfaces;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Rogue.Pomodoro.WPF.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private string title = "Rogue Pomodoro";

    private IViewModel? previousMainContentViewModel;

    private IViewModel currentMainContentViewModel;

    public MainWindowViewModel(IMainContentViewModel mainContentViewModel)
    {
        ViewModelsList = new List<IViewModel>()
        {
            mainContentViewModel,
        };
        SetDefaultViewModel();
    }

    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                OnPropertyChanged();
            }

            title = value;
        }
    }

    public IViewModel CurrentMainContentViewModel
    {
        get => currentMainContentViewModel;
        set
        {
            if (value == currentMainContentViewModel)
            {
                return;
            }

            previousMainContentViewModel = currentMainContentViewModel;
            currentMainContentViewModel = value;
            OnPropertyChanged();
        }
    }

    private List<IViewModel> ViewModelsList { get; }

    [MemberNotNull(nameof(currentMainContentViewModel))]
    private void SetDefaultViewModel()
    {
        currentMainContentViewModel = ViewModelsList.First(x => x is IMainContentViewModel);
    }
}
