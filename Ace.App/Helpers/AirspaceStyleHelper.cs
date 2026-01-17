using Mapsui.Styles;
using Ace.App.Models;

namespace Ace.App.Helpers
{
    public static class AirspaceStyleHelper
    {
        public static VectorStyle GetAirspaceStyle(AirspaceClass airspaceClass)
        {
            var (fillColor, outlineColor, opacity) = GetColors(airspaceClass);

            return new VectorStyle
            {
                Fill = new Brush(Color.FromArgb((int)(opacity * 255), fillColor.R, fillColor.G, fillColor.B)),
                Outline = new Pen(outlineColor, 1.5)
            };
        }

        public static (Color fill, Color outline, double opacity) GetColors(AirspaceClass airspaceClass)
        {
            return airspaceClass switch
            {
                AirspaceClass.A => (Color.FromString("#DC2626"), Color.FromString("#DC2626"), 0.25),
                AirspaceClass.B => (Color.FromString("#2563EB"), Color.FromString("#2563EB"), 0.25),
                AirspaceClass.C => (Color.FromString("#7C3AED"), Color.FromString("#7C3AED"), 0.25),
                AirspaceClass.D => (Color.FromString("#0891B2"), Color.FromString("#0891B2"), 0.25),
                AirspaceClass.E => (Color.FromString("#16A34A"), Color.FromString("#16A34A"), 0.20),
                AirspaceClass.F => (Color.FromString("#84CC16"), Color.FromString("#84CC16"), 0.15),
                AirspaceClass.G => (Color.FromString("#22C55E"), Color.FromString("#22C55E"), 0.10),
                AirspaceClass.CTR => (Color.FromString("#C026D3"), Color.FromString("#C026D3"), 0.30),
                AirspaceClass.TMA => (Color.FromString("#EA580C"), Color.FromString("#EA580C"), 0.20),
                AirspaceClass.ATZ => (Color.FromString("#F59E0B"), Color.FromString("#F59E0B"), 0.20),
                AirspaceClass.RMZ => (Color.FromString("#06B6D4"), Color.FromString("#06B6D4"), 0.15),
                AirspaceClass.TMZ => (Color.FromString("#8B5CF6"), Color.FromString("#8B5CF6"), 0.15),
                AirspaceClass.Prohibited => (Color.FromString("#DC2626"), Color.FromString("#B91C1C"), 0.40),
                AirspaceClass.Restricted => (Color.FromString("#EF4444"), Color.FromString("#DC2626"), 0.35),
                AirspaceClass.Danger => (Color.FromString("#F97316"), Color.FromString("#EA580C"), 0.30),
                AirspaceClass.Glider => (Color.FromString("#22D3EE"), Color.FromString("#06B6D4"), 0.15),
                AirspaceClass.Wave => (Color.FromString("#A5B4FC"), Color.FromString("#818CF8"), 0.15),
                _ => (Color.FromString("#6B7280"), Color.FromString("#4B5563"), 0.15)
            };
        }

        public static string GetAirspaceClassName(AirspaceClass airspaceClass)
        {
            return airspaceClass switch
            {
                AirspaceClass.A => "Class A",
                AirspaceClass.B => "Class B",
                AirspaceClass.C => "Class C",
                AirspaceClass.D => "Class D",
                AirspaceClass.E => "Class E",
                AirspaceClass.F => "Class F",
                AirspaceClass.G => "Class G",
                AirspaceClass.CTR => "CTR",
                AirspaceClass.TMA => "TMA",
                AirspaceClass.ATZ => "ATZ",
                AirspaceClass.RMZ => "RMZ",
                AirspaceClass.TMZ => "TMZ",
                AirspaceClass.Prohibited => "Prohibited",
                AirspaceClass.Restricted => "Restricted",
                AirspaceClass.Danger => "Danger",
                AirspaceClass.Glider => "Glider",
                AirspaceClass.Wave => "Wave",
                _ => "Other"
            };
        }
    }
}
