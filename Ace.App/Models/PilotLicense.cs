using System;
using System.ComponentModel.DataAnnotations;

namespace Ace.App.Models
{
    public class PilotLicense
    {
        [Key]
        public int Id { get; set; }
        
        public int PilotId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; } = DateTime.Today;
        public string IssuingAuthority { get; set; } = string.Empty;
        
        public Pilot? Pilot { get; set; }
    }
}
