using PasteToFile.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PasteToFile.MVVM.Model
{
    internal class ImageClipboardContent : ClipboardContent
    {
        private BitmapSource _image;
        private AppSettings settings = Core.SettingsManager.Instance;
        public BitmapSource Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        public ImageClipboardContent(BitmapSource bitmapSource) : base()
        {
            Image = bitmapSource;
            ContentType = CType.Image;
            PreviewText = "Image (click to open with default photo viewer)";
        }
        public ImageClipboardContent(string dateCreated) : base(dateCreated)
        {
            ContentType = CType.Image;
            PreviewText = "Image (click to open with default photo viewer)";
        }
        public void OpenImage()
        {
            string directoryPath = Path.Combine(settings.OutputFolderPath, "ClipboardHistory");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = ID + ".png";
            string filePath = Path.Combine(directoryPath, fileName);

            OpenImageExplicit(filePath);
        }
        private void OpenImageExplicit(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true,
                        Verb = "open" // This tells Windows to use the default "open" action
                    };
                    Process.Start(psi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot open image: {ex.Message}");
            }
        }


        public override void DeleteFile()
        {
            string directoryPath = Path.Combine(settings.OutputFolderPath, "ClipboardHistory");
            string fileName = ID + ".png";
            string filePath = Path.Combine(directoryPath, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                // optional: log or notify
                MessageBox.Show($"Failed to delete file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void SaveToFile()
        {
            string directoryPath = Path.Combine(settings.OutputFolderPath, "ClipboardHistory");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = ID + ".png";
            string filePath = Path.Combine(directoryPath, fileName);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Image));
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }
    }
}
