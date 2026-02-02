using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Ace.App.Commands;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class PersonnelViewModel : ViewModelBase
    {
        private readonly IPilotRepository _pilotRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IFBORepository _fboRepository;
        private readonly ILoggingService _logger;
        private readonly IFinanceService _financeService;
        private readonly IAchievementService _achievementService;
        private readonly ISettingsService _settingsService;
        private readonly IAircraftPilotAssignmentRepository _assignmentRepository;

        public ObservableCollection<PilotViewModel> AvailablePilots { get; } = new();
        public ObservableCollection<PilotViewModel> EmployedPilots { get; } = new();

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        private string _availablePilotsCountText = string.Empty;
        public string AvailablePilotsCountText
        {
            get => _availablePilotsCountText;
            set => SetProperty(ref _availablePilotsCountText, value);
        }

        private string _employedPilotsCountText = string.Empty;
        public string EmployedPilotsCountText
        {
            get => _employedPilotsCountText;
            set => SetProperty(ref _employedPilotsCountText, value);
        }

        public ICommand HirePilotCommand { get; }
        public ICommand FirePilotCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand EditPilotCommand { get; }
        public ICommand DeletePilotCommand { get; }
        public ICommand CreatePilotCommand { get; }


        public event EventHandler<PilotActionEventArgs>? HireRequested;
        public event EventHandler<PilotActionEventArgs>? HireCompleted;
        public event EventHandler<PilotActionEventArgs>? FireRequested;
        public event EventHandler<PilotActionEventArgs>? FireCompleted;
        public event EventHandler<PilotActionEventArgs>? EditRequested;
        public event EventHandler<PilotActionEventArgs>? EditCompleted;
        public event EventHandler<PilotActionEventArgs>? DeleteRequested;
        public event EventHandler<PilotActionEventArgs>? DeleteCompleted;
        public event EventHandler? CreateRequested;
        public event EventHandler<PilotActionEventArgs>? CreateCompleted;
        public event EventHandler<ErrorEventArgs>? ErrorOccurred;

        public PersonnelViewModel(
            IPilotRepository pilotRepository,
            IAircraftRepository aircraftRepository,
            IFBORepository fboRepository,
            ILoggingService logger,
            IFinanceService financeService,
            IAchievementService achievementService,
            ISettingsService settingsService,
            IAircraftPilotAssignmentRepository assignmentRepository)
        {
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _achievementService = achievementService ?? throw new ArgumentNullException(nameof(achievementService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));

            HirePilotCommand = new RelayCommand<PilotViewModel>(OnHirePilot, CanHirePilot);
            FirePilotCommand = new RelayCommand<PilotViewModel>(OnFirePilot, CanFirePilot);
            RefreshCommand = new RelayCommand(LoadPilots);
            EditPilotCommand = new RelayCommand<PilotViewModel>(OnEditPilot, CanEditPilot);
            DeletePilotCommand = new RelayCommand<PilotViewModel>(OnDeletePilot, CanDeletePilot);
            CreatePilotCommand = new RelayCommand(OnCreatePilot);

            LoadPilots();
        }

        public void LoadPilots()
        {
            _logger.Info("PersonnelViewModel: Loading pilots");

            AvailablePilots.Clear();
            EmployedPilots.Clear();

            try
            {
                var availablePilots = _pilotRepository.GetAvailablePilots();
                foreach (var pilot in availablePilots)
                {
                    AvailablePilots.Add(new PilotViewModel(pilot, _logger, _settingsService));
                }

                var employedPilots = _pilotRepository.GetEmployedPilots();
                foreach (var pilot in employedPilots)
                {
                    var pilotVm = new PilotViewModel(pilot, _logger, _settingsService);
                    EmployedPilots.Add(pilotVm);
                }

                LoadPilotStatuses();

                AvailablePilotsCountText = $"{AvailablePilots.Count} pilots available for hire";
                EmployedPilotsCountText = $"{EmployedPilots.Count} pilots employed";

                Balance = _financeService.Balance;

                _logger.Info($"PersonnelViewModel: Loaded {AvailablePilots.Count} available and {EmployedPilots.Count} employed pilots");
            }
            catch (Exception ex)
            {
                _logger.Error($"PersonnelViewModel: Failed to load pilots: {ex.Message}");
                AvailablePilotsCountText = "Error loading pilots";
                EmployedPilotsCountText = "Error loading pilots";
                ErrorOccurred?.Invoke(this, new ErrorEventArgs($"Failed to load pilots: {ex.Message}"));
            }
        }

        private void LoadPilotStatuses()
        {
            try
            {
                var allAssignments = _assignmentRepository.GetAllAssignments();
                var allAircraft = _aircraftRepository.GetAllAircraft();
                var fbos = _fboRepository.GetAllFBOs();

                foreach (var pilotVm in EmployedPilots)
                {
                    var assignment = allAssignments.FirstOrDefault(a => a.PilotId == pilotVm.Id);
                    var assignedAircraft = assignment != null
                        ? allAircraft.FirstOrDefault(a => a.Id == assignment.AircraftId)
                        : null;
                    if (assignedAircraft != null)
                    {
                        var fbo = fbos.FirstOrDefault(f => f.Id == assignedAircraft.AssignedFBOId);
                        pilotVm.Status = $"ASSIGNED - {assignedAircraft.Registration}";
                        pilotVm.StatusDetail = $"{assignedAircraft.Registration} ({assignedAircraft.Type}) @ {fbo?.ICAO ?? "Unknown"}";
                        pilotVm.HasStatus = true;
                    }
                }

                _logger.Info($"PersonnelViewModel: Loaded statuses for {allAssignments.Count} assigned pilots");
            }
            catch (Exception ex)
            {
                _logger.Error($"PersonnelViewModel: Failed to load pilot statuses: {ex.Message}");
            }
        }

        private bool CanHirePilot(PilotViewModel? pilot)
        {
            return pilot != null;
        }

        private void OnHirePilot(PilotViewModel? pilot)
        {
            if (pilot == null) return;

            _logger.Info($"PersonnelViewModel: Hire attempt for {pilot.Name} at {pilot.SalaryPerMonth:N0} €/month");


            var eventArgs = new PilotActionEventArgs(pilot);
            HireRequested?.Invoke(this, eventArgs);

            if (eventArgs.Confirmed)
            {
                ExecuteHire(pilot);
            }
            else
            {
                _logger.Info("PersonnelViewModel: Hire cancelled by user");
            }
        }

        public void ExecuteHire(PilotViewModel pilot)
        {
            try
            {
                _pilotRepository.HirePilot(pilot.Id);
                _logger.Info($"PersonnelViewModel: Successfully hired {pilot.Name}");

                LoadPilots();
                CheckPilotAchievements();
                HireCompleted?.Invoke(this, new PilotActionEventArgs(pilot) { Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error($"PersonnelViewModel: Error hiring pilot: {ex.Message}");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs($"Failed to hire pilot: {ex.Message}"));
            }
        }

        private void CheckPilotAchievements()
        {
            var employedPilots = _pilotRepository.GetEmployedPilots();
            var pilotCount = employedPilots.Count;
            var playerPilot = _pilotRepository.GetPlayerPilot();
            var playerFlightHours = playerPilot?.TotalFlightHours ?? 0;
            _achievementService.CheckPilotAchievements(pilotCount, playerFlightHours);
        }

        private bool CanFirePilot(PilotViewModel? pilot)
        {
            return pilot != null && !pilot.IsPlayer;
        }

        private void OnFirePilot(PilotViewModel? pilot)
        {
            if (pilot == null) return;

            if (pilot.IsPlayer)
            {
                _logger.Warn("PersonnelViewModel: Attempt to fire player pilot blocked");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs("You cannot fire yourself!"));
                return;
            }

            _logger.Info($"PersonnelViewModel: Fire attempt for {pilot.Name}");


            var eventArgs = new PilotActionEventArgs(pilot);
            FireRequested?.Invoke(this, eventArgs);

            if (eventArgs.Confirmed)
            {
                ExecuteFire(pilot);
            }
            else
            {
                _logger.Info("PersonnelViewModel: Fire cancelled by user");
            }
        }

        public void ExecuteFire(PilotViewModel pilot)
        {
            try
            {
                _pilotRepository.FirePilot(pilot.Id);
                _logger.Info($"PersonnelViewModel: Successfully fired {pilot.Name}");

                LoadPilots();
                FireCompleted?.Invoke(this, new PilotActionEventArgs(pilot) { Success = true });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Cannot fire the player pilot"))
            {
                _logger.Warn("PersonnelViewModel: Attempt to fire player pilot blocked by repository");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs("You cannot fire yourself!"));
            }
            catch (Exception ex)
            {
                _logger.Error($"PersonnelViewModel: Error firing pilot: {ex.Message}");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs($"Failed to fire pilot: {ex.Message}"));
            }
        }

        public void SortEmployedPilots(int sortIndex)
        {
            _logger.Debug($"PersonnelViewModel: Sorting employed pilots by index {sortIndex}");
            var sorted = ApplySort(EmployedPilots.ToList(), sortIndex);
            ApplySortToCollection(EmployedPilots, sorted);
        }

        public void SortAvailablePilots(int sortIndex)
        {
            _logger.Debug($"PersonnelViewModel: Sorting available pilots by index {sortIndex}");
            var sorted = ApplySort(AvailablePilots.ToList(), sortIndex);
            ApplySortToCollection(AvailablePilots, sorted);
        }

        private List<PilotViewModel> ApplySort(List<PilotViewModel> pilots, int sortIndex)
        {
            return sortIndex switch
            {
                0 => pilots.OrderBy(p => p.Name).ToList(),
                1 => pilots.OrderByDescending(p => p.Name).ToList(),
                2 => pilots.OrderBy(p => p.Age).ToList(),
                3 => pilots.OrderByDescending(p => p.Age).ToList(),
                4 => pilots.OrderBy(p => p.SalaryPerMonth).ToList(),
                5 => pilots.OrderByDescending(p => p.SalaryPerMonth).ToList(),
                6 => pilots.OrderBy(p => p.TotalFlightHours).ToList(),
                7 => pilots.OrderByDescending(p => p.TotalFlightHours).ToList(),
                _ => pilots
            };
        }

        private bool CanEditPilot(PilotViewModel? pilot)
        {
            return pilot != null;
        }

        private void OnEditPilot(PilotViewModel? pilot)
        {
            if (pilot == null) return;

            _logger.Info($"PersonnelViewModel: Edit attempt for {pilot.Name}");

            var eventArgs = new PilotActionEventArgs(pilot);
            EditRequested?.Invoke(this, eventArgs);

            if (eventArgs.Confirmed)
            {
                ExecuteEdit(pilot);
            }
            else
            {
                _logger.Info("PersonnelViewModel: Edit cancelled by user");
            }
        }

        public void ExecuteEdit(PilotViewModel pilot)
        {
            try
            {
                var pilotModel = _pilotRepository.GetPilotById(pilot.Id);
                if (pilotModel == null)
                {
                    _logger.Error($"PersonnelViewModel: Pilot {pilot.Id} not found");
                    ErrorOccurred?.Invoke(this, new ErrorEventArgs("Pilot not found"));
                    return;
                }

                _pilotRepository.UpdatePilot(pilotModel);
                _logger.Info($"PersonnelViewModel: Successfully edited {pilot.Name}");

                LoadPilots();
                EditCompleted?.Invoke(this, new PilotActionEventArgs(pilot) { Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error($"PersonnelViewModel: Error editing pilot: {ex.Message}");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs($"Failed to edit pilot: {ex.Message}"));
            }
        }

        private bool CanDeletePilot(PilotViewModel? pilot)
        {
            return pilot != null && !pilot.IsPlayer;
        }

        private void OnDeletePilot(PilotViewModel? pilot)
        {
            if (pilot == null) return;

            if (pilot.IsPlayer)
            {
                _logger.Warn("PersonnelViewModel: Attempt to delete player pilot blocked");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs("You cannot delete yourself!"));
                return;
            }

            _logger.Info($"PersonnelViewModel: Delete attempt for {pilot.Name}");

            var eventArgs = new PilotActionEventArgs(pilot);
            DeleteRequested?.Invoke(this, eventArgs);

            if (eventArgs.Confirmed)
            {
                ExecuteDelete(pilot);
            }
            else
            {
                _logger.Info("PersonnelViewModel: Delete cancelled by user");
            }
        }

        public void ExecuteDelete(PilotViewModel pilot)
        {
            try
            {
                _pilotRepository.DeletePilot(pilot.Id);
                _logger.Info($"PersonnelViewModel: Successfully deleted {pilot.Name}");

                LoadPilots();
                DeleteCompleted?.Invoke(this, new PilotActionEventArgs(pilot) { Success = true });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Cannot delete the player pilot"))
            {
                _logger.Warn("PersonnelViewModel: Attempt to delete player pilot blocked by repository");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs("You cannot delete yourself!"));
            }
            catch (Exception ex)
            {
                _logger.Error($"PersonnelViewModel: Error deleting pilot: {ex.Message}");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs($"Failed to delete pilot: {ex.Message}"));
            }
        }

        private void OnCreatePilot()
        {
            _logger.Info("PersonnelViewModel: Create new pilot attempt");

            CreateRequested?.Invoke(this, EventArgs.Empty);
        }

        public void ExecuteCreate(Pilot newPilot)
        {
            try
            {
                _pilotRepository.CreatePilot(newPilot);
                _logger.Info($"PersonnelViewModel: Successfully created {newPilot.Name}");

                LoadPilots();
                var pilotVm = new PilotViewModel(newPilot, _logger, _settingsService);
                CreateCompleted?.Invoke(this, new PilotActionEventArgs(pilotVm) { Success = true });
            }
            catch (Exception ex)
            {
                _logger.Error($"PersonnelViewModel: Error creating pilot: {ex.Message}");
                ErrorOccurred?.Invoke(this, new ErrorEventArgs($"Failed to create pilot: {ex.Message}"));
            }
        }
    }


    public class PilotActionEventArgs : EventArgs
    {
        public PilotViewModel Pilot { get; }
        public bool Confirmed { get; set; }
        public bool Success { get; set; }

        public PilotActionEventArgs(PilotViewModel pilot)
        {
            Pilot = pilot;
        }
    }


    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; }

        public ErrorEventArgs(string message)
        {
            Message = message;
        }
    }
}
