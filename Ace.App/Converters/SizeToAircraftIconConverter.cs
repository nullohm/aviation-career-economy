using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Ace.App.Converters
{
    public class SizeToAircraftIconConverter : IValueConverter
    {
        private static readonly Geometry SmallAircraftGeometry = Geometry.Parse(
            "M12,2 L14,8 L22,10 L22,12 L14,11 L14,18 L17,20 L17,22 L12,21 L7,22 L7,20 L10,18 L10,11 L2,12 L2,10 L10,8 Z");

        private static readonly Geometry MediumAircraftGeometry = Geometry.Parse(
            "M12,1 L15,7 L24,9 L24,11 L15,10 L15,17 L19,19 L19,22 L12,20 L5,22 L5,19 L9,17 L9,10 L0,11 L0,9 L9,7 Z");

        private static readonly Geometry MediumLargeAircraftGeometry = Geometry.Parse(
            "M12,0 L16,6 L24,8 L24,12 L16,10 L16,17 L20,19 L20,23 L12,20 L4,23 L4,19 L8,17 L8,10 L0,12 L0,8 L8,6 Z");

        private static readonly Geometry LargeAircraftGeometry = Geometry.Parse(
            "M12,0 L17,5 L24,7 L24,13 L17,10 L17,17 L22,19 L22,24 L12,20 L2,24 L2,19 L7,17 L7,10 L0,13 L0,7 L7,5 Z");

        private static readonly Geometry VeryLargeAircraftGeometry = Geometry.Parse(
            "M12,0 L18,4 L24,6 L24,14 L18,10 L18,16 L24,18 L24,24 L12,19 L0,24 L0,18 L6,16 L6,10 L0,14 L0,6 L6,4 Z");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sizeCategory = value as string ?? "Medium";

            return sizeCategory switch
            {
                "Small" => SmallAircraftGeometry,
                "Medium" => MediumAircraftGeometry,
                "Medium-Large" => MediumLargeAircraftGeometry,
                "Large" => LargeAircraftGeometry,
                "Very Large" => VeryLargeAircraftGeometry,
                _ => MediumAircraftGeometry
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
