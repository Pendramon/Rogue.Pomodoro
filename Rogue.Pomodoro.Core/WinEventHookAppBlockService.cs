using Rogue.Pomodoro.Core.Interfaces;
using Rogue.Pomodoro.Core.Natives;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Rogue.Pomodoro.Core;

/// <summary>
/// Provides functionality for blocking apps by minimizing them when they get un-minimized.
/// Uses <see cref="WindowEvent.EVENT_OBJECT_CREATE"/> event to re-hook into a process's
/// current windows when a new window has been created using SetWinEventHook.
/// </summary>
public class WinEventHookAppBlockService : AppBlockServiceBase
{
    private readonly IWindowEventListener windowEventListener;

    public WinEventHookAppBlockService(IProcessWatcher processWatcher, IWindowEventListener windowEventListener)
    {
        processWatcher.Terminated += OnProcessTerminated;
        processWatcher.Started += OnProcessStarted;
        this.windowEventListener = windowEventListener;
        this.windowEventListener.Subscribe += WindowEventListener_Subscribe;
    }

    /// <inheritdoc />
    public override void Add(IBlockedApp app)
    {
        base.Add(app);

        var processes = Process.GetProcessesByName(app.Name);
        foreach (var process in processes)
        {
            if (IsBlocking)
            {
                NativeWrappers.MinimizeProcessWindows(process.Id);
            }

            windowEventListener.HookProcessWindows(process.Id, WindowEvent.EVENT_SYSTEM_MINIMIZEEND, WindowEvent.EVENT_OBJECT_CREATE);
        }
    }

    /// <inheritdoc />
    public override void Remove(IBlockedApp app)
    {
        base.Remove(app);

        var processes = Process.GetProcessesByName(app.Name);
        foreach (var process in processes)
        {
            windowEventListener.UnhookProcessWindows(process.Id, WindowEvent.EVENT_SYSTEM_MINIMIZEEND, WindowEvent.EVENT_OBJECT_CREATE);
        }
    }

    /// <inheritdoc />
    public override void Block()
    {
        base.Block();

        var processes = Process.GetProcesses().Where(p => BlockedApps.Any(a => a.Name == p.ProcessName));
        foreach (var process in processes)
        {
            NativeWrappers.MinimizeProcessWindows(process.Id);
        }
    }

    private void OnProcessTerminated(object? sender, ProcessEventArgs e)
    {
        if (BlockedApps.All(app => app.Name != Path.GetFileNameWithoutExtension(e.ProcessName)))
        {
            return;
        }

        windowEventListener.UnhookProcessWindows(e.ProcessId, WindowEvent.EVENT_SYSTEM_MINIMIZEEND, WindowEvent.EVENT_OBJECT_CREATE);
    }

    private void OnProcessStarted(object? sender, ProcessEventArgs e)
    {
        if (BlockedApps.All(app => app.Name != Path.GetFileNameWithoutExtension(e.ProcessName)))
        {
            return;
        }

        var process = Process.GetProcessById(e.ProcessId);
        if (IsBlocking)
        {
            NativeWrappers.MinimizeProcessWindows(process.Id);
        }

        windowEventListener.HookProcessWindows(e.ProcessId, WindowEvent.EVENT_SYSTEM_MINIMIZEEND, WindowEvent.EVENT_OBJECT_CREATE);
    }

    private void WindowEventListener_Subscribe(object? sender, WindowEventArgs e)
    {
        if (!IsBlocking)
        {
            return;
        }

        switch (e.WindowEvent)
        {
            case WindowEvent.EVENT_SYSTEM_MINIMIZEEND:
                NativeWrappers.ShowWindowAsync(e.WindowHandle, CmdShow.SW_MINIMIZE);
                break;
            case WindowEvent.EVENT_OBJECT_CREATE:
                if (e.ObjectId != ObjectIdentifier.OBJID_WINDOW)
                {
                    return;
                }

                var processId = NativeWrappers.GetProcessId(e.WindowHandle);
                if (processId == 0)
                {
                    return;
                }

                if (IsBlocking)
                {
                    NativeWrappers.MinimizeProcessWindows(processId);
                }

                windowEventListener.HookProcessWindows(processId, WindowEvent.EVENT_SYSTEM_MINIMIZEEND, WindowEvent.EVENT_OBJECT_CREATE);
                break;
        }
    }
}