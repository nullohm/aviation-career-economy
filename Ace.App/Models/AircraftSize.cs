namespace Ace.App.Models
{
    public enum AircraftSize
    {
        Small = 0,
        Medium = 1,
        MediumLarge = 2,
        Large = 3,
        VeryLarge = 4
    }

    public static class AircraftSizeExtensions
    {
        public static AircraftSize GetAircraftSize(int maxPassengers)
        {
            if (maxPassengers <= 4)
                return AircraftSize.Small;           // C172, small GA aircraft
            if (maxPassengers <= 19)
                return AircraftSize.Medium;          // Citation, small business jets
            if (maxPassengers <= 100)
                return AircraftSize.MediumLarge;     // King Air, E190, regional jets
            if (maxPassengers <= 350)
                return AircraftSize.Large;           // A320, B737
            return AircraftSize.VeryLarge;           // A380, Beluga, B747
        }

        public static string GetSizeName(this AircraftSize size)
        {
            return size switch
            {
                AircraftSize.Small => "Small",
                AircraftSize.Medium => "Medium",
                AircraftSize.MediumLarge => "Medium-Large",
                AircraftSize.Large => "Large",
                AircraftSize.VeryLarge => "Very Large",
                _ => "Unknown"
            };
        }

        public static string GetSizeNameGerman(this AircraftSize size)
        {
            return size switch
            {
                AircraftSize.Small => "Klein",
                AircraftSize.Medium => "Mittel",
                AircraftSize.MediumLarge => "Mittelgroß",
                AircraftSize.Large => "Groß",
                AircraftSize.VeryLarge => "Sehr Groß",
                _ => "Unbekannt"
            };
        }

        public static int GetRequiredRunwayLengthFt(this AircraftSize size)
        {
            return size switch
            {
                AircraftSize.Small => 2000,        // C172, small GA - grass strips OK
                AircraftSize.Medium => 4000,       // Citation, King Air - small regional
                AircraftSize.MediumLarge => 6000,  // E190, ATR - regional airports
                AircraftSize.Large => 8000,        // A320, B737 - major airports
                AircraftSize.VeryLarge => 10000,   // A380, B747 - large international
                _ => 6000
            };
        }

        public static TerminalSize GetRequiredTerminalSize(this AircraftSize size)
        {
            return size switch
            {
                AircraftSize.Small => TerminalSize.Small,
                AircraftSize.Medium => TerminalSize.Medium,
                AircraftSize.MediumLarge => TerminalSize.MediumLarge,
                AircraftSize.Large => TerminalSize.Large,
                AircraftSize.VeryLarge => TerminalSize.VeryLarge,
                _ => TerminalSize.Medium
            };
        }
    }
}
