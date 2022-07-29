using Rogue.Pomodoro.Core.Interfaces;
using Rogue.Pomodoro.Core.Natives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Rogue.Pomodoro.Core;

/// <summary>
/// Listens for window events using SetWindowHook.
/// For the hooking to work you must pass a SynchronizationContext for which the context must have a windows message loop.
/// See DispatcherSynchronizationContext for WPF apps and see WindowsFormsSynchronizationContext for a WinForms app.
/// </summary>
internal class WindowEventListener : IWindowEventListener
{
    private readonly List<WindowEventHookInfo> hookedWindows = new();

    private readonly SynchronizationContext synchronizationContext;

    private GCHandle gcHandle;

    public WindowEventListener(SynchronizationContext synchronizationContext)
    {
        this.synchronizationContext = synchronizationContext;
        WindowEventDelegate = OnWindowEvent;
        gcHandle = GCHandle.Alloc(WindowEventDelegate);
    }

    /// <summary>
    /// Used to subscribe or unsubscribe to registered window events from hooked windows.
    /// </summary>
    public event EventHandler<WindowEventArgs>? Subscribe;

    private event NativeWrappers.WindowEventDelegate? WindowEventDelegate;

    /// <summary>
    /// Hooks into a process's current windows and listens for specified window events in each.
    /// This is an expensive operation, you should avoid hooking many events.
    /// </summary>
    /// <param name="processId">The process containing all the windows to hook.</param>
    /// <param name="windowEvents">The window events to listen to for each window.</param>
    public void HookProcessWindows(int processId, params WindowEvent[] windowEvents)
    {
        if (windowEvents.Length == 0)
        {
            return;
        }

        synchronizationContext.Send(
            (_) =>
            {
                foreach (var windowEvent in windowEvents)
                {
                    // If called on an already hooked window with same window event it should be a noop.
                    // With that said if some of the process's windows isn't hooked it will hook it when called again.
                    var windowEventHook = NativeWrappers.HookOne(windowEvent, WindowEventDelegate!, (uint)processId, 0);

                    var existingHookedWindow = hookedWindows.FirstOrDefault(x => x.ProcessId == processId && x.WindowEvent == windowEvent);
                    if (existingHookedWindow is not null)
                    {
                        return;
                    }

                    hookedWindows.Add(new WindowEventHookInfo(
                        processId,
                        windowEventHook,
                        windowEvent));
                }
            },
            null);
    }

    /// <summary>
    /// Unhooks window events from a process's current windows.
    /// </summary>
    /// <param name="processId">The process containing all the windows to unhook from.</param>
    /// <param name="windowEvents">The window events to stop listening for.</param>
    public void UnhookProcessWindows(int processId, params WindowEvent[] windowEvents)
    {
        if (windowEvents.Length == 0)
        {
            return;
        }

        synchronizationContext.Send(
            (_) =>
            {
                foreach (var windowEvent in windowEvents)
                {
                    var hookedWindow = hookedWindows.FirstOrDefault(hookedWindow =>
                        hookedWindow.ProcessId == processId && hookedWindow.WindowEvent == windowEvent);
                    if (hookedWindow is null)
                    {
                        return;
                    }

                    hookedWindows.Remove(hookedWindow);
                    NativeWrappers.UnhookWindowEvent(hookedWindow.WindowEventHook);
                }
            },
            null);
    }

    /// <summary>
    /// Unhooks all windows from all hooked processes.
    /// </summary>
    public void UnhookAllWindows()
    {
        foreach (var hookedWindow in hookedWindows)
        {
            UnhookProcessWindows(hookedWindow.ProcessId, hookedWindow.WindowEvent);
        }
    }

    /// <summary>
    /// Disposes the gc handle to the window event delegate and unhooks all windows.
    /// </summary>
    public void Dispose()
    {
        gcHandle.Free();
        UnhookAllWindows();
        GC.SuppressFinalize(this);
    }

    private void OnWindowEvent(IntPtr hWinEventHook, WindowEvent eventType, IntPtr hwnd, ObjectIdentifier idObject, long idChild, uint dwEventThread, uint dwmsEventTime)
    {
        Subscribe?.Invoke(null, new WindowEventArgs()
        {
            WindowHandle = hwnd,
            WindowEvent = eventType,
            ObjectId = idObject,
        });
    }
}