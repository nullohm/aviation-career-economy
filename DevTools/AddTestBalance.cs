using System;
using Ace.App.Data;
using Ace.App.Models;

namespace Ace.App
{
    public class AddTestBalance
    {
        public static void Run()
        {
            using var db = new AceDbContext();
            db.Database.EnsureCreated();

            var transaction = new Transaction
            {
                Date = DateTime.Now,
                Description = "Testguthaben",
                Type = "Einnahme",
                Amount = 1000000m
            };

            db.Transactions.Add(transaction);
            db.SaveChanges();

            Console.WriteLine("1.000.000 € als Testguthaben hinzugefügt");
        }
    }
}
