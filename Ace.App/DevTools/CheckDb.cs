using System;
using System.Linq;
using Ace.App.Data;

namespace Ace.App.DevTools
{
    public class CheckDb
    {
        public static void Run()
        {
            using var db = new AceDbContext();
            var flights = db.Flights.ToList();
            Console.WriteLine($"Total flights in database: {flights.Count}");
            foreach (var f in flights)
            {
                Console.WriteLine($"  {f.Date:yyyy-MM-dd HH:mm} | {f.Departure} -> {f.Arrival} | {f.Aircraft} | {f.Duration}");
            }
        }
    }
}
