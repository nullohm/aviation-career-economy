using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ace.App.Interfaces;

namespace Ace.App.ViewModels
{
    public class HangarViewModel : ViewModelBase
    {
        private readonly ILoggingService _logger;
        private readonly IPersistenceService _persistenceService;
        private readonly IFinanceService _financeService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IPilotRepository _pilotRepository;
        private readonly IFBORepository _fboRepository;
        private readonly IScheduledRouteRepository _routeRepository;

        private ObservableCollection<AircraftViewModel> _aircraft = new();
        private ObservableCollection<AircraftViewModel> _filteredAircraft = new();
        private string _filterText = string.Empty;
        private string _selectedStatus = "All";
        private string _aircraftCountText = string.Empty;

        public ObservableCollection<AircraftViewModel> Aircraft
        {
            get => _aircraft;
            set => SetProperty(ref _aircraft, value);
        }

        public ObservableCollection<AircraftViewModel> FilteredAircraft
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

        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetProperty(ref _selectedStatus, value))
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

        public HangarViewModel(
            ILoggingService logger,
            IPersistenceService persistenceService,
            IFinanceService financeService,
            IMaintenanceService maintenanceService,
            IAircraftRepository aircraftRepository,
            IPilotRepository pilotRepository,
            IFBORepository fboRepository,
            IScheduledRouteRepository routeRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _persistenceService = persistenceService ?? throw new ArgumentNullException(nameof(persistenceService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _maintenanceService = maintenanceService ?? throw new ArgumentNullException(nameof(maintenanceService));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));

            _persistenceService.FlightRecordsChanged += OnFlightRecordsChanged;
        }

        private void OnFlightRecordsChanged()
        {
            LoadAircraft();
        }

        public void LoadAircraft()
        {
            try
            {
                _logger.Info("HangarViewModel: Starting LoadAircraft");

                var aircraft = _aircraftRepository.GetAllAircraft();
                var pilots = _pilotRepository.GetEmployedPilots();
                var fbos = _fboRepository.GetAllFBOs();
                var scheduledRoutes = _routeRepository.GetActiveRoutes()
                    .Where(r => r.AssignedAircraftId.HasValue).ToList();
                _logger.Database("Aircraft query completed", aircraft.Count);

                Aircraft.Clear();

                foreach (var ac in aircraft)
                {
                    _logger.Debug($"Adding aircraft to UI: {ac.Registration}");
                    var assignedPilot = ac.AssignedPilotId.HasValue
                        ? pilots.FirstOrDefault(p => p.Id == ac.AssignedPilotId.Value)
                        : null;
                    var assignedFBO = ac.AssignedFBOId.HasValue
                        ? fbos.FirstOrDefault(f => f.Id == ac.AssignedFBOId.Value)
                        : null;

                    string? assignedRouteDisplay = null;
                    var assignedRoute = scheduledRoutes.FirstOrDefault(r => r.AssignedAircraftId == ac.Id);
                    if (assignedRoute != null)
                    {
                        var originFBO = fbos.FirstOrDefault(f => f.Id == assignedRoute.OriginFBOId);
                        var destFBO = fbos.FirstOrDefault(f => f.Id == assignedRoute.DestinationFBOId);
                        if (originFBO != null && destFBO != null)
                        {
                            assignedRouteDisplay = $"{originFBO.ICAO}→{destFBO.ICAO}";
                        }
                    }

                    Aircraft.Add(new AircraftViewModel(ac, _maintenanceService, assignedPilot, assignedFBO, assignedRouteDisplay));
                }

                ApplyFilter();
                _logger.Info($"HangarViewModel: Loaded {Aircraft.Count} aircraft");
            }
            catch (Exception ex)
            {
                _logger.Error("HangarViewModel: Error loading aircraft", ex);
            }
        }

        public void ApplyFilter()
        {
            var filterText = FilterText?.Trim().ToLowerInvariant() ?? "";
            var selectedStatus = SelectedStatus ?? "All";

            FilteredAircraft.Clear();

            var filtered = Aircraft.AsEnumerable();

            if (!string.IsNullOrEmpty(filterText))
            {
                filtered = filtered.Where(a =>
                    a.Registration.ToLowerInvariant().Contains(filterText) ||
                    a.TypeVariant.ToLowerInvariant().Contains(filterText) ||
                    a.HomeBase.ToLowerInvariant().Contains(filterText));
            }

            if (selectedStatus != "All")
            {
                filtered = filtered.Where(a => a.Status == selectedStatus);
            }

            foreach (var aircraft in filtered)
            {
                FilteredAircraft.Add(aircraft);
            }

            var filterActive = !string.IsNullOrEmpty(filterText) || selectedStatus != "All";
            AircraftCountText = filterActive
                ? $"{FilteredAircraft.Count} of {Aircraft.Count} aircraft"
                : $"{FilteredAircraft.Count} aircraft in hangar";

            _logger.Debug($"HangarViewModel: Applied filter - showing {FilteredAircraft.Count} of {Aircraft.Count} aircraft");
        }

        public (bool Success, string Message) SellAircraft(AircraftViewModel aircraft)
        {
            if (aircraft == null)
                return (false, "No aircraft selected");

            try
            {
                var dbAircraft = _aircraftRepository.GetAircraftById(aircraft.Id);

                if (dbAircraft == null)
                    return (false, "Aircraft not found in database");

                var salePrice = dbAircraft.CurrentValue;
                var registration = dbAircraft.Registration;

                var assignedRoute = _routeRepository.GetRouteByAircraft(aircraft.Id);
                if (assignedRoute != null)
                {
                    _routeRepository.DeleteRoute(assignedRoute.Id);
                    _logger.Info($"HangarViewModel: Deleted assigned route for aircraft {registration}");
                }

                _aircraftRepository.DeleteAircraft(aircraft.Id);

                _financeService.AddEarnings(salePrice, $"Sold aircraft {registration}");

                _logger.Info($"HangarViewModel: Sold aircraft {registration} for {salePrice:N0} €");

                LoadAircraft();
                return (true, $"{salePrice:N0} €");
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarViewModel: Failed to sell aircraft: {ex.Message}");
                return (false, ex.Message);
            }
        }

        public (bool Success, string Message) DeleteAircraft(AircraftViewModel aircraft)
        {
            if (aircraft == null)
                return (false, "No aircraft selected");

            try
            {
                var dbAircraft = _aircraftRepository.GetAircraftById(aircraft.Id);

                if (dbAircraft == null)
                    return (false, "Aircraft not found in database");

                var registration = dbAircraft.Registration;

                var assignedRoute = _routeRepository.GetRouteByAircraft(aircraft.Id);
                if (assignedRoute != null)
                {
                    _routeRepository.DeleteRoute(assignedRoute.Id);
                    _logger.Info($"HangarViewModel: Deleted assigned route for aircraft {registration}");
                }

                _aircraftRepository.DeleteAircraft(aircraft.Id);

                _logger.Info($"HangarViewModel: Deleted aircraft {registration}");

                LoadAircraft();
                return (true, registration);
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarViewModel: Failed to delete aircraft: {ex.Message}");
                return (false, ex.Message);
            }
        }
    }
}
