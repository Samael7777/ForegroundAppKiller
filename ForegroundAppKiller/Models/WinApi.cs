using System;
using System.Runtime.InteropServices;

namespace ForegroundAppKiller.Models;

internal static class WinApi
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int procId);
}