using Rogue.Pomodoro.Core.Natives;
using System;

namespace Rogue.Pomodoro.Core;

public class WindowEventHookInfo
{
    public WindowEventHookInfo(int processId, IntPtr windowEventHook, WindowEvent windowEvent)
    {
        ProcessId = processId;
        WindowEventHook = windowEventHook;
        WindowEvent = windowEvent;
    }

    public int ProcessId { get; }

    public IntPtr WindowEventHook { get; }

    public WindowEvent WindowEvent { get; }
}
