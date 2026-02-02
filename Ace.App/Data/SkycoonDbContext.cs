using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Ace.App.Models;
using Ace.App.Utilities;

namespace Ace.App.Data
{
    public class AceDbContext : DbContext
    {
        public DbSet<FlightRecord> Flights { get; set; }
        public DbSet<Pilot> Pilots { get; set; }
        public DbSet<PilotLicense> Licenses { get; set; }
        public DbSet<TypeRating> TypeRatings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AppSettingsEntity> Settings { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<AircraftCatalogEntry> AircraftCatalog { get; set; }
        public DbSet<MsfsAircraft> MsfsAircraft { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<FBO> FBOs { get; set; }
        public DbSet<MaintenanceCheck> MaintenanceChecks { get; set; }
        public DbSet<DailyEarningsDetail> DailyEarningsDetails { get; set; }
        public DbSet<MonthlyBillingDetail> MonthlyBillingDetails { get; set; }
        public DbSet<ScheduledRoute> ScheduledRoutes { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<AircraftPilotAssignment> AircraftPilotAssignments { get; set; }


        public AceDbContext() : base()
        {
        }


        public AceDbContext(DbContextOptions<AceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aircraft>().HasKey(a => a.Id);
            modelBuilder.Entity<AircraftCatalogEntry>().HasKey(a => a.Id);
            modelBuilder.Entity<AircraftCatalogEntry>()
                .HasIndex(a => a.Title)
                .IsUnique();

            modelBuilder.Entity<MsfsAircraft>().HasKey(a => a.Id);
            modelBuilder.Entity<MsfsAircraft>()
                .HasIndex(a => a.Title)
                .IsUnique();

            modelBuilder.Entity<Loan>().HasKey(l => l.Id);
            modelBuilder.Entity<TypeRating>().HasKey(t => t.Id);
            modelBuilder.Entity<MaintenanceCheck>().HasKey(m => m.Id);

            modelBuilder.Entity<MaintenanceCheck>()
                .HasOne(m => m.Aircraft)
                .WithMany(a => a.MaintenanceHistory)
                .HasForeignKey(m => m.AircraftId);

            modelBuilder.Entity<ScheduledRoute>().HasKey(r => r.Id);
            modelBuilder.Entity<Achievement>().HasKey(a => a.Id);
            modelBuilder.Entity<Achievement>()
                .HasIndex(a => a.Key)
                .IsUnique();

            modelBuilder.Entity<AircraftPilotAssignment>().HasKey(a => a.Id);
            modelBuilder.Entity<AircraftPilotAssignment>()
                .HasIndex(a => a.PilotId)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var root = PathUtilities.FindSolutionRoot() ?? AppContext.BaseDirectory;
                var currentSaveDir = Path.Combine(root, "Savegames", "Current");

                if (!Directory.Exists(currentSaveDir))
                {
                    Directory.CreateDirectory(currentSaveDir);
                }

                var dbPath = Path.Combine(currentSaveDir, "ace.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }

            optionsBuilder.ConfigureWarnings(w => w.Ignore(
                Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
    }
}
