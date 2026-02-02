using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Ace.App.Interfaces;
using Ace.App.Models;
using AircraftModel = Ace.App.Models.Aircraft;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class AircraftViewModel
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly AircraftModel _aircraft;
        private readonly List<Pilot> _assignedPilots;
        private readonly FBO? _assignedFBO;
        private readonly MaintenanceCheckStatus? _urgentCheck;
        private readonly string? _assignedRouteDisplay;
        private readonly int _requiredCrewCount;
        private readonly double _hoursPerDay;
        private readonly bool _multiCrewShiftsEnabled;

        public AircraftViewModel(AircraftModel aircraft, IMaintenanceService maintenanceService, List<Pilot>? assignedPilots = null, FBO? assignedFBO = null, string? assignedRouteDisplay = null, int requiredCrewCount = 1, double hoursPerDay = 8, bool multiCrewShiftsEnabled = false)
        {
            _aircraft = aircraft;
            _maintenanceService = maintenanceService ?? throw new System.ArgumentNullException(nameof(maintenanceService));
            _assignedPilots = assignedPilots ?? new List<Pilot>();
            _assignedFBO = assignedFBO;
            _urgentCheck = _maintenanceService.GetMostUrgentCheck(aircraft);
            _assignedRouteDisplay = assignedRouteDisplay;
            _requiredCrewCount = requiredCrewCount < 1 ? 1 : requiredCrewCount;
            _hoursPerDay = hoursPerDay;
            _multiCrewShiftsEnabled = multiCrewShiftsEnabled;
        }

        public int Id => _aircraft.Id;
        public string Registration => _aircraft.Registration;
        public string? CustomImagePath => _aircraft.CustomImagePath;
        public string TypeVariant => $"{_aircraft.Type} - {_aircraft.Variant}";
        public string DisplayName => $"{_aircraft.Registration} ({_aircraft.Type})";
        public string HomeBase => _aircraft.HomeBase;
        public string FlightHoursText => $"{_aircraft.TotalFlightHours:F1} h";
        public string RangeText => $"{_aircraft.MaxRangeNM:F0} NM";
        public string ValueText => $"{_aircraft.CurrentValue:N0} €";
        public string PassengersText => $"{_aircraft.MaxPassengers}";
        public string SizeCategory => AircraftSizeExtensions.GetAircraftSize(_aircraft.MaxPassengers).GetSizeName();
        public string Status => _aircraft.Status == AircraftStatus.Stationed
            ? $"Stationed - {_aircraft.HomeBase}"
            : _aircraft.Status.ToString();

        public bool HasAssignedPilot => _assignedPilots.Count > 0;
        public string AssignedPilotName => _assignedPilots.FirstOrDefault()?.Name ?? string.Empty;
        public List<Pilot> AssignedPilots => _assignedPilots;
        public int AssignedPilotCount => _assignedPilots.Count;
        public int RequiredCrewCount => _requiredCrewCount;
        public string CrewStatusText => $"{AssignedPilotCount}/{RequiredCrewCount}";
        public bool IsCrewComplete => AssignedPilotCount >= RequiredCrewCount;
        public string AssignedPilotNames => string.Join(", ", _assignedPilots.Select(p => p.Name));

        public bool HasAssignedFBO => _assignedFBO != null;
        public string AssignedFBOName => _assignedFBO != null ? $"{_assignedFBO.ICAO} - {_assignedFBO.AirportName}" : string.Empty;
        public bool HasAssignedRoute => !string.IsNullOrEmpty(_assignedRouteDisplay);
        public string AssignedRouteDisplay => _assignedRouteDisplay ?? string.Empty;

        public bool ShowMaintenanceWarning => _urgentCheck != null && _urgentCheck.UrgencyScore <= 1 && _aircraft.Status != AircraftStatus.Maintenance;
        public bool IsMaintenanceOverdue => _urgentCheck?.IsOverdue == true && _aircraft.Status != AircraftStatus.Maintenance;
        public string MaintenanceWarningText => _urgentCheck != null ? GetMaintenanceWarningText() : string.Empty;

        private string GetMaintenanceWarningText()
        {
            if (_urgentCheck == null) return string.Empty;
            if (_urgentCheck.IsOverdue) return $"⚠ {_urgentCheck.Name} OVERDUE";
            if (_urgentCheck.UrgencyScore == 1) return $"🔧 {_urgentCheck.Name} due soon";
            return string.Empty;
        }

        public Brush MaintenanceWarningColor
        {
            get
            {
                if (_urgentCheck == null) return Brushes.Transparent;
                if (_urgentCheck.IsOverdue) return new SolidColorBrush(Color.FromRgb(239, 68, 68));
                if (_urgentCheck.UrgencyScore == 1) return new SolidColorBrush(Color.FromRgb(251, 146, 60));
                return Brushes.Transparent;
            }
        }

        public Brush StatusColor
        {
            get
            {
                return _aircraft.Status switch
                {
                    AircraftStatus.Available => new SolidColorBrush(Color.FromRgb(34, 197, 94)),
                    AircraftStatus.InFlight => new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                    AircraftStatus.Maintenance => new SolidColorBrush(Color.FromRgb(251, 146, 60)),
                    AircraftStatus.Grounded => new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                    AircraftStatus.Stationed => new SolidColorBrush(Color.FromRgb(168, 85, 247)),
                    _ => new SolidColorBrush(Color.FromRgb(156, 163, 175))
                };
            }
        }

        public double EffectiveFlightHours
        {
            get
            {
                if (_assignedPilots.Count == 0) return 0;
                if (!_multiCrewShiftsEnabled) return _hoursPerDay;
                var hours = ((double)_assignedPilots.Count / _requiredCrewCount) * _hoursPerDay;
                return System.Math.Min(hours, 24.0);
            }
        }

        public int UtilizationPercent => _assignedPilots.Count == 0
            ? 0
            : (int)System.Math.Round(EffectiveFlightHours / 24.0 * 100);

        public string UtilizationText => $"{UtilizationPercent}%";

        public bool ShowUtilization => _assignedPilots.Count > 0;

        public Brush UtilizationColor
        {
            get
            {
                if (UtilizationPercent >= 90) return new SolidColorBrush(Color.FromRgb(34, 197, 94));
                if (UtilizationPercent >= 50) return new SolidColorBrush(Color.FromRgb(59, 130, 246));
                return new SolidColorBrush(Color.FromRgb(251, 146, 60));
            }
        }

        public Brush CrewStatusColor
        {
            get
            {
                if (AssignedPilotCount == 0) return new SolidColorBrush(Color.FromRgb(156, 163, 175));
                if (IsCrewComplete) return new SolidColorBrush(Color.FromRgb(34, 197, 94));
                return new SolidColorBrush(Color.FromRgb(251, 146, 60));
            }
        }
    }
}
