using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Ace.App.Commands;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.ViewModels
{
    public class AircraftDetailViewModel : AircraftDetailViewModelBase
    {
        private readonly ILoggingService _logger;
        private readonly IFinanceService _financeService;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IFBORepository _fboRepository;
        private readonly int _aircraftId;

        public int AircraftId => _aircraftId;

        private DateTime _purchaseDate;
        public DateTime PurchaseDate
        {
            get => _purchaseDate;
            set => SetProperty(ref _purchaseDate, value);
        }

        private double _maxCargoKg;
        public double MaxCargoKg
        {
            get => _maxCargoKg;
            set => SetProperty(ref _maxCargoKg, value);
        }

        private int _serviceCeilingFt;
        public int ServiceCeilingFt
        {
            get => _serviceCeilingFt;
            set => SetProperty(ref _serviceCeilingFt, value);
        }

        private double _totalFlightHours;
        public double TotalFlightHours
        {
            get => _totalFlightHours;
            set => SetProperty(ref _totalFlightHours, value);
        }

        private double _hoursSinceLastMaintenance;
        public double HoursSinceLastMaintenance
        {
            get => _hoursSinceLastMaintenance;
            set
            {
                SetProperty(ref _hoursSinceLastMaintenance, value);
                OnPropertyChanged(nameof(MaintenanceWarningColor));
            }
        }

        private DateTime _lastMaintenanceDate;
        public DateTime LastMaintenanceDate
        {
            get => _lastMaintenanceDate;
            set => SetProperty(ref _lastMaintenanceDate, value);
        }

        private DateTime? _maintenanceCompletionDate;
        public DateTime? MaintenanceCompletionDate
        {
            get => _maintenanceCompletionDate;
            set => SetProperty(ref _maintenanceCompletionDate, value);
        }

        public string MaintenanceStatusText
        {
            get
            {
                if (Status == "Maintenance" && MaintenanceCompletionDate.HasValue)
                {
                    var remainingDays = (MaintenanceCompletionDate.Value - DateTime.Today).Days;
                    if (remainingDays > 0)
                        return $"In Maintenance - {remainingDays} days remaining";
                    else if (remainingDays == 0)
                        return "Maintenance complete - ready to return to service";
                    else
                        return "Maintenance overdue";
                }
                return Status;
            }
        }

        public Brush MaintenanceWarningColor
        {
            get
            {
                if (HoursSinceLastMaintenance >= 100)
                    return new SolidColorBrush(Color.FromRgb(239, 68, 68));
                if (HoursSinceLastMaintenance >= 75)
                    return new SolidColorBrush(Color.FromRgb(251, 146, 60));
                return new SolidColorBrush(Color.FromRgb(34, 197, 94));
            }
        }

        private int? _assignedFBOId;
        public int? AssignedFBOId
        {
            get => _assignedFBOId;
            set => SetProperty(ref _assignedFBOId, value);
        }

        private string _assignedFBOName = string.Empty;
        public string AssignedFBOName
        {
            get => _assignedFBOName;
            set => SetProperty(ref _assignedFBOName, value);
        }

        private ObservableCollection<FBOSelectionItem> _availableFBOs = new();
        public ObservableCollection<FBOSelectionItem> AvailableFBOs
        {
            get => _availableFBOs;
            set => SetProperty(ref _availableFBOs, value);
        }

        private FBOSelectionItem? _selectedFBO;
        public FBOSelectionItem? SelectedFBO
        {
            get => _selectedFBO;
            set => SetProperty(ref _selectedFBO, value);
        }

        public ICommand ScheduleMaintenanceCommand { get; }
        public ICommand CompleteMaintenanceCommand { get; }
        public ICommand AssignToFBOCommand { get; }
        public ICommand RemoveFromFBOCommand { get; }
        public ICommand EditSpecsCommand { get; }

        public event EventHandler<string>? MaintenanceCompleted;
        public event EventHandler<string>? ErrorOccurred;
        public event EventHandler? EditSpecsRequested;

        public AircraftDetailViewModel(
            int aircraftId,
            ILoggingService logger,
            IFinanceService financeService,
            IAircraftRepository aircraftRepository,
            IFBORepository fboRepository)
        {
            _aircraftId = aircraftId;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));

            ScheduleMaintenanceCommand = new RelayCommand(OnScheduleMaintenance, CanScheduleMaintenance);
            CompleteMaintenanceCommand = new RelayCommand(OnCompleteMaintenance, CanCompleteMaintenance);
            AssignToFBOCommand = new RelayCommand(OnAssignToFBO, CanAssignToFBO);
            RemoveFromFBOCommand = new RelayCommand(OnRemoveFromFBO, CanRemoveFromFBO);
            EditSpecsCommand = new RelayCommand(OnEditSpecs);

            LoadAircraftData();
            LoadAvailableFBOs();
        }

        private void LoadAircraftData()
        {
            _logger.Info($"AircraftDetailViewModel: Loading aircraft data for ID {_aircraftId}");

            try
            {
                var aircraft = _aircraftRepository.GetAircraftById(_aircraftId);

                if (aircraft == null)
                {
                    _logger.Error($"AircraftDetailViewModel: Aircraft with ID {_aircraftId} not found");
                    ErrorOccurred?.Invoke(this, "Aircraft not found");
                    return;
                }

                Registration = aircraft.Registration;
                TypeVariant = $"{aircraft.Type} - {aircraft.Variant}";
                HomeBase = aircraft.HomeBase;
                Status = aircraft.Status == AircraftStatus.Stationed
                    ? $"Stationed - {aircraft.HomeBase}"
                    : aircraft.Status.ToString();
                PurchaseDate = aircraft.PurchaseDate;
                CurrentValue = aircraft.CurrentValue;
                MaxPassengers = aircraft.MaxPassengers;
                MaxCargoKg = aircraft.MaxCargoKg;
                CruiseSpeedKts = aircraft.CruiseSpeedKts;
                MaxRangeNM = aircraft.MaxRangeNM;
                FuelCapacityGal = aircraft.FuelCapacityGal;
                FuelBurnGalPerHour = aircraft.FuelBurnGalPerHour;
                HourlyOperatingCost = aircraft.HourlyOperatingCost;
                ServiceCeilingFt = aircraft.ServiceCeilingFt;
                TotalFlightHours = aircraft.TotalFlightHours;
                HoursSinceLastMaintenance = aircraft.HoursSinceLastMaintenance;
                LastMaintenanceDate = aircraft.LastMaintenanceDate;
                MaintenanceCompletionDate = aircraft.MaintenanceCompletionDate;

                StatusColor = aircraft.Status switch
                {
                    AircraftStatus.Available => new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                    AircraftStatus.InFlight => new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                    AircraftStatus.Maintenance => new SolidColorBrush(Color.FromRgb(251, 146, 60)),
                    AircraftStatus.Grounded => new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                    AircraftStatus.Stationed => new SolidColorBrush(Color.FromRgb(168, 85, 247)),
                    _ => Brushes.Gray
                };

                AssignedFBOId = aircraft.AssignedFBOId;
                if (aircraft.AssignedFBOId.HasValue)
                {
                    var fbo = _fboRepository.GetFBOById(aircraft.AssignedFBOId.Value);
                    AssignedFBOName = fbo != null ? $"{fbo.ICAO} - {fbo.AirportName}" : "Unknown FBO";
                }
                else
                {
                    AssignedFBOName = string.Empty;
                }

                _logger.Info($"AircraftDetailViewModel: Loaded aircraft {Registration}");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftDetailViewModel: Error loading aircraft data: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error loading aircraft data: {ex.Message}");
            }
        }

        private bool CanScheduleMaintenance()
        {
            return Status != "Maintenance" && Status != "InFlight";
        }

        private void OnScheduleMaintenance()
        {
            try
            {
                var maintenanceCost = CurrentValue * 0.02m;

                if (_financeService.Balance < maintenanceCost)
                {
                    ErrorOccurred?.Invoke(this, $"Insufficient funds for maintenance. Cost: {maintenanceCost:N0} €");
                    return;
                }

                _logger.Info($"AircraftDetailViewModel: Scheduling maintenance for {Registration}, cost: {maintenanceCost:N0} €");

                _financeService.AddExpense(maintenanceCost, $"Maintenance: {Registration}");

                var aircraft = _aircraftRepository.GetAircraftById(_aircraftId);

                if (aircraft != null)
                {
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.Status = AircraftStatus.Maintenance;
                    aircraft.MaintenanceCompletionDate = DateTime.Today.AddDays(7);

                    _aircraftRepository.UpdateAircraft(aircraft);

                    _logger.Info($"AircraftDetailViewModel: Maintenance scheduled for {Registration}, completion: {aircraft.MaintenanceCompletionDate:dd.MM.yyyy}");
                }

                LoadAircraftData();
                MaintenanceCompleted?.Invoke(this, $"Maintenance scheduled for {Registration}.\nCost: {maintenanceCost:N0} €\n\nCompletion date: {DateTime.Today.AddDays(7):dd.MM.yyyy}\n\nAircraft status set to Maintenance.");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftDetailViewModel: Error scheduling maintenance: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error scheduling maintenance: {ex.Message}");
            }
        }

        private bool CanCompleteMaintenance()
        {
            return Status == "Maintenance" &&
                   MaintenanceCompletionDate.HasValue &&
                   MaintenanceCompletionDate.Value <= DateTime.Today;
        }

        private void OnCompleteMaintenance()
        {
            try
            {
                _logger.Info($"AircraftDetailViewModel: Completing maintenance for {Registration}");

                var aircraft = _aircraftRepository.GetAircraftById(_aircraftId);

                if (aircraft != null)
                {
                    aircraft.Status = AircraftStatus.Available;
                    aircraft.MaintenanceCompletionDate = null;

                    _aircraftRepository.UpdateAircraft(aircraft);

                    _logger.Info($"AircraftDetailViewModel: Maintenance completed for {Registration}");
                }

                LoadAircraftData();
                MaintenanceCompleted?.Invoke(this, $"{Registration} is now back in service.\n\nAircraft status set to Available.");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftDetailViewModel: Error completing maintenance: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error completing maintenance: {ex.Message}");
            }
        }

        private void LoadAvailableFBOs()
        {
            try
            {
                var fbos = _fboRepository.GetAllFBOs().OrderBy(f => f.ICAO).ToList();

                var aircraftSize = AircraftSizeExtensions.GetAircraftSize(MaxPassengers);
                var requiredRunway = aircraftSize.GetRequiredRunwayLengthFt();
                var requiredTerminal = aircraftSize.GetRequiredTerminalSize();

                AvailableFBOs.Clear();
                foreach (var fbo in fbos)
                {
                    bool runwayOk = fbo.RunwayLengthFt == 0 || fbo.RunwayLengthFt >= requiredRunway;
                    bool terminalOk = fbo.TerminalSize >= requiredTerminal;
                    bool isCompatible = runwayOk && terminalOk;

                    if (isCompatible)
                    {
                        AvailableFBOs.Add(new FBOSelectionItem(
                            fbo.Id, fbo.ICAO, fbo.AirportName,
                            fbo.RunwayLengthFt, fbo.TerminalSize, true));
                    }
                }

                _logger.Debug($"AircraftDetailViewModel: Loaded {AvailableFBOs.Count} compatible FBOs for {aircraftSize} aircraft (need {requiredRunway}ft runway, {requiredTerminal} terminal)");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftDetailViewModel: Error loading FBOs: {ex.Message}");
            }
        }

        private bool CanAssignToFBO()
        {
            return SelectedFBO != null && Status == "Available";
        }

        private void OnAssignToFBO()
        {
            if (SelectedFBO == null) return;

            try
            {
                _logger.Info($"AircraftDetailViewModel: Assigning {Registration} to FBO {SelectedFBO.ICAO}");

                var aircraft = _aircraftRepository.GetAircraftById(_aircraftId);

                if (aircraft != null)
                {
                    aircraft.AssignedFBOId = SelectedFBO.Id;
                    aircraft.Status = AircraftStatus.Stationed;
                    aircraft.HomeBase = SelectedFBO.ICAO;

                    _aircraftRepository.UpdateAircraft(aircraft);

                    _logger.Info($"AircraftDetailViewModel: {Registration} assigned to {SelectedFBO.ICAO}");
                }

                LoadAircraftData();
                MaintenanceCompleted?.Invoke(this, $"{Registration} has been assigned to {SelectedFBO.DisplayName}.\n\nAircraft status set to Stationed.");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftDetailViewModel: Error assigning to FBO: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error assigning to FBO: {ex.Message}");
            }
        }

        private bool CanRemoveFromFBO()
        {
            return AssignedFBOId.HasValue && Status.StartsWith("Stationed");
        }

        private void OnRemoveFromFBO()
        {
            try
            {
                _logger.Info($"AircraftDetailViewModel: Removing {Registration} from FBO assignment");

                var aircraft = _aircraftRepository.GetAircraftById(_aircraftId);

                if (aircraft != null)
                {
                    aircraft.AssignedFBOId = null;
                    aircraft.Status = AircraftStatus.Available;

                    _aircraftRepository.UpdateAircraft(aircraft);

                    _logger.Info($"AircraftDetailViewModel: {Registration} removed from FBO assignment");
                }

                LoadAircraftData();
                MaintenanceCompleted?.Invoke(this, $"{Registration} is now available again.\n\nAircraft status set to Available.");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftDetailViewModel: Error removing from FBO: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error removing from FBO: {ex.Message}");
            }
        }

        private void OnEditSpecs()
        {
            EditSpecsRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RefreshData()
        {
            LoadAircraftData();
        }
    }

    public class FBOSelectionItem
    {
        public int Id { get; }
        public string ICAO { get; }
        public string AirportName { get; }
        public int RunwayLengthFt { get; }
        public TerminalSize TerminalSize { get; }
        public string DisplayName { get; }

        public FBOSelectionItem(int id, string icao, string airportName, int runwayLengthFt, TerminalSize terminalSize, bool isCompatible)
        {
            Id = id;
            ICAO = icao;
            AirportName = airportName;
            RunwayLengthFt = runwayLengthFt;
            TerminalSize = terminalSize;

            if (isCompatible)
            {
                DisplayName = $"{icao} - {airportName}";
            }
            else
            {
                var issues = new System.Collections.Generic.List<string>();
                if (runwayLengthFt == 0)
                    issues.Add("RWY?");
                if (terminalSize == TerminalSize.None)
                    issues.Add("No Terminal");
                DisplayName = $"{icao} - {airportName} ({string.Join(", ", issues)})";
            }
        }
    }
}
