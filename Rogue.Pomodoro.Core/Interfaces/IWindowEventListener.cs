using Rogue.Pomodoro.Core.Natives;
using System;

namespace Rogue.Pomodoro.Core.Interfaces;

public interface IWindowEventListener : IDisposable
{
    public event EventHandler<WindowEventArgs>? Subscribe;

    public void HookProcessWindows(int processId, params WindowEvent[] windowEvents);

    public void UnhookProcessWindows(int processId, params WindowEvent[] windowEvents);

    public void UnhookAllWindows();
}
