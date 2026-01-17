using Mapsui.Styles;

namespace Ace.App.Helpers
{
    public static class AirportSymbolHelper
    {
        public static (double scale, Color color, bool showLabel) GetAirportStyle(int runwayLengthFt)
        {
            if (runwayLengthFt < 2000)
            {
                return (0.25, Color.FromString("#90CAF9"), true);
            }
            else if (runwayLengthFt < 4000)
            {
                return (0.35, Color.FromString("#64B5F6"), true);
            }
            else if (runwayLengthFt < 6000)
            {
                return (0.45, Color.FromString("#42A5F5"), true);
            }
            else if (runwayLengthFt < 8000)
            {
                return (0.55, Color.FromString("#2196F3"), true);
            }
            else
            {
                return (0.65, Color.FromString("#9C27B0"), true);
            }
        }

        public static (double scale, Color color) GetFBOStyle(string fboType)
        {
            var color = fboType switch
            {
                "International" => Color.FromString("#E91E63"),
                "Regional" => Color.FromString("#2196F3"),
                "Local" => Color.FromString("#4CAF50"),
                _ => Color.FromString("#FFC107")
            };

            return (0.7, color);
        }
    }
}
