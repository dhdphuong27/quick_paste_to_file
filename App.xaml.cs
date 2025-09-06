using PasteToFile.Core;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PasteToFile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void MyTrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
                return;

            if (!mainWindow.IsVisible)
                mainWindow.Show();

            if (mainWindow.WindowState == WindowState.Minimized)
                mainWindow.WindowState = WindowState.Normal;

            mainWindow.Activate();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load settings on startup
            var settings = SettingsManager.Instance;

            // Apply settings
            if (settings.StartWithWindows)
            {
                // Add to startup
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Save settings on exit
            SettingsManager.SaveSettings();
            base.OnExit(e);
        }

    }


}
