using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Dialogs
{
    public partial class CreateRouteDialog : Window
    {
        private readonly int _originFboId;
        private readonly string _originIcao;
        private readonly IScheduledRouteService _routeService;
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _logger;
        private readonly IFBORepository _fboRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IPilotRepository _pilotRepository;
        private readonly IAircraftPilotAssignmentRepository _assignmentRepository;

        private List<FBOOption> _availableFBOs = new();
        private List<AircraftOption> _availableAircraft = new();

        public CreateRouteDialog(
            int originFboId,
            string originIcao,
            IScheduledRouteService routeService,
            IAirportDatabase airportDatabase,
            ILoggingService logger,
            IFBORepository fboRepository,
            IAircraftRepository aircraftRepository,
            IPilotRepository pilotRepository,
            IAircraftPilotAssignmentRepository assignmentRepository)
        {
            InitializeComponent();

            _originFboId = originFboId;
            _originIcao = originIcao;
            _routeService = routeService;
            _airportDatabase = airportDatabase;
            _logger = logger;
            _fboRepository = fboRepository;
            _aircraftRepository = aircraftRepository;
            _pilotRepository = pilotRepository;
            _assignmentRepository = assignmentRepository;

            LoadData();
        }

        private void LoadData()
        {
            var originFbo = _fboRepository.GetFBOById(_originFboId);
            if (originFbo == null)
            {
                Close();
                return;
            }

            TxtOriginFBO.Text = $"{originFbo.ICAO} - {originFbo.AirportName}";

            var originUsed = _routeService.GetRouteCountForFBO(_originFboId);
            var originMax = _routeService.GetMaxSlots(originFbo.Type);
            TxtOriginSlots.Text = $"Origin ({_originIcao}): {originUsed}/{originMax} slots used";

            var originAirportDetail = _airportDatabase.GetAirportDetail(_originIcao);
            var originCountry = originAirportDetail?.Country ?? "";

            _availableFBOs = _fboRepository.GetAllFBOs()
                .Where(f => f.Id != _originFboId)
                .Where(f => _routeService.CanAddRouteToFBO(f.Id) &&
                            _routeService.GetRemainingRoutesBetweenFBOs(_originFboId, f.Id) > 0)
                .Where(f => IsRouteAllowedByFBOType(originFbo.Type, originCountry, f.ICAO))
                .Select(f => new FBOOption
                {
                    Id = f.Id,
                    ICAO = f.ICAO,
                    DisplayName = $"{f.ICAO} - {f.AirportName}",
                    Type = f.Type
                })
                .ToList();

            CmbDestinationFBO.ItemsSource = _availableFBOs;

            _availableAircraft = new List<AircraftOption> { new AircraftOption { Id = 0, DisplayName = "(No aircraft)" } };
            CmbAircraft.ItemsSource = _availableAircraft;
            CmbAircraft.SelectedIndex = 0;
            TxtAircraftInfo.Text = "Select destination first";

            UpdateCreateButtonState();
        }

        private void CmbDestinationFBO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbDestinationFBO.SelectedItem is FBOOption selected)
            {
                var used = _routeService.GetRouteCountForFBO(selected.Id);
                var max = _routeService.GetMaxSlots(selected.Type);
                TxtDestinationSlots.Text = $"{used}/{max} slots used at {selected.ICAO}";
                TxtDestSlots.Text = $"Destination ({selected.ICAO}): {used}/{max} slots used";

                LoadAvailableAircraft(selected.Id, selected.ICAO);
            }
            else
            {
                TxtDestinationSlots.Text = "";
                TxtDestSlots.Text = "Destination: Select an FBO";
                _availableAircraft = new List<AircraftOption> { new AircraftOption { Id = 0, DisplayName = "(No aircraft)" } };
                CmbAircraft.ItemsSource = _availableAircraft;
                CmbAircraft.SelectedIndex = 0;
                TxtAircraftInfo.Text = "Select destination first";
            }

            UpdateCreateButtonState();
        }

        private void LoadAvailableAircraft(int destFboId, string destIcao)
        {
            var originFbo = _fboRepository.GetFBOById(_originFboId);
            var destFbo = _fboRepository.GetFBOById(destFboId);
            if (originFbo == null || destFbo == null)
            {
                _availableAircraft = new List<AircraftOption> { new AircraftOption { Id = 0, DisplayName = "(No aircraft)" } };
                CmbAircraft.ItemsSource = _availableAircraft;
                CmbAircraft.SelectedIndex = 0;
                return;
            }

            var minTerminalSize = originFbo.TerminalSize < destFbo.TerminalSize
                ? originFbo.TerminalSize
                : destFbo.TerminalSize;

            var originAirport = _airportDatabase.GetAirport(_originIcao);
            var destAirport = _airportDatabase.GetAirport(destIcao);
            double routeDistance = 0;
            if (originAirport != null && destAirport != null)
            {
                routeDistance = _airportDatabase.CalculateDistanceBetweenAirports(originAirport, destAirport);
            }

            var existingRoutes = _routeService.GetActiveRoutes();
            var assignedAircraftIds = existingRoutes
                .Where(r => r.AssignedAircraftId.HasValue)
                .Select(r => r.AssignedAircraftId!.Value)
                .ToHashSet();

            var aircraftFromOriginFBO = _aircraftRepository.GetAircraftWithPilotByFBOIds(_originFboId)
                .Where(a => !assignedAircraftIds.Contains(a.Id))
                .Where(a =>
                {
                    var aircraftSize = AircraftSizeExtensions.GetAircraftSize(a.MaxPassengers);
                    var requiredTerminal = aircraftSize.GetRequiredTerminalSize();
                    return requiredTerminal <= minTerminalSize;
                })
                .Where(a => routeDistance <= 0 || a.MaxRangeNM >= routeDistance)
                .Select(a =>
                {
                    var pilotAssign = _assignmentRepository.GetAssignmentsByAircraftId(a.Id).FirstOrDefault();
                    var pilot = pilotAssign != null ? _pilotRepository.GetPilotById(pilotAssign.PilotId) : null;
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

            _availableAircraft = new List<AircraftOption> { new AircraftOption { Id = 0, DisplayName = "(No aircraft)" } };
            _availableAircraft.AddRange(aircraftFromOriginFBO);
            CmbAircraft.ItemsSource = _availableAircraft;
            CmbAircraft.SelectedIndex = 0;

            if (aircraftFromOriginFBO.Count == 0)
            {
                TxtAircraftInfo.Text = $"No compatible aircraft (Terminal: {minTerminalSize}, Range: {routeDistance:N0} NM)";
            }
            else
            {
                TxtAircraftInfo.Text = $"{aircraftFromOriginFBO.Count} compatible aircraft";
            }
        }

        private void UpdateCreateButtonState()
        {
            BtnCreate.IsEnabled = CmbDestinationFBO.SelectedItem is FBOOption;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CmbDestinationFBO.SelectedItem is not FBOOption destFbo)
            {
                return;
            }

            var result = _routeService.CreateRoute(_originFboId, destFbo.Id);
            if (!result.Success)
            {
                var errorDialog = new MessageDialog("Error", result.Message);
                errorDialog.Owner = this;
                errorDialog.ShowDialog();
                return;
            }

            if (CmbAircraft.SelectedItem is AircraftOption aircraft && aircraft.Id > 0)
            {
                var routes = _routeService.GetRoutesByFBO(_originFboId);
                var newRoute = routes.FirstOrDefault(r =>
                    r.OriginFBOId == _originFboId && r.DestinationFBOId == destFbo.Id && !r.AssignedAircraftId.HasValue);

                if (newRoute != null)
                {
                    var assignResult = _routeService.AssignAircraftToRoute(newRoute.Id, aircraft.Id);
                    if (!assignResult.Success)
                    {
                        _logger.Warn($"Route created but aircraft assignment failed: {assignResult.Message}");
                    }
                }
            }

            _logger.Info($"Created route: {_originIcao} → {destFbo.ICAO}");
            DialogResult = true;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool IsRouteAllowedByFBOType(FBOType originFboType, string originCountry, string destIcao)
        {
            if (originFboType == FBOType.International)
                return true;

            var destAirportDetail = _airportDatabase.GetAirportDetail(destIcao);
            var destCountry = destAirportDetail?.Country ?? "";

            if (string.IsNullOrEmpty(originCountry) || string.IsNullOrEmpty(destCountry))
                return true;

            if (originFboType == FBOType.Local)
            {
                return originCountry == destCountry;
            }

            if (originFboType == FBOType.Regional)
            {
                if (originCountry == destCountry)
                    return true;

                return AreCountriesInSameRegion(originCountry, destCountry);
            }

            return true;
        }

        private static bool AreCountriesInSameRegion(string country1, string country2)
        {
            var europeanCountries = new HashSet<string>
            {
                "Germany", "France", "Italy", "Spain", "Portugal", "Netherlands", "Belgium",
                "Austria", "Switzerland", "Poland", "Czech Republic", "Hungary", "Slovakia",
                "Slovenia", "Croatia", "Greece", "Denmark", "Sweden", "Norway", "Finland",
                "Ireland", "United Kingdom", "Luxembourg", "Estonia", "Latvia", "Lithuania",
                "Romania", "Bulgaria", "Serbia", "Montenegro", "Albania", "North Macedonia",
                "Bosnia and Herzegovina", "Iceland", "Malta", "Cyprus"
            };

            var northAmericanCountries = new HashSet<string>
            {
                "United States", "Canada", "Mexico"
            };

            var southAmericanCountries = new HashSet<string>
            {
                "Brazil", "Argentina", "Chile", "Colombia", "Peru", "Venezuela", "Ecuador",
                "Bolivia", "Paraguay", "Uruguay", "Guyana", "Suriname"
            };

            var asianCountries = new HashSet<string>
            {
                "China", "Japan", "South Korea", "India", "Thailand", "Vietnam", "Philippines",
                "Indonesia", "Malaysia", "Singapore", "Taiwan", "Hong Kong"
            };

            var middleEasternCountries = new HashSet<string>
            {
                "United Arab Emirates", "Saudi Arabia", "Qatar", "Kuwait", "Bahrain", "Oman",
                "Israel", "Jordan", "Lebanon", "Turkey", "Egypt"
            };

            var oceaniaCountries = new HashSet<string>
            {
                "Australia", "New Zealand", "Fiji", "Papua New Guinea"
            };

            var africanCountries = new HashSet<string>
            {
                "South Africa", "Kenya", "Nigeria", "Morocco", "Ethiopia", "Tanzania", "Ghana"
            };

            return (europeanCountries.Contains(country1) && europeanCountries.Contains(country2)) ||
                   (northAmericanCountries.Contains(country1) && northAmericanCountries.Contains(country2)) ||
                   (southAmericanCountries.Contains(country1) && southAmericanCountries.Contains(country2)) ||
                   (asianCountries.Contains(country1) && asianCountries.Contains(country2)) ||
                   (middleEasternCountries.Contains(country1) && middleEasternCountries.Contains(country2)) ||
                   (oceaniaCountries.Contains(country1) && oceaniaCountries.Contains(country2)) ||
                   (africanCountries.Contains(country1) && africanCountries.Contains(country2));
        }

        private class FBOOption
        {
            public int Id { get; set; }
            public string ICAO { get; set; } = "";
            public string DisplayName { get; set; } = "";
            public FBOType Type { get; set; }
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
