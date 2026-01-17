using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IAirspaceService
    {
        List<Airspace> GetAllAirspaces();
        void LoadAirspaces();
        void RefreshAirspaces();
        bool HasAirspaceData { get; }
    }
}
