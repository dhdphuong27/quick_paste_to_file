using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PasteToFile.Core
{
    public static class SettingsManager
    {
        private static readonly string SettingsFileName = "settings.json";
        private static readonly string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);

        private static AppSettings _instance;
        private static readonly object _lock = new object();

        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = LoadSettings();
                            // Initialize startup setting AFTER the instance is loaded
                            InitializeStartupSetting();
                        }
                    }
                }
                return _instance;
            }
        }

        public static AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true
                    });
                    // DON'T call InitializeStartupSetting() here - it causes circular dependency
                    return settings ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                // Log the error or show a message
                MessageBox.Show($"Error loading settings: {ex.Message}\nUsing default settings.",
                               "Settings Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return new AppSettings();
        }

        private static void InitializeStartupSetting()
        {
            // This method is now called AFTER _instance is set, so no circular dependency
            var currentStartupStatus = StartupHelper.IsStartupEnabled();

            if (_instance.StartWithWindows != currentStartupStatus)
            {
                // Update setting to match reality (useful for detecting external changes)
                _instance.StartWithWindows = currentStartupStatus;
                SaveSettings(_instance);
            }
        }

        public static bool SaveSettings(AppSettings settings)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(SettingsPath, json);

                // Update the cached instance
                lock (_lock)
                {
                    _instance = settings;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}",
                               "Settings Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool SaveSettings()
        {
            return SaveSettings(Instance);
        }

        public static void ResetToDefaults()
        {
            lock (_lock)
            {
                _instance = new AppSettings();
                SaveSettings(_instance);
            }
        }

        public static string GetSettingsFilePath()
        {
            return SettingsPath;
        }

        public static void ReloadInstance()
        {
            lock (_lock)
            {
                _instance = LoadSettings();
                InitializeStartupSetting();
            }
        }
    }
}