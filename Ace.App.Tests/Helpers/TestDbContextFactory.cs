using System;
using Microsoft.EntityFrameworkCore;
using Ace.App.Data;

namespace Ace.App.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        /// <summary>
        /// Creates an in-memory database context for testing with a unique database name
        /// </summary>
        public static AceDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AceDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AceDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        /// <summary>
        /// Creates an in-memory database context with a specific database name
        /// </summary>
        public static AceDbContext CreateInMemoryContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AceDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            var context = new AceDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
