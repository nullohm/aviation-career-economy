using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IAircraftRepository
    {
        List<Aircraft> GetAllAircraft();
        Aircraft? GetAircraftById(int id);
        Aircraft? GetAircraftByRegistration(string registration);
        void AddAircraft(Aircraft aircraft);
        void UpdateAircraft(Aircraft aircraft);
        void DeleteAircraft(int id);
        List<Aircraft> GetAircraftByStatus(AircraftStatus status);
        List<Aircraft> GetAircraftByFBOId(int fboId);
        List<Aircraft> GetAircraftWithPilotByFBOIds(params int[] fboIds);
        List<string> GetDistinctAircraftTypes();
        Aircraft? GetAircraftByType(string aircraftType);
        List<Aircraft> GetUnassignedStationedAircraft();
        Aircraft? GetAircraftByPilotId(int pilotId);
        decimal GetTotalFleetValue();
        int GetAircraftCount();
    }
}
