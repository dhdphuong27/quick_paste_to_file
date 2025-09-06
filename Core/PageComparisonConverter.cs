using System;
using System.Globalization;
using System.Windows.Data;

namespace PasteToFile.Core
{
    public sealed class PageComparisonConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 &&
                values[0] is string pageString &&
                int.TryParse(pageString, out var pageOneBased) &&
                values[1] is int currentZeroBased)
            {
                return pageOneBased == currentZeroBased + 1; // CurrentPageNumber is 0-based
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
