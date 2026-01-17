using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ace.App.Utilities;

namespace Ace.App.Models
{
    public class Pilot
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "New Pilot";
        public DateTime Birthday { get; set; } = DateTime.Today.AddYears(-25);
        public string ImagePath { get; set; } = GetDefaultImagePath();
        public double TotalFlightHours { get; set; } = 0;
        public double TotalDistanceNM { get; set; } = 0;
        public bool IsEmployed { get; set; } = false;
        public bool IsPlayer { get; set; } = false;
        public decimal SalaryPerMonth { get; set; } = 0m;

        public virtual ICollection<PilotLicense> Licenses { get; set; } = new List<PilotLicense>();
        public virtual ICollection<TypeRating> TypeRatings { get; set; } = new List<TypeRating>();

        private static string GetDefaultImagePath()
        {
            return PathUtilities.GetDefaultPilotImagePath();
        }
    }
}

