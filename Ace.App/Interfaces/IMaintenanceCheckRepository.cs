using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IMaintenanceCheckRepository
    {
        void AddMaintenanceCheck(MaintenanceCheck check);
        MaintenanceCheck? GetPendingCheckForAircraft(int aircraftId);
        void CompleteCheck(int aircraftId);
        List<MaintenanceCheck> GetAllChecks();
    }
}
