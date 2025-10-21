using System;
using System.Runtime.InteropServices;
using Hardcodet.Wpf.TaskbarNotification;

namespace PasteToFile.Core
{
    public static class NotificationManager
    {
        private static TaskbarIcon _trayIcon;

        [DllImport("user32.dll")]
        private static extern bool MessageBeep(uint uType);

        // Initialize with your existing tray icon
        public static void Initialize(TaskbarIcon trayIcon)
        {
            _trayIcon = trayIcon;
        }

        // Show a notification without sound
        public static void ShowNotification(string title, string message, BalloonIcon icon = BalloonIcon.Info)
        {
            try
            {
                if (_trayIcon != null)
                {
                    // Using the simple 3-parameter overload
                    _trayIcon.ShowBalloonTip(title, message, icon);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notification error: {ex.Message}");
            }
        }

        // Convenience methods for different notification types
        public static void ShowSuccess(string message, string title = "Success")
        {
            ShowNotification(title, message, BalloonIcon.Info);
        }

        public static void ShowError(string message, string title = "Error")
        {
            ShowNotification(title, message, BalloonIcon.Error);
        }

        public static void ShowWarning(string message, string title = "Warning")
        {
            ShowNotification(title, message, BalloonIcon.Warning);
        }

        // Show file saved notification
        public static void ShowFileSaved(string filename, string folderPath = null)
        {
            string message = $"Saved: {filename}";
            if (!string.IsNullOrEmpty(folderPath))
            {
                message += $"\n{folderPath}";
            }
            ShowNotification("File Saved", message, BalloonIcon.Info);
        }
    }
}