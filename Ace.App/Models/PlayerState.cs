using System.Collections.Generic;

namespace Ace.App.Models
{
    public class PlayerState
    {
        public decimal Money { get; set; }
        public List<System.Guid> OwnedAircraftIds { get; set; } = new List<System.Guid>();
        public List<string> DiscoveredAirportIcaos { get; set; } = new List<string>();
    }
}
