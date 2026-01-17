using System;
using System.IO;
using System.Windows.Media.Imaging;
using Ace.App.Utilities;

namespace Ace.App.Helpers
{
    public static class AirportImageHelper
    {
        private static readonly string[] SupportedExtensions = { ".png", ".jpg", ".jpeg", ".bmp" };

        public static string? GetCustomImagePath(string icao)
        {
            if (string.IsNullOrWhiteSpace(icao))
                return null;

            var directory = PathUtilities.GetAirportImagesDirectory();
            if (!Directory.Exists(directory))
                return null;

            var upperIcao = icao.ToUpperInvariant();

            foreach (var ext in SupportedExtensions)
            {
                var path = Path.Combine(directory, upperIcao + ext);
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        public static bool HasCustomImage(string icao)
        {
            return GetCustomImagePath(icao) != null;
        }

        public static BitmapImage? LoadCustomImage(string icao)
        {
            var path = GetCustomImagePath(icao);
            if (path == null)
                return null;

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        public static string GetDefaultImagePath(string icao)
        {
            var directory = PathUtilities.GetAirportImagesDirectory();
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return Path.Combine(directory, icao.ToUpperInvariant() + ".png");
        }

        public static void EnsureDirectoryExists()
        {
            var directory = PathUtilities.GetAirportImagesDirectory();
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}
