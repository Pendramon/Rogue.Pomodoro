using System;

namespace Rogue.Pomodoro.Core.Interfaces;

public interface IProcessWatcher : IDisposable
{
    event EventHandler<ProcessEventArgs> Started;

    event EventHandler<ProcessEventArgs> Terminated;
}
