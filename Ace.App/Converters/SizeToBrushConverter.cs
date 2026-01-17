using System;
using System.Globalization;
using System.Windows.Data;

namespace Ace.App.Converters
{
    public class SizeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string sizeCategory)
            {
                return sizeCategory switch
                {
                    "Small" => ConverterColors.SizeSmall.ToBrush(),
                    "Medium" => ConverterColors.SizeMedium.ToBrush(),
                    "MediumLarge" => ConverterColors.SizeMediumLarge.ToBrush(),
                    "Large" => ConverterColors.SizeLarge.ToBrush(),
                    "VeryLarge" => ConverterColors.SizeVeryLarge.ToBrush(),
                    _ => ConverterColors.MediumGray.ToBrush()
                };
            }

            return ConverterColors.MediumGray.ToBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
