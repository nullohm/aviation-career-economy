using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IScheduledRouteService
    {
        List<ScheduledRoute> GetAllRoutes();
        List<ScheduledRoute> GetActiveRoutes();
        List<ScheduledRoute> GetRoutesByFBO(int fboId);
        ScheduledRoute? GetRouteForAircraft(int aircraftId);
        int GetRouteCountForFBO(int fboId);
        int GetOutgoingRouteCountForFBO(int fboId);

        (bool Success, string Message) CreateRoute(int originFBOId, int destinationFBOId);
        (bool Success, string Message) AssignAircraftToRoute(int routeId, int aircraftId);
        (bool Success, string Message) UnassignAircraftFromRoute(int routeId);
        (bool Success, string Message) DeleteRoute(int routeId);

        bool CanAddRouteToFBO(int fboId);
        int GetRemainingSlots(int fboId);
        int GetMaxSlots(FBOType fboType);

        int GetRouteCountBetweenFBOs(int fboId1, int fboId2);
        int GetRemainingRoutesBetweenFBOs(int fboId1, int fboId2);

        bool HasScheduledRoute(int aircraftId);
        HashSet<int> GetAircraftIdsWithScheduledRoutes();
    }
}
