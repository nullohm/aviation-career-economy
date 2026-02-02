using System;

namespace Ace.App.Models
{
    public class AircraftPilotAssignment
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public int PilotId { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.Today;
    }
}
