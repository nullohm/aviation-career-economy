using Ace.App.Data;
using Ace.App.Services;

using var dbContext = new AceDbContext();

var pilots = dbContext.Pilots.ToList();
var updated = 0;

foreach (var pilot in pilots)
{
    if (string.IsNullOrEmpty(pilot.ImagePath))
    {
        pilot.ImagePath = "../../../Pilots/Pictures/myPilot.bmp";
        updated++;
    }
}

if (updated > 0)
{
    dbContext.SaveChanges();
    Console.WriteLine($"Updated {updated} pilots with default image path");
}
else
{
    Console.WriteLine("No pilots needed updating");
}
