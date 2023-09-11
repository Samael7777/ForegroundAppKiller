using System;
using System.Diagnostics;
using System.Linq;
using HooksLib;

namespace ForegroundAppKiller.Models;

public class MainModel : IDisposable
{
    private readonly KeyboardHook _keyboardHook;
    private readonly KeyCombinationHook _keyCombinationHook;
    private bool _isPaused;

    public KeyboardShortcut CurrentShortcut { get; set; }
    
    public MainModel()
    {
        _keyboardHook = new KeyboardHook();
        _keyCombinationHook = new KeyCombinationHook(_keyboardHook);
        _keyCombinationHook.OnKeyPressed += OnKeyPressed;
        CurrentShortcut = new KeyboardShortcut();
        _isPaused = false;
    }

    public void PauseEnable() => _isPaused = true;
    public void PauseDisable() => _isPaused = false;

    private void OnKeyPressed(object sender, KeyPressedEventArgs args)
    {
        if(_isPaused || CurrentShortcut.Key == (int)Key.None) 
            return;

        var pressedKey = args.PressedKey.ToWpfKey();
        var pressedMods = args.Modifiers.Select(key => key.ToWpfKey());
        var pressedCombo = new KeyboardShortcut(pressedKey, pressedMods);
        if (CurrentShortcut.Equals(pressedCombo))
        {
            KillForegroundWindow();
        }
    }

    private static void KillForegroundWindow()
    {
        var winHandle = WinApi.GetForegroundWindow();
        _ = WinApi.GetWindowThreadProcessId(winHandle, out var windowProcessId);
        var foregroundProcess = Process.GetProcessById(windowProcessId);
        var selfProcessId = Process.GetCurrentProcess().Id;
        
        if (selfProcessId == foregroundProcess.Id)
            return;
        
        foregroundProcess.Kill(true);
    }

    #region Dispose
        
    private bool _disposed;

    ~MainModel()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            //dispose managed state (managed objects)
            _keyboardHook.Dispose();
        }

        //free unmanaged resources (unmanaged objects) and override finalizer
        //set large fields to null
        _disposed = true;
    }

    #endregion
}