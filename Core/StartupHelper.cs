using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace PasteToFile.Core
{
    public static class StartupHelper
    {
        private static readonly string AppName = "PasteToFile";
        private static readonly string StartupKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Gets the current executable path with startup arguments
        /// </summary>
        private static string GetStartupCommand()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;

            // For .NET Core/5+ apps, we need to handle the dll case
            if (exePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                exePath = Path.ChangeExtension(exePath, ".exe");
            }

            // Add startup argument to minimize to tray on startup
            return $"\"{exePath}\" --startup";
        }

        /// <summary>
        /// Enables the application to start with Windows
        /// </summary>
        public static bool EnableStartup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true))
                {
                    if (key != null)
                    {
                        key.SetValue(AppName, GetStartupCommand());
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error enabling startup: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Disables the application from starting with Windows
        /// </summary>
        public static bool DisableStartup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true))
                {
                    if (key != null)
                    {
                        if (key.GetValue(AppName) != null)
                        {
                            key.DeleteValue(AppName);
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error disabling startup: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Checks if the application is currently set to start with Windows
        /// </summary>
        public static bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(AppName);
                        return value != null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking startup status: {ex.Message}");
            }
            return false;
        }
    }
}