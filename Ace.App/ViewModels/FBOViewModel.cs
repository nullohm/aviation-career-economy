using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Models;
using Ace.App.Interfaces;

namespace Ace.App.ViewModels
{
    public class FBOViewModel
    {
        private readonly FBO _fbo;
        private readonly ISettingsService? _settingsService;
        private readonly List<Aircraft> _stationedAircraft;
        private readonly int _routeCount;
        private readonly int _maxRouteSlots;
        private readonly int _unassignedAircraftCount;

        public FBOViewModel(FBO fbo, ISettingsService? settingsService = null, int routeCount = 0, int maxRouteSlots = 0)
        {
            _fbo = fbo;
            _settingsService = settingsService;
            _routeCount = routeCount;
            _maxRouteSlots = maxRouteSlots;

            using var db = new AceDbContext();
            _stationedAircraft = db.Aircraft
                .Where(a => a.HomeBase == fbo.ICAO)
                .ToList();

            var assignedAircraftIds = db.ScheduledRoutes
                .Where(r => r.IsActive && r.AssignedAircraftId.HasValue)
                .Select(r => r.AssignedAircraftId!.Value)
                .ToHashSet();

            _unassignedAircraftCount = _stationedAircraft.Count(a => !assignedAircraftIds.Contains(a.Id));
        }

        public int Id => _fbo.Id;
        public string ICAO => _fbo.ICAO;
        public string AirportName => _fbo.AirportName;
        public string Type => _fbo.Type.ToString();
        public string MonthlyRent => $"{_fbo.MonthlyRent:N0} €";
        public string RentedSince => _fbo.RentedSince.ToString("dd.MM.yyyy");

        public string TerminalInfo => _fbo.TerminalSize == TerminalSize.None
            ? "No Terminal"
            : $"{_fbo.TerminalSize} Terminal ({_fbo.TerminalMonthlyCost:N0} €/month)";

        public string ServicesInfo
        {
            get
            {
                var services = new System.Collections.Generic.List<string>();
                if (_fbo.HasRefuelingService) services.Add("Refueling");
                if (_fbo.HasHangarService) services.Add("Hangar");
                if (_fbo.HasCateringService) services.Add("Catering");
                if (_fbo.HasGroundHandling) services.Add("Ground Handling");
                if (_fbo.HasDeIcingService) services.Add("De-Icing");

                return services.Count > 0 ? string.Join(", ", services) : "No Services";
            }
        }

        public string TotalMonthlyCost
        {
            get
            {
                return $"{TotalMonthlyCostValue:N0} €/mo";
            }
        }

        public int AircraftCount => _stationedAircraft.Count;
        public int UnassignedAircraftCount => _unassignedAircraftCount;
        public bool HasUnassignedAircraft => _unassignedAircraftCount > 0;
        public bool HasAircraft => _stationedAircraft.Count > 0;
        public string AircraftCountText => $"{_unassignedAircraftCount}/{_stationedAircraft.Count} free";

        public int RouteCount => _routeCount;
        public int MaxRouteSlots => _maxRouteSlots;
        public string RouteSlotText => $"{_routeCount}/{_maxRouteSlots}";
        public bool HasAvailableRouteSlots => _routeCount < _maxRouteSlots;
        public bool HasRoutes => _routeCount > 0;
        public bool IsRouteFull => _routeCount >= _maxRouteSlots;

        public decimal TotalMonthlyCostValue
        {
            get
            {
                decimal total = _fbo.MonthlyRent + _fbo.TerminalMonthlyCost;

                if (_settingsService != null)
                {
                    var settings = _settingsService.CurrentSettings;
                    if (_fbo.HasRefuelingService) total += settings.ServiceCostRefueling;
                    if (_fbo.HasHangarService) total += settings.ServiceCostHangar;
                    if (_fbo.HasCateringService) total += settings.ServiceCostCatering;
                    if (_fbo.HasGroundHandling) total += settings.ServiceCostGroundHandling;
                    if (_fbo.HasDeIcingService) total += settings.ServiceCostDeIcing;
                }

                return total;
            }
        }
    }
}
