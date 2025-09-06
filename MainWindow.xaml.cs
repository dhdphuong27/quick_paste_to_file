using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PasteToFile.MVVM.Model;

namespace PasteToFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PASTE_HOTKEY_ID = 9000;
        private const int SCREENSHOT_HOTKEY_ID = 9001;

        // Modifier keys constants (add these if not in HotkeyManager)
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                // Register Ctrl+Alt+V for paste functionality
                HotkeyManager.RegisterHotKey(this, PASTE_HOTKEY_ID, MOD_CONTROL | MOD_ALT, Key.V, () =>
                {
                    // Get VM and call SaveContentCommand
                    if (DataContext is PasteToFile.MVVM.ViewModel.MainViewModel mainVM)
                    {
                        var clipboardVM = mainVM.ClipboardVM;
                        if (clipboardVM.SaveContentCommand.CanExecute(null))
                            clipboardVM.SaveContentCommand.Execute(null);
                    }
                });

                // Register Ctrl+Alt+PrintScreen for screenshot functionality
                HotkeyManager.RegisterHotKey(this, SCREENSHOT_HOTKEY_ID, MOD_CONTROL | MOD_ALT, Key.PrintScreen, () =>
                {
                    if (DataContext is PasteToFile.MVVM.ViewModel.MainViewModel mainVM)
                    {
                        var clipboardVM = mainVM.ClipboardVM;
                        if (clipboardVM.SaveScreenshotCommand.CanExecute(null))
                            clipboardVM.SaveScreenshotCommand.Execute(null);
                    }
                });
            };

            Closed += (s, e) =>
            {
                HotkeyManager.UnregisterHotKey(this, PASTE_HOTKEY_ID);
                HotkeyManager.UnregisterHotKey(this, SCREENSHOT_HOTKEY_ID);
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the screen's working area (excluding taskbar)
            var workingArea = SystemParameters.WorkArea;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}