using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ForegroundAppKiller.Models;
using HooksLib;
using Key = System.Windows.Input.Key;


namespace ForegroundAppKiller.ViewModels;

internal class SettingsViewModel : BaseObservable
{
    private bool _isChanging;
    private string _shortcutText;
    private string _changeBtnText;
    private KeyboardShortcut _currentShortcut;

    private readonly IShortcutStorage _storage;
    private readonly Action? _onFinished;

    private HashSet<Key> _keyModifiers;
    private Key _key;

    public KeyboardShortcut CurrentShortcut
    {
        get => _currentShortcut;
        private set => SetField(ref _currentShortcut, value);
    }

    public string ShortcutText
    {
        get => _shortcutText;
        set => SetField(ref _shortcutText, value);
    }

    public string ChangeBtnText
    {
        get => _changeBtnText;
        set => SetField(ref _changeBtnText, value);
    }

    public bool ChangeBtnIsEnabled
    {
        get => !_isChanging;
        set => SetField(ref _isChanging, !value);
    }

    public RelayCommand? ChangeShortcut { get; }
    public RelayCommand? SaveSettings { get; }
    public RelayCommand? CloseWindowCommand { get; }
    public RelayCommand? OnKeyDownCommand { get; }
    public RelayCommand? OnKeyUpCommand { get; }

    public SettingsViewModel(IShortcutStorage storage, Action? onFinished = null)
    {
        _currentShortcut = new KeyboardShortcut();
        _shortcutText = "";
        _key = Key.None;
        _keyModifiers = new HashSet<Key>();
        _changeBtnText = "Change";
        _isChanging = false;
        _storage = storage;
        _onFinished = onFinished;
        
        ChangeShortcut = new RelayCommand(_ => ChangeShortcutCommand());
        SaveSettings = new RelayCommand(_ => SaveSettingsCommand());
        CloseWindowCommand = new RelayCommand(_ => CloseWindow());
        OnKeyDownCommand = new RelayCommand(KeyDownCommand);
        OnKeyUpCommand = new RelayCommand(KeyUpCommand);

        GetShortcutFromStorage();
        UpdateShortcutText();
    }

    private void GetShortcutFromStorage()
    {
        var shortcut = _storage.GetShortcut();
        if (shortcut.Key == Key.None) 
            return;
        CurrentShortcut = shortcut;

        _key = shortcut.Key;
        _keyModifiers = new HashSet<Key>(shortcut.Modifiers);
    }

    private void KeyDownCommand(object? o)
    {
        if (!_isChanging) return;
        if (o is not KeyEventArgs args)
            return;
        
        args.Handled = true;
        var key = args.Key == Key.System ? args.SystemKey : args.Key;
       
        if (KeyHelper.IsModifier(key.ToHookKey()))
        {
            _keyModifiers.Add(key);
        }
        else
        {
            _key = key;
            ShortcutChanged();
        }

        UpdateShortcutText();
    }

    private void KeyUpCommand(object? o)
    {
        if (!_isChanging) return;
        if (o is not KeyEventArgs args)
            return;
        
        args.Handled = true;
        var key = args.Key == Key.System ? args.SystemKey : args.Key;
        
        if (KeyHelper.IsModifier(key.ToHookKey()))
        {
            _keyModifiers.Remove(key);
        }
        else if (_key == key)
        {
            _key = Key.None;
        }

        UpdateShortcutText();
    }

    private void UpdateShortcutText()
    {
        var mods = _keyModifiers.Select(k=>k.ToString()).ToArray();
        var shortcut = string.Join(" + ", mods);
        shortcut += _key == Key.None 
            ? "" 
            : (mods.Any() ? " + " : "") + $"{_key}";
        ShortcutText = shortcut;
    }

    private void ChangeShortcutCommand()
    {
        ChangeBtnText = "Changing...";
        ChangeBtnIsEnabled = false;
        _key = Key.None;
        _keyModifiers.Clear();
    }

    private void ShortcutChanged()
    {
        ChangeBtnText = "Change";
        ChangeBtnIsEnabled = true;
    }

    private void SaveSettingsCommand()
    {
        CurrentShortcut = new KeyboardShortcut(_key, _keyModifiers);
        _storage.SaveShortcut(CurrentShortcut);
        CloseWindow();
    }

    private void CloseWindow()
    {
        _onFinished?.Invoke();
    }
}