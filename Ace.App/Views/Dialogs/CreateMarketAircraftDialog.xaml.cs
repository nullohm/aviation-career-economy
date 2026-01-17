using System;
using System.Globalization;
using System.Windows;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Dialogs
{
    public partial class CreateMarketAircraftDialog : Window
    {
        private readonly ILoggingService _logger;
        private readonly IAircraftCatalogRepository _catalogRepository;
        public bool Created { get; private set; }

        public CreateMarketAircraftDialog(ILoggingService logger, IAircraftCatalogRepository catalogRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));

            InitializeComponent();
            Loaded += Dialog_Loaded;
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner == null) return;
            Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
            Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
            TxtTitle.Focus();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var title = TxtTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                InfoDialog.Show("Validation Error", "Aircraft Title is required.", this);
                return;
            }

            if (!int.TryParse(TxtMaxPassengers.Text, out int maxPassengers) || maxPassengers < 0)
            {
                InfoDialog.Show("Validation Error", "Max Passengers must be a valid positive number.", this);
                return;
            }

            if (!double.TryParse(TxtMaxCargo.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double maxCargo) || maxCargo < 0)
            {
                InfoDialog.Show("Validation Error", "Max Cargo must be a valid positive number.", this);
                return;
            }

            if (!double.TryParse(TxtCruiseSpeed.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double cruiseSpeed) || cruiseSpeed < 0)
            {
                InfoDialog.Show("Validation Error", "Cruise Speed must be a valid positive number.", this);
                return;
            }

            if (!double.TryParse(TxtMaxRange.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double maxRange) || maxRange < 0)
            {
                InfoDialog.Show("Validation Error", "Max Range must be a valid positive number.", this);
                return;
            }

            if (!double.TryParse(TxtFuelCapacity.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double fuelCapacity) || fuelCapacity < 0)
            {
                InfoDialog.Show("Validation Error", "Fuel Capacity must be a valid positive number.", this);
                return;
            }

            if (!double.TryParse(TxtFuelBurn.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double fuelBurn) || fuelBurn < 0)
            {
                InfoDialog.Show("Validation Error", "Fuel Burn must be a valid positive number.", this);
                return;
            }

            if (!decimal.TryParse(TxtBasePrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal basePrice) || basePrice <= 0)
            {
                InfoDialog.Show("Validation Error", "Base Price must be a valid positive number.", this);
                return;
            }

            var manufacturer = TxtManufacturer.Text.Trim();
            var type = TxtType.Text.Trim();
            var category = TxtCategory.Text.Trim();
            if (string.IsNullOrEmpty(category)) category = "Aircraft";

            try
            {
                var existingCatalog = _catalogRepository.GetAircraftByTitle(title);
                if (existingCatalog != null)
                {
                    InfoDialog.Show("Error", $"An aircraft with title '{title}' already exists in the catalog.", this);
                    return;
                }

                decimal hourlyOperatingCost = 0;
                if (fuelBurn > 0)
                {
                    var fuelCostPerGallon = 6.0m;
                    hourlyOperatingCost = Math.Round((decimal)fuelBurn * fuelCostPerGallon * 1.5m, 0);
                }

                var catalogEntry = new AircraftCatalogEntry
                {
                    Title = title,
                    Manufacturer = manufacturer,
                    Type = type,
                    Category = category,
                    CrewCount = 1,
                    PassengerCapacity = maxPassengers,
                    MaxCargoKg = maxCargo,
                    MarketPrice = basePrice,
                    CruiseSpeedKts = cruiseSpeed,
                    MaxRangeNM = maxRange,
                    FuelCapacityGal = fuelCapacity,
                    FuelBurnGalPerHour = fuelBurn,
                    HourlyOperatingCost = hourlyOperatingCost,
                    FirstSeen = DateTime.Now,
                    LastSeen = DateTime.Now
                };

                _catalogRepository.AddAircraftCatalogEntry(catalogEntry);

                Created = true;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                _logger.Error($"CreateMarketAircraft: Failed to create: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to create aircraft: {ex.Message}", this);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Created = false;
            DialogResult = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Created = false;
            DialogResult = false;
        }
    }
}
