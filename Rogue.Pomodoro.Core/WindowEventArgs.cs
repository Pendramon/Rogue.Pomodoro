using Rogue.Pomodoro.Core.Natives;
using System;

namespace Rogue.Pomodoro.Core;

public class WindowEventArgs : EventArgs
{
    public IntPtr WindowHandle { get; init; }

    public WindowEvent WindowEvent { get; init; }

    public ObjectIdentifier ObjectId { get; init; }
}
