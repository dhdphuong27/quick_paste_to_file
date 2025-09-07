using System;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using PasteToFile.Core;
using PasteToFile.MVVM.Model;
using PasteToFile.MVVM.View;

namespace PasteToFile.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private object _currentView;
        private TaskbarIcon _trayIcon;

        public RelayCommand ClipboardViewCommand { get; set; }
        public RelayCommand HowToUseViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }

        public ClipboardViewModel ClipboardVM { get; set; }
        public HowToUseView HowToUseView { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ClipboardVM = new ClipboardViewModel();
            HowToUseView = new HowToUseView();
            SettingsVM = new SettingsViewModel();
            SettingsVM.OnSettingsApplied = () =>
            {
                ClipboardVM.ReloadData();
            };
            CurrentView = ClipboardVM;

            ClipboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = ClipboardVM;
            });

            HowToUseViewCommand = new RelayCommand(o =>
            {
                CurrentView = HowToUseView;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            Application.Current.MainWindow.StateChanged += MainWindow_StateChanged;

            MinimizeCommand = new RelayCommand(MinimizeWindow);
            CloseCommand = new RelayCommand(CloseApp);
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            _trayIcon = (TaskbarIcon)Application.Current.FindResource("MyTrayIcon");

            // Double click restores window
            _trayIcon.TrayMouseDoubleClick -= TrayIcon_DoubleClick;
            _trayIcon.TrayMouseDoubleClick += TrayIcon_DoubleClick;

            // Context menu
            _trayIcon.ContextMenu = new ContextMenu();
            var showMenuItem = new MenuItem { Header = "Show" };
            showMenuItem.Click += (s, e) => RestoreFromTray();
            var exitMenuItem = new MenuItem { Header = "Exit" };
            exitMenuItem.Click += (s, e) => CloseApp();
            _trayIcon.ContextMenu.Items.Add(showMenuItem);
            _trayIcon.ContextMenu.Items.Add(exitMenuItem);
        }

        private void MinimizeToTray()
        {
            Application.Current.MainWindow.Hide();
            // Show balloon tip (only on minimize, not on startup)
            //_trayIcon.ShowBalloonTip("MyApp", "App is running in background", BalloonIcon.Info);
        }

        private void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            var window = sender as Window;

            // If MinimizeToTray is enabled and window is minimized, hide to tray
            if (SettingsManager.Instance.MinimizeToTray && window.WindowState == WindowState.Minimized)
            {
                MinimizeToTray();
            }
        }

        private void TrayIcon_DoubleClick(object sender, RoutedEventArgs e)
        {
            RestoreFromTray();
        }

        private void RestoreFromTray()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
                return;

            if (!mainWindow.IsVisible)
                mainWindow.Show();

            if (mainWindow.WindowState == WindowState.Minimized)
                mainWindow.WindowState = WindowState.Normal;

            mainWindow.Activate();

            // Additional steps to ensure window comes to front
            mainWindow.Topmost = true;
            mainWindow.Topmost = false;
            mainWindow.Focus();
        }

        private void CloseApp()
        {
            if (_trayIcon != null)
                _trayIcon.Dispose();
            Application.Current.Shutdown();
        }
    }
}