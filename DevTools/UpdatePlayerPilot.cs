using System.Linq;
using Ace.App.Data;
using Ace.App.Services;

public static class UpdatePlayerPilot
{
    public static void Run()
    {
        LoggingService.Instance.Initialize();

        using var dbContext = new AceDbContext();

        var pilots = dbContext.Pilots.ToList();

        if (pilots.Count == 0)
        {
            LoggingService.Instance.Info("UpdatePlayerPilot: No pilots found");
            return;
        }

        var firstPilot = pilots.First();

        if (!firstPilot.IsPlayer)
        {
            firstPilot.IsPlayer = true;
            firstPilot.IsEmployed = true;
            dbContext.SaveChanges();
            LoggingService.Instance.Info($"UpdatePlayerPilot: Marked {firstPilot.Name} as player pilot");
        }
        else
        {
            LoggingService.Instance.Info($"UpdatePlayerPilot: {firstPilot.Name} is already marked as player");
        }
    }
}
