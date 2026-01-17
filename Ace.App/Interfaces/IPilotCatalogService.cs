using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IPilotCatalogService
    {
        List<Pilot> GetAvailablePilots();
        List<Pilot> GetEmployedPilots();
        string GetPilotImagePath(string imageFileName);
    }
}
