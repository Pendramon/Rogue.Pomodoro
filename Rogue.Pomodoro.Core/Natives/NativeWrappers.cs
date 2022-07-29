using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rogue.Pomodoro.Core.Natives;

public static class NativeWrappers
{
    private const SetWinEventHookFlags WinEventHookInternalFlags =
           SetWinEventHookFlags.WINEVENT_OUTOFCONTEXT |
           SetWinEventHookFlags.WINEVENT_SKIPOWNPROCESS |
           SetWinEventHookFlags.WINEVENT_SKIPOWNTHREAD;

    internal delegate void WindowEventDelegate(
        IntPtr hWinEventHook,
        WindowEvent eventType,
        IntPtr hwnd,
        ObjectIdentifier idObject,
        long idChild,
        uint dwEventThread,
        uint dwmsEventTime);

    internal delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

    internal static IntPtr HookOne(
        WindowEvent eventId,
        WindowEventDelegate eventDelegate,
        uint idProcess,
        uint idThread)
    {
        return NativeMethods.SetWinEventHook(
            eventId,
            eventId,
            IntPtr.Zero,
            eventDelegate,
            idProcess,
            idThread,
            WinEventHookInternalFlags);
    }

    internal static bool UnhookWindowEvent(IntPtr hWinEventHook)
    {
        return NativeMethods.UnhookWinEvent(hWinEventHook);
    }

    internal static int GetProcessId(IntPtr windowHandle)
    {
        _ = NativeMethods.GetWindowThreadProcessId(windowHandle, out var processId);
        return processId;
    }

    internal static void ShowWindowAsync(IntPtr windowHandle, CmdShow cmdShow)
    {
        NativeMethods.ShowWindowAsync(windowHandle, (int)cmdShow);
    }

    internal static void MinimizeProcessWindows(int processId)
    {
        var childWindows = EnumerateProcessWindowHandles(processId);
        foreach (var childWindow in childWindows)
        {
            var style = NativeMethods.GetWindowLong(childWindow, -16);
            if ((style & 0x10000000L) != 0x10000000L)
            {
                continue;
            }

            ShowWindowAsync(childWindow, CmdShow.SW_MINIMIZE);
        }
    }

    private static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
    {
        var handles = new List<IntPtr>();
        try
        {
            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
            {
                NativeMethods.EnumThreadWindows(
                    thread.Id,
                    (hWnd, lParam) =>
                    {
                        handles.Add(hWnd);
                        return true;
                    },
                    IntPtr.Zero);
            }
        }
        catch (ArgumentException)
        {
            // The process doesn't exist.
        }
        catch (InvalidOperationException)
        {
            // The process was not started by this object(?).
            // Bad documentation on MSDN, I suppose it never throws this exception based on a glance at the source code.
        }

        return handles;
    }
}
