using System;
using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IFlightRepository
    {
        List<FlightRecord> GetAllFlights();
        FlightRecord? GetFlightById(int id);
        void SaveFlight(FlightRecord flight);
        void DeleteFlight(int id);
        TimeSpan GetTotalFlightTime();
    }
}
