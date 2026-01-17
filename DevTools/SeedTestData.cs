using System;
using System.Linq;
using Ace.App.Data;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App
{
    public static class SeedTestData
    {
        public static void CreateTestFlights()
        {
            var testFlights = new[]
            {
                new FlightRecord
                {
                    Aircraft = "Cessna 172 Skyhawk",
                    Departure = "EDDS",
                    Arrival = "EDDM",
                    Date = DateTime.Now.AddDays(-3).AddHours(-2),
                    Duration = TimeSpan.FromMinutes(58),
                    LandingRate = 145.5,
                    DistanceNM = 112.3,
                    Status = "Landed",
                    Earnings = 48.33m
                },
                new FlightRecord
                {
                    Aircraft = "Cessna 172 Skyhawk",
                    Departure = "EDDM",
                    Arrival = "EDDF",
                    Date = DateTime.Now.AddDays(-2).AddHours(-1),
                    Duration = TimeSpan.FromMinutes(65),
                    LandingRate = 120.2,
                    DistanceNM = 156.8,
                    Status = "Landed",
                    Earnings = 54.17m
                },
                new FlightRecord
                {
                    Aircraft = "Cessna 172 Skyhawk",
                    Departure = "EDDF",
                    Arrival = "EDDH",
                    Date = DateTime.Now.AddDays(-1).AddHours(-3),
                    Duration = TimeSpan.FromMinutes(72),
                    LandingRate = 158.7,
                    DistanceNM = 189.4,
                    Status = "Landed",
                    Earnings = 60.00m
                }
            };

            try
            {
                using var db = new AceDbContext();
                db.Database.EnsureCreated();

                // Check if test data already exists
                if (db.Flights.Any())
                {
                    LoggingService.Instance.Info("Test data already exists, skipping seed");
                    return;
                }

                // Add flights and create corresponding transactions
                foreach (var flight in testFlights)
                {
                    db.Flights.Add(flight);
                    db.SaveChanges(); // Save to get the FlightId

                    // Create transaction directly in DB (don't use AddEarnings to avoid duplicates)
                    var transaction = new Transaction
                    {
                        Date = flight.Date,
                        Description = $"Flug {flight.Departure} → {flight.Arrival}",
                        Type = "Einnahme",
                        Amount = flight.Earnings,
                        FlightId = flight.Id
                    };
                    db.Transactions.Add(transaction);

                    // Update balance in FinanceService without creating another transaction
                    FinanceService.Instance.SetBalance(FinanceService.Instance.Balance + flight.Earnings);
                }

                db.SaveChanges();
                FinanceService.Instance.LoadTransactions();
                LoggingService.Instance.Info($"Test data seeded: {testFlights.Length} flights and transactions added");
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Error("Failed to seed test data", ex);
            }
        }
    }
}
