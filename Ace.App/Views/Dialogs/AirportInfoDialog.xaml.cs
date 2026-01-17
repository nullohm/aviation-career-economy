using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ace.App.Interfaces;

namespace Ace.App.Views.Dialogs
{
    public partial class AirportInfoDialog : Window
    {
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _logger;
        private readonly string _icao;

        public AirportInfoDialog(string icao, IAirportDatabase airportDatabase, ILoggingService logger)
        {
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _icao = icao;

            InitializeComponent();
            Loaded += (s, e) => LoadAirportData();

            // Enable mouse wheel scrolling through DataGrids
            RunwaysGrid.PreviewMouseWheel += DataGrid_PreviewMouseWheel;
            FrequenciesGrid.PreviewMouseWheel += DataGrid_PreviewMouseWheel;
            IlsGrid.PreviewMouseWheel += DataGrid_PreviewMouseWheel;
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Forward mouse wheel to parent ScrollViewer
            if (sender is DataGrid)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent,
                    Source = sender
                };
                MainScrollViewer.RaiseEvent(eventArg);
            }
        }

        private void LoadAirportData()
        {
            _logger.Info($"AirportInfoDialog: Loading data for {_icao}");

            var detail = _airportDatabase.GetAirportDetail(_icao);

            if (detail == null)
            {
                TxtTitle.Text = $"{_icao} - Airport Details";
                TxtNoData.Visibility = Visibility.Visible;
                return;
            }

            var iataDisplay = string.IsNullOrEmpty(detail.Iata) ? "" : $" ({detail.Iata})";
            TxtTitle.Text = $"{detail.Icao}{iataDisplay} - {detail.Name}";
            Title = $"{detail.Icao} - {detail.Name}";

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

            _logger.Info($"AirportInfoDialog: Loaded {detail.Icao} - {detail.Runways.Count} runways, {detail.Frequencies.Count} frequencies, {detail.IlsSystems.Count} ILS");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
