using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PasteToFile.MVVM.Model
{
    public abstract class ClipboardContent : INotifyPropertyChanged
    {
        public string ID { get; set; }
        public CType ContentType { get; set; }

        public DateTime DateCreated { get; set; }
        public Status Status { get; set; }

        private string _previewText;
        public string PreviewText
        {
            get { return _previewText; }
            set
            {
                if (_previewText != value)
                {
                    _previewText = value;
                    OnPropertyChanged(nameof(PreviewText));
                }
            }
        }

        public ClipboardContent()
        {
            DateCreated = DateTime.Now;
            ID = DateCreated.ToString("yyyyMMddHHmmssfff");
            Status = Status.New;
        }
        public ClipboardContent(string dateCreated)
        {
            DateTime parsedDate;
            Status = Status.Local;

            bool success = DateTime.TryParseExact(dateCreated, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
            if (success)
            {
                ID = dateCreated;
                DateCreated = parsedDate;
            }
            else
            {
                DateCreated = DateTime.Now;
                ID = DateCreated.ToString("yyyyMMddHHmmssfff");
                MessageBox.Show("Parse date failed, " + dateCreated);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetPreviewText(string fullText)
        {
            return fullText?.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }

        public abstract void SaveToFile();
        public abstract void DeleteFile();

    }
    public enum Status
    {
        New,
        Local,
        Cloud
    }
    public enum CType
    {
        Text,
        Image
    }
}
