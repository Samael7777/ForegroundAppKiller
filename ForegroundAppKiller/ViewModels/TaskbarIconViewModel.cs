using System;

namespace ForegroundAppKiller.ViewModels;

public class TaskbarIconViewModel
{
    public RelayCommand ShowSettingsCommand { get; }
    public RelayCommand ExitApplicationCommand { get; }

    public TaskbarIconViewModel(Action showSettings, Action exitApp)
    {
        ShowSettingsCommand = new RelayCommand((_) => showSettings());
        ExitApplicationCommand = new RelayCommand((_) => exitApp());
    }
}