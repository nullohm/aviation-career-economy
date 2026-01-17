using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly ILoggingService _logger;
        private readonly IFinanceService _financeService;
        private readonly ISettingsService _settingsService;
        private readonly ISoundService _soundService;

        public event Action<Achievement>? AchievementUnlocked;

        private static readonly List<AchievementDefinition> AchievementDefinitions = new()
        {
            // Flights - Bronze
            new("flights_1", "First Flight", "Complete your first flight", AchievementCategory.Flights, AchievementTier.Bronze, "✈️", 1, 1000),
            new("flights_10", "Getting Started", "Complete 10 flights", AchievementCategory.Flights, AchievementTier.Bronze, "✈️", 10, 2500),
            new("flights_50", "Frequent Flyer", "Complete 50 flights", AchievementCategory.Flights, AchievementTier.Silver, "✈️", 50, 10000),
            new("flights_100", "Centurion", "Complete 100 flights", AchievementCategory.Flights, AchievementTier.Silver, "🎖️", 100, 25000),
            new("flights_500", "Sky Veteran", "Complete 500 flights", AchievementCategory.Flights, AchievementTier.Gold, "🎖️", 500, 100000),
            new("flights_1000", "Mile High Club", "Complete 1000 flights", AchievementCategory.Flights, AchievementTier.Platinum, "👑", 1000, 500000),

            // Distance
            new("distance_100", "Short Hop", "Fly 100 nautical miles", AchievementCategory.Distance, AchievementTier.Bronze, "🗺️", 100, 500),
            new("distance_1000", "Cross Country", "Fly 1,000 nautical miles", AchievementCategory.Distance, AchievementTier.Bronze, "🗺️", 1000, 2500),
            new("distance_10000", "Continental", "Fly 10,000 nautical miles", AchievementCategory.Distance, AchievementTier.Silver, "🌍", 10000, 15000),
            new("distance_50000", "Globe Trotter", "Fly 50,000 nautical miles", AchievementCategory.Distance, AchievementTier.Gold, "🌍", 50000, 75000),
            new("distance_100000", "World Traveler", "Fly 100,000 nautical miles", AchievementCategory.Distance, AchievementTier.Platinum, "🌏", 100000, 250000),

            // Fleet
            new("fleet_2", "Fleet Owner", "Own 2 aircraft", AchievementCategory.Fleet, AchievementTier.Bronze, "🛩️", 2, 5000),
            new("fleet_5", "Growing Fleet", "Own 5 aircraft", AchievementCategory.Fleet, AchievementTier.Silver, "🛩️", 5, 25000),
            new("fleet_10", "Airline Founder", "Own 10 aircraft", AchievementCategory.Fleet, AchievementTier.Gold, "🛫", 10, 100000),
            new("fleet_25", "Major Airline", "Own 25 aircraft", AchievementCategory.Fleet, AchievementTier.Platinum, "🛫", 25, 500000),
            new("fleet_value_1m", "Million Dollar Fleet", "Fleet value exceeds €1M", AchievementCategory.Fleet, AchievementTier.Silver, "💰", 1000000, 50000),
            new("fleet_value_10m", "10 Million Fleet", "Fleet value exceeds €10M", AchievementCategory.Fleet, AchievementTier.Gold, "💎", 10000000, 250000),
            new("fleet_value_100m", "Aviation Empire", "Fleet value exceeds €100M", AchievementCategory.Fleet, AchievementTier.Platinum, "👑", 100000000, 1000000),

            // Finance
            new("assets_100k", "Financially Stable", "Reach €100,000 total assets", AchievementCategory.Finance, AchievementTier.Bronze, "💵", 100000, 5000),
            new("assets_500k", "Half Millionaire", "Reach €500,000 total assets", AchievementCategory.Finance, AchievementTier.Silver, "💵", 500000, 25000),
            new("assets_1m", "Millionaire", "Reach €1,000,000 total assets", AchievementCategory.Finance, AchievementTier.Gold, "💰", 1000000, 50000),
            new("assets_10m", "Multi-Millionaire", "Reach €10,000,000 total assets", AchievementCategory.Finance, AchievementTier.Platinum, "💎", 10000000, 250000),
            new("revenue_1m", "Revenue Machine", "Earn €1M total revenue", AchievementCategory.Finance, AchievementTier.Silver, "📈", 1000000, 25000),
            new("revenue_10m", "Revenue Giant", "Earn €10M total revenue", AchievementCategory.Finance, AchievementTier.Gold, "📈", 10000000, 100000),
            new("profit_500k", "Profitable", "Earn €500,000 total profit", AchievementCategory.Finance, AchievementTier.Silver, "📊", 500000, 25000),
            new("profit_5m", "Highly Profitable", "Earn €5M total profit", AchievementCategory.Finance, AchievementTier.Gold, "📊", 5000000, 150000),

            // FBOs
            new("fbo_1", "Base of Operations", "Rent your first FBO", AchievementCategory.FBOs, AchievementTier.Bronze, "🏢", 1, 2500),
            new("fbo_5", "Regional Network", "Operate 5 FBOs", AchievementCategory.FBOs, AchievementTier.Silver, "🏢", 5, 25000),
            new("fbo_10", "National Presence", "Operate 10 FBOs", AchievementCategory.FBOs, AchievementTier.Gold, "🏛️", 10, 75000),
            new("fbo_25", "Global Network", "Operate 25 FBOs", AchievementCategory.FBOs, AchievementTier.Platinum, "🌐", 25, 250000),

            // Pilots
            new("pilots_5", "Small Team", "Employ 5 pilots", AchievementCategory.Pilots, AchievementTier.Bronze, "👨‍✈️", 5, 5000),
            new("pilots_10", "Growing Crew", "Employ 10 pilots", AchievementCategory.Pilots, AchievementTier.Silver, "👨‍✈️", 10, 15000),
            new("pilots_25", "Pilot Academy", "Employ 25 pilots", AchievementCategory.Pilots, AchievementTier.Gold, "🎓", 25, 50000),
            new("pilots_50", "Major Employer", "Employ 50 pilots", AchievementCategory.Pilots, AchievementTier.Platinum, "🎓", 50, 150000),
            new("flight_hours_100", "100 Flight Hours", "Log 100 total flight hours", AchievementCategory.Pilots, AchievementTier.Bronze, "⏱️", 100, 5000),
            new("flight_hours_1000", "1000 Flight Hours", "Log 1000 total flight hours", AchievementCategory.Pilots, AchievementTier.Silver, "⏱️", 1000, 50000),
            new("flight_hours_10000", "10000 Flight Hours", "Log 10000 total flight hours", AchievementCategory.Pilots, AchievementTier.Gold, "⏱️", 10000, 250000),

            // Special
            new("landing_butter", "Butter Landing", "Land with less than -50 fpm", AchievementCategory.Special, AchievementTier.Gold, "🧈", 1, 10000),
            new("landing_perfect", "Perfect Landing", "Land with less than -100 fpm", AchievementCategory.Special, AchievementTier.Silver, "✨", 1, 5000),
            new("landing_good", "Good Landing", "Land with less than -200 fpm", AchievementCategory.Special, AchievementTier.Bronze, "👍", 1, 1000),
        };

        public AchievementService(ILoggingService logger, IFinanceService financeService, ISettingsService settingsService, ISoundService soundService)
        {
            _logger = logger;
            _financeService = financeService;
            _settingsService = settingsService;
            _soundService = soundService;
        }

        public void InitializeAchievements()
        {
            using var db = new AceDbContext();
            var existingKeys = db.Achievements.Select(a => a.Key).ToHashSet();

            foreach (var def in AchievementDefinitions)
            {
                if (!existingKeys.Contains(def.Key))
                {
                    db.Achievements.Add(new Achievement
                    {
                        Key = def.Key,
                        Title = def.Title,
                        Description = def.Description,
                        Category = def.Category,
                        Tier = def.Tier,
                        Icon = def.Icon,
                        Target = def.Target,
                        Reward = def.Reward,
                        Progress = 0,
                        IsUnlocked = false
                    });
                }
            }

            db.SaveChanges();
            _logger.Debug($"Initialized {AchievementDefinitions.Count} achievements");
        }

        public List<Achievement> GetAllAchievements()
        {
            using var db = new AceDbContext();
            return db.Achievements.OrderBy(a => a.Category).ThenBy(a => a.Tier).ToList();
        }

        public List<Achievement> GetAchievementsByCategory(AchievementCategory category)
        {
            using var db = new AceDbContext();
            return db.Achievements.Where(a => a.Category == category).OrderBy(a => a.Tier).ToList();
        }

        public List<Achievement> GetUnlockedAchievements()
        {
            using var db = new AceDbContext();
            return db.Achievements.Where(a => a.IsUnlocked).OrderByDescending(a => a.UnlockedDate).ToList();
        }

        public Achievement? GetAchievement(string key)
        {
            using var db = new AceDbContext();
            return db.Achievements.FirstOrDefault(a => a.Key == key);
        }

        public void UpdateProgress(string key, int progress)
        {
            using var db = new AceDbContext();
            var achievement = db.Achievements.FirstOrDefault(a => a.Key == key);
            if (achievement == null || achievement.IsUnlocked) return;

            achievement.Progress = progress;

            if (achievement.Progress >= achievement.Target)
            {
                UnlockAchievement(db, achievement);
            }

            db.SaveChanges();
        }

        public void IncrementProgress(string key, int amount = 1)
        {
            using var db = new AceDbContext();
            var achievement = db.Achievements.FirstOrDefault(a => a.Key == key);
            if (achievement == null || achievement.IsUnlocked) return;

            achievement.Progress += amount;

            if (achievement.Progress >= achievement.Target)
            {
                UnlockAchievement(db, achievement);
            }

            db.SaveChanges();
        }

        public void CheckAndUnlock(string key)
        {
            using var db = new AceDbContext();
            var achievement = db.Achievements.FirstOrDefault(a => a.Key == key);
            if (achievement == null || achievement.IsUnlocked) return;

            achievement.Progress = achievement.Target;
            UnlockAchievement(db, achievement);
            db.SaveChanges();
        }

        private void UnlockAchievement(AceDbContext db, Achievement achievement)
        {
            achievement.IsUnlocked = true;
            achievement.UnlockedDate = DateTime.Now;

            _logger.Info($"Achievement unlocked: {achievement.Title}");

            if (achievement.Reward.HasValue && achievement.Reward.Value > 0)
            {
                var multiplier = _settingsService.CurrentSettings.AchievementRewardMultiplier;
                var adjustedReward = achievement.Reward.Value * multiplier;
                _financeService.AddEarnings(adjustedReward, $"Achievement Bonus: {achievement.Title}");
                _logger.Debug($"Achievement reward: €{achievement.Reward.Value:N0} x {multiplier} = €{adjustedReward:N0}");
            }

            _soundService.PlayAchievementUnlocked();
            AchievementUnlocked?.Invoke(achievement);
        }

        public void CheckFlightAchievements(int totalFlights, double totalDistanceNM)
        {
            using var db = new AceDbContext();
            var achievements = db.Achievements.Where(a => !a.IsUnlocked).ToList();
            bool changed = false;

            foreach (var achievement in achievements)
            {
                if (achievement.Category == AchievementCategory.Flights && achievement.Key.StartsWith("flights_"))
                {
                    achievement.Progress = totalFlights;
                    if (achievement.Progress >= achievement.Target)
                    {
                        UnlockAchievement(db, achievement);
                        changed = true;
                    }
                }
                else if (achievement.Category == AchievementCategory.Distance && achievement.Key.StartsWith("distance_"))
                {
                    achievement.Progress = (int)totalDistanceNM;
                    if (achievement.Progress >= achievement.Target)
                    {
                        UnlockAchievement(db, achievement);
                        changed = true;
                    }
                }
            }

            if (changed) db.SaveChanges();
        }

        public void CheckFleetAchievements(int aircraftCount, decimal fleetValue)
        {
            using var db = new AceDbContext();
            var achievements = db.Achievements.Where(a => !a.IsUnlocked && a.Category == AchievementCategory.Fleet).ToList();
            bool changed = false;

            foreach (var achievement in achievements)
            {
                if (achievement.Key.StartsWith("fleet_value_"))
                {
                    achievement.Progress = (int)fleetValue;
                }
                else if (achievement.Key.StartsWith("fleet_"))
                {
                    achievement.Progress = aircraftCount;
                }

                if (achievement.Progress >= achievement.Target)
                {
                    UnlockAchievement(db, achievement);
                    changed = true;
                }
            }

            if (changed) db.SaveChanges();
        }

        public void CheckFinanceAchievements(decimal totalAssets, decimal totalRevenue, decimal totalProfit)
        {
            using var db = new AceDbContext();
            var achievements = db.Achievements.Where(a => !a.IsUnlocked && a.Category == AchievementCategory.Finance).ToList();
            bool changed = false;

            foreach (var achievement in achievements)
            {
                if (achievement.Key.StartsWith("assets_"))
                {
                    achievement.Progress = DecimalToSafeInt(totalAssets);
                }
                else if (achievement.Key.StartsWith("revenue_"))
                {
                    achievement.Progress = DecimalToSafeInt(totalRevenue);
                }
                else if (achievement.Key.StartsWith("profit_"))
                {
                    achievement.Progress = DecimalToSafeInt(totalProfit);
                }

                if (achievement.Progress >= achievement.Target)
                {
                    UnlockAchievement(db, achievement);
                    changed = true;
                }
            }

            if (changed) db.SaveChanges();
        }

        public void CheckFBOAchievements(int fboCount)
        {
            using var db = new AceDbContext();
            var achievements = db.Achievements.Where(a => !a.IsUnlocked && a.Category == AchievementCategory.FBOs).ToList();
            bool changed = false;

            foreach (var achievement in achievements)
            {
                achievement.Progress = fboCount;
                if (achievement.Progress >= achievement.Target)
                {
                    UnlockAchievement(db, achievement);
                    changed = true;
                }
            }

            if (changed) db.SaveChanges();
        }

        public void CheckPilotAchievements(int pilotCount, double totalFlightHours)
        {
            using var db = new AceDbContext();
            var achievements = db.Achievements.Where(a => !a.IsUnlocked && a.Category == AchievementCategory.Pilots).ToList();
            bool changed = false;

            foreach (var achievement in achievements)
            {
                if (achievement.Key.StartsWith("pilots_"))
                {
                    achievement.Progress = pilotCount;
                }
                else if (achievement.Key.StartsWith("flight_hours_"))
                {
                    achievement.Progress = (int)totalFlightHours;
                }

                if (achievement.Progress >= achievement.Target)
                {
                    UnlockAchievement(db, achievement);
                    changed = true;
                }
            }

            if (changed) db.SaveChanges();
        }

        public void CheckLandingAchievement(double landingRateFpm)
        {
            var absRate = Math.Abs(landingRateFpm);

            if (absRate < 50)
            {
                CheckAndUnlock("landing_butter");
                CheckAndUnlock("landing_perfect");
                CheckAndUnlock("landing_good");
            }
            else if (absRate < 100)
            {
                CheckAndUnlock("landing_perfect");
                CheckAndUnlock("landing_good");
            }
            else if (absRate < 200)
            {
                CheckAndUnlock("landing_good");
            }
        }

        public int GetTotalUnlocked()
        {
            using var db = new AceDbContext();
            return db.Achievements.Count(a => a.IsUnlocked);
        }

        public int GetTotalAchievements()
        {
            using var db = new AceDbContext();
            return db.Achievements.Count();
        }

        public double GetCompletionPercent()
        {
            using var db = new AceDbContext();
            var total = db.Achievements.Count();
            if (total == 0) return 0;
            var unlocked = db.Achievements.Count(a => a.IsUnlocked);
            return (unlocked / (double)total) * 100;
        }

        private static int DecimalToSafeInt(decimal value)
        {
            if (value > int.MaxValue) return int.MaxValue;
            if (value < int.MinValue) return int.MinValue;
            return (int)value;
        }

        private record AchievementDefinition(
            string Key,
            string Title,
            string Description,
            AchievementCategory Category,
            AchievementTier Tier,
            string Icon,
            int Target,
            decimal Reward);
    }
}
