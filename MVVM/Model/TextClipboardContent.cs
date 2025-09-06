using PasteToFile.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace PasteToFile.MVVM.Model
{
    public class TextClipboardContent : ClipboardContent
    {
        private string _fullText;
        private AppSettings settings = Core.SettingsManager.Instance;

        public string FullText
        {
            get { return _fullText; }
            set
            {
                if (_fullText != value)
                {
                    _fullText = value;
                    OnPropertyChanged(nameof(FullText));

                    // Update PreviewText whenever FullText changes
                    PreviewText = GetPreviewText(_fullText);
                }
            }
        }

        public TextClipboardContent(string text) : base()
        {
            FullText = text;
            PreviewText = GetPreviewText(text);
            ContentType = CType.Text;
        }
        public TextClipboardContent(string text, string dateCreated) : base(dateCreated)
        {
            FullText = text;
            PreviewText = GetPreviewText(text);
            ContentType = CType.Text;
        }
        private string GetPreviewText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            // Limit preview to 100 characters
            return text.Length > 100 ? text.Substring(0, 100) + "..." : text;
        }

        public override void SaveToFile()
        {
            string directoryPath = Path.Combine(settings.OutputFolderPath, "ClipboardHistory");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = ID + ".txt";

            string filePath = Path.Combine(directoryPath, fileName);
            File.WriteAllText(filePath, FullText);
        }
        public override void DeleteFile()
        {
            string directoryPath = Path.Combine(settings.OutputFolderPath, "ClipboardHistory");
            string fileName = ID + ".txt";
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

    }
}
