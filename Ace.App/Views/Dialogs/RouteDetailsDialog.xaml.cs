using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Dialogs
{
    public partial class RouteDetailsDialog : Window
    {
        private readonly int _routeId;
        private readonly IScheduledRouteService _routeService;
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _logger;
        private readonly IScheduledRouteRepository _routeRepository;
        private readonly IFBORepository _fboRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IPilotRepository _pilotRepository;

        private ScheduledRoute? _route;
        private Models.FBO? _originFbo;
        private Models.FBO? _destFbo;
        private List<AircraftOption> _availableAircraft = new();

        public RouteDetailsDialog(
            int routeId,
            IScheduledRouteService routeService,
            IAirportDatabase airportDatabase,
            ILoggingService logger,
            IScheduledRouteRepository routeRepository,
            IFBORepository fboRepository,
            IAircraftRepository aircraftRepository,
            IPilotRepository pilotRepository)
        {
            InitializeComponent();

            _routeId = routeId;
            _routeService = routeService;
            _airportDatabase = airportDatabase;
            _logger = logger;
            _routeRepository = routeRepository;
            _fboRepository = fboRepository;
            _aircraftRepository = aircraftRepository;
            _pilotRepository = pilotRepository;

            LoadData();
        }

        private void LoadData()
        {
            _route = _routeRepository.GetRouteById(_routeId);
            if (_route == null)
            {
                Close();
                return;
            }

            _originFbo = _fboRepository.GetFBOById(_route.OriginFBOId);
            _destFbo = _fboRepository.GetFBOById(_route.DestinationFBOId);

            if (_originFbo == null || _destFbo == null)
            {
                Close();
                return;
            }

            TxtOriginICAO.Text = _originFbo.ICAO;
            TxtOriginName.Text = _originFbo.AirportName;
            TxtDestICAO.Text = _destFbo.ICAO;
            TxtDestName.Text = _destFbo.AirportName;

            if (_route.AssignedAircraftId.HasValue)
            {
                var aircraft = _aircraftRepository.GetAircraftById(_route.AssignedAircraftId.Value);
                var pilot = aircraft?.AssignedPilotId.HasValue == true
                    ? _pilotRepository.GetPilotById(aircraft.AssignedPilotId.Value)
                    : null;

                TxtAircraftReg.Text = $"{aircraft?.Registration} ({aircraft?.Type})";
                TxtPilotName.Text = pilot?.Name ?? "Unknown";
                PanelAssigned.Visibility = Visibility.Visible;
                TxtNoAircraft.Visibility = Visibility.Collapsed;
                BtnUnassign.IsEnabled = true;
            }
            else
            {
                PanelAssigned.Visibility = Visibility.Collapsed;
                TxtNoAircraft.Visibility = Visibility.Visible;
                BtnUnassign.IsEnabled = false;
            }

            LoadAvailableAircraft();
        }

        private void LoadAvailableAircraft()
        {
            var existingRoutes = _routeService.GetActiveRoutes();
            var assignedAircraftIds = existingRoutes
                .Where(r => r.AssignedAircraftId.HasValue && r.Id != _routeId)
                .Select(r => r.AssignedAircraftId!.Value)
                .ToHashSet();

            var originFboId = _route!.OriginFBOId;
            var destFboId = _route.DestinationFBOId;

            var aircraftFromBothFBOs = _aircraftRepository.GetAircraftWithPilotByFBOIds(originFboId, destFboId)
                .Where(a => !assignedAircraftIds.Contains(a.Id))
                .Select(a =>
                {
                    var pilot = a.AssignedPilotId.HasValue ? _pilotRepository.GetPilotById(a.AssignedPilotId.Value) : null;
                    var fbo = a.AssignedFBOId.HasValue ? _fboRepository.GetFBOById(a.AssignedFBOId.Value) : null;
                    var fboIcao = fbo?.ICAO ?? "?";
                    return new AircraftOption
                    {
                        Id = a.Id,
                        Registration = a.Registration,
                        DisplayName = $"{a.Registration} ({a.Type}) @ {fboIcao}" + (pilot != null ? $" - {pilot.Name}" : ""),
                        PilotName = pilot?.Name ?? ""
                    };
                })
                .ToList();

            _availableAircraft = new List<AircraftOption> { new AircraftOption { Id = 0, DisplayName = "(Select aircraft)" } };
            _availableAircraft.AddRange(aircraftFromBothFBOs);
            CmbAircraft.ItemsSource = _availableAircraft;
            CmbAircraft.SelectedIndex = 0;

            if (aircraftFromBothFBOs.Count == 0)
            {
                TxtAircraftInfo.Text = $"No aircraft at {_originFbo?.ICAO} or {_destFbo?.ICAO} with pilot";
                BtnAssign.IsEnabled = false;
            }
            else
            {
                TxtAircraftInfo.Text = $"{aircraftFromBothFBOs.Count} aircraft available";
                BtnAssign.IsEnabled = true;
            }
        }

        private void BtnAssign_Click(object sender, RoutedEventArgs e)
        {
            if (CmbAircraft.SelectedItem is not AircraftOption aircraft || aircraft.Id == 0)
            {
                return;
            }

            var result = _routeService.AssignAircraftToRoute(_routeId, aircraft.Id);
            if (result.Success)
            {
                _logger.Info($"Assigned aircraft {aircraft.Registration} to route {_routeId}");
                DialogResult = true;
                Close();
            }
            else
            {
                var errorDialog = new MessageDialog("Error", result.Message);
                errorDialog.Owner = this;
                errorDialog.ShowDialog();
            }
        }

        private void BtnUnassign_Click(object sender, RoutedEventArgs e)
        {
            var result = _routeService.UnassignAircraftFromRoute(_routeId);
            if (result.Success)
            {
                _logger.Info($"Unassigned aircraft from route {_routeId}");
                DialogResult = true;
                Close();
            }
            else
            {
                var errorDialog = new MessageDialog("Error", result.Message);
                errorDialog.Owner = this;
                errorDialog.ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var confirmDialog = new ConfirmDialog(
                "Delete Route",
                $"Delete route {_originFbo?.ICAO} ↔ {_destFbo?.ICAO}?");
            confirmDialog.Owner = this;
            confirmDialog.ShowDialog();

            if (!confirmDialog.Result)
                return;

            var result = _routeService.DeleteRoute(_routeId);
            if (result.Success)
            {
                _logger.Info($"Deleted route {_routeId}");
                DialogResult = true;
                Close();
            }
            else
            {
                var errorDialog = new MessageDialog("Error", result.Message);
                errorDialog.Owner = this;
                errorDialog.ShowDialog();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private class AircraftOption
        {
            public int Id { get; set; }
            public string Registration { get; set; } = "";
            public string DisplayName { get; set; } = "";
            public string PilotName { get; set; } = "";
        }
    }
}
