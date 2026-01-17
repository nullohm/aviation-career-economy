using System;
using System.Windows.Media;
using Ace.App.Interfaces;

namespace Ace.App.ViewModels
{
    public class MarketAircraftDetailViewModel : AircraftDetailViewModelBase
    {
        private readonly IFinanceService _financeService;
        private readonly IAircraftCatalogRepository _catalogRepository;

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public bool IsMarketPreview => true;

        public MarketAircraftDetailViewModel(
            MarketAircraftViewModel marketAircraft,
            IFinanceService financeService,
            IAircraftCatalogRepository catalogRepository)
        {
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));

            Registration = "Not Yet Purchased";
            HomeBase = "N/A";
            Status = "Available for Purchase";
            StatusColor = new SolidColorBrush(Color.FromRgb(34, 197, 94));
            TypeVariant = marketAircraft.DisplayName;
            CurrentValue = marketAircraft.Price;
            MaxPassengers = marketAircraft.PassengerCapacity;

            LoadSpecsFromCatalog(marketAircraft.Title);

            Balance = _financeService.Balance;
        }

        private void LoadSpecsFromCatalog(string title)
        {
            var catalogEntry = _catalogRepository.GetAircraftByTitle(title);

            if (catalogEntry != null)
            {
                if (MaxPassengers == 0 && catalogEntry.PassengerCapacity > 0)
                    MaxPassengers = catalogEntry.PassengerCapacity;
                CruiseSpeedKts = catalogEntry.CruiseSpeedKts;
                MaxRangeNM = catalogEntry.MaxRangeNM;
                FuelCapacityGal = catalogEntry.FuelCapacityGal;
                FuelBurnGalPerHour = catalogEntry.FuelBurnGalPerHour;
                HourlyOperatingCost = catalogEntry.HourlyOperatingCost;
                return;
            }

            CruiseSpeedKts = 0;
            MaxRangeNM = 0;
            FuelCapacityGal = 0;
            FuelBurnGalPerHour = 0;
            HourlyOperatingCost = 0;
        }
    }
}
