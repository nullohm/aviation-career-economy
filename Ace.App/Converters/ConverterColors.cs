using System.Windows.Media;

namespace Ace.App.Converters
{
    public static class ConverterColors
    {
        public static readonly Color SuccessGreen = Color.FromRgb(76, 175, 80);
        public static readonly Color ErrorRed = Color.FromRgb(244, 67, 54);
        public static readonly Color DefaultGray = Color.FromRgb(200, 200, 200);
        public static readonly Color MediumGray = Color.FromRgb(158, 158, 158);

        public static readonly Color SizeSmall = Color.FromRgb(76, 175, 80);
        public static readonly Color SizeMedium = Color.FromRgb(33, 150, 243);
        public static readonly Color SizeMediumLarge = Color.FromRgb(255, 152, 0);
        public static readonly Color SizeLarge = Color.FromRgb(156, 39, 176);
        public static readonly Color SizeVeryLarge = Color.FromRgb(244, 67, 54);

        public static readonly Color StatusReady = Color.FromRgb(34, 197, 94);
        public static readonly Color StatusError = Color.FromRgb(239, 68, 68);
        public static readonly Color StatusCompleted = Color.FromRgb(88, 166, 255);

        public static readonly Color UrgencyOverdue = Color.FromRgb(239, 68, 68);
        public static readonly Color UrgencyDueSoon = Color.FromRgb(251, 146, 60);
        public static readonly Color UrgencyUpcoming = Color.FromRgb(250, 204, 21);
        public static readonly Color UrgencyOk = Color.FromRgb(34, 197, 94);

        public static SolidColorBrush ToBrush(this Color color) => new(color);
    }
}
