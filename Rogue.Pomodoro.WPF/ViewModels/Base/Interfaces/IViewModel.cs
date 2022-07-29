using System;

namespace Rogue.Pomodoro.WPF.ViewModels.Base.Interfaces;

public interface IViewModel
{
    event EventHandler<ChildMessageArgs> ChildMessage;
}
