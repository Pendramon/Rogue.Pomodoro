using Rogue.Pomodoro.WPF.ViewModels.Base.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rogue.Pomodoro.WPF.ViewModels.Base;

public class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
