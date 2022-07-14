using System;

namespace Rogue.Pomodoro.Core.Interfaces;

public interface IPomodoroTimer
{
    public TimeSpan PomodoroLength { get; }

    public TimeSpan RemainingTime { get; }

    public bool IsRunning { get; }

    public void Start();

    public void Stop();

    public void SetPomodoroLength(TimeSpan time);
}
