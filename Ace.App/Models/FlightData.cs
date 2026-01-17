using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OxyPlot;

namespace Ace.App.Models
{
    public class AltitudePoint
    {
        public double DistanceNM { get; set; }
        public double AltitudeFt { get; set; }
        public double GroundSpeedKts { get; set; }
        public double VerticalSpeedFpm { get; set; }
    }
    public enum FlightPhase
    {
        None,
        Ready,
        Taxi,
        Climbing,
        Cruise,
        Descending,
        Landed,
        AtDestination,
        Completed
    }

    public class FlightData : INotifyPropertyChanged
    {
        private bool _isConnected;
        private bool _onGround;
        private string _aircraft = "---";
        private double _latitude;
        private double _longitude;
        private double _altitude;
        private double _groundSpeed;
        private double _heading;
        private double _verticalSpeed;
        private bool _parkingBrake;
        private double _simulationRate = 1.0;
        private double _fuelQuantityGallons;
        private double _fuelCapacityGallons;
        private double _fuelFlowGallonsPerHour;
        private double _angleOfAttack;
        private bool _stallWarning;
        private bool _hasActiveFlightPlan;
        private FlightPhase _flightPhase = FlightPhase.None;
        private string _currentAirport = string.Empty;
        private string _destinationAirport = string.Empty;
        private decimal _balance;
        private int _fbosWithoutAircraft;
        private int _aircraftWithoutFBO;
        private int _pilotsWithoutAircraft;
        private double _flightProgress;
        private double _distanceRemaining;
        private string _departureIcao = string.Empty;
        private string _arrivalIcao = string.Empty;
        private PlotModel? _altitudeProfileModel;
        private List<AltitudePoint> _altitudeTrack = new();
        private double _totalDistanceNM;
        private double _cruiseAltitudeFt;
        private string _flightPlanAircraftRegistration = string.Empty;
        private string _flightPlanAircraftType = string.Empty;
        private int _flightPlanPassengers;
        private double _flightPlanCargoKg;
        private double _courseToDest;

        public PlotModel? AltitudeProfileModel
        {
            get => _altitudeProfileModel;
            set
            {
                _altitudeProfileModel = value;
                OnPropertyChanged();
            }
        }

        public List<AltitudePoint> AltitudeTrack
        {
            get => _altitudeTrack;
            set
            {
                _altitudeTrack = value;
                OnPropertyChanged();
            }
        }

        public double TotalDistanceNM
        {
            get => _totalDistanceNM;
            set
            {
                _totalDistanceNM = value;
                OnPropertyChanged();
            }
        }

        public double CruiseAltitudeFt
        {
            get => _cruiseAltitudeFt;
            set
            {
                _cruiseAltitudeFt = value;
                OnPropertyChanged();
            }
        }

        public string FlightPlanAircraftRegistration
        {
            get => _flightPlanAircraftRegistration;
            set
            {
                _flightPlanAircraftRegistration = value;
                OnPropertyChanged();
            }
        }

        public string FlightPlanAircraftType
        {
            get => _flightPlanAircraftType;
            set
            {
                _flightPlanAircraftType = value;
                OnPropertyChanged();
            }
        }

        public int FlightPlanPassengers
        {
            get => _flightPlanPassengers;
            set
            {
                _flightPlanPassengers = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightPlanPassengersText));
            }
        }

        public double FlightPlanCargoKg
        {
            get => _flightPlanCargoKg;
            set
            {
                _flightPlanCargoKg = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightPlanCargoText));
            }
        }

        public double CourseToDest
        {
            get => _courseToDest;
            set
            {
                _courseToDest = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CourseToDestText));
            }
        }

        private double _aircraftCruiseSpeedKts;
        private double _aircraftServiceCeilingFt;
        private double _destinationElevationFt;

        public double AircraftCruiseSpeedKts
        {
            get => _aircraftCruiseSpeedKts;
            set { _aircraftCruiseSpeedKts = value; OnPropertyChanged(); OnPropertyChanged(nameof(AircraftCruiseSpeedText)); }
        }

        public double AircraftServiceCeilingFt
        {
            get => _aircraftServiceCeilingFt;
            set { _aircraftServiceCeilingFt = value; OnPropertyChanged(); OnPropertyChanged(nameof(AircraftServiceCeilingText)); }
        }

        public double DestinationElevationFt
        {
            get => _destinationElevationFt;
            set
            {
                _destinationElevationFt = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TopOfDescentDistanceNM));
                OnPropertyChanged(nameof(TopOfDescentDistanceText));
                OnPropertyChanged(nameof(ShouldStartDescent));
            }
        }

        public string FlightPlanPassengersText => FlightPlanPassengers > 0 ? $"{FlightPlanPassengers} PAX" : "---";
        public string FlightPlanCargoText => FlightPlanCargoKg > 0 ? $"{FlightPlanCargoKg:F0} kg" : "---";
        public string CourseToDestText => CourseToDest > 0 ? $"{CourseToDest:F0}°" : "---";
        public string AircraftCruiseSpeedText => AircraftCruiseSpeedKts > 0 ? $"{AircraftCruiseSpeedKts:F0} kts" : "---";
        public string AircraftServiceCeilingText => AircraftServiceCeilingFt > 0 ? $"{AircraftServiceCeilingFt:N0} ft" : "---";

        public double TopOfDescentDistanceNM
        {
            get
            {
                if (Altitude <= DestinationElevationFt + 1000) return 0;
                var altitudeToLose = Altitude - DestinationElevationFt - 1000;
                return altitudeToLose / 318.0;
            }
        }

        public string TopOfDescentDistanceText
        {
            get
            {
                var tod = TopOfDescentDistanceNM;
                if (tod <= 0) return "---";
                return $"{tod:F1} NM";
            }
        }

        public bool ShouldStartDescent => HasActiveFlightPlan && DistanceRemaining > 0 && DistanceRemaining <= TopOfDescentDistanceNM + 2;

        public double RecommendedDescentRateFpm
        {
            get
            {
                if (GroundSpeed < 50) return 0;
                return GroundSpeed * 5.3;
            }
        }

        public string RecommendedDescentRateText
        {
            get
            {
                var rate = RecommendedDescentRateFpm;
                if (rate <= 0) return "---";
                return $"-{rate:F0} fpm";
            }
        }

        public double FuelAtDestinationGal
        {
            get
            {
                if (FuelFlowGallonsPerHour <= 0 || GroundSpeed < 30 || DistanceRemaining <= 0) return FuelQuantityGallons;
                var hoursRemaining = DistanceRemaining / GroundSpeed;
                var fuelNeeded = hoursRemaining * FuelFlowGallonsPerHour;
                return Math.Max(0, FuelQuantityGallons - fuelNeeded);
            }
        }

        public string FuelAtDestinationText
        {
            get
            {
                if (!HasActiveFlightPlan || DistanceRemaining <= 0) return "---";
                var fuelAtDest = FuelAtDestinationGal;
                if (FuelCapacityGallons > 0)
                {
                    var percent = (fuelAtDest / FuelCapacityGallons) * 100;
                    return $"{fuelAtDest:F0} gal ({percent:F0}%)";
                }
                return $"{fuelAtDest:F0} gal";
            }
        }

        public bool FuelAtDestinationLow => HasActiveFlightPlan && FuelCapacityGallons > 0 && (FuelAtDestinationGal / FuelCapacityGallons) < 0.15;

        public string Aircraft
        {
            get => _aircraft;
            set { _aircraft = value; OnPropertyChanged(); }
        }

        public double Latitude
        {
            get => _latitude;
            set { _latitude = value; OnPropertyChanged(); OnPropertyChanged(nameof(Position)); }
        }

        public double Longitude
        {
            get => _longitude;
            set { _longitude = value; OnPropertyChanged(); OnPropertyChanged(nameof(Position)); }
        }

        public double Altitude
        {
            get => _altitude;
            set
            {
                _altitude = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TopOfDescentDistanceNM));
                OnPropertyChanged(nameof(TopOfDescentDistanceText));
                OnPropertyChanged(nameof(ShouldStartDescent));
                OnPropertyChanged(nameof(RecommendedDescentRateFpm));
                OnPropertyChanged(nameof(RecommendedDescentRateText));
            }
        }

        public double GroundSpeed
        {
            get => _groundSpeed;
            set
            {
                _groundSpeed = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EstimatedTimeRemaining));
                OnPropertyChanged(nameof(RecommendedDescentRateFpm));
                OnPropertyChanged(nameof(RecommendedDescentRateText));
            }
        }

        public double Heading
        {
            get => _heading;
            set { _heading = value; OnPropertyChanged(); }
        }

        public double VerticalSpeed
        {
            get => _verticalSpeed;
            set { _verticalSpeed = value; OnPropertyChanged(); }
        }

        public bool ParkingBrake
        {
            get => _parkingBrake;
            set { _parkingBrake = value; OnPropertyChanged(); }
        }

        public double SimulationRate
        {
            get => _simulationRate;
            set { _simulationRate = value; OnPropertyChanged(); OnPropertyChanged(nameof(SimulationRateText)); }
        }

        public string SimulationRateText
        {
            get
            {
                if (SimulationRate <= 0) return "---";
                if (SimulationRate >= 1.0) return $"{SimulationRate:F0}x";
                return $"{SimulationRate:F1}x";
            }
        }

        public double FuelQuantityGallons
        {
            get => _fuelQuantityGallons;
            set
            {
                _fuelQuantityGallons = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FuelPercent));
                OnPropertyChanged(nameof(FuelPercentText));
                OnPropertyChanged(nameof(FuelLow));
            }
        }

        public double FuelCapacityGallons
        {
            get => _fuelCapacityGallons;
            set
            {
                _fuelCapacityGallons = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FuelPercent));
                OnPropertyChanged(nameof(FuelPercentText));
                OnPropertyChanged(nameof(FuelLow));
            }
        }

        public double FuelFlowGallonsPerHour
        {
            get => _fuelFlowGallonsPerHour;
            set
            {
                _fuelFlowGallonsPerHour = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FuelFlowText));
            }
        }

        public double FuelPercent => FuelCapacityGallons > 0 ? (FuelQuantityGallons / FuelCapacityGallons) * 100 : 0;
        public string FuelPercentText => FuelCapacityGallons > 0 ? $"{FuelPercent:F0}" : "---";
        public string FuelFlowText => FuelFlowGallonsPerHour > 0 ? $"{FuelFlowGallonsPerHour:F1}" : "---";
        public bool FuelLow => FuelCapacityGallons > 0 && FuelPercent < 10;

        public double AngleOfAttack
        {
            get => _angleOfAttack;
            set
            {
                _angleOfAttack = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AoAText));
                OnPropertyChanged(nameof(AoAWarning));
                OnPropertyChanged(nameof(AoACritical));
            }
        }

        public bool StallWarning
        {
            get => _stallWarning;
            set
            {
                _stallWarning = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AoACritical));
            }
        }

        public string AoAText => IsConnected ? $"{AngleOfAttack:F1}" : "---";
        public bool AoAWarning => AngleOfAttack > 12;
        public bool AoACritical => StallWarning || AngleOfAttack > 15;

        public bool OnGround
        {
            get => _onGround;
            set 
            { 
                _onGround = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(FlightStatusText)); 
            }
        }

        public DateTime Timestamp { get; set; }

        public bool IsConnected
        {
            get => _isConnected;
            set 
            { 
                _isConnected = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(ConnectionStatus)); 
                OnPropertyChanged(nameof(Position));
                OnPropertyChanged(nameof(FlightStatusText));
            }
        }

        public bool HasActiveFlightPlan
        {
            get => _hasActiveFlightPlan;
            set
            {
                _hasActiveFlightPlan = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightStatusText));
            }
        }

        public FlightPhase FlightPhase
        {
            get => _flightPhase;
            set
            {
                _flightPhase = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightStatusText));
            }
        }

        public string CurrentAirport
        {
            get => _currentAirport;
            set
            {
                _currentAirport = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightStatusText));
            }
        }

        public string DestinationAirport
        {
            get => _destinationAirport;
            set
            {
                _destinationAirport = value;
                OnPropertyChanged();
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        public int FBOsWithoutAircraft
        {
            get => _fbosWithoutAircraft;
            set
            {
                _fbosWithoutAircraft = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFBOsWithoutAircraft));
                OnPropertyChanged(nameof(AllOperationsOK));
            }
        }

        public int AircraftWithoutFBO
        {
            get => _aircraftWithoutFBO;
            set
            {
                _aircraftWithoutFBO = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAircraftWithoutFBO));
                OnPropertyChanged(nameof(AllOperationsOK));
            }
        }

        public int PilotsWithoutAircraft
        {
            get => _pilotsWithoutAircraft;
            set
            {
                _pilotsWithoutAircraft = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasPilotsWithoutAircraft));
                OnPropertyChanged(nameof(AllOperationsOK));
            }
        }

        public bool HasFBOsWithoutAircraft => FBOsWithoutAircraft > 0;
        public bool HasAircraftWithoutFBO => AircraftWithoutFBO > 0;
        public bool HasPilotsWithoutAircraft => PilotsWithoutAircraft > 0;
        public bool AllOperationsOK => FBOsWithoutAircraft == 0 && AircraftWithoutFBO == 0 && PilotsWithoutAircraft == 0;

        public double FlightProgress
        {
            get => _flightProgress;
            set
            {
                _flightProgress = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightProgressPercent));
            }
        }

        public double DistanceRemaining
        {
            get => _distanceRemaining;
            set
            {
                _distanceRemaining = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DistanceRemainingText));
                OnPropertyChanged(nameof(EstimatedTimeRemaining));
                OnPropertyChanged(nameof(ShouldStartDescent));
                OnPropertyChanged(nameof(FuelAtDestinationGal));
                OnPropertyChanged(nameof(FuelAtDestinationText));
                OnPropertyChanged(nameof(FuelAtDestinationLow));
            }
        }

        public string DepartureIcao
        {
            get => _departureIcao;
            set
            {
                _departureIcao = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightRouteText));
            }
        }

        public string ArrivalIcao
        {
            get => _arrivalIcao;
            set
            {
                _arrivalIcao = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlightRouteText));
            }
        }

        public string FlightProgressPercent => $"{FlightProgress:F0}%";
        public string DistanceRemainingText => DistanceRemaining > 0 ? $"{DistanceRemaining:F0} NM" : "---";
        public string FlightRouteText => HasActiveFlightPlan ? $"{DepartureIcao} → {ArrivalIcao}" : "---";
        public string EstimatedTimeRemaining
        {
            get
            {
                if (DistanceRemaining <= 0 || GroundSpeed < 30) return "---";
                var hoursRemaining = DistanceRemaining / GroundSpeed;
                var hours = (int)hoursRemaining;
                var minutes = (int)((hoursRemaining - hours) * 60);
                return hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
            }
        }

        public string ConnectionStatus => IsConnected ? "Connected" : "Disconnected";
        public string Position => IsConnected ? $"Lat {Latitude:F4}\nLon {Longitude:F4}" : "---";
        public string FlightStatusText
        {
            get
            {
                if (!IsConnected) return "---";
                if (!HasActiveFlightPlan) return "No Flight Plan";

                return FlightPhase switch
                {
                    FlightPhase.None => OnGround ? "Parked" : "In Flight",
                    FlightPhase.Ready => "Ready for Departure",
                    FlightPhase.Taxi => "Taxiing",
                    FlightPhase.Climbing => "Climbing",
                    FlightPhase.Cruise => "Cruise",
                    FlightPhase.Descending => "Descending",
                    FlightPhase.Landed => string.IsNullOrEmpty(CurrentAirport) ? "Landed" : $"Landed ({CurrentAirport})",
                    FlightPhase.AtDestination => string.IsNullOrEmpty(CurrentAirport) ? "At Destination" : $"At Destination ({CurrentAirport})",
                    FlightPhase.Completed => "Completed",
                    _ => "---"
                };
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
