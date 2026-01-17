using System;
using System.Globalization;
using System.Windows.Data;

namespace Ace.App.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected && isConnected)
            {
                return ConverterColors.SuccessGreen.ToBrush();
            }

            return ConverterColors.DefaultGray.ToBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

