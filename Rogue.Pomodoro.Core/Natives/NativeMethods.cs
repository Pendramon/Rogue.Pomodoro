using System;
using System.Runtime.InteropServices;

namespace Rogue.Pomodoro.Core.Natives;

internal static class NativeMethods
{
    [DllImport("user32.dll")]
    internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    internal static extern IntPtr SetWinEventHook(
        WindowEvent eventMin,
        WindowEvent eventMax,
        IntPtr hmodWinEventProc,
        NativeWrappers.WindowEventDelegate lpfnWinEventProc,
        uint idProcess,
        uint idThread,
        SetWinEventHookFlags dwFlags);

    [DllImport("user32.dll")]
    internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll")]
    internal static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    internal static extern bool EnumThreadWindows(int dwThreadId, NativeWrappers.EnumThreadDelegate lpfn, IntPtr lParam);

    [DllImport("dwmapi.dll")]
    internal static extern int DwmSetWindowAttribute(IntPtr hwnd, WindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);
}