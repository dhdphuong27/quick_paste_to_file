using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteToFile.Core
{
    public class AppSettings : INotifyPropertyChanged
    {
        // General Settings
        private bool _startWithWindows = false;
        private bool _minimizeToTray = true;
        private bool _showNotifications = false;
        private bool _soundEffectWhenSaving = true;

        // File Management
        private bool _createDateFolders = false;
        private bool _overwriteFiles = true;
        private bool _includeFileExtension = true;
        private string _outputFolderPath = AppDomain.CurrentDomain.BaseDirectory;

        // Advanced
        private bool _enableDebugLogging = false;
        private bool _autoCheckUpdates = true;

        public bool StartWithWindows
        {
            get => _startWithWindows;
            set { _startWithWindows = value; OnPropertyChanged(); }
        }

        public bool MinimizeToTray
        {
            get => _minimizeToTray;
            set { _minimizeToTray = value; OnPropertyChanged(); }
        }

        public bool ShowNotifications
        {
            get => _showNotifications;
            set { _showNotifications = value; OnPropertyChanged(); }
        }

        public bool SoundEffectWhenSaving
        {
            get => _soundEffectWhenSaving;
            set { _soundEffectWhenSaving = value; OnPropertyChanged(); }
        }

        public bool CreateDateFolders
        {
            get => _createDateFolders;
            set { _createDateFolders = value; OnPropertyChanged(); }
        }

        public bool OverwriteFiles
        {
            get => _overwriteFiles;
            set { _overwriteFiles = value; OnPropertyChanged(); }
        }

        public bool IncludeFileExtension
        {
            get => _includeFileExtension;
            set { _includeFileExtension = value; OnPropertyChanged(); }
        }

        public string OutputFolderPath
        {
            get => _outputFolderPath;
            set { _outputFolderPath = value; OnPropertyChanged(); }
        }

        public bool EnableDebugLogging
        {
            get => _enableDebugLogging;
            set { _enableDebugLogging = value; OnPropertyChanged(); }
        }

        public bool AutoCheckUpdates
        {
            get => _autoCheckUpdates;
            set { _autoCheckUpdates = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
