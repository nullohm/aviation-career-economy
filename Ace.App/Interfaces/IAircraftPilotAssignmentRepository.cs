using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IAircraftPilotAssignmentRepository
    {
        List<AircraftPilotAssignment> GetAssignmentsByAircraftId(int aircraftId);
        List<AircraftPilotAssignment> GetAllAssignments();
        AircraftPilotAssignment? GetAssignmentByPilotId(int pilotId);
        void AssignPilot(int aircraftId, int pilotId);
        void UnassignPilot(int pilotId);
        void UnassignAllFromAircraft(int aircraftId);
        int GetAssignedPilotCount(int aircraftId);
        List<int> GetAssignedPilotIds(int aircraftId);
        bool IsPilotAssigned(int pilotId);
    }
}
