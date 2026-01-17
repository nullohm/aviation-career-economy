using System;
using System.Linq;
using Ace.App.Data;
using Ace.App.Services;

public class TestDbQuery
{
    public static void Run()
    {
        var log = LoggingService.Instance;
        log.Info("=== DATABASE TEST START ===");

        try
        {
            using var db = new AceDbContext();

            log.Info($"MsfsAircraft count: {db.MsfsAircraft.Count()}");
            log.Info($"AircraftCatalog count: {db.AircraftCatalog.Count()}");

            var msfsFirst = db.MsfsAircraft.FirstOrDefault();
            if (msfsFirst != null)
            {
                log.Info($"First MsfsAircraft: {msfsFirst.Title} - {msfsFirst.NewPrice:N0} €");
            }

            var catalogFirst = db.AircraftCatalog.FirstOrDefault();
            if (catalogFirst != null)
            {
                log.Info($"First AircraftCatalog: {catalogFirst.Title} - {catalogFirst.MarketPrice:N0} €");
            }

            var c172 = db.AircraftCatalog.FirstOrDefault(c => c.Title.Contains("172") || c.Title.Contains("C172"));
            if (c172 != null)
            {
                log.Info($"=== C172 Performance Data ===");
                log.Info($"Title: {c172.Title}");
                log.Info($"CruiseSpeedKts: {c172.CruiseSpeedKts}");
                log.Info($"MaxRangeNM: {c172.MaxRangeNM}");
                log.Info($"FuelCapacityGal: {c172.FuelCapacityGal}");
                log.Info($"FuelBurnGalPerHour: {c172.FuelBurnGalPerHour}");
                log.Info($"HourlyOperatingCost: {c172.HourlyOperatingCost}");
            }

            var availableCount = AircraftCatalogService.Instance.AvailableAircraft.Count;
            log.Info($"AvailableAircraft count: {availableCount}");
        }
        catch (Exception ex)
        {
            log.Error($"DB Test failed: {ex.Message}");
            log.Error($"Exception type: {ex.GetType().Name}");
            log.Error($"Stack trace: {ex.StackTrace}");
        }

        log.Info("=== DATABASE TEST END ===");
    }
}
