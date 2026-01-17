using System;
using System.Windows;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Dialogs
{
    public partial class FlightDetailDialog : Window
    {
        private readonly ILoggingService _logger;
        private readonly IPricingService _pricingService;
        private readonly IFlightRepository _flightRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ITransactionRepository _transactionRepository;

        public FlightDetailDialog(
            int flightId,
            ILoggingService logger,
            IPricingService pricingService,
            IFlightRepository flightRepository,
            IAircraftRepository aircraftRepository,
            ITransactionRepository transactionRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));

            InitializeComponent();
            Loaded += (s, e) => CenterOnOwner();
            LoadFlightDetails(flightId);
        }

        private void CenterOnOwner()
        {
            if (Owner == null) return;
            Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
            Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
        }

        private void LoadFlightDetails(int flightId)
        {
            try
            {
                var flight = _flightRepository.GetFlightById(flightId);

                if (flight == null)
                {
                    _logger.Warn($"FlightDetailDialog: Flight {flightId} not found");
                    TxtDeparture.Text = "?";
                    TxtArrival.Text = "?";
                    return;
                }

                TxtDeparture.Text = flight.Departure;
                TxtArrival.Text = flight.Arrival;
                TxtAircraft.Text = flight.Aircraft;
                TxtDate.Text = flight.Date.ToString("dd.MM.yyyy HH:mm");
                TxtDistance.Text = $"{flight.DistanceNM:F0} NM";
                TxtDuration.Text = $"{flight.Duration.Hours}:{flight.Duration.Minutes:D2}";
                TxtLandingRate.Text = $"{-flight.LandingRate:F0} fpm";

                int passengers = ExtractPassengersFromEarnings(flight);
                TxtPassengers.Text = $"{passengers} PAX";

                var aircraft = _aircraftRepository.GetAircraftByRegistration(flight.Aircraft);
                if (aircraft != null)
                {
                    var breakdown = _pricingService.CalculateFlightPrice(
                        aircraft,
                        flight.DistanceNM,
                        passengers,
                        flight.Duration.TotalHours);

                    TxtFuel.Text = $"-{breakdown.FuelCost:N2}";
                    TxtMaintenance.Text = $"-{breakdown.MaintenanceCost:N2}";
                    TxtDepreciation.Text = $"-{breakdown.DepreciationCost:N2}";
                    TxtInsurance.Text = $"-{breakdown.InsuranceCost:N2}";
                    TxtCrew.Text = $"-{breakdown.CrewCost:N2}";
                    TxtLanding.Text = $"-{breakdown.LandingFees:N2}";
                    TxtGround.Text = $"-{breakdown.GroundHandlingCost:N2}";
                    TxtCatering.Text = $"-{breakdown.CateringCost:N2}";

                    TxtTotalCosts.Text = $"-{breakdown.TotalOperatingCost:N2}";
                    TxtRevenue.Text = $"+{breakdown.TotalPrice:N2}";

                    // Calculate and show player bonus if actual earnings differ from calculated profit
                    decimal baseProfit = breakdown.Profit;
                    decimal actualEarnings = flight.Earnings;
                    decimal bonusAmount = actualEarnings - baseProfit;

                    if (bonusAmount > 0.01m)
                    {
                        TxtBonusLabel.Visibility = Visibility.Visible;
                        TxtBonus.Visibility = Visibility.Visible;
                        TxtBonus.Text = $"+{bonusAmount:N2}";
                    }

                    // Show actual earnings (with bonus applied)
                    TxtProfit.Text = actualEarnings >= 0 ? $"+{actualEarnings:N2}" : $"{actualEarnings:N2}";
                    TxtProfit.Foreground = actualEarnings >= 0
                        ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80))
                        : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 67, 54));
                }
                else
                {
                    // Simple display from stored earnings
                    TxtFuel.Text = "-";
                    TxtMaintenance.Text = "-";
                    TxtDepreciation.Text = "-";
                    TxtInsurance.Text = "-";
                    TxtCrew.Text = "-";
                    TxtLanding.Text = "-";
                    TxtGround.Text = "-";
                    TxtCatering.Text = "-";

                    TxtTotalCosts.Text = "-";
                    TxtRevenue.Text = "-";
                    TxtProfit.Text = $"{flight.Earnings:N2}";
                }

                TitleText.Text = $"Flight {flight.Departure} -> {flight.Arrival}";
            }
            catch (Exception ex)
            {
                _logger.Error($"FlightDetailDialog: Error loading flight {flightId}", ex);
            }
        }

        private int ExtractPassengersFromEarnings(FlightRecord flight)
        {
            try
            {
                var transaction = _transactionRepository.GetTransactionByFlightId(flight.Id);
                if (transaction != null && transaction.Description.Contains("PAX"))
                {
                    var paxMatch = System.Text.RegularExpressions.Regex.Match(
                        transaction.Description, @"\((\d+)\s*PAX\)");
                    if (paxMatch.Success && int.TryParse(paxMatch.Groups[1].Value, out int pax))
                    {
                        return pax;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Debug($"FlightDetailDialog: Failed to extract passengers: {ex.Message}");
            }

            return 0;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
