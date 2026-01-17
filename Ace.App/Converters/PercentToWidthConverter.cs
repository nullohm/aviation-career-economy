using System;
using System.Globalization;
using System.Windows.Data;

namespace Ace.App.Converters
{
    public class PercentToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double percent && parameter is string maxWidthStr)
            {
                if (double.TryParse(maxWidthStr, out var maxWidth))
                {
                    return (percent / 100.0) * maxWidth;
                }
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
