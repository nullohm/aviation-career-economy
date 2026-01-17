using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IAircraftCatalogRepository
    {
        List<AircraftCatalogEntry> GetAllAircraft();
        AircraftCatalogEntry? GetAircraftByTitle(string title);
        void UpdateMarketPrice(int id, decimal newPrice);
        void UpdateAircraftCatalogEntry(AircraftCatalogEntry entry);
        void AddAircraftCatalogEntry(AircraftCatalogEntry entry);
        void UpdateCustomImagePath(string title, string? imagePath);
        void DeleteByTitle(string title);
    }
}
