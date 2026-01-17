using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface ILicenseRepository
    {
        List<PilotLicense> GetLicensesByPilotId(int pilotId);
        void AddLicense(PilotLicense license);
        void DeleteLicense(int id);
    }
}
