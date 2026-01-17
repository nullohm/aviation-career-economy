using System;
using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IPersistenceService
    {
        event Action? FlightRecordsChanged;
        event Action? PilotProfileChanged;

        List<FlightRecord> LoadFlightRecords();
        void SaveFlightRecord(FlightRecord record);
        Pilot GetActivePilot();
        void SavePilot(Pilot pilot);
        TimeSpan GetTotalFlightTime();
    }
}
