using System;
using System.Globalization;
using System.Windows.Data;

namespace Ace.App.Converters
{
    public class FlightPlanStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status switch
                {
                    "Ready" => ConverterColors.StatusReady.ToBrush(),
                    "Recording" => ConverterColors.StatusReady.ToBrush(),
                    "No Flight Plan" => ConverterColors.StatusError.ToBrush(),
                    "Completed" => ConverterColors.StatusCompleted.ToBrush(),
                    _ => ConverterColors.DefaultGray.ToBrush()
                };
            }

            return ConverterColors.DefaultGray.ToBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
