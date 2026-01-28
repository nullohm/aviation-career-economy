using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IPilotRepository
    {
        List<Pilot> GetAvailablePilots();
        List<Pilot> GetEmployedPilots();
        List<Pilot> GetEmployedNonPlayerPilots();
        Pilot? GetPlayerPilot();
        Pilot? GetPilotById(int id);
        void HirePilot(int pilotId);
        void FirePilot(int pilotId);
        void UpdatePilot(Pilot pilot);
        void CreatePilot(Pilot pilot);
        void DeletePilot(int pilotId);
    }
}
