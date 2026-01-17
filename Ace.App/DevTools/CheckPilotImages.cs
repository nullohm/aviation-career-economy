using Ace.App.Data;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Services;

namespace Ace.App.DevTools
{
    public static class CheckPilotImages
    {
        public static void Run()
        {
            var logger = ServiceLocator.GetService<ILoggingService>();

            using var dbContext = new AceDbContext();

            var pilots = dbContext.Pilots.ToList();

            logger.Info($"Total pilots in database: {pilots.Count}");

            foreach (var pilot in pilots)
            {
                logger.Info($"Pilot: {pilot.Name}, ImagePath: '{pilot.ImagePath}'");
            }
        }
    }
}
