using System;

namespace Ace.App.Models
{
    public class ScheduledRoute
    {
        public int Id { get; set; }
        public int OriginFBOId { get; set; }
        public int DestinationFBOId { get; set; }
        public int? AssignedAircraftId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
