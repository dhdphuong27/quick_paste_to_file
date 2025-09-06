using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace PasteToFile.MVVM.Model
{
    public static class HotkeyManager
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Modifier keys
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;

        // Windows message ID for hotkey
        private const int WM_HOTKEY = 0x0312;

        public static void RegisterHotKey(Window window, int hotkeyId, uint modifiers, Key key, Action callback)
        {
            var helper = new WindowInteropHelper(window);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
            {
                if (msg == WM_HOTKEY && wParam.ToInt32() == hotkeyId)
                {
                    callback?.Invoke();
                    handled = true;
                }
                return IntPtr.Zero;
            });

            // Register the specified hotkey
            RegisterHotKey(helper.Handle, hotkeyId, modifiers, (uint)KeyInterop.VirtualKeyFromKey(key));
        }

        public static void UnregisterHotKey(Window window, int hotkeyId)
        {
            var helper = new WindowInteropHelper(window);
            UnregisterHotKey(helper.Handle, hotkeyId);
        }
    }
}
