using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Ace.App.Data;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Services;
using Ace.App.Models;
using Ace.App.Views.Core;
using Ace.App.Views.Dialogs;

namespace Ace.App
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loadingWindow = new LoadingWindow();
            loadingWindow.Show();


            ServiceProvider = ServiceConfiguration.ConfigureServices();
            ServiceLocator.Initialize(ServiceProvider);


            var logger = ServiceProvider.GetRequiredService<ILoggingService>();
            logger.Initialize();
            logger.Info("Application startup - DI container initialized");

            var userDataService = ServiceProvider.GetRequiredService<IUserDataService>();
            userDataService.EnsureUserDataFiles();

            var backupService = ServiceProvider.GetRequiredService<IDatabaseBackupService>();
            backupService.CreateBackup();

            try
            {
                loadingWindow.UpdateStatus("Initializing logging...");
                await Task.Delay(100);

                loadingWindow.UpdateStatus("Initializing database...");
                logger.Info("App: Initializing database");
                var dbInitializer = ServiceProvider.GetRequiredService<DatabaseInitializer>();
                await Task.Run(() =>
                {
                    using var context = new AceDbContext();
                    dbInitializer.Initialize(context);
                });
                logger.Info("App: Database initialized successfully");


                var settingsService = ServiceProvider.GetRequiredService<ISettingsService>();
                var aircraftCatalogService = ServiceProvider.GetRequiredService<IAircraftCatalogService>();
                var maintenanceService = ServiceProvider.GetRequiredService<IMaintenanceService>();
                var dailyEarningsService = ServiceProvider.GetRequiredService<IDailyEarningsService>();
                var fboBillingService = ServiceProvider.GetRequiredService<IFBOBillingService>();

                loadingWindow.UpdateStatus("Loading settings...");
                logger.Info("App: Loading settings");
                settingsService.CurrentSettings.ToString();
                logger.Info("App: Settings loaded");

                loadingWindow.UpdateStatus("Applying theme...");
                var themeService = ServiceProvider.GetRequiredService<IThemeService>();
                themeService.ApplyCurrentTheme();
                logger.Info($"App: Theme '{themeService.CurrentTheme}' applied");

                loadingWindow.UpdateStatus("Searching for MSFS installation...");
                logger.Info("App: Searching for MSFS installation");
                var msfsPath = Environment.GetEnvironmentVariable("MSFS_PACKAGES_PATH");
                if (string.IsNullOrEmpty(msfsPath))
                {
                    msfsPath = System.IO.Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Packages",
                        "Microsoft.FlightSimulator_8wekyb3d8bbwe",
                        "LocalCache",
                        "Packages"
                    );
                }
                logger.Info($"App: MSFS path: {msfsPath}");

                loadingWindow.UpdateStatus("Initializing airports...");
                logger.Info("App: Initializing airport database");
                var airportDatabase = ServiceProvider.GetRequiredService<IAirportDatabase>();
                await Task.Run(() => airportDatabase.Initialize(msfsPath));
                logger.Info("App: Airport database initialized");

                if (!airportDatabase.IsAvailable)
                {
                    logger.Warn("App: Little Navmap database not found - showing warning to user");
                    MessageBox.Show(
                        "Little Navmap airport database not found.\n\n" +
                        "For full functionality with 45,000+ airports, please install Little Navmap and load the MSFS 2024 scenery library.\n\n" +
                        "Download: https://albar965.github.io/littlenavmap.html\n\n" +
                        "Airport data will not be available until Little Navmap is configured.",
                        "Little Navmap Required",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }

                loadingWindow.UpdateStatus("Scanning available aircraft...");
                logger.Info("App: Scanning available aircraft");
                await Task.Run(aircraftCatalogService.LoadAvailableAircraft);

                var aircraftCount = aircraftCatalogService.AvailableAircraft.Count;
                logger.Info($"App: Aircraft scan completed, found {aircraftCount} aircraft");

                if (aircraftCount == 0)
                {
                    logger.Warn("App: No aircraft found in MSFS installation, continuing anyway");
                }

                loadingWindow.UpdateStatus("Checking maintenance status...");
                maintenanceService.CompleteOverdueMaintenances();
                maintenanceService.GroundAircraftWithOverdueChecks();

                loadingWindow.UpdateStatus("Processing daily earnings...");
                await dailyEarningsService.ProcessDailyEarnings();

                loadingWindow.UpdateStatus("Processing FBO billing...");
                await fboBillingService.ProcessMonthlyFBOBilling();

                loadingWindow.UpdateStatus("Initializing achievements...");
                var achievementService = ServiceProvider.GetRequiredService<IAchievementService>();
                achievementService.InitializeAchievements();
                achievementService.AchievementUnlocked += OnAchievementUnlocked;

                loadingWindow.UpdateStatus("Starting main application...");
                await Task.Delay(300);

                logger.Info("App: Opening main window");
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                MainWindow = mainWindow;
                mainWindow.Show();

                loadingWindow.Close();
                logger.Info("App: Startup completed successfully");
            }
            catch (Exception ex)
            {
                logger.Error($"App: Startup failed: {ex.Message}");
                logger.Error($"App: Exception type: {ex.GetType().Name}");
                logger.Error($"App: Stack trace: {ex.StackTrace}");

                loadingWindow.Close();

                var result = MessageBox.Show(
                    $"Application startup error:\n\n{ex.Message}\n\nThe application will close.",
                    "Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                logger.Info("App: User confirmed error, shutting down");
                Current.Shutdown();
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = ServiceProvider?.GetService<ILoggingService>();
            logger?.Error("CRITICAL UI ERROR", e.Exception);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                var logger = ServiceProvider?.GetService<ILoggingService>();
                logger?.Error("CRITICAL DOMAIN ERROR", ex);
            }
        }

        private static void OnAchievementUnlocked(Achievement achievement)
        {
            AchievementNotification.Show(achievement);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var logger = ServiceProvider?.GetService<ILoggingService>();
            logger?.Info("Application exit");
            try
            {
                if (logger is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to dispose logger: {ex.Message}");
            }
            base.OnExit(e);
        }
    }
}
