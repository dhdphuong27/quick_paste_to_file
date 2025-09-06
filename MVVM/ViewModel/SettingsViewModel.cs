using Microsoft.Win32;
using PasteToFile.Core;
using PasteToFile.MVVM.Model;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PasteToFile.MVVM.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private AppSettings _settings;
        public Action OnSettingsApplied { get; set; }

        public SettingsViewModel()
        {
            _settings = SettingsManager.Instance;

            // Initialize commands
            BrowseFolderCommand = new RelayCommand(BrowseFolder);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ResetSettingsCommand = new RelayCommand(ResetSettings);
        }

        // Property bindings - these bind directly to the settings
        public bool StartWithWindows
        {
            get => _settings.StartWithWindows;
            set { _settings.StartWithWindows = value; OnPropertyChanged(); }
        }

        public bool MinimizeToTray
        {
            get => _settings.MinimizeToTray;
            set { _settings.MinimizeToTray = value; OnPropertyChanged(); }
        }

        public bool ShowNotifications
        {
            get => _settings.ShowNotifications;
            set { _settings.ShowNotifications = value; OnPropertyChanged(); }
        }

        public bool AutoSaveClipboard
        {
            get => _settings.AutoSaveClipboard;
            set { _settings.AutoSaveClipboard = value; OnPropertyChanged(); }
        }

        public bool CreateDateFolders
        {
            get => _settings.CreateDateFolders;
            set { _settings.CreateDateFolders = value; OnPropertyChanged(); }
        }

        public bool OverwriteFiles
        {
            get => _settings.OverwriteFiles;
            set { _settings.OverwriteFiles = value; OnPropertyChanged(); }
        }

        public bool IncludeFileExtension
        {
            get => _settings.IncludeFileExtension;
            set { _settings.IncludeFileExtension = value; OnPropertyChanged(); }
        }

        public string OutputFolderPath
        {
            get => _settings.OutputFolderPath;
            set { _settings.OutputFolderPath = value; OnPropertyChanged(); }
        }

        public bool EnableDebugLogging
        {
            get => _settings.EnableDebugLogging;
            set { _settings.EnableDebugLogging = value; OnPropertyChanged(); }
        }

        public bool AutoCheckUpdates
        {
            get => _settings.AutoCheckUpdates;
            set { _settings.AutoCheckUpdates = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand BrowseFolderCommand { get; private set; }
        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand ResetSettingsCommand { get; private set; }

        private void BrowseFolder()
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog()
            {
                Title = "Select Output Folder",
                InitialDirectory = OutputFolderPath
            };

            if (dialog.ShowDialog() == true)
            {
                OutputFolderPath = dialog.FolderName;
            }
        }

        private void SaveSettings()
        {
            if (SettingsManager.SaveSettings())
            {
                MessageBox.Show("Settings saved successfully!", "Settings",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                ForceRefresh();
            }
        }

        private void ResetSettings()
        {
            var result = MessageBox.Show("Are you sure you want to reset all settings to default values?",
                                       "Reset Settings", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SettingsManager.ResetToDefaults();
                // Refresh all properties
                OnPropertyChanged(string.Empty);
                MessageBox.Show("Settings reset to defaults!", "Settings",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                ForceRefresh();
            }
        }
        private void ForceRefresh()
        {
            OnSettingsApplied?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

