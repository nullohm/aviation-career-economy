using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class ScheduledRouteService : IScheduledRouteService
    {
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAirportDatabase _airportDatabase;

        public ScheduledRouteService(
            ILoggingService logger,
            ISettingsService settingsService,
            IServiceProvider serviceProvider,
            IAirportDatabase airportDatabase)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
        }

        public List<ScheduledRoute> GetAllRoutes()
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            return repo.GetAllRoutes();
        }

        public List<ScheduledRoute> GetActiveRoutes()
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            return repo.GetActiveRoutes();
        }

        public List<ScheduledRoute> GetRoutesByFBO(int fboId)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            return repo.GetRoutesByFBO(fboId);
        }

        public ScheduledRoute? GetRouteForAircraft(int aircraftId)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            return repo.GetRouteByAircraft(aircraftId);
        }

        public int GetRouteCountForFBO(int fboId)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            return repo.GetRouteCountForFBO(fboId);
        }

        public (bool Success, string Message) CreateRoute(int originFBOId, int destinationFBOId)
        {
            if (originFBOId == destinationFBOId)
                return (false, "Origin and destination must be different FBOs");

            using var scope = _serviceProvider.CreateScope();
            var fboRepo = scope.ServiceProvider.GetRequiredService<IFBORepository>();
            var routeRepo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();

            var originFBO = fboRepo.GetFBOById(originFBOId);
            var destFBO = fboRepo.GetFBOById(destinationFBOId);

            if (originFBO == null || destFBO == null)
                return (false, "Both FBOs must be owned");

            var routeAllowed = IsRouteAllowedByFBOType(originFBO, destFBO);
            if (!routeAllowed.Allowed)
                return (false, routeAllowed.Reason);

            if (!CanAddRouteToFBO(originFBOId))
                return (false, $"No outgoing route slots available at {originFBO.ICAO}");

            var pairLimit = _settingsService.CurrentSettings.RoutesPerFBOPairLimit;
            var existingPairRoutes = GetRouteCountBetweenFBOs(originFBOId, destinationFBOId);
            if (existingPairRoutes >= pairLimit)
                return (false, $"Maximum {pairLimit} routes allowed between {originFBO.ICAO} and {destFBO.ICAO}");

            var route = new ScheduledRoute
            {
                OriginFBOId = originFBOId,
                DestinationFBOId = destinationFBOId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            routeRepo.AddRoute(route);
            _logger.Info($"ScheduledRouteService: Created route {originFBO.ICAO} -> {destFBO.ICAO}");

            return (true, $"Route {originFBO.ICAO} -> {destFBO.ICAO} created");
        }

        public (bool Success, string Message) AssignAircraftToRoute(int routeId, int aircraftId)
        {
            using var scope = _serviceProvider.CreateScope();
            var routeRepo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            var aircraftRepo = scope.ServiceProvider.GetRequiredService<IAircraftRepository>();

            var route = routeRepo.GetRouteById(routeId);
            if (route == null)
                return (false, "Route not found");

            var aircraft = aircraftRepo.GetAircraftById(aircraftId);
            if (aircraft == null)
                return (false, "Aircraft not found");

            var assignmentRepo = scope.ServiceProvider.GetRequiredService<IAircraftPilotAssignmentRepository>();
            var assignedPilots = assignmentRepo.GetAssignmentsByAircraftId(aircraftId);
            if (assignedPilots.Count == 0)
                return (false, "Aircraft must have an assigned pilot");

            var existingRoute = routeRepo.GetRouteByAircraft(aircraftId);
            if (existingRoute != null && existingRoute.Id != routeId)
                return (false, "Aircraft is already assigned to another route");

            route.AssignedAircraftId = aircraftId;
            routeRepo.UpdateRoute(route);
            _logger.Info($"ScheduledRouteService: Assigned {aircraft.Registration} to route {routeId}");

            return (true, $"{aircraft.Registration} assigned to route");
        }

        public (bool Success, string Message) UnassignAircraftFromRoute(int routeId)
        {
            using var scope = _serviceProvider.CreateScope();
            var routeRepo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();

            var route = routeRepo.GetRouteById(routeId);
            if (route == null)
                return (false, "Route not found");

            route.AssignedAircraftId = null;
            routeRepo.UpdateRoute(route);
            _logger.Info($"ScheduledRouteService: Unassigned aircraft from route {routeId}");

            return (true, "Aircraft unassigned from route");
        }

        public (bool Success, string Message) DeleteRoute(int routeId)
        {
            using var scope = _serviceProvider.CreateScope();
            var routeRepo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();

            var route = routeRepo.GetRouteById(routeId);
            if (route == null)
                return (false, "Route not found");

            routeRepo.DeleteRoute(routeId);
            _logger.Info($"ScheduledRouteService: Deleted route {routeId}");

            return (true, "Route deleted");
        }

        public bool CanAddRouteToFBO(int fboId)
        {
            return GetRemainingSlots(fboId) > 0;
        }

        public int GetRemainingSlots(int fboId)
        {
            using var scope = _serviceProvider.CreateScope();
            var fboRepo = scope.ServiceProvider.GetRequiredService<IFBORepository>();
            var routeRepo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();

            var fbo = fboRepo.GetFBOById(fboId);
            if (fbo == null) return 0;

            int maxSlots = GetMaxSlots(fbo.Type);
            int usedSlots = routeRepo.GetOutgoingRouteCountForFBO(fboId);

            return Math.Max(0, maxSlots - usedSlots);
        }

        public int GetOutgoingRouteCountForFBO(int fboId)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            return repo.GetOutgoingRouteCountForFBO(fboId);
        }

        public int GetMaxSlots(FBOType fboType)
        {
            var settings = _settingsService.CurrentSettings;
            return fboType switch
            {
                FBOType.Local => settings.RouteSlotLimitLocal,
                FBOType.Regional => settings.RouteSlotLimitRegional,
                FBOType.International => settings.RouteSlotLimitInternational,
                _ => 2
            };
        }

        public bool HasScheduledRoute(int aircraftId)
        {
            return GetRouteForAircraft(aircraftId) != null;
        }

        public HashSet<int> GetAircraftIdsWithScheduledRoutes()
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            return repo.GetAircraftIdsWithActiveRoutes();
        }

        public int GetRouteCountBetweenFBOs(int fboId1, int fboId2)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IScheduledRouteRepository>();
            var routes = repo.GetActiveRoutes();
            return routes.Count(r =>
                (r.OriginFBOId == fboId1 && r.DestinationFBOId == fboId2) ||
                (r.OriginFBOId == fboId2 && r.DestinationFBOId == fboId1));
        }

        public int GetRemainingRoutesBetweenFBOs(int fboId1, int fboId2)
        {
            var limit = _settingsService.CurrentSettings.RoutesPerFBOPairLimit;
            var used = GetRouteCountBetweenFBOs(fboId1, fboId2);
            return Math.Max(0, limit - used);
        }

        private (bool Allowed, string Reason) IsRouteAllowedByFBOType(FBO originFBO, FBO destFBO)
        {
            if (originFBO.Type == FBOType.International)
                return (true, "");

            var originDetail = _airportDatabase.GetAirportDetail(originFBO.ICAO);
            var destDetail = _airportDatabase.GetAirportDetail(destFBO.ICAO);

            var originCountry = originDetail?.Country ?? "";
            var destCountry = destDetail?.Country ?? "";

            if (string.IsNullOrEmpty(originCountry) || string.IsNullOrEmpty(destCountry))
                return (true, "");

            if (originFBO.Type == FBOType.Local)
            {
                if (originCountry != destCountry)
                    return (false, $"Local FBO can only connect within the same country ({originCountry})");
                return (true, "");
            }

            if (originFBO.Type == FBOType.Regional)
            {
                if (originCountry == destCountry)
                    return (true, "");

                if (!AreCountriesInSameRegion(originCountry, destCountry))
                    return (false, $"Regional FBO can only connect within the same region ({originCountry} and {destCountry} are in different regions)");

                return (true, "");
            }

            return (true, "");
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
    }
}
