using System;
using System.Globalization;
using System.Windows.Data;

namespace Ace.App.Converters
{
    public class AmountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
            {
                if (amount >= 0)
                {
                    return ConverterColors.SuccessGreen.ToBrush();
                }
                return ConverterColors.ErrorRed.ToBrush();
            }

            return ConverterColors.DefaultGray.ToBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
