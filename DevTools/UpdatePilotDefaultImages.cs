using System;
using System.IO;
using Ace.App.Data;
using Ace.App.Services;
using Ace.App.Utilities;

public static class UpdatePilotDefaultImages
{
    public static void Run()
    {
        LoggingService.Instance.Initialize();

        var defaultImagePath = PathUtilities.GetDefaultPilotImagePath();

        LoggingService.Instance.Info($"Default pilot image path: {defaultImagePath}");

        using var dbContext = new AceDbContext();

        var pilots = dbContext.Pilots.ToList();
        var updated = 0;

        foreach (var pilot in pilots)
        {
            if (string.IsNullOrEmpty(pilot.ImagePath) || pilot.ImagePath.Contains("../../../"))
            {
                pilot.ImagePath = defaultImagePath;
                updated++;
                LoggingService.Instance.Info($"Updated pilot {pilot.Name} with default image");
            }
        }

        if (updated > 0)
        {
            dbContext.SaveChanges();
            LoggingService.Instance.Info($"Database updated: {updated} pilots now have default image");
        }
        else
        {
            LoggingService.Instance.Info("No pilots needed updating");
        }
    }
}
