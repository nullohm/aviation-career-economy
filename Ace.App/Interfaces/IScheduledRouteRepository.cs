using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IScheduledRouteRepository
    {
        List<ScheduledRoute> GetAllRoutes();
        List<ScheduledRoute> GetActiveRoutes();
        List<ScheduledRoute> GetRoutesByFBO(int fboId);
        ScheduledRoute? GetRouteById(int id);
        ScheduledRoute? GetRouteByAircraft(int aircraftId);
        HashSet<int> GetAircraftIdsWithActiveRoutes();
        void AddRoute(ScheduledRoute route);
        void UpdateRoute(ScheduledRoute route);
        void DeleteRoute(int id);
        int GetRouteCountForFBO(int fboId);
        int GetOutgoingRouteCountForFBO(int fboId);
    }
}
