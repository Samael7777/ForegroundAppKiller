using System;
using System.Runtime.InteropServices;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo

namespace HooksLib.WinAPI
{
    // KBDLLHOOKSTRUCT
// https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms644967(v=vs.85).aspx
    public struct KeyboardHookStruct
    {
        public uint VKCode;
        public uint ScanCode;
        public uint Flags;
        public uint Time;
        public IntPtr dwExtraInfo;
    }

// MSLLHOOKSTRUCT
// https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms644970(v=vs.85).aspx
    public struct MouseHookStruct
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr dwExtraInfo;
    }

// Константы WH_*
    public enum WindowsHook 
    {
        KeyboardLowLevel = 13,
        MouseLowLevel = 14,
    }

// Константы WM_*
    public enum WindowsMessage 
    {
        KeyDown = 0x100,
        KeyUp = 0x101,

        SysKeyDown = 0x104,
        SysKeyUp = 0x105,

        MouseMove = 0x200,
        LeftButtonDown = 0x201,
        LeftButtonUp = 0x202,

        RightButtonDown = 0x204,
        RightButtonUp = 0x205,

        MiddleButtonDown = 0x207,
        MiddleButtonUp = 0x208,
    }

    public delegate int KeyboardHookProc(int code,
        WindowsMessage wParam, ref KeyboardHookStruct lParam);

    public delegate int MouseHookProc(int code,
        WindowsMessage wParam, ref MouseHookStruct lParam);

    public static class User32
    {
        [DllImport("user32")]
        public static extern int CallNextHookEx(IntPtr hHk, int nCode,
            WindowsMessage wParam, ref KeyboardHookStruct lParam);

        [DllImport("user32")]
        public static extern int CallNextHookEx(IntPtr hHk, int nCode,
            WindowsMessage wParam, ref MouseHookStruct lParam);

        [DllImport("user32")]
        public static extern IntPtr SetWindowsHookEx(WindowsHook idHook,
            KeyboardHookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32")]
        public static extern IntPtr SetWindowsHookEx(WindowsHook idHook,
            MouseHookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32")]
        public static extern bool UnhookWindowsHookEx(IntPtr hHk);
    }
}