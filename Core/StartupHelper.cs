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
            string exePath = null;

            // For .NET 5+, use Environment.ProcessPath (most reliable)
            exePath = Environment.ProcessPath;

            // Fallback 1: Try Process.MainModule
            if (string.IsNullOrEmpty(exePath))
            {
                try
                {
                    using (var process = System.Diagnostics.Process.GetCurrentProcess())
                    {
                        exePath = process.MainModule?.FileName;
                    }
                }
                catch { }
            }

            // Fallback 2: If we got a DLL or null, construct the EXE path
            if (string.IsNullOrEmpty(exePath) || exePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                // Get the base directory
                string baseDir = AppContext.BaseDirectory;

                // Get assembly name
                string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

                // Construct exe path
                exePath = Path.Combine(baseDir, assemblyName + ".exe");
            }

            // Verify the file exists
            if (!File.Exists(exePath))
            {
                string error = $"Executable not found at: {exePath}\nBase Directory: {AppContext.BaseDirectory}";
                System.Diagnostics.Debug.WriteLine(error);
                throw new FileNotFoundException(error);
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
                string command = GetStartupCommand();

                
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true))
                {
                    if (key != null)
                    {
                        key.SetValue(AppName, command);

                        
                        
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error enabling startup: {ex.Message}");
                System.Windows.MessageBox.Show($"Error: {ex.Message}", "Error");
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