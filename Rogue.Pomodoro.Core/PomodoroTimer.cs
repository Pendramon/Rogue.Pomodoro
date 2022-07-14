using Rogue.Pomodoro.Core.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Rogue.Pomodoro.Core;

public class PomodoroTimer : IPomodoroTimer, INotifyPropertyChanged
{
    private TimeSpan pomodoroLength = TimeSpan.FromMinutes(25);

    private TimeSpan remainingTime;

    public PomodoroTimer()
    {
        RemainingTime = PomodoroLength;
        Timer.Interval = 1000;
        Timer.Elapsed += OnTick;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public TimeSpan PomodoroLength
    {
        get
        {
            return pomodoroLength;
        }

        private set
        {
            pomodoroLength = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan RemainingTime
    {
        get
        {
            return remainingTime;
        }

        private set
        {
            remainingTime = value;
            OnPropertyChanged();
        }
    }

    public bool IsRunning => Timer.Enabled;

    private Timer Timer { get; set; } = new Timer();

    private DateTime StartTime { get; set; }

    public void Start()
    {
        if (Timer.Enabled)
        {
            return;
        }

        Timer.Start();
        StartTime = DateTime.Now;
        RemainingTime = PomodoroLength;
        OnPropertyChanged(nameof(IsRunning));
    }

    public void Stop()
    {
        Timer.Stop();
        OnPropertyChanged(nameof(IsRunning));
    }

    public void SetPomodoroLength(TimeSpan time)
    {
        if (time < TimeSpan.FromDays(1) && time >= TimeSpan.FromMinutes(1))
        {
            throw new ArgumentOutOfRangeException(nameof(time), "Invalid Pomodoro Length. Valid range is from a minute to less than a day.");
        }

        PomodoroLength = time;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (RemainingTime <= TimeSpan.Zero)
        {
            Stop();
            RemainingTime = PomodoroLength;
            return;
        }

        var time = PomodoroLength - (DateTime.Now - StartTime);
        RemainingTime = TimeSpan.FromSeconds(Math.Round(time.TotalSeconds));
    }
}