using System;
using System.ComponentModel;
using System.Windows;
using ForegroundAppKiller.Models;
using ForegroundAppKiller.ViewModels;
using ForegroundAppKiller.Views;
using Hardcodet.Wpf.TaskbarNotification;

namespace ForegroundAppKiller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        private readonly Settings _settingsWindow;
        private readonly TaskbarIcon _taskbarIcon;
        private readonly MainModel _mainModel;

        public App()
        {
            InitializeComponent();

            var storage = new ShortcutStorage();

            _mainModel = new MainModel
            {
                CurrentShortcut = storage.GetShortcut()
            };

            var settingsVm = new SettingsViewModel(storage, HideSettings);
            settingsVm.PropertyChanged += OnShortcutChanged;

            _settingsWindow = new Settings
            {
                DataContext = settingsVm
            };

            var taskbarVm = new TaskbarIconViewModel(ShowSettings, Current.Shutdown);
            _taskbarIcon = Current.Resources["TaskbarIcon"] as TaskbarIcon
                          ?? throw new ApplicationException();
            
            _taskbarIcon.DataContext = taskbarVm;
        }

        private void OnShortcutChanged(object? sender, PropertyChangedEventArgs e)
        {
            var vm = sender as SettingsViewModel
                     ?? throw new ApplicationException();

            if (e.PropertyName != nameof(SettingsViewModel.CurrentShortcut))
                return;

            _mainModel.CurrentShortcut = vm.CurrentShortcut;
        }

        private void ShowSettings()
        {
            _mainModel.PauseEnable();
            _settingsWindow.Show();
        }

        private void HideSettings()
        {
            _settingsWindow.Hide();
            _mainModel.PauseDisable();
        }

        void OnStartup(object sender, StartupEventArgs e)
        {

        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _mainModel.Dispose();
            _taskbarIcon.Dispose();
        }
    }
}
