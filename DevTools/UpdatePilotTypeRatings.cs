using System;
using System.Linq;
using Ace.App.Data;
using Ace.App.Models;
using Ace.App.Services;

public static class UpdatePilotTypeRatings
{
    public static void Run()
    {
        LoggingService.Instance.Initialize();

        using var dbContext = new AceDbContext();

        var pilots = dbContext.Pilots.ToList();
        var added = 0;

        foreach (var pilot in pilots)
        {
            var hasCessna172Rating = dbContext.TypeRatings
                .Any(tr => tr.PilotId == pilot.Id && tr.AircraftType == "Cessna 172");

            if (!hasCessna172Rating)
            {
                var typeRating = new TypeRating
                {
                    PilotId = pilot.Id,
                    AircraftType = "Cessna 172",
                    EarnedDate = DateTime.Today.AddYears(-5),
                    IssuingAuthority = "EASA"
                };
                dbContext.TypeRatings.Add(typeRating);
                added++;
                LoggingService.Instance.Info($"Added Cessna 172 type rating for pilot {pilot.Name}");
            }
        }

        if (added > 0)
        {
            dbContext.SaveChanges();
            LoggingService.Instance.Info($"Database updated: {added} pilots received Cessna 172 type rating");
        }
        else
        {
            LoggingService.Instance.Info("All pilots already have Cessna 172 type rating");
        }
    }
}
