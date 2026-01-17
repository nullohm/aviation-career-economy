using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Ace.App.Commands;
using Ace.App.Converters;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class MaintenanceScheduleViewModel : ViewModelBase
    {
        private readonly IFinanceService _financeService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly int _aircraftId;
        private Aircraft? _aircraft;

        public string Registration { get; private set; } = string.Empty;
        public string TypeVariant { get; private set; } = string.Empty;
        public string CategoryName { get; private set; } = string.Empty;

        private ObservableCollection<MaintenanceCheckViewModel> _availableChecks = new();
        public ObservableCollection<MaintenanceCheckViewModel> AvailableChecks
        {
            get => _availableChecks;
            set => SetProperty(ref _availableChecks, value);
        }

        private MaintenanceCheckViewModel? _selectedCheck;
        public MaintenanceCheckViewModel? SelectedCheck
        {
            get => _selectedCheck;
            set
            {
                SetProperty(ref _selectedCheck, value);
                OnPropertyChanged(nameof(CanSchedule));
            }
        }

        private string _currentMaintenanceInfo = string.Empty;
        public string CurrentMaintenanceInfo
        {
            get => _currentMaintenanceInfo;
            set => SetProperty(ref _currentMaintenanceInfo, value);
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public bool CanSchedule => SelectedCheck != null && _aircraft?.Status != AircraftStatus.Maintenance && _aircraft?.Status != AircraftStatus.InFlight;
        public bool IsInMaintenance => _aircraft?.Status == AircraftStatus.Maintenance;
        public bool CanComplete => IsInMaintenance && _aircraft?.MaintenanceCompletionDate.HasValue == true && _aircraft.MaintenanceCompletionDate.Value <= DateTime.Today;

        public ICommand ScheduleCheckCommand { get; }
        public ICommand CompleteMaintenanceCommand { get; }

        public event EventHandler<string>? OperationCompleted;
        public event EventHandler<string>? ErrorOccurred;
        public event EventHandler? MaintenanceUpdated;

        public MaintenanceScheduleViewModel(int aircraftId, IFinanceService financeService, IMaintenanceService maintenanceService, ILoggingService logger, ISettingsService settingsService)
        {
            _aircraftId = aircraftId;
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _maintenanceService = maintenanceService ?? throw new ArgumentNullException(nameof(maintenanceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            ScheduleCheckCommand = new RelayCommand(OnScheduleCheck, () => CanSchedule);
            CompleteMaintenanceCommand = new RelayCommand(OnCompleteMaintenance, () => CanComplete);

            LoadAircraftData();
        }

        private void LoadAircraftData()
        {
            try
            {
                using var db = new AceDbContext();
                _aircraft = db.Aircraft.Find(_aircraftId);

                if (_aircraft == null)
                {
                    _logger.Error($"MaintenanceScheduleViewModel: Aircraft {_aircraftId} not found");
                    return;
                }

                Registration = _aircraft.Registration;
                TypeVariant = $"{_aircraft.Type} - {_aircraft.Variant}";
                CategoryName = _aircraft.Category.ToString();
                Balance = _financeService.Balance;

                if (_aircraft.Status == AircraftStatus.Maintenance && _aircraft.CurrentMaintenanceType.HasValue)
                {
                    var info = MaintenanceCheckDefinitions.GetCheckInfo(_aircraft.CurrentMaintenanceType.Value, _aircraft.Category, _aircraft.CurrentValue, _settingsService.CurrentSettings);
                    var daysRemaining = _aircraft.MaintenanceCompletionDate.HasValue
                        ? (_aircraft.MaintenanceCompletionDate.Value - DateTime.Today).Days
                        : 0;

                    if (daysRemaining > 0)
                        CurrentMaintenanceInfo = $"Currently in {info.Name} - {daysRemaining} days remaining";
                    else if (daysRemaining == 0)
                        CurrentMaintenanceInfo = $"{info.Name} complete - ready to return to service";
                    else
                        CurrentMaintenanceInfo = $"{info.Name} complete - click 'Complete Maintenance'";
                }
                else
                {
                    CurrentMaintenanceInfo = string.Empty;
                }

                LoadMaintenanceChecks();

                OnPropertyChanged(nameof(IsInMaintenance));
                OnPropertyChanged(nameof(CanComplete));
                OnPropertyChanged(nameof(CanSchedule));
            }
            catch (Exception ex)
            {
                _logger.Error($"MaintenanceScheduleViewModel: Error loading aircraft: {ex.Message}");
            }
        }

        private void LoadMaintenanceChecks()
        {
            if (_aircraft == null) return;

            AvailableChecks.Clear();

            var statuses = _maintenanceService.GetSchedulableChecks(_aircraft);

            foreach (var status in statuses)
            {
                var vm = new MaintenanceCheckViewModel
                {
                    CheckType = status.CheckType,
                    Name = status.Name,
                    Description = status.Description,
                    StatusText = status.StatusText,
                    UrgencyText = status.UrgencyText,
                    Cost = status.Cost,
                    DurationDays = status.DurationDays,
                    IsOverdue = status.IsOverdue,
                    UrgencyScore = status.UrgencyScore,
                    LastPerformed = status.LastPerformed,
                    HoursRemaining = status.HoursRemaining,
                    DaysRemaining = status.DaysRemaining
                };

                vm.StatusColor = status.UrgencyScore switch
                {
                    0 => ConverterColors.UrgencyOverdue.ToBrush(),
                    1 => ConverterColors.UrgencyDueSoon.ToBrush(),
                    2 => ConverterColors.UrgencyUpcoming.ToBrush(),
                    _ => ConverterColors.UrgencyOk.ToBrush()
                };

                AvailableChecks.Add(vm);
            }
        }

        private void OnScheduleCheck()
        {
            if (SelectedCheck == null || _aircraft == null) return;

            if (Balance < SelectedCheck.Cost)
            {
                ErrorOccurred?.Invoke(this, $"Insufficient funds.\nRequired: {SelectedCheck.Cost:N0} €\nBalance: {Balance:N0} €");
                return;
            }

            var result = _maintenanceService.ScheduleMaintenance(_aircraftId, SelectedCheck.CheckType);

            if (result.Success)
            {
                _logger.Info($"MaintenanceScheduleViewModel: Scheduled {SelectedCheck.Name} for {Registration}");
                LoadAircraftData();
                MaintenanceUpdated?.Invoke(this, EventArgs.Empty);
                OperationCompleted?.Invoke(this, result.Message);
            }
            else
            {
                ErrorOccurred?.Invoke(this, result.Message);
            }
        }

        private void OnCompleteMaintenance()
        {
            var result = _maintenanceService.CompleteMaintenance(_aircraftId);

            if (result.Success)
            {
                _logger.Info($"MaintenanceScheduleViewModel: Completed maintenance for {Registration}");
                LoadAircraftData();
                MaintenanceUpdated?.Invoke(this, EventArgs.Empty);
                OperationCompleted?.Invoke(this, result.Message);
            }
            else
            {
                ErrorOccurred?.Invoke(this, result.Message);
            }
        }

        public void Refresh()
        {
            LoadAircraftData();
        }
    }

    public class MaintenanceCheckViewModel : ViewModelBase
    {
        public MaintenanceCheckType CheckType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusText { get; set; } = string.Empty;
        public string UrgencyText { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public int DurationDays { get; set; }
        public bool IsOverdue { get; set; }
        public int UrgencyScore { get; set; }
        public DateTime? LastPerformed { get; set; }
        public double? HoursRemaining { get; set; }
        public int? DaysRemaining { get; set; }
        public Brush StatusColor { get; set; } = Brushes.Gray;

        public string CostText => $"{Cost:N0} €";
        public string DurationText => DurationDays == 1 ? "1 day" : $"{DurationDays} days";
        public string LastPerformedText => LastPerformed.HasValue ? LastPerformed.Value.ToString("dd.MM.yyyy") : "Never";
    }
}
