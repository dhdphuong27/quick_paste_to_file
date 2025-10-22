# PasteToFile

PasteToFile is a modern WPF application that lets you quickly save clipboard content (text or images) directly to files with a single shortcut. It features a clean UI, system tray integration, and flexible settings for power users.

---

## Features

- **Quick Paste:** Instantly save clipboard text or images to files (e.g., `.txt`, `.png`) with keyboard shortcuts.
- **Clipboard History:** Browse, search, and manage your saved clipboard items.
- **System Tray Integration:** Minimize to tray, restore, or exit from the tray icon.
- **Customizable Output Folder:** Choose where your files are saved.
- **Notifications & Sound:** Get notified and play a sound when saving.
- **Settings:** Control startup, notifications, sound, and more.
- **Modern UI:** Built with Material Design and MahApps icon packs.

---

## Download link

- **Drive: https://drive.google.com/file/d/19rEGJctSNC_aWkuI19l1JsQNY50va7DG/view?usp=sharing
---

## Screenshots

> _Add screenshots here to showcase the main window, clipboard history, and settings page._


- ![Clipboard History]<img width="960" height="640" alt="image" src="https://github.com/user-attachments/assets/7ba6efc5-12df-4f48-9ab5-fa15813a7d47" />
- ![How To Use] <img width="960" height="640" alt="image" src="https://github.com/user-attachments/assets/7809931c-4adc-4ce0-8614-598da2eb2763" />
- ![Settings] <img width="960" height="640" alt="image" src="https://github.com/user-attachments/assets/c8929111-8800-4a61-85d4-7a9c236f2020" />


---

## Usage

1. **Copy** any text or image to your clipboard.
2. **Use the shortcut** (`Ctrl + Shift + V` for text, `Ctrl + Alt + PrtScn` for screenshots) to save the content to a file.
3. **Browse your history** in the app, copy, view, or delete items as needed.
4. **Change settings** (output folder, notifications, etc.) from the Settings page.

---

## Building

- **Requirements:**  
  - Visual Studio 2022 or later  
  - .NET 8 SDK  
  - Windows OS

- **How to build:**  
  1. Clone the repository.
  2. Open the solution in Visual Studio.
  3. Restore NuGet packages.
  4. Build and run.
  5. Or you can just download from the drive link above

---

## Customization

- **Tray Icon:**  
  The tray icon is loaded from `/Images/app.ico`. You can replace this file to use your own icon.

- **Shortcuts:**  
  Shortcuts are hardcoded in the app. To change them, modify the relevant commands in `ClipboardViewModel.cs`.

---

## Troubleshooting

- For issues with saving files, check the output folder path in Settings.

---

## License

MIT License. See [LICENSE](LICENSE) for details.

---

## Credits

- [MahApps.Metro.IconPacks](https://github.com/MahApps/MahApps.Metro.IconPacks)
- [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [Hardcodet.Wpf.TaskbarNotification](https://github.com/hardcodet/wpf-notifyicon)

---

## Contact

For questions or suggestions, open an issue or contact the maintainer.
Email: dhdongphuong27@gmail.com 
Phone: 0522052709
