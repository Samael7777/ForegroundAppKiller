using System;
using System.Runtime.InteropServices;

namespace HooksLib.WinAPI
{
    public static class Kernel32
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetModuleHandle(string name = null);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}