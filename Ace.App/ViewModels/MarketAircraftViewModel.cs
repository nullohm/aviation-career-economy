using Ace.App.Data;
using Ace.App.Models;
using Ace.App.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ace.App.ViewModels
{
    public class MarketAircraftViewModel : INotifyPropertyChanged
    {
        private readonly AircraftInfo _aircraftInfo;
        private bool _canAfford;

        public MarketAircraftViewModel(AircraftInfo aircraftInfo)
        {
            _aircraftInfo = aircraftInfo;
            Price = GetPriceFromDatabase();
        }

        public MarketAircraftViewModel(AircraftInfo aircraftInfo, decimal price)
        {
            _aircraftInfo = aircraftInfo;
            Price = price;
        }

        public MarketAircraftViewModel(AircraftInfo aircraftInfo, decimal price, double cruiseSpeedKts, double maxRangeNM, double fuelCapacityGal, double fuelBurnGalPerHour, decimal hourlyOperatingCost, int serviceCeilingFt = 0, string? customImagePath = null, bool isOldtimer = false, decimal profitPerHour = 0)
        {
            _aircraftInfo = aircraftInfo;
            Price = price;
            CruiseSpeedKts = cruiseSpeedKts;
            MaxRangeNM = maxRangeNM;
            FuelCapacityGal = fuelCapacityGal;
            FuelBurnGalPerHour = fuelBurnGalPerHour;
            HourlyOperatingCost = hourlyOperatingCost;
            ServiceCeilingFt = serviceCeilingFt;
            CustomImagePath = customImagePath;
            IsOldtimer = isOldtimer;
            ProfitPerHour = profitPerHour;
        }

        public string Title => _aircraftInfo.Title;
        public string? CustomImagePath { get; }
        public string Manufacturer => _aircraftInfo.Manufacturer;
        public string Type => _aircraftInfo.Type;
        public string Category => _aircraftInfo.Category;
        public int PassengerCapacity => _aircraftInfo.PassengerCapacity;
        public double MaxCargoKg => _aircraftInfo.MaxCargoKg;
        public int CrewCount => _aircraftInfo.CrewCount;
        public decimal Price { get; }
        public double CruiseSpeedKts { get; }
        public double MaxRangeNM { get; }
        public double FuelCapacityGal { get; }
        public double FuelBurnGalPerHour { get; }
        public decimal HourlyOperatingCost { get; }
        public int ServiceCeilingFt { get; }
        public bool IsOldtimer { get; }
        public decimal ProfitPerHour { get; }
        public string ProfitPerHourInfo => ProfitPerHour > 0 ? $"{ProfitPerHour:N0} €/h" : "—";

        public bool CanAfford
        {
            get => _canAfford;
            set
            {
                if (_canAfford != value)
                {
                    _canAfford = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DisplayName => string.IsNullOrEmpty(Manufacturer)
            ? Title
            : $"{Manufacturer} {Title}";

        public string FormattedPrice => $"{Price:N0} €";

        public string SizeCategory => AircraftSizeExtensions.GetAircraftSize(PassengerCapacity).GetSizeName();

        public string CapacityInfo
        {
            get
            {
                if (PassengerCapacity > 0)
                    return $"{PassengerCapacity} Passagiere";

                return "Keine Passagiere";
            }
        }

        public string CargoInfo => MaxCargoKg > 0 ? $"{MaxCargoKg:F0} kg" : "—";

        public string SpeedInfo => CruiseSpeedKts > 0 ? $"{CruiseSpeedKts:N0} kts" : "—";

        public string RangeInfo => MaxRangeNM > 0 ? $"{MaxRangeNM:N0} NM" : "—";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private decimal GetPriceFromDatabase()
        {
            try
            {
                using var db = new AceDbContext();
                var catalogEntry = db.AircraftCatalog.FirstOrDefault(a => a.Title == Title);

                if (catalogEntry != null)
                {
                    System.Diagnostics.Debug.WriteLine($"MarketAircraftViewModel: Price for {Title}: {catalogEntry.MarketPrice:N0} € (from catalog)");
                    return catalogEntry.MarketPrice;
                }

                var msfsEntry = db.MsfsAircraft.FirstOrDefault(a => a.Title == Title);
                if (msfsEntry != null)
                {
                    System.Diagnostics.Debug.WriteLine($"MarketAircraftViewModel: Using NewPrice for {Title}: {msfsEntry.NewPrice:N0} € (from MsfsAircraft table)");
                    return msfsEntry.NewPrice;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MarketAircraftViewModel: Failed to get price from database for {Title}: {ex.Message}");
            }

            System.Diagnostics.Debug.WriteLine($"MarketAircraftViewModel: No price found for {Title}, using default 100000 €");
            return 100000m;
        }
    }
}
