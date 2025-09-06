using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using PasteToFile.MVVM.Model;
using static System.Net.Mime.MediaTypeNames;

namespace PasteToFile.MVVM.ViewModel
{
    public class ViewContentViewModel
    {
        public String ContentText { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public FlowDocument Document { get; }


        public ViewContentViewModel(ClipboardContent content)
        {

            ContentText = ((TextClipboardContent)content).FullText;

            // Initialize commands
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            var doc = new FlowDocument();
            doc.Blocks.Add(new Paragraph(new Run(ContentText)));

            Document = doc;
        }

        private void Save(object parameter)
        {
            Clipboard.SetText(ContentText);
            var window = parameter as System.Windows.Window;
            window?.Close();
        }

        private void Cancel(object parameter)
        {
            var window = parameter as System.Windows.Window;
            window?.Close();
        }

    }
}
