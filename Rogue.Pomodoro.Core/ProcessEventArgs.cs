using System;

namespace Rogue.Pomodoro.Core;

public class ProcessEventArgs : EventArgs
{
    public ProcessEventArgs(int processId, string processName)
    {
        ProcessId = processId;
        ProcessName = processName;
    }

    public int ProcessId { get; }

    public string ProcessName { get; }
}