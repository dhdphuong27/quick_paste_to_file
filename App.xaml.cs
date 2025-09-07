using PasteToFile.Core;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PasteToFile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex;
        private const string MutexName = "PasteToFileAppMutex";
        private const string PipeName = "PasteToFileAppPipe";
        private NamedPipeServerStream _pipeServer;
        private CancellationTokenSource _cancellationTokenSource;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Check if another instance is already running
            _mutex = new Mutex(true, MutexName, out bool isNewInstance);

            if (!isNewInstance)
            {
                // Another instance is running, send message to show it
                SendMessageToRunningInstance();
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            bool startedFromWindowsStartup = e.Args.Contains("--startup");

            // If started from Windows startup and MinimizeToTray is enabled, start minimized
            if (startedFromWindowsStartup && SettingsManager.Instance.MinimizeToTray)
            {
                // Create main window but don't show it
                MainWindow = new MainWindow();
                // The MainViewModel should handle the tray initialization
                // No need to call Show() on the window
            }
            else
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            }

            // Start listening for messages from other instances
            StartPipeServer();
        }

        private void SendMessageToRunningInstance()
        {
            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
                {
                    pipeClient.Connect(1000); // Wait up to 1 second
                    using (var writer = new StreamWriter(pipeClient))
                    {
                        writer.WriteLine("SHOW_WINDOW");
                        writer.Flush();
                    }
                }
            }
            catch
            {
                // If we can't send the message, just exit silently
            }
        }

        private void StartPipeServer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        _pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.In);
                        await _pipeServer.WaitForConnectionAsync(_cancellationTokenSource.Token);

                        using (var reader = new StreamReader(_pipeServer))
                        {
                            string message = await reader.ReadLineAsync();
                            if (message == "SHOW_WINDOW")
                            {
                                // Use Dispatcher to show window on UI thread
                                Dispatcher.Invoke(() =>
                                {
                                    ShowMainWindow();
                                });
                            }
                        }

                        _pipeServer.Disconnect();
                        _pipeServer.Dispose();
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch
                    {
                        // Handle other exceptions, continue listening
                        await Task.Delay(1000, _cancellationTokenSource.Token);
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        private void ShowMainWindow()
        {
            var mainWindow = Current.MainWindow;
            if (mainWindow == null)
                return;

            if (!mainWindow.IsVisible)
                mainWindow.Show();

            if (mainWindow.WindowState == WindowState.Minimized)
                mainWindow.WindowState = WindowState.Normal;

            mainWindow.Activate();
            mainWindow.Topmost = true; // Bring to front
            mainWindow.Topmost = false; // Reset topmost
            mainWindow.Focus();
        }

        private void MyTrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowMainWindow();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Clean up resources
            _cancellationTokenSource?.Cancel();
            _pipeServer?.Dispose();
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();

            // Save settings on exit
            SettingsManager.SaveSettings();
            base.OnExit(e);
        }
    }
}