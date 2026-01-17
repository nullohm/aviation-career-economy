using System;
using System.ComponentModel.DataAnnotations;

namespace Ace.App.Models
{
    public enum AchievementCategory
    {
        Flights,
        Distance,
        Fleet,
        Finance,
        FBOs,
        Pilots,
        Special
    }

    public enum AchievementTier
    {
        Bronze,
        Silver,
        Gold,
        Platinum
    }

    public class Achievement
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AchievementCategory Category { get; set; }
        public AchievementTier Tier { get; set; }
        public string Icon { get; set; } = "🏆";
        public bool IsUnlocked { get; set; }
        public DateTime? UnlockedDate { get; set; }
        public int Progress { get; set; }
        public int Target { get; set; }
        public decimal? Reward { get; set; }

        public double ProgressPercent => Target > 0 ? Math.Min(100, (Progress / (double)Target) * 100) : 0;
        public bool IsComplete => Progress >= Target;
        public string ProgressText => $"{Progress:N0} / {Target:N0}";
    }
}
