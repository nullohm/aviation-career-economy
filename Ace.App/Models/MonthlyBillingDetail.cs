using System;
using System.ComponentModel.DataAnnotations;

namespace Ace.App.Models
{
    public class MonthlyBillingDetail
    {
        [Key]
        public int Id { get; set; }
        public DateTime BillingDate { get; set; }
        public string Category { get; set; } = string.Empty; // "FBO" or "Pilot"
        public string Name { get; set; } = string.Empty; // FBO ICAO or Pilot Name
        public string Description { get; set; } = string.Empty; // Details like services
        public decimal Amount { get; set; }

        // FBO-specific
        public decimal RentCost { get; set; }
        public decimal TerminalCost { get; set; }
        public decimal ServicesCost { get; set; }

        // Pilot-specific
        public decimal BaseSalary { get; set; }
        public decimal BonusPay { get; set; }
    }
}
