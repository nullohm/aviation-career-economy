using System;
using System.Globalization;
using System.Windows;
using Ace.App.Interfaces;

namespace Ace.App.Views.Dialogs
{
    public partial class EditAircraftSpecsDialog : Window
    {
        private readonly int _aircraftId;
        private readonly ILoggingService _logger;
        private readonly IAircraftRepository _aircraftRepository;
        public bool Saved { get; private set; }

        public EditAircraftSpecsDialog(int aircraftId, string aircraftName, int maxPassengers, double maxCargo, double cruiseSpeed, double maxRange, double fuelCapacity, double fuelBurn, int serviceCeiling, decimal operatingCost, double flightHours, ILoggingService logger, IAircraftRepository aircraftRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));

            InitializeComponent();
            _aircraftId = aircraftId;

            AircraftName.Text = aircraftName;
            TxtMaxPassengers.Text = maxPassengers.ToString();
            TxtMaxCargo.Text = maxCargo.ToString("F0", CultureInfo.InvariantCulture);
            TxtCruiseSpeed.Text = cruiseSpeed.ToString("F0", CultureInfo.InvariantCulture);
            TxtMaxRange.Text = maxRange.ToString("F0", CultureInfo.InvariantCulture);
            TxtFuelCapacity.Text = fuelCapacity.ToString("F0", CultureInfo.InvariantCulture);
            TxtFuelBurn.Text = fuelBurn.ToString("F1", CultureInfo.InvariantCulture);
            TxtServiceCeiling.Text = serviceCeiling.ToString();
            TxtOperatingCost.Text = operatingCost.ToString("F0", CultureInfo.InvariantCulture);
            TxtFlightHours.Text = flightHours.ToString("F1", CultureInfo.InvariantCulture);

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

            if (!decimal.TryParse(TxtOperatingCost.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal operatingCost) || operatingCost < 0)
            {
                InfoDialog.Show("Validation Error", "Operating Cost must be a valid positive number.", this);
                return;
            }

            if (!double.TryParse(TxtFlightHours.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double flightHours) || flightHours < 0)
            {
                InfoDialog.Show("Validation Error", "Flight Hours must be a valid positive number.", this);
                return;
            }

            if (operatingCost == 0 && fuelBurn > 0)
            {
                var fuelCostPerGallon = 6.0m;
                var baseCost = (decimal)fuelBurn * fuelCostPerGallon;
                operatingCost = Math.Round(baseCost * 1.5m, 0);
                _logger.Info($"EditAircraftSpecs: Auto-calculated operating cost: {operatingCost} €/h from fuel burn {fuelBurn} gal/h");
            }

            try
            {
                var aircraft = _aircraftRepository.GetAircraftById(_aircraftId);

                if (aircraft != null)
                {
                    aircraft.MaxPassengers = maxPassengers;
                    aircraft.MaxCargoKg = maxCargo;
                    aircraft.CruiseSpeedKts = cruiseSpeed;
                    aircraft.MaxRangeNM = maxRange;
                    aircraft.FuelCapacityGal = fuelCapacity;
                    aircraft.FuelBurnGalPerHour = fuelBurn;
                    aircraft.ServiceCeilingFt = serviceCeiling;
                    aircraft.HourlyOperatingCost = operatingCost;
                    aircraft.TotalFlightHours = flightHours;

                    _aircraftRepository.UpdateAircraft(aircraft);

                    _logger.Info($"EditAircraftSpecs: Updated {aircraft.Registration} - PAX:{maxPassengers}, Cargo:{maxCargo}kg, Speed:{cruiseSpeed}kts, Range:{maxRange}NM, Fuel:{fuelCapacity}gal, Burn:{fuelBurn}gph, Cost:{operatingCost}€/h, Hours:{flightHours}h");
                }

                Saved = true;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                _logger.Error($"EditAircraftSpecs: Failed to save: {ex.Message}");
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
