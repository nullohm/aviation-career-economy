using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using AircraftModel = Ace.App.Models.Aircraft;

namespace Ace.App.ViewModels
{
    public class MarketViewModel : ViewModelBase
    {
        private readonly ILoggingService _logger;
        private readonly IFinanceService _financeService;
        private readonly IAircraftCatalogRepository _catalogRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAircraftImageService _imageService;
        private readonly IAchievementService _achievementService;
        private readonly IPricingService _pricingService;

        private ObservableCollection<MarketAircraftViewModel> _marketAircraft = new();
        private ObservableCollection<MarketAircraftViewModel> _filteredAircraft = new();
        private string _filterText = string.Empty;
        private string _aircraftCountText = string.Empty;
        private string _balanceText = string.Empty;
        private int _currentSortIndex = 0;
        private bool _showOnlyFavorites;

        public ObservableCollection<MarketAircraftViewModel> MarketAircraft
        {
            get => _marketAircraft;
            set => SetProperty(ref _marketAircraft, value);
        }

        public ObservableCollection<MarketAircraftViewModel> FilteredAircraft
        {
            get => _filteredAircraft;
            set => SetProperty(ref _filteredAircraft, value);
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                if (SetProperty(ref _filterText, value))
                {
                    ApplyFilter();
                }
            }
        }

        public string AircraftCountText
        {
            get => _aircraftCountText;
            set => SetProperty(ref _aircraftCountText, value);
        }

        public string BalanceText
        {
            get => _balanceText;
            set => SetProperty(ref _balanceText, value);
        }

        public bool ShowOnlyFavorites
        {
            get => _showOnlyFavorites;
            set
            {
                if (SetProperty(ref _showOnlyFavorites, value))
                {
                    ApplyFilter();
                }
            }
        }

        public MarketViewModel(
            ILoggingService logger,
            IFinanceService financeService,
            IAircraftCatalogRepository catalogRepository,
            IAircraftRepository aircraftRepository,
            ITransactionRepository transactionRepository,
            IAircraftImageService imageService,
            IAchievementService achievementService,
            IPricingService pricingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _achievementService = achievementService ?? throw new ArgumentNullException(nameof(achievementService));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        }

        public void LoadMarketAircraft()
        {
            try
            {
                _logger.Info("MarketViewModel: Loading available aircraft for purchase");

                MarketAircraft.Clear();
                var currentBalance = _financeService.Balance;

                var catalogEntries = _catalogRepository.GetAllAircraft()
                    .Where(a => !a.Title.ToLower().Contains("family"))
                    .ToList();
                _logger.Info($"MarketViewModel: Loaded {catalogEntries.Count} aircraft from AircraftCatalog (filtered out family entries)");

                foreach (var entry in catalogEntries)
                {
                    var aircraftInfo = new AircraftInfo
                    {
                        Title = entry.Title,
                        Manufacturer = entry.Manufacturer,
                        Type = entry.Type,
                        Category = entry.Category,
                        CrewCount = entry.CrewCount,
                        PassengerCapacity = entry.PassengerCapacity,
                        MaxCargoKg = entry.MaxCargoKg,
                        CruiseSpeedKts = entry.CruiseSpeedKts,
                        MaxRangeNM = entry.MaxRangeNM,
                        FuelCapacityGal = entry.FuelCapacityGal,
                        FuelBurnGalPerHour = entry.FuelBurnGalPerHour,
                        HourlyOperatingCost = entry.HourlyOperatingCost
                    };

                    var profitPerHour = _pricingService.CalculateProfitPerHour(entry);

                    var viewModel = new MarketAircraftViewModel(
                        aircraftInfo,
                        entry.MarketPrice,
                        entry.CruiseSpeedKts,
                        entry.MaxRangeNM,
                        entry.FuelCapacityGal,
                        entry.FuelBurnGalPerHour,
                        entry.HourlyOperatingCost,
                        entry.ServiceCeilingFt,
                        entry.CustomImagePath,
                        entry.IsOldtimer,
                        profitPerHour,
                        entry.IsFavorite);
                    viewModel.CanAfford = currentBalance >= viewModel.Price;
                    MarketAircraft.Add(viewModel);
                }

                ApplyFilter();
                UpdateBalance();

                _logger.Info($"MarketViewModel: Loaded {MarketAircraft.Count} aircraft for purchase");
            }
            catch (Exception ex)
            {
                _logger.Error("MarketViewModel: Failed to load market aircraft", ex);
                AircraftCountText = "Error loading";
            }
        }

        public void ApplyFilter()
        {
            var filterText = FilterText?.Trim().ToLowerInvariant() ?? "";

            var filtered = string.IsNullOrEmpty(filterText)
                ? MarketAircraft.AsEnumerable()
                : MarketAircraft.Where(a =>
                    a.DisplayName.ToLowerInvariant().Contains(filterText) ||
                    a.Manufacturer.ToLowerInvariant().Contains(filterText) ||
                    a.Title.ToLowerInvariant().Contains(filterText));

            if (_showOnlyFavorites)
                filtered = filtered.Where(a => a.IsFavorite);

            var sorted = ApplySort(filtered.ToList(), _currentSortIndex);

            FilteredAircraft = new ObservableCollection<MarketAircraftViewModel>(sorted);

            var isFiltered = !string.IsNullOrEmpty(filterText) || _showOnlyFavorites;
            AircraftCountText = isFiltered
                ? $"{FilteredAircraft.Count} of {MarketAircraft.Count} aircraft"
                : $"{FilteredAircraft.Count} aircraft available";
        }

        public void SortAircraft(int sortIndex)
        {
            _logger.Debug($"MarketViewModel: Sorting aircraft by index {sortIndex}");
            _currentSortIndex = sortIndex;
            ApplyFilter();
        }

        private System.Collections.Generic.List<MarketAircraftViewModel> ApplySort(System.Collections.Generic.List<MarketAircraftViewModel> aircraft, int sortIndex)
        {
            IOrderedEnumerable<MarketAircraftViewModel> sorted = sortIndex switch
            {
                0 => aircraft.OrderBy(a => a.DisplayName),
                1 => aircraft.OrderByDescending(a => a.DisplayName),
                2 => aircraft.OrderBy(a => a.PassengerCapacity),
                3 => aircraft.OrderByDescending(a => a.PassengerCapacity),
                4 => aircraft.OrderBy(a => a.MaxCargoKg),
                5 => aircraft.OrderByDescending(a => a.MaxCargoKg),
                6 => aircraft.OrderBy(a => a.Price),
                7 => aircraft.OrderByDescending(a => a.Price),
                8 => aircraft.OrderBy(a => a.ProfitPerHour),
                9 => aircraft.OrderByDescending(a => a.ProfitPerHour),
                10 => aircraft.OrderBy(a => a.ReturnOnInvestment),
                11 => aircraft.OrderByDescending(a => a.ReturnOnInvestment),
                _ => aircraft.OrderBy(a => a.DisplayName)
            };
            return sorted.ToList();
        }

        public void ToggleFavorite(MarketAircraftViewModel aircraft)
        {
            try
            {
                _catalogRepository.ToggleFavorite(aircraft.Title);
                aircraft.IsFavorite = !aircraft.IsFavorite;
                ApplyFilter();
                _logger.Info($"MarketViewModel: Toggled favorite for {aircraft.DisplayName} to {aircraft.IsFavorite}");
            }
            catch (Exception ex)
            {
                _logger.Error($"MarketViewModel: Failed to toggle favorite: {ex.Message}", ex);
            }
        }

        public void UpdateBalance()
        {
            var balance = _financeService.Balance;
            BalanceText = $"{balance:N0} €";
            _logger.Debug($"MarketViewModel: Updated balance display to {balance:N0} €");

            var fleetValue = _aircraftRepository.GetAllAircraft().Sum(a => a.CurrentValue);
            var totalBudget = balance + fleetValue;

            foreach (var aircraft in MarketAircraft)
            {
                aircraft.CanAfford = balance >= aircraft.Price;
                aircraft.CanSellToAfford = !aircraft.CanAfford && totalBudget >= aircraft.Price;
            }
        }

        public (bool Success, string Message) PurchaseAircraft(MarketAircraftViewModel aircraft)
        {
            try
            {
                _logger.Info($"MarketViewModel: Creating aircraft purchase for {aircraft.DisplayName}");

                var registration = GenerateRegistration();
                _logger.Info($"MarketViewModel: Generated registration: {registration}");

                var cruiseSpeed = aircraft.CruiseSpeedKts > 0
                    ? aircraft.CruiseSpeedKts
                    : EstimateCruiseSpeed(aircraft.PassengerCapacity, aircraft.Price);
                var range = aircraft.MaxRangeNM > 0
                    ? aircraft.MaxRangeNM
                    : EstimateRange(aircraft.PassengerCapacity, aircraft.Price);
                var fuelCapacity = aircraft.FuelCapacityGal;
                var fuelBurn = aircraft.FuelBurnGalPerHour;
                var operatingCost = aircraft.HourlyOperatingCost;

                var aircraftType = !string.IsNullOrWhiteSpace(aircraft.Type) ? aircraft.Type : ExtractAircraftType(aircraft.Title);

                var copiedImagePath = _imageService.CopyCustomImage(aircraft.CustomImagePath, registration);

                var newAircraft = new AircraftModel
                {
                    Registration = registration,
                    Type = aircraftType,
                    Variant = aircraft.Title,
                    HomeBase = "EDNY",
                    Status = AircraftStatus.Available,
                    TotalFlightHours = 0,
                    PurchasePrice = aircraft.Price,
                    CurrentValue = aircraft.Price,
                    MaxPassengers = aircraft.PassengerCapacity,
                    MaxCargoKg = aircraft.MaxCargoKg,
                    MaxRangeNM = range,
                    CruiseSpeedKts = cruiseSpeed,
                    FuelCapacityGal = fuelCapacity,
                    FuelBurnGalPerHour = fuelBurn,
                    HourlyOperatingCost = operatingCost,
                    LastMaintenanceDate = DateTime.Now,
                    PurchaseDate = DateTime.Now,
                    CustomImagePath = copiedImagePath
                };

                _aircraftRepository.AddAircraft(newAircraft);
                _logger.Info($"MarketViewModel: Added aircraft to database: {registration} - {aircraft.DisplayName}");
                if (copiedImagePath != null)
                {
                    _logger.Info($"MarketViewModel: Copied custom image to {copiedImagePath}");
                }

                var transaction = new Transaction
                {
                    Date = DateTime.Now,
                    Amount = -aircraft.Price,
                    Type = "Aircraft Purchase",
                    Description = $"Purchase: {aircraft.DisplayName} ({registration})"
                };

                _transactionRepository.AddTransaction(transaction);
                _logger.Info($"MarketViewModel: Created purchase transaction: {transaction.Amount:N0} €");

                _financeService.LoadTransactions();
                _logger.Info($"MarketViewModel: Balance refreshed. New balance: {_financeService.Balance:N0} €");

                CheckFleetAchievements();

                return (true, $"{aircraft.DisplayName} ({registration})");
            }
            catch (Exception ex)
            {
                _logger.Error($"MarketViewModel: Error purchasing aircraft: {ex.Message}", ex);
                return (false, ex.Message);
            }
        }

        private void CheckFleetAchievements()
        {
            var aircraftCount = _aircraftRepository.GetAircraftCount();
            var fleetValue = _aircraftRepository.GetTotalFleetValue();
            _achievementService.CheckFleetAchievements(aircraftCount, fleetValue);
        }

        private string GenerateRegistration()
        {
            var random = new Random();
            var existingRegistrations = _aircraftRepository.GetAllAircraft()
                .Select(a => a.Registration)
                .ToHashSet();
            string registration;

            do
            {
                var letters = $"{(char)random.Next('A', 'Z' + 1)}{(char)random.Next('A', 'Z' + 1)}";
                var numbers = random.Next(100, 1000);
                registration = $"D-E{letters}{numbers}";
            }
            while (existingRegistrations.Contains(registration));

            return registration;
        }

        private double EstimateCruiseSpeed(int passengerCapacity, decimal price)
        {
            if (passengerCapacity <= 0) passengerCapacity = 1;

            if (passengerCapacity <= 4) return 120 + (passengerCapacity * 10);
            if (passengerCapacity <= 9) return 180 + (passengerCapacity * 5);
            if (passengerCapacity <= 19) return 220 + (passengerCapacity * 3);
            if (passengerCapacity <= 50) return 280 + (passengerCapacity * 2);
            return 450 + (passengerCapacity * 0.5);
        }

        private double EstimateRange(int passengerCapacity, decimal price)
        {
            if (passengerCapacity <= 0) passengerCapacity = 1;

            if (passengerCapacity <= 4) return 500 + (passengerCapacity * 50);
            if (passengerCapacity <= 9) return 800 + (passengerCapacity * 40);
            if (passengerCapacity <= 19) return 1200 + (passengerCapacity * 30);
            if (passengerCapacity <= 50) return 1800 + (passengerCapacity * 20);
            return 3000 + (passengerCapacity * 15);
        }

        private string ExtractAircraftType(string title)
        {
            var parts = title.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2)
            {
                var manufacturer = parts[0];
                var model = parts[1];

                if (model.Any(char.IsDigit))
                {
                    return model;
                }

                if (parts.Length >= 3 && parts[2].Any(char.IsDigit))
                {
                    return parts[2];
                }
            }

            return parts.Length > 0 ? parts[0] : title;
        }
    }
}
