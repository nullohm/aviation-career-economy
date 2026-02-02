using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Ace.App.Commands;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Helpers;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class PilotDetailViewModel : ViewModelBase
    {
        private readonly IPilotRepository _pilotRepository;
        private readonly IFinanceService _financeService;
        private readonly ISettingsService _settingsService;
        private readonly ILoggingService _logger;
        private readonly int _pilotId;

        public ObservableCollection<PilotLicense> Licenses { get; } = new();
        public ObservableCollection<TypeRating> TypeRatings { get; } = new();
        public ObservableCollection<AvailableLicenseItem> AvailableLicenses { get; } = new();
        public ObservableCollection<AvailableTypeRatingItem> AvailableTypeRatings { get; } = new();
        public ObservableCollection<AvailableAircraftItem> AvailableAircraft { get; } = new();

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _age;
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        private double _totalFlightHours;
        public double TotalFlightHours
        {
            get => _totalFlightHours;
            set => SetProperty(ref _totalFlightHours, value);
        }

        private decimal _salaryPerMonth;
        public decimal SalaryPerMonth
        {
            get => _salaryPerMonth;
            set => SetProperty(ref _salaryPerMonth, value);
        }

        private bool _isPlayer;
        public bool IsPlayer
        {
            get => _isPlayer;
            set => SetProperty(ref _isPlayer, value);
        }

        private string _pilotImage = string.Empty;
        public string PilotImage
        {
            get => _pilotImage;
            set => SetProperty(ref _pilotImage, value);
        }

        private bool _canPurchaseTraining;
        public bool CanPurchaseTraining
        {
            get => _canPurchaseTraining;
            set => SetProperty(ref _canPurchaseTraining, value);
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        private AvailableLicenseItem? _selectedLicense;
        public AvailableLicenseItem? SelectedLicense
        {
            get => _selectedLicense;
            set => SetProperty(ref _selectedLicense, value);
        }

        private AvailableTypeRatingItem? _selectedTypeRating;
        public AvailableTypeRatingItem? SelectedTypeRating
        {
            get => _selectedTypeRating;
            set => SetProperty(ref _selectedTypeRating, value);
        }

        private AvailableAircraftItem? _selectedAircraft;
        public AvailableAircraftItem? SelectedAircraft
        {
            get => _selectedAircraft;
            set => SetProperty(ref _selectedAircraft, value);
        }

        private bool _canAssignAircraft;
        public bool CanAssignAircraft
        {
            get => _canAssignAircraft;
            set => SetProperty(ref _canAssignAircraft, value);
        }

        private bool _hasAssignedAircraft;
        public bool HasAssignedAircraft
        {
            get => _hasAssignedAircraft;
            set => SetProperty(ref _hasAssignedAircraft, value);
        }

        private bool _canSelectAircraft;
        public bool CanSelectAircraft
        {
            get => _canSelectAircraft;
            set => SetProperty(ref _canSelectAircraft, value);
        }

        private bool _noAircraftAvailable;
        public bool NoAircraftAvailable
        {
            get => _noAircraftAvailable;
            set => SetProperty(ref _noAircraftAvailable, value);
        }

        private string _assignedAircraftRegistration = string.Empty;
        public string AssignedAircraftRegistration
        {
            get => _assignedAircraftRegistration;
            set => SetProperty(ref _assignedAircraftRegistration, value);
        }

        private string _assignedAircraftType = string.Empty;
        public string AssignedAircraftType
        {
            get => _assignedAircraftType;
            set => SetProperty(ref _assignedAircraftType, value);
        }

        public ICommand PurchaseLicenseCommand { get; }
        public ICommand PurchaseTypeRatingCommand { get; }
        public ICommand AssignAircraftCommand { get; }
        public ICommand UnassignAircraftCommand { get; }

        public event EventHandler<string>? PurchaseCompleted;
        public event EventHandler<string>? ErrorOccurred;

        public PilotDetailViewModel(int pilotId, IPilotRepository pilotRepository, IFinanceService financeService, ISettingsService settingsService, ILoggingService logger)
        {
            _pilotId = pilotId;
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            PurchaseLicenseCommand = new RelayCommand(OnPurchaseLicense, CanPurchaseLicense);
            PurchaseTypeRatingCommand = new RelayCommand(OnPurchaseTypeRating, CanPurchaseTypeRating);
            AssignAircraftCommand = new RelayCommand(OnAssignAircraft, CanAssignAircraftExecute);
            UnassignAircraftCommand = new RelayCommand(OnUnassignAircraft);

            LoadPilotData();
            LoadAvailableTraining();
            LoadAircraftAssignment();
        }

        private void LoadPilotData()
        {
            _logger.Info($"PilotDetailViewModel: Loading pilot data for ID {_pilotId}");

            try
            {
                using var db = new AceDbContext();
                var pilot = db.Pilots.Find(_pilotId);

                if (pilot == null)
                {
                    _logger.Error($"PilotDetailViewModel: Pilot with ID {_pilotId} not found");
                    ErrorOccurred?.Invoke(this, "Pilot not found");
                    return;
                }

                Name = pilot.Name;
                Age = DateTime.Today.Year - pilot.Birthday.Year;
                TotalFlightHours = pilot.TotalFlightHours;
                SalaryPerMonth = pilot.SalaryPerMonth;
                IsPlayer = pilot.IsPlayer;
                PilotImage = pilot.ImagePath;
                CanPurchaseTraining = pilot.IsEmployed;

                Licenses.Clear();
                var licenses = db.Licenses.Where(l => l.PilotId == _pilotId).ToList();
                foreach (var license in licenses)
                {
                    Licenses.Add(license);
                }

                TypeRatings.Clear();
                var ratings = db.TypeRatings.Where(tr => tr.PilotId == _pilotId).ToList();
                foreach (var rating in ratings)
                {
                    TypeRatings.Add(rating);
                }

                Balance = _financeService.Balance;

                _logger.Info($"PilotDetailViewModel: Loaded {Licenses.Count} licenses and {TypeRatings.Count} type ratings");
            }
            catch (Exception ex)
            {
                _logger.Error($"PilotDetailViewModel: Error loading pilot data: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error loading pilot data: {ex.Message}");
            }
        }

        private void LoadAvailableTraining()
        {
            AvailableLicenses.Clear();
            AvailableLicenses.Add(new AvailableLicenseItem { Name = "Instrument Rating (IR)", Price = 15000m });
            AvailableLicenses.Add(new AvailableLicenseItem { Name = "Multi-Engine Rating (ME)", Price = 20000m });
            AvailableLicenses.Add(new AvailableLicenseItem { Name = "Commercial Pilot License (CPL)", Price = 35000m });
            AvailableLicenses.Add(new AvailableLicenseItem { Name = "Airline Transport Pilot License (ATPL)", Price = 50000m });
            AvailableLicenses.Add(new AvailableLicenseItem { Name = "Flight Instructor Rating (FI)", Price = 25000m });

            AvailableTypeRatings.Clear();

            using var db = new AceDbContext();
            var catalogAircraft = db.AircraftCatalog.OrderBy(a => a.Manufacturer).ThenBy(a => a.Title).ToList();

            var assignedAircraftIds = db.AircraftPilotAssignments
                .Select(a => a.AircraftId)
                .Distinct()
                .ToList();
            var unassignedAircraftTypes = db.Aircraft
                .Where(a => !assignedAircraftIds.Contains(a.Id))
                .Select(a => a.Type)
                .Distinct()
                .ToList();

            var settings = _settingsService.CurrentSettings;

            foreach (var aircraft in catalogAircraft)
            {
                var aircraftSize = AircraftSizeExtensions.GetAircraftSize(aircraft.PassengerCapacity);
                var trainingPrice = aircraftSize switch
                {
                    AircraftSize.Small => settings.TypeRatingCostSmall,
                    AircraftSize.Medium => settings.TypeRatingCostMedium,
                    AircraftSize.MediumLarge => settings.TypeRatingCostMediumLarge,
                    AircraftSize.Large => settings.TypeRatingCostLarge,
                    AircraftSize.VeryLarge => settings.TypeRatingCostVeryLarge,
                    _ => settings.TypeRatingCostMedium
                };

                var aircraftType = aircraft.Title;
                var parts = aircraftType.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 3 && parts[1].Length <= 3)
                {
                    aircraftType = $"{parts[0]} {parts[1]}-{parts[2]}";
                }
                else if (parts.Length >= 2)
                {
                    aircraftType = $"{parts[0]} {parts[1]}";
                }

                var hasUnassignedAircraft = unassignedAircraftTypes.Any(type =>
                    type.Equals(aircraftType, StringComparison.OrdinalIgnoreCase) ||
                    type.Contains(aircraftType, StringComparison.OrdinalIgnoreCase) ||
                    aircraftType.Contains(type, StringComparison.OrdinalIgnoreCase));

                AvailableTypeRatings.Add(new AvailableTypeRatingItem
                {
                    AircraftType = aircraftType,
                    Price = trainingPrice,
                    HasUnassignedAircraft = hasUnassignedAircraft
                });
            }

            _logger.Info($"PilotDetailViewModel: Loaded {AvailableLicenses.Count} licenses and {AvailableTypeRatings.Count} type ratings for purchase");
        }

        private bool CanPurchaseLicense()
        {
            return CanPurchaseTraining && SelectedLicense != null && Balance >= SelectedLicense.Price;
        }

        private void OnPurchaseLicense()
        {
            if (SelectedLicense == null) return;

            try
            {
                _logger.Info($"PilotDetailViewModel: Purchasing license '{SelectedLicense.Name}' for {SelectedLicense.Price:N0} €");

                if (Balance < SelectedLicense.Price)
                {
                    ErrorOccurred?.Invoke(this, "Insufficient funds for license training");
                    return;
                }

                _financeService.AddExpense(SelectedLicense.Price, $"License training: {SelectedLicense.Name}");

                using var db = new AceDbContext();
                var newLicense = new PilotLicense
                {
                    PilotId = _pilotId,
                    Name = SelectedLicense.Name,
                    IssuedDate = DateTime.Today,
                    IssuingAuthority = "Aviation Career Economy Flight Academy"
                };

                db.Licenses.Add(newLicense);
                db.SaveChanges();

                _logger.Info($"PilotDetailViewModel: Successfully purchased license '{SelectedLicense.Name}'");

                LoadPilotData();
                PurchaseCompleted?.Invoke(this, $"Successfully purchased {SelectedLicense.Name} for {SelectedLicense.Price:N0} €");
            }
            catch (Exception ex)
            {
                _logger.Error($"PilotDetailViewModel: Error purchasing license: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error purchasing license: {ex.Message}");
            }
        }

        private bool CanPurchaseTypeRating()
        {
            return CanPurchaseTraining && SelectedTypeRating != null && Balance >= SelectedTypeRating.Price;
        }

        private void OnPurchaseTypeRating()
        {
            if (SelectedTypeRating == null) return;

            try
            {
                _logger.Info($"PilotDetailViewModel: Purchasing type rating '{SelectedTypeRating.AircraftType}' for {SelectedTypeRating.Price:N0} €");

                if (Balance < SelectedTypeRating.Price)
                {
                    ErrorOccurred?.Invoke(this, "Insufficient funds for type rating");
                    return;
                }

                _financeService.AddExpense(SelectedTypeRating.Price, $"Type rating: {SelectedTypeRating.AircraftType}");

                using var db = new AceDbContext();
                var newRating = new TypeRating
                {
                    PilotId = _pilotId,
                    AircraftType = SelectedTypeRating.AircraftType,
                    EarnedDate = DateTime.Today,
                    IssuingAuthority = "Aviation Career Economy Flight Academy"
                };

                db.TypeRatings.Add(newRating);
                db.SaveChanges();

                _logger.Info($"PilotDetailViewModel: Successfully purchased type rating '{SelectedTypeRating.AircraftType}'");

                LoadPilotData();
                LoadAircraftAssignment();
                PurchaseCompleted?.Invoke(this, $"Successfully purchased type rating for {SelectedTypeRating.AircraftType} ({SelectedTypeRating.Price:N0} €)");
            }
            catch (Exception ex)
            {
                _logger.Error($"PilotDetailViewModel: Error purchasing type rating: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error purchasing type rating: {ex.Message}");
            }
        }

        private void LoadAircraftAssignment()
        {
            _logger.Info($"PilotDetailViewModel: Loading aircraft assignment for pilot ID {_pilotId}");

            try
            {
                using var db = new AceDbContext();
                var pilot = db.Pilots.Find(_pilotId);

                if (pilot == null || !pilot.IsEmployed)
                {
                    CanAssignAircraft = false;
                    return;
                }

                CanAssignAircraft = true;

                var assignment = db.AircraftPilotAssignments.FirstOrDefault(a => a.PilotId == _pilotId);
                var assignedAircraft = assignment != null ? db.Aircraft.FirstOrDefault(a => a.Id == assignment.AircraftId) : null;
                if (assignedAircraft != null)
                {
                    HasAssignedAircraft = true;
                    CanSelectAircraft = false;
                    NoAircraftAvailable = false;
                    AssignedAircraftRegistration = assignedAircraft.Registration;
                    AssignedAircraftType = $"{assignedAircraft.Type} {assignedAircraft.Variant}".Trim();
                    _logger.Info($"PilotDetailViewModel: Pilot is assigned to aircraft {assignedAircraft.Registration}");
                }
                else
                {
                    HasAssignedAircraft = false;
                    LoadAvailableAircraft(db);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"PilotDetailViewModel: Error loading aircraft assignment: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error loading aircraft assignment: {ex.Message}");
            }
        }

        private void LoadAvailableAircraft(AceDbContext db)
        {
            AvailableAircraft.Clear();

            var pilotTypeRatings = db.TypeRatings
                .Where(tr => tr.PilotId == _pilotId)
                .Select(tr => tr.AircraftType)
                .ToList();

            _logger.Info($"PilotDetailViewModel: Pilot has {pilotTypeRatings.Count} type ratings");

            var assignedAircraftIds = db.AircraftPilotAssignments
                .Select(a => a.AircraftId)
                .Distinct()
                .ToList();
            var aircraft = db.Aircraft
                .Where(a => a.AssignedFBOId != null &&
                           !assignedAircraftIds.Contains(a.Id) &&
                           a.Status == AircraftStatus.Stationed)
                .ToList();

            var fbos = db.FBOs.ToList();

            foreach (var ac in aircraft)
            {
                var aircraftFullType = $"{ac.Type} {ac.Variant}".Trim();

                if (!TypeRatingMatchHelper.HasMatchingTypeRating(ac.Type, pilotTypeRatings))
                {
                    _logger.Debug($"PilotDetailViewModel: Skipping aircraft {ac.Registration} ({aircraftFullType}) - no type rating");
                    continue;
                }

                _logger.Debug($"PilotDetailViewModel: Aircraft {ac.Registration} ({aircraftFullType}) matches type rating!");

                var fbo = fbos.FirstOrDefault(f => f.Id == ac.AssignedFBOId);
                AvailableAircraft.Add(new AvailableAircraftItem
                {
                    Id = ac.Id,
                    Registration = ac.Registration,
                    Type = aircraftFullType,
                    FboIcao = fbo?.ICAO ?? "Unknown"
                });
            }

            CanSelectAircraft = AvailableAircraft.Count > 0;
            NoAircraftAvailable = AvailableAircraft.Count == 0;

            _logger.Info($"PilotDetailViewModel: Found {AvailableAircraft.Count} available aircraft for assignment (filtered by type rating)");
        }

        private bool CanAssignAircraftExecute()
        {
            return CanAssignAircraft && SelectedAircraft != null;
        }

        private void OnAssignAircraft()
        {
            if (SelectedAircraft == null) return;

            try
            {
                _logger.Info($"PilotDetailViewModel: Assigning pilot {_pilotId} to aircraft {SelectedAircraft.Registration}");

                using var db = new AceDbContext();
                var aircraft = db.Aircraft.Find(SelectedAircraft.Id);

                if (aircraft == null)
                {
                    ErrorOccurred?.Invoke(this, "Aircraft not found");
                    return;
                }

                var existingAssignment = db.AircraftPilotAssignments.FirstOrDefault(a => a.PilotId == _pilotId);
                if (existingAssignment != null)
                {
                    ErrorOccurred?.Invoke(this, "Pilot is already assigned to an aircraft");
                    LoadAircraftAssignment();
                    return;
                }

                db.AircraftPilotAssignments.Add(new AircraftPilotAssignment
                {
                    AircraftId = aircraft.Id,
                    PilotId = _pilotId,
                    AssignedDate = DateTime.Today
                });
                db.SaveChanges();

                _logger.Info($"PilotDetailViewModel: Successfully assigned pilot to aircraft {SelectedAircraft.Registration}");
                LoadAircraftAssignment();
            }
            catch (Exception ex)
            {
                _logger.Error($"PilotDetailViewModel: Error assigning aircraft: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error assigning aircraft: {ex.Message}");
            }
        }

        private void OnUnassignAircraft()
        {
            try
            {
                _logger.Info($"PilotDetailViewModel: Unassigning pilot {_pilotId} from aircraft");

                using var db = new AceDbContext();
                var pilotAssignment = db.AircraftPilotAssignments.FirstOrDefault(a => a.PilotId == _pilotId);

                if (pilotAssignment == null)
                {
                    ErrorOccurred?.Invoke(this, "No aircraft assigned to this pilot");
                    LoadAircraftAssignment();
                    return;
                }

                var aircraft = db.Aircraft.FirstOrDefault(a => a.Id == pilotAssignment.AircraftId);
                var registration = aircraft?.Registration ?? "Unknown";
                db.AircraftPilotAssignments.Remove(pilotAssignment);
                db.SaveChanges();

                _logger.Info($"PilotDetailViewModel: Successfully unassigned pilot from aircraft {registration}");
                LoadAircraftAssignment();
            }
            catch (Exception ex)
            {
                _logger.Error($"PilotDetailViewModel: Error unassigning aircraft: {ex.Message}");
                ErrorOccurred?.Invoke(this, $"Error unassigning aircraft: {ex.Message}");
            }
        }
    }

    public class AvailableLicenseItem
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class AvailableTypeRatingItem
    {
        public string AircraftType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool HasUnassignedAircraft { get; set; }
    }

    public class AvailableAircraftItem
    {
        public int Id { get; set; }
        public string Registration { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string FboIcao { get; set; } = string.Empty;
    }
}
