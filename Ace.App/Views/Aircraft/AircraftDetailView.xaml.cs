using System;
using System.Windows;
using Ace.App.Interfaces;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;

namespace Ace.App.Views.Aircraft
{
    public partial class AircraftDetailView : Window
    {
        private readonly AircraftDetailViewModel _viewModel;
        private readonly IFinanceService _financeService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IAircraftRepository _aircraftRepository;

        public AircraftDetailView(
            AircraftDetailViewModel viewModel,
            IFinanceService financeService,
            IMaintenanceService maintenanceService,
            ILoggingService logger,
            ISettingsService settingsService,
            IAircraftRepository aircraftRepository)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _maintenanceService = maintenanceService ?? throw new ArgumentNullException(nameof(maintenanceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            DataContext = viewModel;

            viewModel.MaintenanceCompleted += OnMaintenanceCompleted;
            viewModel.ErrorOccurred += OnErrorOccurred;
            viewModel.EditSpecsRequested += OnEditSpecsRequested;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnMaintenanceCompleted(object? sender, string message)
        {
            var dialog = new MessageDialog("Maintenance Scheduled", message);
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void OnErrorOccurred(object? sender, string error)
        {
            var dialog = new MessageDialog("Error", error);
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void OnEditSpecsRequested(object? sender, EventArgs e)
        {
            if (DataContext is not AircraftDetailViewModel viewModel) return;

            var dialog = new EditAircraftSpecsDialog(
                viewModel.AircraftId,
                viewModel.TypeVariant,
                viewModel.MaxPassengers,
                viewModel.MaxCargoKg,
                viewModel.CruiseSpeedKts,
                viewModel.MaxRangeNM,
                viewModel.FuelCapacityGal,
                viewModel.FuelBurnGalPerHour,
                viewModel.ServiceCeilingFt,
                viewModel.HourlyOperatingCost,
                viewModel.TotalFlightHours,
                _logger,
                _aircraftRepository
            );
            dialog.Owner = this;

            if (dialog.ShowDialog() == true && dialog.Saved)
            {
                viewModel.RefreshData();
            }
        }

        private void BtnMaintenanceSchedule_Click(object sender, RoutedEventArgs e)
        {
            var scheduleViewModel = new MaintenanceScheduleViewModel(_viewModel.AircraftId, _financeService, _maintenanceService, _logger, _settingsService);

            scheduleViewModel.MaintenanceUpdated += (s, args) => _viewModel.RefreshData();

            var scheduleView = new MaintenanceScheduleView(scheduleViewModel);
            scheduleView.Owner = this;
            scheduleView.ShowDialog();
        }
    }
}
