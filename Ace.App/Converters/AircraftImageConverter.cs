using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;

namespace Ace.App.Converters
{
    public class AircraftImageConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return null;

            var customImagePath = values[0] as string;
            var sizeCategory = values[1] as string ?? "Medium";
            var title = values.Length > 2 ? values[2] as string : null;

            try
            {
                var imageService = ServiceLocator.GetService<IAircraftImageService>();
                var path = imageService.GetImagePath(customImagePath, sizeCategory, title);

                if (File.Exists(path))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(path);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AircraftImageConverter: Failed to load image: {ex.Message}");
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HasCustomImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Visibility.Collapsed;

            var customImagePath = values[0] as string;
            var sizeCategory = values[1] as string ?? "Medium";
            var title = values.Length > 2 ? values[2] as string : null;

            try
            {
                var imageService = ServiceLocator.GetService<IAircraftImageService>();
                var path = imageService.GetImagePath(customImagePath, sizeCategory, title);

                if (File.Exists(path))
                {
                    return Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HasCustomImageConverter: Failed to check image: {ex.Message}");
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HasNoCustomImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Visibility.Visible;

            var customImagePath = values[0] as string;
            var sizeCategory = values[1] as string ?? "Medium";
            var title = values.Length > 2 ? values[2] as string : null;

            try
            {
                var imageService = ServiceLocator.GetService<IAircraftImageService>();
                var path = imageService.GetImagePath(customImagePath, sizeCategory, title);

                if (File.Exists(path))
                {
                    return Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HasNoCustomImageConverter: Failed to check image: {ex.Message}");
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
