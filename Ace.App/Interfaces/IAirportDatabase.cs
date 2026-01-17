using System.Collections.Generic;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.Interfaces
{
    public interface IAirportDatabase
    {
        bool IsAvailable { get; }
        void Initialize(string? msfsPackagesPath = null);
        Airport? GetAirport(string icao);
        AirportDetail? GetAirportDetail(string icao);
        List<Airport> GetAllAirports();
        double CalculateDistanceBetweenAirports(Airport airport1, Airport airport2);
        double CalculateBearing(Airport from, Airport to);
        string FindNearestAirport(double latitude, double longitude, double maxDistanceNM = 10.0);
    }
}
