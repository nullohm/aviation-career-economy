using System;

namespace Ace.App.Models
{
    public class FBO
    {
        public int Id { get; set; }
        public string ICAO { get; set; } = string.Empty;
        public string AirportName { get; set; } = string.Empty;
        public FBOType Type { get; set; } = FBOType.Local;
        public decimal MonthlyRent { get; set; }
        public DateTime RentedSince { get; set; } = DateTime.Now;
        public int RunwayLengthFt { get; set; }

        public TerminalSize TerminalSize { get; set; } = TerminalSize.None;
        public decimal TerminalMonthlyCost { get; set; }


        public bool HasRefuelingService { get; set; }
        public bool HasHangarService { get; set; }
        public bool HasCateringService { get; set; }
        public bool HasGroundHandling { get; set; }
        public bool HasDeIcingService { get; set; }
    }

    public enum FBOType
    {
        Local,
        Regional,
        International
    }

    public enum TerminalSize
    {
        None,
        Small,
        Medium,
        MediumLarge,
        Large,
        VeryLarge
    }
}
