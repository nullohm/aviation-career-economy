using Ace.App.Services;

namespace Ace.App.Models
{
    public enum PilotRankType
    {
        Junior = 0,
        Senior = 1,
        Captain = 2,
        SeniorCaptain = 3,
        ChiefPilot = 4
    }

    public static class PilotRank
    {
        public static PilotRankType GetRank(double totalFlightHours, AppSettings settings)
        {
            if (totalFlightHours >= (double)settings.PilotRankChiefPilotHours)
                return PilotRankType.ChiefPilot;

            if (totalFlightHours >= (double)settings.PilotRankSeniorCaptainHours)
                return PilotRankType.SeniorCaptain;

            if (totalFlightHours >= (double)settings.PilotRankCaptainHours)
                return PilotRankType.Captain;

            if (totalFlightHours >= (double)settings.PilotRankSeniorHours)
                return PilotRankType.Senior;

            return PilotRankType.Junior;
        }

        public static decimal GetSalaryBonus(PilotRankType rank, AppSettings settings)
        {
            return rank switch
            {
                PilotRankType.ChiefPilot => settings.PilotRankChiefPilotBonus,
                PilotRankType.SeniorCaptain => settings.PilotRankSeniorCaptainBonus,
                PilotRankType.Captain => settings.PilotRankCaptainBonus,
                PilotRankType.Senior => settings.PilotRankSeniorBonus,
                PilotRankType.Junior => settings.PilotRankJuniorBonus,
                _ => 0m
            };
        }

        public static string GetRankName(PilotRankType rank)
        {
            return rank switch
            {
                PilotRankType.ChiefPilot => "Chief Pilot",
                PilotRankType.SeniorCaptain => "Senior Captain",
                PilotRankType.Captain => "Captain",
                PilotRankType.Senior => "Senior",
                PilotRankType.Junior => "Junior",
                _ => "Unknown"
            };
        }

        public static decimal CalculateAdjustedSalary(PilotRankType rank, AppSettings settings)
        {
            var bonus = GetSalaryBonus(rank, settings);
            return settings.PilotBaseSalary * (1 + bonus);
        }
    }
}
