using System;
using Microsoft.Extensions.DependencyInjection;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Ace.App.Services;
using Ace.App.ViewModels;
using Ace.App.Views.Aircraft;
using Ace.App.Views.Core;
using Ace.App.Views.FBO;
using Ace.App.Views.Finance;
using Ace.App.Views.Menus;
using Ace.App.Views.Pilots;

namespace Ace.App.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();


            services.AddDbContext<AceDbContext>(ServiceLifetime.Scoped);


            services.AddScoped<IPilotRepository, PilotRepository>();
            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IAircraftRepository, AircraftRepository>();
            services.AddScoped<IFBORepository, FBORepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IAircraftCatalogRepository, AircraftCatalogRepository>();
            services.AddScoped<IScheduledRouteRepository, ScheduledRouteRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<ITypeRatingRepository, TypeRatingRepository>();
            services.AddScoped<IDailyEarningsRepository, DailyEarningsRepository>();
            services.AddScoped<IMonthlyBillingRepository, MonthlyBillingRepository>();


            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IThemeLoaderService, ThemeLoaderService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<IFinanceService, FinanceService>();
            services.AddSingleton<IPersistenceService, PersistenceService>();
            services.AddSingleton<ILoanService, LoanService>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            services.AddSingleton<IAirportDatabase, AirportDatabase>();
            services.AddSingleton<IActiveFlightPlanService, ActiveFlightPlanService>();
            services.AddSingleton<IAircraftCatalogService, AircraftCatalogService>();
            services.AddSingleton<IPilotCatalogService, PilotCatalogService>();
            services.AddSingleton<IPricingService, PricingService>();
            services.AddSingleton<IFlightEarningsCalculator, FlightEarningsCalculator>();
            services.AddSingleton<IRouteSuggestionService, RouteSuggestionService>();
            services.AddSingleton<IDailyEarningsService, DailyEarningsService>();
            services.AddSingleton<IFBOBillingService, FBOBillingService>();
            services.AddSingleton<IDatabaseBackupService, DatabaseBackupService>();
            services.AddSingleton<IUserDataService, UserDataService>();
            services.AddSingleton<IAircraftImageService, AircraftImageService>();
            services.AddSingleton<IScheduledRouteService, ScheduledRouteService>();
            services.AddSingleton<IAchievementService, AchievementService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<ISoundService, SoundService>();
            services.AddSingleton<IAirspaceService, AirspaceService>();


            services.AddSingleton<INavigationService, NavigationService>();


            services.AddSingleton<SimConnectService>();
            services.AddSingleton<IHardwareInputService, HardwareInputService>();


            services.AddSingleton<DatabaseInitializer>();


            services.AddTransient<PersonnelViewModel>();
            services.AddTransient<BankViewModel>();
            services.AddTransient<HangarViewModel>();
            services.AddTransient<FBOListViewModel>();
            services.AddTransient<MarketViewModel>();
            services.AddTransient<MarketAircraftViewModel>();
            services.AddTransient<AircraftDetailViewModel>();
            services.AddTransient<PilotDetailViewModel>();
            services.AddTransient<MaintenanceScheduleViewModel>();
            services.AddTransient<PilotViewModel>();
            services.AddTransient<FlightMapViewModel>();
            services.AddTransient<FlightPlanMapViewModel>();
            services.AddTransient<FBOMapViewModel>();


            services.AddTransient<HomeMenuView>();
            services.AddTransient<FlightplanView>();
            services.AddTransient<HangarView>();
            services.AddTransient<FBOMenuView>();
            services.AddTransient<FBOView>();
            services.AddTransient<MarketView>();
            services.AddTransient<StatisticsView>();
            services.AddTransient<BankView>();
            services.AddTransient<PersonnelView>();
            services.AddTransient<ManualView>();
            services.AddTransient<SettingsView>();
            services.AddTransient<DashboardView>();
            services.AddTransient<DashboardMenuView>();
            services.AddTransient<SaveLoadView>();
            services.AddTransient<SaveLoadMenuView>();
            services.AddTransient<PilotMenuView>();
            services.AddTransient<AchievementsView>();


            services.AddTransient<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}
