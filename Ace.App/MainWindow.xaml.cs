using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Ace.App.Interfaces;
using Ace.App.Services;
using Ace.App.Views.Aircraft;
using Ace.App.Views.Core;
using Ace.App.Views.FBO;
using Ace.App.Views.Finance;
using Ace.App.Views.Menus;
using Ace.App.Views.Pilots;

namespace Ace.App
{
    public partial class MainWindow : Window
    {
        public SimConnectService SimConnectService { get; }
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;

        public MainWindow(
            INavigationService navigationService,
            SimConnectService simConnectService,
            ILoggingService logger,
            ISettingsService settingsService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            SimConnectService = simConnectService ?? throw new ArgumentNullException(nameof(simConnectService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            InitializeComponent();
            DataContext = this;
            SimConnectService.ConnectionChanged += OnConnectionChanged;

            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            TxtVersion.Text = $"Build {version}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _logger.Debug("MainWindow: Window_Loaded - initializing NavigationService and loading HomeMenuView");


            if (_navigationService is NavigationService navService)
            {
                navService.Initialize(MainContent);
            }


            Dispatcher.BeginInvoke(new Action(() =>
            {
                _navigationService.NavigateTo<HomeMenuView>();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var settings = _settingsService.CurrentSettings;

            var screenWidth = SystemParameters.VirtualScreenWidth;
            var screenHeight = SystemParameters.VirtualScreenHeight;
            var screenLeft = SystemParameters.VirtualScreenLeft;
            var screenTop = SystemParameters.VirtualScreenTop;

            if (settings.WindowLeft < screenLeft || settings.WindowLeft > screenLeft + screenWidth ||
                settings.WindowTop < screenTop || settings.WindowTop > screenTop + screenHeight)
            {
                _logger.Warn($"Window position ({settings.WindowLeft},{settings.WindowTop}) is off-screen, resetting to default");
                Top = 100;
                Left = 100;
            }
            else
            {
                Top = settings.WindowTop;
                Left = settings.WindowLeft;
            }

            Width = settings.WindowWidth;
            Height = settings.WindowHeight;
            if (settings.IsMaximized)
            {
                WindowState = WindowState.Maximized;
            }
            _logger.Info($"Window position restored: ({Left},{Top}) {settings.WindowWidth}x{settings.WindowHeight} Maximized={settings.IsMaximized}");

            IntPtr handle = new WindowInteropHelper(this).Handle;
            SimConnectService.SetWindowHandle(handle);
            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(SimConnectService.WndProc);
            SimConnectService.Initialize();

            if (_settingsService.CurrentSettings.IsSimConnectEnabled)
            {
                _logger.Debug("MainWindow: SimConnect enabled, connecting...");
                SimConnectService.Connect();
            }
        }

        private void OnConnectionChanged(bool connected)
        {
            _logger.Info($"MainWindow: SimConnect Status changed: {(connected ? "Connected" : "Disconnected")}");
        }

        private void Titlebar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void BtnMax_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void SetSelectedNavButton(Button selectedButton)
        {
            BtnHome.Tag = null;
            BtnFlightplan.Tag = null;
            BtnFBO.Tag = null;
            BtnHangar.Tag = null;
            BtnPersonnel.Tag = null;
            BtnMarket.Tag = null;
            BtnFlights.Tag = null;
            BtnBank.Tag = null;
            BtnAchievements.Tag = null;
            BtnManual.Tag = null;
            BtnSettings.Tag = null;
            selectedButton.Tag = "Selected";
        }

        private void OnHomeClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnHome);
            _navigationService.NavigateTo<HomeMenuView>();
        }

        private void OnFlightplanClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnFlightplan);
            _navigationService.NavigateTo<FlightplanView>();
        }

        private void OnHangarClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnHangar);
            _navigationService.NavigateTo<HangarView>();
        }

        private void OnFBOClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnFBO);
            _navigationService.NavigateTo<FBOMenuView>();
        }

        private void OnMarketClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnMarket);
            _navigationService.NavigateTo<MarketView>();
        }

        private void OnFlightsClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnFlights);
            _navigationService.NavigateTo<StatisticsView>();
        }

        private void OnBankClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnBank);
            _navigationService.NavigateTo<BankView>();
        }

        private void OnAchievementsClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnAchievements);
            _navigationService.NavigateTo<AchievementsView>();
        }

        private void OnPersonnelClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnPersonnel);
            _navigationService.NavigateTo<PersonnelView>();
        }

        private void OnManualClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnManual);
            _navigationService.NavigateTo<ManualView>();
        }

        private void OnSettingsClicked(object sender, RoutedEventArgs e)
        {
            SetSelectedNavButton(BtnSettings);
            _navigationService.NavigateTo<SettingsView>();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            settings.IsMaximized = WindowState == WindowState.Maximized;

            _logger.Info($"Window_Closing: Current state = {WindowState}, Top={Top}, Left={Left}, Width={Width}, Height={Height}, ActualWidth={ActualWidth}, ActualHeight={ActualHeight}");

            if (WindowState == WindowState.Normal)
            {
                settings.WindowTop = Top;
                settings.WindowLeft = Left;
                settings.WindowWidth = ActualWidth;
                settings.WindowHeight = ActualHeight;
                _logger.Info($"Window_Closing: Saving Normal state values");
            }
            else
            {
                _logger.Info($"Window_Closing: Window is {WindowState}, not saving size");
            }

            _settingsService.Save();
            _logger.Info($"Window position saved: ({settings.WindowLeft},{settings.WindowTop}) {settings.WindowWidth}x{settings.WindowHeight} Maximized={settings.IsMaximized}");

            await SimConnectService.StopAutoConnectAsync();
        }
    }
}

