
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ace.App.Data;

#nullable disable

namespace Ace.App.Data.Migrations
{
    [DbContext(typeof(AceDbContext))]
    [Migration("20251229113659_AddAircraftPricing")]
    partial class AddAircraftPricing
    {

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Ace.App.Models.Aircraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AssignedFBOId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AssignedPilotId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryString")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("CruiseSpeedKts")
                        .HasColumnType("REAL");

                    b.Property<int?>("CurrentMaintenanceType")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("CurrentValue")
                        .HasColumnType("TEXT");

                    b.Property<double>("FuelBurnGalPerHour")
                        .HasColumnType("REAL");

                    b.Property<double>("FuelCapacityGal")
                        .HasColumnType("REAL");

                    b.Property<string>("HomeBase")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("HourlyOperatingCost")
                        .HasColumnType("TEXT");

                    b.Property<double>("HoursSinceACheck")
                        .HasColumnType("REAL");

                    b.Property<double>("HoursSinceLastMaintenance")
                        .HasColumnType("REAL");

                    b.Property<double>("HoursSinceTBO")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("LastACheck")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastAnnualInspection")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastBCheck")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastCCheck")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastDCheck")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastMaintenanceDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("MaintenanceCompletionDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxPassengers")
                        .HasColumnType("INTEGER");

                    b.Property<double>("MaxRangeNM")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("TEXT");

                    b.Property<string>("Registration")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalFlightHours")
                        .HasColumnType("REAL");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Variant")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Aircraft");
                });

            modelBuilder.Entity("Ace.App.Models.AircraftCatalogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CrewCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("CruiseSpeedKts")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("FirstSeen")
                        .HasColumnType("TEXT");

                    b.Property<double>("FuelBurnGalPerHour")
                        .HasColumnType("REAL");

                    b.Property<double>("FuelCapacityGal")
                        .HasColumnType("REAL");

                    b.Property<decimal>("HourlyOperatingCost")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("TEXT");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("MarketPrice")
                        .HasColumnType("TEXT");

                    b.Property<double>("MaxRangeNM")
                        .HasColumnType("REAL");

                    b.Property<int>("PassengerCapacity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("AircraftCatalog");
                });

            modelBuilder.Entity("Ace.App.Models.AircraftPricing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AircraftNamePattern")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AircraftPricings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AircraftNamePattern = "747",
                            BasePrice = 418000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 197, DateTimeKind.Local).AddTicks(5891),
                            Priority = 1
                        },
                        new
                        {
                            Id = 2,
                            AircraftNamePattern = "a380",
                            BasePrice = 445000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7379),
                            Priority = 1
                        },
                        new
                        {
                            Id = 3,
                            AircraftNamePattern = "787",
                            BasePrice = 248000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7395),
                            Priority = 2
                        },
                        new
                        {
                            Id = 4,
                            AircraftNamePattern = "dreamliner",
                            BasePrice = 248000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7398),
                            Priority = 2
                        },
                        new
                        {
                            Id = 5,
                            AircraftNamePattern = "a350",
                            BasePrice = 317000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7400),
                            Priority = 2
                        },
                        new
                        {
                            Id = 6,
                            AircraftNamePattern = "777",
                            BasePrice = 375000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7418),
                            Priority = 3
                        },
                        new
                        {
                            Id = 7,
                            AircraftNamePattern = "a330",
                            BasePrice = 264000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7420),
                            Priority = 3
                        },
                        new
                        {
                            Id = 8,
                            AircraftNamePattern = "a340",
                            BasePrice = 238000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7421),
                            Priority = 4
                        },
                        new
                        {
                            Id = 9,
                            AircraftNamePattern = "737 max",
                            BasePrice = 122000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7423),
                            Priority = 20
                        },
                        new
                        {
                            Id = 10,
                            AircraftNamePattern = "737",
                            BasePrice = 100000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7425),
                            Priority = 21
                        },
                        new
                        {
                            Id = 11,
                            AircraftNamePattern = "b737",
                            BasePrice = 100000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7426),
                            Priority = 21
                        },
                        new
                        {
                            Id = 12,
                            AircraftNamePattern = "a320neo",
                            BasePrice = 111000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7428),
                            Priority = 20
                        },
                        new
                        {
                            Id = 13,
                            AircraftNamePattern = "a320",
                            BasePrice = 101000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7429),
                            Priority = 21
                        },
                        new
                        {
                            Id = 14,
                            AircraftNamePattern = "a321",
                            BasePrice = 130000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7431),
                            Priority = 21
                        },
                        new
                        {
                            Id = 15,
                            AircraftNamePattern = "a319",
                            BasePrice = 92000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7432),
                            Priority = 21
                        },
                        new
                        {
                            Id = 16,
                            AircraftNamePattern = "a220",
                            BasePrice = 81000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7434),
                            Priority = 22
                        },
                        new
                        {
                            Id = 17,
                            AircraftNamePattern = "c series",
                            BasePrice = 81000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7436),
                            Priority = 22
                        },
                        new
                        {
                            Id = 18,
                            AircraftNamePattern = "a310",
                            BasePrice = 65000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7437),
                            Priority = 23
                        },
                        new
                        {
                            Id = 19,
                            AircraftNamePattern = "a300",
                            BasePrice = 65000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7439),
                            Priority = 23
                        },
                        new
                        {
                            Id = 20,
                            AircraftNamePattern = "757",
                            BasePrice = 75000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7440),
                            Priority = 24
                        },
                        new
                        {
                            Id = 21,
                            AircraftNamePattern = "767",
                            BasePrice = 85000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7442),
                            Priority = 24
                        },
                        new
                        {
                            Id = 22,
                            AircraftNamePattern = "717",
                            BasePrice = 42000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7443),
                            Priority = 25
                        },
                        new
                        {
                            Id = 23,
                            AircraftNamePattern = "md-",
                            BasePrice = 42000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7445),
                            Priority = 25
                        },
                        new
                        {
                            Id = 24,
                            AircraftNamePattern = "crj",
                            BasePrice = 33000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7446),
                            Priority = 41
                        },
                        new
                        {
                            Id = 25,
                            AircraftNamePattern = "e170",
                            BasePrice = 47000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7448),
                            Priority = 42
                        },
                        new
                        {
                            Id = 26,
                            AircraftNamePattern = "e175",
                            BasePrice = 53000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7458),
                            Priority = 42
                        },
                        new
                        {
                            Id = 27,
                            AircraftNamePattern = "e190",
                            BasePrice = 50000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7460),
                            Priority = 42
                        },
                        new
                        {
                            Id = 28,
                            AircraftNamePattern = "e195",
                            BasePrice = 54000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7461),
                            Priority = 42
                        },
                        new
                        {
                            Id = 29,
                            AircraftNamePattern = "embraer e",
                            BasePrice = 50000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7463),
                            Priority = 43
                        },
                        new
                        {
                            Id = 30,
                            AircraftNamePattern = "e-jet",
                            BasePrice = 50000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7464),
                            Priority = 43
                        },
                        new
                        {
                            Id = 31,
                            AircraftNamePattern = "atr 72",
                            BasePrice = 27000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7466),
                            Priority = 61
                        },
                        new
                        {
                            Id = 32,
                            AircraftNamePattern = "atr72",
                            BasePrice = 27000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7468),
                            Priority = 61
                        },
                        new
                        {
                            Id = 33,
                            AircraftNamePattern = "atr 42",
                            BasePrice = 18000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7469),
                            Priority = 62
                        },
                        new
                        {
                            Id = 34,
                            AircraftNamePattern = "atr42",
                            BasePrice = 18000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7471),
                            Priority = 62
                        },
                        new
                        {
                            Id = 35,
                            AircraftNamePattern = "atr",
                            BasePrice = 22000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7472),
                            Priority = 63
                        },
                        new
                        {
                            Id = 36,
                            AircraftNamePattern = "dash 8",
                            BasePrice = 32000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7474),
                            Priority = 64
                        },
                        new
                        {
                            Id = 37,
                            AircraftNamePattern = "q400",
                            BasePrice = 32000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7475),
                            Priority = 64
                        },
                        new
                        {
                            Id = 38,
                            AircraftNamePattern = "dhc-8",
                            BasePrice = 32000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7477),
                            Priority = 64
                        },
                        new
                        {
                            Id = 39,
                            AircraftNamePattern = "saab 340",
                            BasePrice = 12000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7478),
                            Priority = 65
                        },
                        new
                        {
                            Id = 40,
                            AircraftNamePattern = "saab340",
                            BasePrice = 12000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7480),
                            Priority = 65
                        },
                        new
                        {
                            Id = 41,
                            AircraftNamePattern = "cessna 172",
                            BasePrice = 400000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7481),
                            Priority = 81
                        },
                        new
                        {
                            Id = 42,
                            AircraftNamePattern = "c172",
                            BasePrice = 400000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7483),
                            Priority = 81
                        },
                        new
                        {
                            Id = 43,
                            AircraftNamePattern = "skyhawk",
                            BasePrice = 400000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7484),
                            Priority = 81
                        },
                        new
                        {
                            Id = 44,
                            AircraftNamePattern = "cessna 152",
                            BasePrice = 180000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7486),
                            Priority = 82
                        },
                        new
                        {
                            Id = 45,
                            AircraftNamePattern = "c152",
                            BasePrice = 180000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7487),
                            Priority = 82
                        },
                        new
                        {
                            Id = 46,
                            AircraftNamePattern = "cessna 208",
                            BasePrice = 2700000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7497),
                            Priority = 83
                        },
                        new
                        {
                            Id = 47,
                            AircraftNamePattern = "c208",
                            BasePrice = 2700000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7498),
                            Priority = 83
                        },
                        new
                        {
                            Id = 48,
                            AircraftNamePattern = "caravan",
                            BasePrice = 2700000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7500),
                            Priority = 83
                        },
                        new
                        {
                            Id = 49,
                            AircraftNamePattern = "grand caravan",
                            BasePrice = 2900000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7501),
                            Priority = 82
                        },
                        new
                        {
                            Id = 50,
                            AircraftNamePattern = "citation longitude",
                            BasePrice = 28000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7503),
                            Priority = 84
                        },
                        new
                        {
                            Id = 51,
                            AircraftNamePattern = "citation cj4",
                            BasePrice = 11500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7504),
                            Priority = 85
                        },
                        new
                        {
                            Id = 52,
                            AircraftNamePattern = "cj4",
                            BasePrice = 11500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7506),
                            Priority = 85
                        },
                        new
                        {
                            Id = 53,
                            AircraftNamePattern = "citation cj3",
                            BasePrice = 9500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7507),
                            Priority = 85
                        },
                        new
                        {
                            Id = 54,
                            AircraftNamePattern = "cj3",
                            BasePrice = 9500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7509),
                            Priority = 85
                        },
                        new
                        {
                            Id = 55,
                            AircraftNamePattern = "citation",
                            BasePrice = 10000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7510),
                            Priority = 86
                        },
                        new
                        {
                            Id = 56,
                            AircraftNamePattern = "c400",
                            BasePrice = 750000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7512),
                            Priority = 87
                        },
                        new
                        {
                            Id = 57,
                            AircraftNamePattern = "corvalis",
                            BasePrice = 750000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7513),
                            Priority = 87
                        },
                        new
                        {
                            Id = 58,
                            AircraftNamePattern = "columbia",
                            BasePrice = 750000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7515),
                            Priority = 87
                        },
                        new
                        {
                            Id = 59,
                            AircraftNamePattern = "cessna",
                            BasePrice = 450000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7516),
                            Priority = 88
                        },
                        new
                        {
                            Id = 60,
                            AircraftNamePattern = "textron",
                            BasePrice = 500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7518),
                            Priority = 88
                        },
                        new
                        {
                            Id = 61,
                            AircraftNamePattern = "piper cub",
                            BasePrice = 180000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7519),
                            Priority = 101
                        },
                        new
                        {
                            Id = 62,
                            AircraftNamePattern = "pa-18",
                            BasePrice = 200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7521),
                            Priority = 101
                        },
                        new
                        {
                            Id = 63,
                            AircraftNamePattern = "j-3",
                            BasePrice = 120000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7522),
                            Priority = 101
                        },
                        new
                        {
                            Id = 64,
                            AircraftNamePattern = "super cub",
                            BasePrice = 220000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7524),
                            Priority = 101
                        },
                        new
                        {
                            Id = 65,
                            AircraftNamePattern = "cherokee",
                            BasePrice = 320000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7532),
                            Priority = 102
                        },
                        new
                        {
                            Id = 66,
                            AircraftNamePattern = "pa-28",
                            BasePrice = 320000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7534),
                            Priority = 102
                        },
                        new
                        {
                            Id = 67,
                            AircraftNamePattern = "archer",
                            BasePrice = 380000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7535),
                            Priority = 102
                        },
                        new
                        {
                            Id = 68,
                            AircraftNamePattern = "warrior",
                            BasePrice = 320000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7537),
                            Priority = 102
                        },
                        new
                        {
                            Id = 69,
                            AircraftNamePattern = "seminole",
                            BasePrice = 680000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7538),
                            Priority = 103
                        },
                        new
                        {
                            Id = 70,
                            AircraftNamePattern = "pa-44",
                            BasePrice = 680000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7540),
                            Priority = 103
                        },
                        new
                        {
                            Id = 71,
                            AircraftNamePattern = "m350",
                            BasePrice = 1200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7541),
                            Priority = 100
                        },
                        new
                        {
                            Id = 72,
                            AircraftNamePattern = "m500",
                            BasePrice = 3000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7543),
                            Priority = 100
                        },
                        new
                        {
                            Id = 73,
                            AircraftNamePattern = "m600",
                            BasePrice = 3500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7544),
                            Priority = 100
                        },
                        new
                        {
                            Id = 74,
                            AircraftNamePattern = "piper",
                            BasePrice = 380000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7546),
                            Priority = 104
                        },
                        new
                        {
                            Id = 75,
                            AircraftNamePattern = "da62",
                            BasePrice = 1400000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7547),
                            Priority = 121
                        },
                        new
                        {
                            Id = 76,
                            AircraftNamePattern = "da42",
                            BasePrice = 950000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7549),
                            Priority = 122
                        },
                        new
                        {
                            Id = 77,
                            AircraftNamePattern = "twin star",
                            BasePrice = 950000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7551),
                            Priority = 122
                        },
                        new
                        {
                            Id = 78,
                            AircraftNamePattern = "da40",
                            BasePrice = 550000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7552),
                            Priority = 123
                        },
                        new
                        {
                            Id = 79,
                            AircraftNamePattern = "diamond star",
                            BasePrice = 550000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7554),
                            Priority = 123
                        },
                        new
                        {
                            Id = 80,
                            AircraftNamePattern = "da20",
                            BasePrice = 220000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7555),
                            Priority = 124
                        },
                        new
                        {
                            Id = 81,
                            AircraftNamePattern = "katana",
                            BasePrice = 220000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7557),
                            Priority = 124
                        },
                        new
                        {
                            Id = 82,
                            AircraftNamePattern = "diamond",
                            BasePrice = 500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7558),
                            Priority = 125
                        },
                        new
                        {
                            Id = 83,
                            AircraftNamePattern = "sr22t",
                            BasePrice = 950000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7584),
                            Priority = 140
                        },
                        new
                        {
                            Id = 84,
                            AircraftNamePattern = "sr22",
                            BasePrice = 850000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7586),
                            Priority = 141
                        },
                        new
                        {
                            Id = 85,
                            AircraftNamePattern = "sr20",
                            BasePrice = 650000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7596),
                            Priority = 142
                        },
                        new
                        {
                            Id = 86,
                            AircraftNamePattern = "vision jet",
                            BasePrice = 3200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7597),
                            Priority = 139
                        },
                        new
                        {
                            Id = 87,
                            AircraftNamePattern = "sf50",
                            BasePrice = 3200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7599),
                            Priority = 139
                        },
                        new
                        {
                            Id = 88,
                            AircraftNamePattern = "cirrus",
                            BasePrice = 800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7601),
                            Priority = 143
                        },
                        new
                        {
                            Id = 89,
                            AircraftNamePattern = "king air 350i",
                            BasePrice = 7400000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7602),
                            Priority = 150
                        },
                        new
                        {
                            Id = 90,
                            AircraftNamePattern = "king air 350",
                            BasePrice = 6400000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7604),
                            Priority = 151
                        },
                        new
                        {
                            Id = 91,
                            AircraftNamePattern = "king air 260",
                            BasePrice = 4800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7605),
                            Priority = 151
                        },
                        new
                        {
                            Id = 92,
                            AircraftNamePattern = "king air",
                            BasePrice = 5500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7607),
                            Priority = 152
                        },
                        new
                        {
                            Id = 93,
                            AircraftNamePattern = "kingair",
                            BasePrice = 5500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7608),
                            Priority = 152
                        },
                        new
                        {
                            Id = 94,
                            AircraftNamePattern = "baron g58",
                            BasePrice = 1500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7610),
                            Priority = 153
                        },
                        new
                        {
                            Id = 95,
                            AircraftNamePattern = "baron",
                            BasePrice = 1300000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7611),
                            Priority = 153
                        },
                        new
                        {
                            Id = 96,
                            AircraftNamePattern = "g58",
                            BasePrice = 1500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7613),
                            Priority = 153
                        },
                        new
                        {
                            Id = 97,
                            AircraftNamePattern = "bonanza g36",
                            BasePrice = 1100000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7614),
                            Priority = 154
                        },
                        new
                        {
                            Id = 98,
                            AircraftNamePattern = "bonanza",
                            BasePrice = 950000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7616),
                            Priority = 154
                        },
                        new
                        {
                            Id = 99,
                            AircraftNamePattern = "g36",
                            BasePrice = 1100000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7617),
                            Priority = 154
                        },
                        new
                        {
                            Id = 100,
                            AircraftNamePattern = "beechcraft",
                            BasePrice = 700000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7619),
                            Priority = 155
                        },
                        new
                        {
                            Id = 101,
                            AircraftNamePattern = "global 7500",
                            BasePrice = 75000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7620),
                            Priority = 171
                        },
                        new
                        {
                            Id = 102,
                            AircraftNamePattern = "global7500",
                            BasePrice = 75000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7622),
                            Priority = 171
                        },
                        new
                        {
                            Id = 103,
                            AircraftNamePattern = "global 6500",
                            BasePrice = 58000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7623),
                            Priority = 171
                        },
                        new
                        {
                            Id = 104,
                            AircraftNamePattern = "bombardier global",
                            BasePrice = 65000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7625),
                            Priority = 172
                        },
                        new
                        {
                            Id = 105,
                            AircraftNamePattern = "gulfstream g700",
                            BasePrice = 78000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7633),
                            Priority = 170
                        },
                        new
                        {
                            Id = 106,
                            AircraftNamePattern = "gulfstream g650",
                            BasePrice = 65000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7635),
                            Priority = 171
                        },
                        new
                        {
                            Id = 107,
                            AircraftNamePattern = "gulfstream g280",
                            BasePrice = 24500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7636),
                            Priority = 173
                        },
                        new
                        {
                            Id = 108,
                            AircraftNamePattern = "gulfstream",
                            BasePrice = 45000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7638),
                            Priority = 173
                        },
                        new
                        {
                            Id = 109,
                            AircraftNamePattern = "challenger 650",
                            BasePrice = 32000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7639),
                            Priority = 174
                        },
                        new
                        {
                            Id = 110,
                            AircraftNamePattern = "challenger 350",
                            BasePrice = 27000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7641),
                            Priority = 174
                        },
                        new
                        {
                            Id = 111,
                            AircraftNamePattern = "challenger",
                            BasePrice = 28000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7643),
                            Priority = 174
                        },
                        new
                        {
                            Id = 112,
                            AircraftNamePattern = "learjet 75",
                            BasePrice = 9500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7644),
                            Priority = 175
                        },
                        new
                        {
                            Id = 113,
                            AircraftNamePattern = "learjet",
                            BasePrice = 9500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7646),
                            Priority = 175
                        },
                        new
                        {
                            Id = 114,
                            AircraftNamePattern = "lear",
                            BasePrice = 9500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7647),
                            Priority = 175
                        },
                        new
                        {
                            Id = 115,
                            AircraftNamePattern = "phenom 300",
                            BasePrice = 10500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7649),
                            Priority = 176
                        },
                        new
                        {
                            Id = 116,
                            AircraftNamePattern = "phenom",
                            BasePrice = 9000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7650),
                            Priority = 177
                        },
                        new
                        {
                            Id = 117,
                            AircraftNamePattern = "praetor 600",
                            BasePrice = 21000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7652),
                            Priority = 175
                        },
                        new
                        {
                            Id = 118,
                            AircraftNamePattern = "praetor 500",
                            BasePrice = 17500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7653),
                            Priority = 175
                        },
                        new
                        {
                            Id = 119,
                            AircraftNamePattern = "praetor",
                            BasePrice = 18000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7655),
                            Priority = 176
                        },
                        new
                        {
                            Id = 120,
                            AircraftNamePattern = "falcon 8x",
                            BasePrice = 58000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7656),
                            Priority = 172
                        },
                        new
                        {
                            Id = 121,
                            AircraftNamePattern = "falcon 900",
                            BasePrice = 45000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7658),
                            Priority = 173
                        },
                        new
                        {
                            Id = 122,
                            AircraftNamePattern = "falcon 2000",
                            BasePrice = 35000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7659),
                            Priority = 174
                        },
                        new
                        {
                            Id = 123,
                            AircraftNamePattern = "falcon",
                            BasePrice = 40000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7661),
                            Priority = 175
                        },
                        new
                        {
                            Id = 124,
                            AircraftNamePattern = "hawker",
                            BasePrice = 8000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7669),
                            Priority = 178
                        },
                        new
                        {
                            Id = 125,
                            AircraftNamePattern = "hondajet",
                            BasePrice = 5800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7671),
                            Priority = 177
                        },
                        new
                        {
                            Id = 126,
                            AircraftNamePattern = "pc-12 ngx",
                            BasePrice = 6200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7672),
                            Priority = 190
                        },
                        new
                        {
                            Id = 127,
                            AircraftNamePattern = "pc-12",
                            BasePrice = 6000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7674),
                            Priority = 191
                        },
                        new
                        {
                            Id = 128,
                            AircraftNamePattern = "pilatus pc12",
                            BasePrice = 6000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7675),
                            Priority = 191
                        },
                        new
                        {
                            Id = 129,
                            AircraftNamePattern = "pc-6",
                            BasePrice = 2500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7677),
                            Priority = 192
                        },
                        new
                        {
                            Id = 130,
                            AircraftNamePattern = "porter",
                            BasePrice = 2500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7678),
                            Priority = 192
                        },
                        new
                        {
                            Id = 131,
                            AircraftNamePattern = "pc-24",
                            BasePrice = 11000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7680),
                            Priority = 189
                        },
                        new
                        {
                            Id = 132,
                            AircraftNamePattern = "pilatus",
                            BasePrice = 5000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7682),
                            Priority = 193
                        },
                        new
                        {
                            Id = 133,
                            AircraftNamePattern = "tbm 960",
                            BasePrice = 4800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7683),
                            Priority = 194
                        },
                        new
                        {
                            Id = 134,
                            AircraftNamePattern = "tbm960",
                            BasePrice = 4800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7685),
                            Priority = 194
                        },
                        new
                        {
                            Id = 135,
                            AircraftNamePattern = "tbm 940",
                            BasePrice = 4200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7686),
                            Priority = 195
                        },
                        new
                        {
                            Id = 136,
                            AircraftNamePattern = "tbm940",
                            BasePrice = 4200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7688),
                            Priority = 195
                        },
                        new
                        {
                            Id = 137,
                            AircraftNamePattern = "tbm 930",
                            BasePrice = 3900000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7689),
                            Priority = 196
                        },
                        new
                        {
                            Id = 138,
                            AircraftNamePattern = "tbm930",
                            BasePrice = 3900000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7691),
                            Priority = 196
                        },
                        new
                        {
                            Id = 139,
                            AircraftNamePattern = "tbm",
                            BasePrice = 4200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7692),
                            Priority = 197
                        },
                        new
                        {
                            Id = 140,
                            AircraftNamePattern = "kodiak 100",
                            BasePrice = 2900000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7694),
                            Priority = 197
                        },
                        new
                        {
                            Id = 141,
                            AircraftNamePattern = "kodiak",
                            BasePrice = 2700000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7695),
                            Priority = 198
                        },
                        new
                        {
                            Id = 142,
                            AircraftNamePattern = "bell 412",
                            BasePrice = 8500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7697),
                            Priority = 211
                        },
                        new
                        {
                            Id = 143,
                            AircraftNamePattern = "bell412",
                            BasePrice = 8500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7698),
                            Priority = 211
                        },
                        new
                        {
                            Id = 144,
                            AircraftNamePattern = "bell 429",
                            BasePrice = 7500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7707),
                            Priority = 211
                        },
                        new
                        {
                            Id = 145,
                            AircraftNamePattern = "bell 407",
                            BasePrice = 4200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7708),
                            Priority = 212
                        },
                        new
                        {
                            Id = 146,
                            AircraftNamePattern = "bell407",
                            BasePrice = 4200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7710),
                            Priority = 212
                        },
                        new
                        {
                            Id = 147,
                            AircraftNamePattern = "bell 206",
                            BasePrice = 1500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7711),
                            Priority = 213
                        },
                        new
                        {
                            Id = 148,
                            AircraftNamePattern = "jetranger",
                            BasePrice = 1500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7713),
                            Priority = 213
                        },
                        new
                        {
                            Id = 149,
                            AircraftNamePattern = "bell 505",
                            BasePrice = 1600000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7714),
                            Priority = 213
                        },
                        new
                        {
                            Id = 150,
                            AircraftNamePattern = "bell",
                            BasePrice = 3000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7716),
                            Priority = 215
                        },
                        new
                        {
                            Id = 151,
                            AircraftNamePattern = "h225",
                            BasePrice = 32000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7717),
                            Priority = 231
                        },
                        new
                        {
                            Id = 152,
                            AircraftNamePattern = "super puma",
                            BasePrice = 32000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7719),
                            Priority = 231
                        },
                        new
                        {
                            Id = 153,
                            AircraftNamePattern = "h175",
                            BasePrice = 18000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7720),
                            Priority = 231
                        },
                        new
                        {
                            Id = 154,
                            AircraftNamePattern = "h160",
                            BasePrice = 14500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7722),
                            Priority = 232
                        },
                        new
                        {
                            Id = 155,
                            AircraftNamePattern = "h145",
                            BasePrice = 12000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7723),
                            Priority = 233
                        },
                        new
                        {
                            Id = 156,
                            AircraftNamePattern = "ec145",
                            BasePrice = 12000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7725),
                            Priority = 233
                        },
                        new
                        {
                            Id = 157,
                            AircraftNamePattern = "h135",
                            BasePrice = 5500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7726),
                            Priority = 234
                        },
                        new
                        {
                            Id = 158,
                            AircraftNamePattern = "ec135",
                            BasePrice = 5500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7728),
                            Priority = 234
                        },
                        new
                        {
                            Id = 159,
                            AircraftNamePattern = "h130",
                            BasePrice = 3200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7729),
                            Priority = 234
                        },
                        new
                        {
                            Id = 160,
                            AircraftNamePattern = "h125",
                            BasePrice = 3800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7731),
                            Priority = 235
                        },
                        new
                        {
                            Id = 161,
                            AircraftNamePattern = "as350",
                            BasePrice = 3800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7732),
                            Priority = 235
                        },
                        new
                        {
                            Id = 162,
                            AircraftNamePattern = "ecureuil",
                            BasePrice = 3800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7734),
                            Priority = 235
                        },
                        new
                        {
                            Id = 163,
                            AircraftNamePattern = "robinson r66",
                            BasePrice = 1050000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7736),
                            Priority = 251
                        },
                        new
                        {
                            Id = 164,
                            AircraftNamePattern = "r66",
                            BasePrice = 1050000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7744),
                            Priority = 251
                        },
                        new
                        {
                            Id = 165,
                            AircraftNamePattern = "robinson r44",
                            BasePrice = 550000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7745),
                            Priority = 252
                        },
                        new
                        {
                            Id = 166,
                            AircraftNamePattern = "r44",
                            BasePrice = 550000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7747),
                            Priority = 252
                        },
                        new
                        {
                            Id = 167,
                            AircraftNamePattern = "robinson r22",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7748),
                            Priority = 253
                        },
                        new
                        {
                            Id = 168,
                            AircraftNamePattern = "r22",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7750),
                            Priority = 253
                        },
                        new
                        {
                            Id = 169,
                            AircraftNamePattern = "robinson",
                            BasePrice = 600000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7751),
                            Priority = 254
                        },
                        new
                        {
                            Id = 170,
                            AircraftNamePattern = "cabri g2",
                            BasePrice = 420000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7753),
                            Priority = 261
                        },
                        new
                        {
                            Id = 171,
                            AircraftNamePattern = "cabri",
                            BasePrice = 420000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7754),
                            Priority = 261
                        },
                        new
                        {
                            Id = 172,
                            AircraftNamePattern = "schweizer 300",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7756),
                            Priority = 262
                        },
                        new
                        {
                            Id = 173,
                            AircraftNamePattern = "schweizer",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7757),
                            Priority = 262
                        },
                        new
                        {
                            Id = 174,
                            AircraftNamePattern = "s300",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7759),
                            Priority = 262
                        },
                        new
                        {
                            Id = 175,
                            AircraftNamePattern = "300c",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7760),
                            Priority = 262
                        },
                        new
                        {
                            Id = 176,
                            AircraftNamePattern = "md 500",
                            BasePrice = 2800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7762),
                            Priority = 263
                        },
                        new
                        {
                            Id = 177,
                            AircraftNamePattern = "md500",
                            BasePrice = 2800000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7763),
                            Priority = 263
                        },
                        new
                        {
                            Id = 178,
                            AircraftNamePattern = "md 530",
                            BasePrice = 3200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7765),
                            Priority = 263
                        },
                        new
                        {
                            Id = 179,
                            AircraftNamePattern = "md530",
                            BasePrice = 3200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7767),
                            Priority = 263
                        },
                        new
                        {
                            Id = 180,
                            AircraftNamePattern = "leonardo aw",
                            BasePrice = 15000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7768),
                            Priority = 264
                        },
                        new
                        {
                            Id = 181,
                            AircraftNamePattern = "aw139",
                            BasePrice = 15000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7770),
                            Priority = 264
                        },
                        new
                        {
                            Id = 182,
                            AircraftNamePattern = "aw109",
                            BasePrice = 6500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7771),
                            Priority = 265
                        },
                        new
                        {
                            Id = 183,
                            AircraftNamePattern = "sikorsky s-76",
                            BasePrice = 13000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7779),
                            Priority = 264
                        },
                        new
                        {
                            Id = 184,
                            AircraftNamePattern = "sikorsky",
                            BasePrice = 10000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7781),
                            Priority = 266
                        },
                        new
                        {
                            Id = 185,
                            AircraftNamePattern = "c-17",
                            BasePrice = 220000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7782),
                            Priority = 271
                        },
                        new
                        {
                            Id = 186,
                            AircraftNamePattern = "globemaster",
                            BasePrice = 220000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7784),
                            Priority = 271
                        },
                        new
                        {
                            Id = 187,
                            AircraftNamePattern = "a400",
                            BasePrice = 120000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7785),
                            Priority = 272
                        },
                        new
                        {
                            Id = 188,
                            AircraftNamePattern = "atlas",
                            BasePrice = 120000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7787),
                            Priority = 272
                        },
                        new
                        {
                            Id = 189,
                            AircraftNamePattern = "c-130",
                            BasePrice = 35000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7788),
                            Priority = 273
                        },
                        new
                        {
                            Id = 190,
                            AircraftNamePattern = "hercules",
                            BasePrice = 35000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7790),
                            Priority = 273
                        },
                        new
                        {
                            Id = 191,
                            AircraftNamePattern = "c-5",
                            BasePrice = 180000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7791),
                            Priority = 271
                        },
                        new
                        {
                            Id = 192,
                            AircraftNamePattern = "galaxy",
                            BasePrice = 180000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7793),
                            Priority = 271
                        },
                        new
                        {
                            Id = 193,
                            AircraftNamePattern = "dc-6",
                            BasePrice = 1200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7794),
                            Priority = 291
                        },
                        new
                        {
                            Id = 194,
                            AircraftNamePattern = "dc-3",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7796),
                            Priority = 292
                        },
                        new
                        {
                            Id = 195,
                            AircraftNamePattern = "dakota",
                            BasePrice = 350000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7798),
                            Priority = 292
                        },
                        new
                        {
                            Id = 196,
                            AircraftNamePattern = "twin otter",
                            BasePrice = 6500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7799),
                            Priority = 292
                        },
                        new
                        {
                            Id = 197,
                            AircraftNamePattern = "dhc-6",
                            BasePrice = 6500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7801),
                            Priority = 292
                        },
                        new
                        {
                            Id = 198,
                            AircraftNamePattern = "otter",
                            BasePrice = 1500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7802),
                            Priority = 293
                        },
                        new
                        {
                            Id = 199,
                            AircraftNamePattern = "dhc-3",
                            BasePrice = 1500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7804),
                            Priority = 293
                        },
                        new
                        {
                            Id = 200,
                            AircraftNamePattern = "beaver",
                            BasePrice = 750000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7805),
                            Priority = 294
                        },
                        new
                        {
                            Id = 201,
                            AircraftNamePattern = "dhc-2",
                            BasePrice = 750000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7807),
                            Priority = 294
                        },
                        new
                        {
                            Id = 202,
                            AircraftNamePattern = "an-2",
                            BasePrice = 280000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7808),
                            Priority = 295
                        },
                        new
                        {
                            Id = 203,
                            AircraftNamePattern = "antonov",
                            BasePrice = 280000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7810),
                            Priority = 295
                        },
                        new
                        {
                            Id = 204,
                            AircraftNamePattern = "jet",
                            BasePrice = 12000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7811),
                            Priority = 901
                        },
                        new
                        {
                            Id = 205,
                            AircraftNamePattern = "helicopter",
                            BasePrice = 2500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7813),
                            Priority = 902
                        },
                        new
                        {
                            Id = 206,
                            AircraftNamePattern = "heli",
                            BasePrice = 2500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7814),
                            Priority = 902
                        },
                        new
                        {
                            Id = 207,
                            AircraftNamePattern = "turboprop",
                            BasePrice = 3500000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7816),
                            Priority = 903
                        },
                        new
                        {
                            Id = 208,
                            AircraftNamePattern = "airliner",
                            BasePrice = 85000000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7817),
                            Priority = 900
                        },
                        new
                        {
                            Id = 209,
                            AircraftNamePattern = "twin",
                            BasePrice = 1200000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7819),
                            Priority = 904
                        },
                        new
                        {
                            Id = 210,
                            AircraftNamePattern = "single",
                            BasePrice = 450000m,
                            LastUpdated = new DateTime(2025, 12, 29, 12, 36, 59, 199, DateTimeKind.Local).AddTicks(7820),
                            Priority = 905
                        });
                });

            modelBuilder.Entity("Ace.App.Models.AppSettingsEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AutoStartTracking")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("FBORentInternational")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FBORentLocal")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FBORentRegional")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsMaximized")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSimConnectEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastArrivalIcao")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastDailyEarningsDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastDepartureIcao")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("RatePerPaxPerNM")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ServiceCostPerService")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TerminalCostLarge")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TerminalCostMedium")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TerminalCostSmall")
                        .HasColumnType("TEXT");

                    b.Property<double>("WindowHeight")
                        .HasColumnType("REAL");

                    b.Property<double>("WindowLeft")
                        .HasColumnType("REAL");

                    b.Property<double>("WindowTop")
                        .HasColumnType("REAL");

                    b.Property<double>("WindowWidth")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Ace.App.Models.FBO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AirportName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasCateringService")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasDeIcingService")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasGroundHandling")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasHangarService")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasRefuelingService")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ICAO")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("MonthlyRent")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RentedSince")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TerminalMonthlyCost")
                        .HasColumnType("TEXT");

                    b.Property<int>("TerminalSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("FBOs");
                });

            modelBuilder.Entity("Ace.App.Models.FlightRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Aircraft")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Arrival")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Departure")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("DistanceNM")
                        .HasColumnType("REAL");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Earnings")
                        .HasColumnType("TEXT");

                    b.Property<double>("LandingRate")
                        .HasColumnType("REAL");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("Ace.App.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRepaid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("RepaidDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TakenDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalRepayment")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("Ace.App.Models.MaintenanceCheck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AircraftId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CheckType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<int>("DurationDays")
                        .HasColumnType("INTEGER");

                    b.Property<double>("FlightHoursAtCheck")
                        .HasColumnType("REAL");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.ToTable("MaintenanceChecks");
                });

            modelBuilder.Entity("Ace.App.Models.MsfsAircraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CrewCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("CruiseSpeedKts")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("FirstDetected")
                        .HasColumnType("TEXT");

                    b.Property<double>("FuelBurnGalPerHour")
                        .HasColumnType("REAL");

                    b.Property<double>("FuelCapacityGal")
                        .HasColumnType("REAL");

                    b.Property<decimal>("HourlyOperatingCost")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastDetected")
                        .HasColumnType("TEXT");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("MaxRangeNM")
                        .HasColumnType("REAL");

                    b.Property<decimal>("NewPrice")
                        .HasColumnType("TEXT");

                    b.Property<int>("PassengerCapacity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("MsfsAircraft");
                });

            modelBuilder.Entity("Ace.App.Models.Pilot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEmployed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPlayer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SalaryPerMonth")
                        .HasColumnType("TEXT");

                    b.Property<double>("TotalFlightHours")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Pilots");
                });

            modelBuilder.Entity("Ace.App.Models.PilotLicense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("IssuingAuthority")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PilotId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PilotId");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("Ace.App.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Ace.App.Models.TypeRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AircraftType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EarnedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("IssuingAuthority")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PilotId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PilotId");

                    b.ToTable("TypeRatings");
                });

            modelBuilder.Entity("Ace.App.Models.MaintenanceCheck", b =>
                {
                    b.HasOne("Ace.App.Models.Aircraft", "Aircraft")
                        .WithMany("MaintenanceHistory")
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aircraft");
                });

            modelBuilder.Entity("Ace.App.Models.PilotLicense", b =>
                {
                    b.HasOne("Ace.App.Models.Pilot", "Pilot")
                        .WithMany("Licenses")
                        .HasForeignKey("PilotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pilot");
                });

            modelBuilder.Entity("Ace.App.Models.TypeRating", b =>
                {
                    b.HasOne("Ace.App.Models.Pilot", "Pilot")
                        .WithMany("TypeRatings")
                        .HasForeignKey("PilotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pilot");
                });

            modelBuilder.Entity("Ace.App.Models.Aircraft", b =>
                {
                    b.Navigation("MaintenanceHistory");
                });

            modelBuilder.Entity("Ace.App.Models.Pilot", b =>
                {
                    b.Navigation("Licenses");

                    b.Navigation("TypeRatings");
                });
#pragma warning restore 612, 618
        }
    }
}
