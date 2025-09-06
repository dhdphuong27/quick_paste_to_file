using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PasteToFile.Core
{
    public class FunctionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Example: Call a custom function to double a value
            if (value is string fullText)
            {
                return ShortenString(fullText);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string ShortenString(string fullText)
        {
            // Split by newline and carriage return, then take the first line
            return fullText?.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }
    }
}
