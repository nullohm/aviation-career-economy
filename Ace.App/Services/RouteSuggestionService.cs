using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public enum MissionType
    {
        ShortHop,
        LocalFlight,
        RegionalFlight,
        CrossCountry,
        LongHaul
    }

    public class SuggestedRoute
    {
        public string DepartureIcao { get; set; } = string.Empty;
        public string ArrivalIcao { get; set; } = string.Empty;
        public string DepartureName { get; set; } = string.Empty;
        public string ArrivalName { get; set; } = string.Empty;
        public double DistanceNM { get; set; }
        public double Bearing { get; set; }
        public MissionType MissionType { get; set; }
        public int Passengers { get; set; }
        public double CargoKg { get; set; }
        public double CruiseSpeedKts { get; set; }
        public double EstimatedFlightHours => CruiseSpeedKts > 0 ? DistanceNM / CruiseSpeedKts : 0;
        public bool IsNetworkFlight { get; set; }
        public decimal NetworkBonusPercent { get; set; }

        private IPricingService? _pricingService;
        private Aircraft? _aircraft;

        public void SetPricingService(IPricingService pricingService, Aircraft aircraft)
        {
            _pricingService = pricingService;
            _aircraft = aircraft;
        }

        public decimal EstimatedRevenue
        {
            get
            {
                if (_pricingService == null || _aircraft == null) return 0m;
                var priceBreakdown = _pricingService.CalculateFlightPrice(_aircraft, DistanceNM, Passengers, EstimatedFlightHours);
                return priceBreakdown.TotalRevenue;
            }
        }

        public decimal NetworkBonusAmount => IsNetworkFlight ? EstimatedRevenue * (NetworkBonusPercent / 100m) : 0m;

        public string DisplayText => $"{DepartureIcao} → {ArrivalIcao}";
        public string MissionDescription => MissionType switch
        {
            MissionType.ShortHop => "Short Hop",
            MissionType.LocalFlight => "Local Flight",
            MissionType.RegionalFlight => "Regional Flight",
            MissionType.CrossCountry => "Cross Country",
            MissionType.LongHaul => "Long Haul",
            _ => "Unknown"
        };
        public string CompassDirection => Bearing switch
        {
            >= 348.75 or < 11.25 => "N",
            >= 11.25 and < 33.75 => "NNE",
            >= 33.75 and < 56.25 => "NE",
            >= 56.25 and < 78.75 => "ENE",
            >= 78.75 and < 101.25 => "E",
            >= 101.25 and < 123.75 => "ESE",
            >= 123.75 and < 146.25 => "SE",
            >= 146.25 and < 168.75 => "SSE",
            >= 168.75 and < 191.25 => "S",
            >= 191.25 and < 213.75 => "SSW",
            >= 213.75 and < 236.25 => "SW",
            >= 236.25 and < 258.75 => "WSW",
            >= 258.75 and < 281.25 => "W",
            >= 281.25 and < 303.75 => "WNW",
            >= 303.75 and < 326.25 => "NW",
            >= 326.25 and < 348.75 => "NNW",
            _ => "N"
        };
        public string LoadDescription => $"{Passengers} PAX + {CargoKg:F0} kg";
        public string NetworkBonusLabel => IsNetworkFlight ? $" (+{NetworkBonusPercent:F0}% Netzwerk)" : "";
        public string RouteDescription => $"{MissionDescription} • {DistanceNM:F0} NM • {CompassDirection} {Bearing:F0}° • {LoadDescription} • ${EstimatedRevenue:N0}{NetworkBonusLabel}\n{DepartureName} → {ArrivalName}";
        public string ShortDescription => $"{MissionDescription} • {DistanceNM:F0} NM • {LoadDescription}";
        public string RevenueDisplay => $"${EstimatedRevenue:N0}";
    }

    public class RouteSuggestionService : IRouteSuggestionService
    {
        private readonly ILoggingService _loggingService;
        private readonly IAirportDatabase _airportDatabase;
        private readonly ISettingsService _settingsService;
        private readonly IPricingService _pricingService;

        public RouteSuggestionService(
            ILoggingService loggingService,
            IAirportDatabase airportDatabase,
            ISettingsService settingsService,
            IPricingService pricingService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        }

        public List<SuggestedRoute> GetSuggestedRoutes(Aircraft aircraft, string? lastArrivalIcao = null)
        {
            var suggestions = new List<SuggestedRoute>();

            var departureIcao = lastArrivalIcao ?? aircraft.HomeBase;
            if (string.IsNullOrEmpty(departureIcao))
            {
                departureIcao = "EDDS";
            }

            _loggingService.Debug($"RouteSuggestionService: Generating routes for {aircraft.Registration} from {departureIcao}, Range: {aircraft.MaxRangeNM:F0} NM");

            var departureAirport = _airportDatabase.GetAirport(departureIcao);
            if (departureAirport == null)
            {
                _loggingService.Warn($"RouteSuggestionService: Departure airport {departureIcao} not found");
                return suggestions;
            }

            var availableDestinations = GenerateDynamicRoutes(departureAirport, aircraft.MaxRangeNM);
            var routes = new List<SuggestedRoute>();

            foreach (var (icao, name) in availableDestinations)
            {
                var arrivalAirport = _airportDatabase.GetAirport(icao);
                if (arrivalAirport == null) continue;

                var distance = _airportDatabase.CalculateDistanceBetweenAirports(departureAirport, arrivalAirport);

                if (distance > aircraft.MaxRangeNM * 0.95) continue;

                var bearing = _airportDatabase.CalculateBearing(departureAirport, arrivalAirport);
                var missionType = ClassifyMission(distance, aircraft.MaxRangeNM);

                bool isNetworkFlight = _pricingService.IsNetworkFlight(departureIcao, icao);
                decimal networkBonusPercent = isNetworkFlight ? _pricingService.GetNetworkBonusPercent() : 0m;

                var route = new SuggestedRoute
                {
                    DepartureIcao = departureIcao,
                    ArrivalIcao = icao,
                    DepartureName = departureAirport.Name,
                    ArrivalName = name,
                    DistanceNM = distance,
                    Bearing = bearing,
                    MissionType = missionType,
                    Passengers = aircraft.MaxPassengers,
                    CargoKg = aircraft.MaxCargoKg,
                    CruiseSpeedKts = aircraft.CruiseSpeedKts,
                    IsNetworkFlight = isNetworkFlight,
                    NetworkBonusPercent = networkBonusPercent
                };
                route.SetPricingService(_pricingService, aircraft);
                routes.Add(route);
            }

            var sortedRoutes = routes.OrderBy(s => s.DistanceNM).ToList();
            var selectedRoutes = SelectDistributedRoutes(sortedRoutes, 50);
            suggestions.AddRange(selectedRoutes);

            var sortedSuggestions = suggestions
                .OrderBy(s => s.DistanceNM)
                .ToList();

            _loggingService.Info($"RouteSuggestionService: Generated {sortedSuggestions.Count} route suggestions from {departureIcao} for {aircraft.Registration}");

            return sortedSuggestions;
        }

        private List<(string icao, string name)> GenerateDynamicRoutes(Airport departureAirport, double maxRangeNM)
        {
            var allAirports = _airportDatabase.GetAllAirports();

            _loggingService.Info($"RouteSuggestionService: GetAllAirports returned {allAirports.Count} total airports");

            var routesWithDistance = new List<(string icao, string name, double distance)>();

            foreach (var airport in allAirports)
            {
                if (airport.Icao == departureAirport.Icao) continue;

                var distance = _airportDatabase.CalculateDistanceBetweenAirports(departureAirport, airport);

                if (distance <= maxRangeNM * 0.95)
                {
                    routesWithDistance.Add((airport.Icao, airport.Name, distance));
                }
            }

            var sortedRoutes = routesWithDistance.OrderBy(r => r.distance).ToList();

            _loggingService.Info($"RouteSuggestionService: Found {sortedRoutes.Count} airports within {maxRangeNM * 0.95:F0} NM range");

            if (sortedRoutes.Count > 0)
            {
                _loggingService.Info($"RouteSuggestionService: Closest 5 airports: {string.Join(", ", sortedRoutes.Take(5).Select(r => $"{r.icao}@{r.distance:F0}NM"))}");
            }

            if (sortedRoutes.Count == 0)
            {
                _loggingService.Warn($"RouteSuggestionService: No airports found within range for {departureAirport.Icao}");
                return new List<(string icao, string name)>();
            }

            const int maxRoutes = 100;

            if (sortedRoutes.Count <= maxRoutes)
            {
                _loggingService.Info($"RouteSuggestionService: Using all {sortedRoutes.Count} available airports");
                return sortedRoutes.Select(r => (r.icao, r.name)).ToList();
            }

            var selectedRoutes = new List<(string icao, string name, double distance)>();
            var minDist = sortedRoutes.First().distance;
            var maxDist = sortedRoutes.Last().distance;
            var span = maxDist - minDist;

            _loggingService.Info($"RouteSuggestionService: Distance range: {minDist:F0} - {maxDist:F0} NM (span: {span:F0} NM)");

            for (int i = 0; i < maxRoutes && i < sortedRoutes.Count; i++)
            {
                var targetIndex = (int)((double)i / maxRoutes * sortedRoutes.Count);
                if (targetIndex >= sortedRoutes.Count) targetIndex = sortedRoutes.Count - 1;

                var route = sortedRoutes[targetIndex];
                if (!selectedRoutes.Any(s => s.icao == route.icao))
                {
                    selectedRoutes.Add(route);
                    _loggingService.Debug($"RouteSuggestionService: Selected {route.icao} at {route.distance:F0} NM (index {targetIndex})");
                }
            }

            var finalRoutes = selectedRoutes
                .OrderBy(r => r.distance)
                .Select(r => (r.icao, r.name))
                .ToList();

            _loggingService.Info($"RouteSuggestionService: Generated {finalRoutes.Count} routes, range: {minDist:F0} - {maxDist:F0} NM");

            return finalRoutes;
        }

        private MissionType ClassifyMission(double distanceNM, double maxRangeNM)
        {
            var rangePercentage = distanceNM / maxRangeNM;

            return rangePercentage switch
            {
                < 0.10 => MissionType.ShortHop,
                < 0.25 => MissionType.LocalFlight,
                < 0.50 => MissionType.RegionalFlight,
                < 0.75 => MissionType.CrossCountry,
                _ => MissionType.LongHaul
            };
        }

        private List<SuggestedRoute> SelectDistributedRoutes(List<SuggestedRoute> sortedRoutes, int maxCount)
        {
            if (sortedRoutes.Count <= maxCount)
            {
                return sortedRoutes;
            }

            var selectedRoutes = new List<SuggestedRoute>();
            for (int i = 0; i < maxCount && i < sortedRoutes.Count; i++)
            {
                var targetIndex = (int)((double)i / maxCount * sortedRoutes.Count);
                if (targetIndex >= sortedRoutes.Count) targetIndex = sortedRoutes.Count - 1;

                var route = sortedRoutes[targetIndex];
                if (!selectedRoutes.Any(s => s.ArrivalIcao == route.ArrivalIcao))
                {
                    selectedRoutes.Add(route);
                }
            }

            return selectedRoutes.OrderBy(r => r.DistanceNM).ToList();
        }

    }
}
