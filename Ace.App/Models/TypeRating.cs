using System;
using System.ComponentModel.DataAnnotations;

namespace Ace.App.Models
{
    public class TypeRating
    {
        [Key]
        public int Id { get; set; }

        public int PilotId { get; set; }
        public string AircraftType { get; set; } = string.Empty;
        public DateTime EarnedDate { get; set; } = DateTime.Today;
        public string IssuingAuthority { get; set; } = string.Empty;

        public Pilot? Pilot { get; set; }
    }
}
