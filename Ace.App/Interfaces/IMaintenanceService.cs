using System.Collections.Generic;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.Interfaces
{
    public interface IMaintenanceService
    {
        MaintenanceCheckStatus? GetMostUrgentCheck(Aircraft aircraft);
        List<MaintenanceCheckStatus> GetSchedulableChecks(Aircraft aircraft);
        MaintenanceResult ScheduleMaintenance(int aircraftId, MaintenanceCheckType checkType);
        MaintenanceResult CompleteMaintenance(int aircraftId);
        void CompleteOverdueMaintenances();
        void GroundAircraftWithOverdueChecks();
    }
}
