using System;
using System.Windows;
using System.Windows.Controls;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.FBO
{
    public partial class AirportDetailView : UserControl
    {
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _loggingService;
        private readonly string _icao;

        public event EventHandler? BackRequested;

        public AirportDetailView(
            IAirportDatabase airportDatabase,
            ILoggingService loggingService,
            string icao)
        {
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _icao = icao ?? throw new ArgumentNullException(nameof(icao));

            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadAirportData();
        }

        private void LoadAirportData()
        {
            _loggingService.Info($"AirportDetailView: Loading data for {_icao}");

            var detail = _airportDatabase.GetAirportDetail(_icao);

            if (detail == null)
            {
                TxtTitle.Text = $"{_icao} - Airport Details";
                TxtNoData.Visibility = Visibility.Visible;
                return;
            }

            var iataDisplay = string.IsNullOrEmpty(detail.Iata) ? "" : $" ({detail.Iata})";
            TxtTitle.Text = $"{detail.Icao}{iataDisplay} - {detail.Name}";

            var cityCountry = new[] { detail.City, detail.Country };
            TxtLocation.Text = string.Join(", ", Array.FindAll(cityCountry, s => !string.IsNullOrEmpty(s)));
            if (string.IsNullOrEmpty(TxtLocation.Text))
                TxtLocation.Text = detail.Name;

            TxtCoordinates.Text = $"{detail.Latitude:F4}N, {detail.Longitude:F4}E";
            TxtElevation.Text = $"{detail.Elevation} ft";

            BadgeAvgas.Visibility = Visibility.Visible;
            BadgeJetfuel.Visibility = Visibility.Visible;
            TxtNoFuel.Visibility = Visibility.Collapsed;

            RunwaysGrid.ItemsSource = detail.Runways;
            FrequenciesGrid.ItemsSource = detail.Frequencies;

            if (detail.IlsSystems.Count > 0)
            {
                IlsGrid.ItemsSource = detail.IlsSystems;
                TxtNoIls.Visibility = Visibility.Collapsed;
                TxtIlsHeader.Visibility = Visibility.Visible;
                IlsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                TxtNoIls.Visibility = Visibility.Visible;
                TxtIlsHeader.Visibility = Visibility.Collapsed;
                IlsPanel.Visibility = Visibility.Collapsed;
            }

            _loggingService.Info($"AirportDetailView: Loaded {detail.Icao} - {detail.Runways.Count} runways, {detail.Frequencies.Count} frequencies, {detail.IlsSystems.Count} ILS");
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
