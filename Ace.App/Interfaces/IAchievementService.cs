using System;
using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IAchievementService
    {
        event Action<Achievement>? AchievementUnlocked;

        List<Achievement> GetAllAchievements();
        List<Achievement> GetAchievementsByCategory(AchievementCategory category);
        List<Achievement> GetUnlockedAchievements();
        Achievement? GetAchievement(string key);

        void InitializeAchievements();
        void RefreshAllAchievements();
        void UpdateProgress(string key, int progress);
        void IncrementProgress(string key, int amount = 1);
        void CheckAndUnlock(string key);

        void CheckFlightAchievements(int totalFlights, double totalDistanceNM);
        void CheckFleetAchievements(int aircraftCount, decimal fleetValue);
        void CheckFinanceAchievements(decimal totalAssets, decimal totalRevenue, decimal totalProfit);
        void CheckFBOAchievements(int fboCount);
        void CheckPilotAchievements(int pilotCount, double totalFlightHours);
        void CheckLandingAchievement(double landingRateFpm);

        int GetTotalUnlocked();
        int GetTotalAchievements();
        double GetCompletionPercent();

        Dictionary<string, decimal> GetDefaultRewards();
        void ResetRewardsToDefaults();
        void UpdateReward(string key, decimal reward);
    }
}
