using System;
using System.Globalization;
using System.Windows;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Dialogs
{
    public partial class EditMarketAircraftSpecsDialog : Window
    {
        private readonly string _aircraftTitle;
        private readonly ILoggingService _logger;
        private readonly IAircraftCatalogRepository _catalogRepository;
        public bool Saved { get; private set; }

        public EditMarketAircraftSpecsDialog(
            string aircraftTitle,
            int maxPassengers,
            double maxCargo,
            double cruiseSpeed,
            double maxRange,
            double fuelCapacity,
            double fuelBurn,
            int serviceCeiling,
            decimal basePrice,
            ILoggingService logger,
            IAircraftCatalogRepository catalogRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));

            InitializeComponent();
            _aircraftTitle = aircraftTitle;

            AircraftName.Text = aircraftTitle;
            TxtMaxPassengers.Text = maxPassengers.ToString();
            TxtMaxCargo.Text = maxCargo.ToString("F0", CultureInfo.InvariantCulture);
            TxtCruiseSpeed.Text = cruiseSpeed.ToString("F0", CultureInfo.InvariantCulture);
            TxtMaxRange.Text = maxRange.ToString("F0", CultureInfo.InvariantCulture);
            TxtFuelCapacity.Text = fuelCapacity.ToString("F0", CultureInfo.InvariantCulture);
            TxtFuelBurn.Text = fuelBurn.ToString("F1", CultureInfo.InvariantCulture);
            TxtServiceCeiling.Text = serviceCeiling.ToString();
            TxtBasePrice.Text = basePrice.ToString("F0", CultureInfo.InvariantCulture);

            Loaded += Dialog_Loaded;
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner == null) return;
            Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
            Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
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

            if (!int.TryParse(TxtServiceCeiling.Text, out int serviceCeiling) || serviceCeiling < 0)
            {
                InfoDialog.Show("Validation Error", "Service Ceiling must be a valid positive number.", this);
                return;
            }

            if (!decimal.TryParse(TxtBasePrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal basePrice) || basePrice < 0)
            {
                InfoDialog.Show("Validation Error", "Base Price must be a valid positive number.", this);
                return;
            }

            try
            {
                var catalogEntry = _catalogRepository.GetAircraftByTitle(_aircraftTitle);
                if (catalogEntry != null)
                {
                    catalogEntry.PassengerCapacity = maxPassengers;
                    catalogEntry.MaxCargoKg = maxCargo;
                    catalogEntry.CruiseSpeedKts = cruiseSpeed;
                    catalogEntry.MaxRangeNM = maxRange;
                    catalogEntry.FuelCapacityGal = fuelCapacity;
                    catalogEntry.FuelBurnGalPerHour = fuelBurn;
                    catalogEntry.ServiceCeilingFt = serviceCeiling;
                    catalogEntry.MarketPrice = basePrice;

                    if (fuelBurn > 0)
                    {
                        var fuelCostPerGallon = 6.0m;
                        catalogEntry.HourlyOperatingCost = Math.Round((decimal)fuelBurn * fuelCostPerGallon * 1.5m, 0);
                    }

                    _catalogRepository.UpdateAircraftCatalogEntry(catalogEntry);
                    _logger.Info($"EditMarketAircraftSpecs: Updated AircraftCatalog for {_aircraftTitle} - PAX:{maxPassengers}, Cargo:{maxCargo}kg, Speed:{cruiseSpeed}kts, Price:{basePrice}€");
                }

                Saved = true;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                _logger.Error($"EditMarketAircraftSpecs: Failed to save: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to save changes: {ex.Message}", this);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Saved = false;
            DialogResult = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Saved = false;
            DialogResult = false;
        }
    }
}
