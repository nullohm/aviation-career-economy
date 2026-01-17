using System.Collections.Generic;
using Ace.App.Services;

namespace Ace.App.Interfaces
{
    public interface IAircraftCatalogService
    {
        IReadOnlyList<AircraftInfo> AvailableAircraft { get; }
        void LoadAvailableAircraft();
    }
}
