using System;
using HooksLib.WinAPI;

namespace HooksLib
{
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    public sealed class GlobalHook : IDisposable
    {
        // Дескрипторы хуков
        private readonly IntPtr _keyboardHookHandle;
        private readonly IntPtr _mouseHookHandle;

        // Хуки
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly KeyboardHookProc _keyboardCallback;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly MouseHookProc _mouseCallback;

        // События
        
        // ReSharper disable UnusedParameter.Local
        public event KeyEventHandler KeyDown = (s, e) => { };
        public event KeyEventHandler KeyUp = (s, e) => { };
        public event MouseEventHandler MouseButtonDown = (s, e) => { };
        public event MouseEventHandler MouseButtonUp = (s, e) => { };
        public event MouseEventHandler MouseMove = (s, e) => { };
        // ReSharper restore UnusedParameter.Local

        public GlobalHook(bool useKeyboardHook = true, bool useMouseHook = true)
        {

            _keyboardCallback = KeyboardHookProc;
            _mouseCallback = MouseHookProc;
            
            // В SetWindowsHookEx следует передать дескриптор библиотеки user32.dll
            // Библиотека user32 всё равно всегда загружена в приложениях .NET,
            // хранить и освобождать дескриптор или что-либо ещё с ним делать нет необходимости
            var user32Handle = Kernel32.LoadLibrary("user32");
           
            // Установим хуки
            if (useKeyboardHook)
            {
                _keyboardHookHandle = User32.SetWindowsHookEx(
                    WindowsHook.KeyboardLowLevel, _keyboardCallback, user32Handle, 0);
            }

            if (useMouseHook)
            {
                _mouseHookHandle = User32.SetWindowsHookEx(
                    WindowsHook.MouseLowLevel, _mouseCallback, user32Handle, 0);
            }
        }

        private int KeyboardHookProc(int code, WindowsMessage wParam, ref KeyboardHookStruct lParam)
        {

            if (code < 0) 
                return User32.CallNextHookEx(_keyboardHookHandle, code, wParam, ref lParam);
            
            var key = (Keys)lParam.VKCode;
            var eventArgs = new KeyEventArgs(key);

            switch (wParam)
            {
                case WindowsMessage.KeyDown:
                case WindowsMessage.SysKeyDown:
                    KeyDown(this, eventArgs);
                    break;

                case WindowsMessage.KeyUp:
                case WindowsMessage.SysKeyUp:
                    KeyUp(this, eventArgs);
                    break;
            }

            return eventArgs.Handled
                ? 1
                : User32.CallNextHookEx(_keyboardHookHandle, code, wParam, ref lParam);
        }

        private int MouseHookProc(int code, WindowsMessage wParam, ref MouseHookStruct lParam)
        {
            if (code < 0) 
                return User32.CallNextHookEx(_mouseHookHandle, code, wParam, ref lParam);

            switch (wParam)
            {
                case WindowsMessage.MouseMove:
                    MouseMove(this,
                        new MouseEventArgs(MouseButtons.None, 0, lParam.X, lParam.Y, 0));
                    break;

                case WindowsMessage.LeftButtonDown:
                    MouseButtonDown(this,
                        new MouseEventArgs(MouseButtons.Left, 0, lParam.X, lParam.Y, 0));
                    break;

                case WindowsMessage.RightButtonDown:
                    MouseButtonDown(this,
                        new MouseEventArgs(MouseButtons.Right, 0, lParam.X, lParam.Y, 0));
                    break;

                case WindowsMessage.MiddleButtonDown:
                    MouseButtonDown(this,
                        new MouseEventArgs(MouseButtons.Middle, 0, lParam.X, lParam.Y, 0));
                    break;

                case WindowsMessage.LeftButtonUp:
                    MouseButtonUp(this,
                        new MouseEventArgs(MouseButtons.Left, 0, lParam.X, lParam.Y, 0));
                    break;

                case WindowsMessage.RightButtonUp:
                    MouseButtonUp(this,
                        new MouseEventArgs(MouseButtons.Right, 0, lParam.X, lParam.Y, 0));
                    break;

                case WindowsMessage.MiddleButtonUp:
                    MouseButtonUp(this,
                        new MouseEventArgs(MouseButtons.Middle, 0, lParam.X, lParam.Y, 0));
                    break;
            }

            return User32.CallNextHookEx(_mouseHookHandle, code, wParam, ref lParam);
        }

        #region IDisposable implementation

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            // Удалим хуки
            if (_keyboardHookHandle != IntPtr.Zero)
                User32.UnhookWindowsHookEx(_keyboardHookHandle);

            if(_mouseHookHandle != IntPtr.Zero)
                User32.UnhookWindowsHookEx(_mouseHookHandle);
        }

        ~GlobalHook()
        {
            Dispose();
        }

        #endregion
    }
}