using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IMsfsAircraftRepository
    {
        List<MsfsAircraft> GetAllMsfsAircraft();
        MsfsAircraft? GetByTitle(string title);
        void AddOrUpdate(MsfsAircraft aircraft);
        void SaveChanges();
    }
}
