using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Threading;
using Microsoft.FlightSimulator.SimConnect;
using Ace.App.Data;
using Ace.App.Models;
using Ace.App.Interfaces;

namespace Ace.App.Services
{
    public enum SimConnectMessage { WM_USER_SIMCONNECT = 0x0402 }
    public enum DataRequests { FlightData, NearestAirport }
    public enum Definitions { FlightDataStruct }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FlightDataStruct
    {
        public double Latitude;
        public double Longitude;
        public double Altitude;
        public double GroundSpeed;
        public double Heading;
        public int SimOnGround;
        public double VerticalSpeed;
        public int ParkingBrakeOn;
        public double SimulationRate;
        public double FuelQuantityGallons;
        public double FuelCapacityGallons;
        public double FuelFlowGallonsPerHour;
        public double AngleOfAttack;
        public int StallWarning;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Title;
    }

    public class SimConnectService
    {
        public const int WM_USER_SIMCONNECT = 0x0402;
        private SimConnect? _simConnect;
        private DispatcherTimer? _timer;
        private IntPtr _handle = IntPtr.Zero;
        private bool _isConnected;
        private FlightRecord? _currentFlight;
        private bool _wasOnGround = true;
        private bool _wasParkingBrakeOn = false;
        private double _lastVerticalSpeed;
        private double _startLat;
        private double _startLon;
        private bool _waitingForDepartureAirport = false;
        private bool _waitingForArrivalAirport = false;
        private bool _isLanded = false;
        private DateTime _landingTime;
        private double _landingLat;
        private double _landingLon;
        private string _landingAirport = string.Empty;
        private List<FlightRecord> _intermediateLegs = new List<FlightRecord>();
        private DateTime _legStartTime;
        private double _legStartLat;
        private double _legStartLon;
        private bool _usedSimRateAcceleration = false;
        private bool _simConnectDllFailed = false;
        private string _currentAircraftTitle = string.Empty;

        private readonly ILoggingService _logger;
        private readonly IActiveFlightPlanService _activeFlightPlanService;
        private readonly IPersistenceService _persistenceService;
        private readonly IFinanceService _financeService;
        private readonly IAirportDatabase _airportDatabase;
        private readonly IFlightEarningsCalculator _earningsCalculator;
        private readonly IAchievementService _achievementService;
        private readonly ISoundService _soundService;

        public event Action<FlightData>? FlightDataReceived;
        public event Action<bool>? ConnectionChanged;
        public event Action? FlightRecorded;

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    ConnectionChanged?.Invoke(_isConnected);
                }
            }
        }

        public bool IsRecording => _currentFlight != null;
        public bool CanRecord => _activeFlightPlanService.HasValidFlightPlan();
        public bool CanManuallyComplete => _activeFlightPlanService.HasValidFlightPlan();

        public void ManuallyCompleteFlightPlan()
        {
            var activePlan = _activeFlightPlanService.GetActivePlan();
            if (activePlan == null)
            {
                _logger.Warn("ManuallyCompleteFlightPlan: No active flight plan");
                return;
            }

            _logger.Info($"ManuallyCompleteFlightPlan: Completing flight plan {activePlan.DepartureIcao} → {activePlan.ArrivalIcao} with estimated values");

            double estimatedFlightHours = EstimateFlightHours(activePlan.AircraftRegistration, activePlan.DistanceNM);

            var earningsResult = _earningsCalculator.CalculateEarnings(new FlightEarningsRequest
            {
                AircraftRegistration = activePlan.AircraftRegistration,
                DepartureIcao = activePlan.DepartureIcao,
                ArrivalIcao = activePlan.ArrivalIcao,
                DistanceNM = activePlan.DistanceNM,
                Passengers = activePlan.Passengers,
                FlightHours = estimatedFlightHours,
                IsManualCompletion = true
            });

            string aircraftTitle = GetAircraftTypeByRegistration(activePlan.AircraftRegistration);
            var flightRecord = new FlightRecord
            {
                Aircraft = activePlan.AircraftRegistration,
                AircraftTitle = aircraftTitle,
                Departure = activePlan.DepartureIcao,
                Arrival = activePlan.ArrivalIcao,
                Date = activePlan.ActivatedAt,
                Duration = TimeSpan.FromHours(estimatedFlightHours),
                DistanceNM = activePlan.DistanceNM,
                LandingRate = 150,
                Status = "Completed (Manual)",
                LegType = FlightLegType.Complete,
                PlannedDestination = activePlan.ArrivalIcao,
                Earnings = earningsResult.TotalEarnings
            };

            _persistenceService.SaveFlightRecord(flightRecord);

            string flightDesc = $"Flight {flightRecord.Departure} → {flightRecord.Arrival} ({activePlan.Passengers} PAX) [Manual]";
            _financeService.AddEarnings(flightRecord.Earnings, flightDesc, flightRecord.Id);

            _activeFlightPlanService.MarkFlightPlanCompleted();
            _currentFlight = null;
            _isLanded = false;
            _intermediateLegs.Clear();

            _soundService.PlayFlightCompleted();
            FlightRecorded?.Invoke();
            _logger.Info($"ManuallyCompleteFlightPlan: Flight completed and recorded");
        }

        private double EstimateFlightHours(string aircraftRegistration, double distanceNM)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.Aircraft.FirstOrDefault(a => a.Registration == aircraftRegistration);
                double cruiseSpeed = aircraft?.CruiseSpeedKts ?? 120.0;
                return distanceNM / cruiseSpeed;
            }
            catch (Exception ex)
            {
                _logger.Error($"EstimateFlightHours: Failed to load aircraft: {ex.Message}");
                return distanceNM / 120.0;
            }
        }

        private string GetAircraftTypeByRegistration(string registration)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.Aircraft.FirstOrDefault(a => a.Registration == registration);
                return aircraft?.Type ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAircraftTypeByRegistration: Failed to load aircraft: {ex.Message}");
                return string.Empty;
            }
        }

        public SimConnectService(
            ILoggingService loggingService,
            IActiveFlightPlanService activeFlightPlanService,
            IPersistenceService persistenceService,
            IFinanceService financeService,
            IAirportDatabase airportDatabase,
            IFlightEarningsCalculator earningsCalculator,
            IAchievementService achievementService,
            ISoundService soundService)
        {
            _logger = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _activeFlightPlanService = activeFlightPlanService ?? throw new ArgumentNullException(nameof(activeFlightPlanService));
            _persistenceService = persistenceService ?? throw new ArgumentNullException(nameof(persistenceService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _earningsCalculator = earningsCalculator ?? throw new ArgumentNullException(nameof(earningsCalculator));
            _achievementService = achievementService ?? throw new ArgumentNullException(nameof(achievementService));
            _soundService = soundService ?? throw new ArgumentNullException(nameof(soundService));
        }

        public void Initialize()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => AttemptConnection();
        }

        public void SetWindowHandle(IntPtr handle) => _handle = handle;

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_USER_SIMCONNECT) { ReceiveMessage(); handled = true; }
            return IntPtr.Zero;
        }

        public void ReceiveMessage()
        {
            try
            {
                _simConnect?.ReceiveMessage();
            }
            catch (Exception ex)
            {
                _logger.Error($"SimConnect: Error receiving message: {ex.Message}");
                CloseConnection();
            }
        }

        public void Connect()
        {
            if (!_isConnected && _timer != null && !_timer.IsEnabled)
            {
                _timer.Start();
                AttemptConnection();
            }
        }

        public async System.Threading.Tasks.Task StopAutoConnectAsync()
        {
            _timer?.Stop();
            CloseConnection();
            await System.Threading.Tasks.Task.CompletedTask;
        }

        private void AttemptConnection()
        {
            if (_simConnect != null || _handle == IntPtr.Zero || _simConnectDllFailed) return;
            try
            {
                _simConnect = new SimConnect("ACE", _handle, WM_USER_SIMCONNECT, null, 0);
                _simConnect.OnRecvOpen += (s, e) => _logger.Info("SimConnect Open");
                _simConnect.OnRecvQuit += (s, e) => CloseConnection();
                _simConnect.OnRecvSimobjectData += OnRecvSimobjectData;
                _simConnect.OnRecvAirportList += OnRecvAirportList;
                _simConnect.OnRecvException += OnRecvException;
                IsConnected = true;
                RegisterDataDefinitions();
            }
            catch (BadImageFormatException ex)
            {
                _simConnectDllFailed = true;
                _timer?.Stop();
                _logger.Warn($"SimConnect DLL not compatible - SimConnect permanently disabled: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.Debug($"SimConnect: Connection attempt failed: {ex.Message}");
            }
        }

        private void CloseConnection()
        {
            if (_simConnect != null)
            {
                _simConnect.Dispose();
                _simConnect = null;
            }
            IsConnected = false;
            _logger.Info("SimConnect connection closed");
        }

        private void RegisterDataDefinitions()
        {
            if (_simConnect == null) return;
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "PLANE LATITUDE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "PLANE LONGITUDE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "INDICATED ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "GROUND VELOCITY", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "PLANE HEADING DEGREES MAGNETIC", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "SIM ON GROUND", "Bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "VERTICAL SPEED", "feet per minute", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "BRAKE PARKING POSITION", "Bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "SIMULATION RATE", "number", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "FUEL TOTAL QUANTITY", "gallons", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "FUEL TOTAL CAPACITY", "gallons", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "ENG FUEL FLOW GPH:1", "gallons per hour", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "INCIDENCE ALPHA", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "STALL WARNING", "Bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.AddToDataDefinition(Definitions.FlightDataStruct, "TITLE", null, SIMCONNECT_DATATYPE.STRING128, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _simConnect.RegisterDataDefineStruct<FlightDataStruct>(Definitions.FlightDataStruct);
            _simConnect.RequestDataOnSimObject(DataRequests.FlightData, Definitions.FlightDataStruct, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.SECOND, SIMCONNECT_DATA_REQUEST_FLAG.CHANGED, 0, 0, 0);
        }

        private void OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            if (data.dwRequestID == (uint)DataRequests.FlightData && data.dwData != null && data.dwData.Length > 0)
            {
                var raw = (FlightDataStruct)data.dwData[0];
                var model = new FlightData {
                    Aircraft = CleanString(raw.Title),
                    Latitude = raw.Latitude,
                    Longitude = raw.Longitude,
                    Altitude = raw.Altitude,
                    GroundSpeed = raw.GroundSpeed,
                    Heading = raw.Heading,
                    VerticalSpeed = raw.VerticalSpeed,
                    ParkingBrake = raw.ParkingBrakeOn != 0,
                    SimulationRate = raw.SimulationRate,
                    FuelQuantityGallons = raw.FuelQuantityGallons,
                    FuelCapacityGallons = raw.FuelCapacityGallons,
                    FuelFlowGallonsPerHour = raw.FuelFlowGallonsPerHour,
                    AngleOfAttack = raw.AngleOfAttack,
                    StallWarning = raw.StallWarning != 0,
                    OnGround = raw.SimOnGround != 0,
                    IsConnected = true,
                    FlightPhase = DetermineFlightPhase(raw.SimOnGround != 0, raw.GroundSpeed, raw.VerticalSpeed),
                    CurrentAirport = _isLanded ? _landingAirport : string.Empty,
                    DestinationAirport = _currentFlight?.PlannedDestination ?? string.Empty
                };
                if (!model.OnGround) _lastVerticalSpeed = raw.VerticalSpeed;
                FlightDataReceived?.Invoke(model);
                ProcessFlightLogic(model);
            }
        }


        private void OnRecvException(SimConnect _, SIMCONNECT_RECV_EXCEPTION data)
        {
            _logger.Error($"SimConnect Exception: {data.dwException}");
        }

        private void OnRecvAirportList(SimConnect _, SIMCONNECT_RECV_AIRPORT_LIST data)
        {
            try
            {
                _logger.Info($"Nearest airport list received: {data.dwArraySize} airports, RequestID: {data.dwRequestID}");

                if (data.dwArraySize == 0 || data.rgData == null || data.rgData.Length == 0)
                {
                    _logger.Warn("Airport list is empty");
                    HandleAirportNotFound();
                    return;
                }

                dynamic airport = data.rgData[0];
                string icao = airport.Icao.ToString().Trim('\0', ' ');

                _logger.Info($"Nearest airport: {icao}");

                if (_waitingForDepartureAirport && _currentFlight != null)
                {
                    _currentFlight.Departure = icao;
                    _waitingForDepartureAirport = false;
                    _logger.Info($"Departure airport set to: {icao}");
                }
                else if (_waitingForArrivalAirport && _currentFlight != null)
                {
                    _currentFlight.Arrival = icao;
                    _waitingForArrivalAirport = false;
                    _logger.Info($"Arrival airport set to: {icao}, saving flight...");

                    CalculateAndSaveFlightEarnings();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error processing nearest airport list", ex);
                HandleAirportNotFound();
            }
        }

        private void HandleAirportNotFound()
        {
            if (_waitingForDepartureAirport && _currentFlight != null)
            {
                string fallback = _airportDatabase.FindNearestAirport(_startLat, _startLon, 25.0);
                _currentFlight.Departure = fallback;
                _waitingForDepartureAirport = false;
                _logger.Warn($"Using fallback departure: {fallback}");
            }
            else if (_waitingForArrivalAirport && _currentFlight != null)
            {
                string fallback = _airportDatabase.FindNearestAirport(_landingLat, _landingLon, 25.0);
                _currentFlight.Arrival = fallback;
                _waitingForArrivalAirport = false;
                _logger.Warn($"Using fallback arrival: {fallback}");

                CalculateAndSaveFlightEarnings();
            }
        }

        private void CalculateAndSaveFlightEarnings()
        {
            if (_currentFlight == null) return;

            if (!IsValidFlight(_currentFlight))
            {
                _logger.Warn($"Invalid flight discarded: {_currentFlight.Departure} → {_currentFlight.Arrival}, " +
                    $"Landing Rate: {_currentFlight.LandingRate:F0} fpm, Distance: {_currentFlight.DistanceNM:F1} NM");
                _currentFlight = null;
                return;
            }

            var activePlan = _activeFlightPlanService.GetActivePlan();
            int passengers = activePlan?.Passengers ?? 0;
            string aircraftRegistration = activePlan?.AircraftRegistration ?? _currentFlight.Aircraft;
            double distanceNM = _currentFlight.DistanceNM;

            double flightHours = DetermineFlightHours(aircraftRegistration, distanceNM);

            var earningsResult = _earningsCalculator.CalculateEarnings(new FlightEarningsRequest
            {
                AircraftRegistration = aircraftRegistration,
                DepartureIcao = _currentFlight.Departure,
                ArrivalIcao = _currentFlight.Arrival,
                DistanceNM = distanceNM,
                Passengers = passengers,
                FlightHours = flightHours,
                IsManualCompletion = false
            });

            _currentFlight.Earnings = earningsResult.TotalEarnings;

            _persistenceService.SaveFlightRecord(_currentFlight);

            string flightDesc = $"Flight {_currentFlight.Departure} → {_currentFlight.Arrival} ({passengers} PAX)";
            _financeService.AddEarnings(_currentFlight.Earnings, flightDesc, _currentFlight.Id);

            CheckFlightAchievements(_currentFlight);

            _soundService.PlayFlightCompleted();

            _currentFlight = null;
            _activeFlightPlanService.MarkFlightPlanCompleted();
            FlightRecorded?.Invoke();
        }

        private double DetermineFlightHours(string aircraftRegistration, double distanceNM)
        {
            if (!_usedSimRateAcceleration)
            {
                return _currentFlight?.Duration.TotalHours ?? 0;
            }

            double flightHours = EstimateFlightHours(aircraftRegistration, distanceNM);
            if (_currentFlight != null)
            {
                _currentFlight.Duration = TimeSpan.FromHours(flightHours);
            }
            _logger.Info($"Sim rate was used - replacing actual duration with estimated: {flightHours:F2}h");
            return flightHours;
        }

        private void CheckFlightAchievements(FlightRecord flight)
        {
            var flights = _persistenceService.LoadFlightRecords() ?? new List<FlightRecord>();
            var totalFlights = flights.Count;
            var totalDistanceNM = flights.Sum(f => f.DistanceNM);

            _achievementService.CheckFlightAchievements(totalFlights, totalDistanceNM);
            _achievementService.CheckLandingAchievement(flight.LandingRate);
        }

        private bool IsValidFlight(FlightRecord flight)
        {
            // Reject flights with same departure and arrival (likely crash/teleport)
            if (flight.Departure == flight.Arrival)
            {
                _logger.Debug($"Flight rejected: same departure and arrival ({flight.Departure})");
                return false;
            }

            // Reject crashes - landing rate > 1000 fpm is a crash
            if (flight.LandingRate > 1000)
            {
                _logger.Debug($"Flight rejected: crash landing ({flight.LandingRate:F0} fpm)");
                return false;
            }

            // Reject very short flights (< 5 NM) - likely test/abort
            if (flight.DistanceNM < 5)
            {
                _logger.Debug($"Flight rejected: too short ({flight.DistanceNM:F1} NM)");
                return false;
            }

            // Reject very short duration (< 2 minutes) - likely abort
            if (flight.Duration.TotalMinutes < 2)
            {
                _logger.Debug($"Flight rejected: too short duration ({flight.Duration.TotalMinutes:F1} min)");
                return false;
            }

            return true;
        }

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "Unknown";
            string s = input.Split('\0')[0];
            return new string(s.Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-').ToArray()).Trim();
        }

        private void ProcessFlightLogic(FlightData data)
        {
            bool isFlying = !data.OnGround;

            // Takeoff detection
            if (isFlying && _wasOnGround && data.GroundSpeed > 40)
            {
                if (_currentFlight == null)
                {
                    if (!_activeFlightPlanService.HasValidFlightPlan())
                    {
                        _logger.Warn("Flight start detected but no valid flight plan active - flight recording blocked");
                        _wasOnGround = false;
                        return;
                    }

                    _logger.Info($"Flight started! Speed: {data.GroundSpeed:F0} kts, Position: {data.Latitude:F4}, {data.Longitude:F4}");

                    var activePlan = _activeFlightPlanService.GetActivePlan();
                    string aircraftRegistration = activePlan?.AircraftRegistration ?? "Unknown";

                    _currentAircraftTitle = data.Aircraft;
                    _currentFlight = new FlightRecord
                    {
                        Departure = "Searching...",
                        Date = DateTime.Now,
                        Aircraft = aircraftRegistration,
                        AircraftTitle = data.Aircraft,
                        Status = "In Air",
                        PlannedDestination = activePlan?.ArrivalIcao ?? string.Empty
                    };
                    _startLat = data.Latitude;
                    _startLon = data.Longitude;
                    _legStartLat = data.Latitude;
                    _legStartLon = data.Longitude;
                    _legStartTime = DateTime.Now;
                    _waitingForDepartureAirport = true;
                    _isLanded = false;
                    _usedSimRateAcceleration = false;
                    _intermediateLegs.Clear();
                    RequestNearestAirport(data.Latitude, data.Longitude);
                }
                else if (_isLanded)
                {
                    // Debounce: Ignore takeoff detection within 10 seconds of landing to prevent false triggers from sim data noise
                    var timeSinceLanding = (DateTime.Now - _landingTime).TotalSeconds;
                    if (timeSinceLanding < 10)
                    {
                        _logger.Debug($"Ignoring false takeoff {timeSinceLanding:F1}s after landing (debounce period)");
                        return;
                    }

                    // Continuing flight after an intermediate landing
                    _logger.Info($"Continuing flight from intermediate stop, new leg starting at {data.Latitude:F4}, {data.Longitude:F4}");
                    _legStartLat = data.Latitude;
                    _legStartLon = data.Longitude;
                    _legStartTime = DateTime.Now;
                    _isLanded = false;
                }
                _wasOnGround = false;
            }
            // Landing detection
            else if (data.OnGround && !_wasOnGround)
            {
                if (_currentFlight != null)
                {
                    var legDuration = DateTime.Now - _legStartTime;
                    _logger.Info($"Landing detected! Leg duration: {legDuration.TotalSeconds:F0}s, Position: {data.Latitude:F4}, {data.Longitude:F4}");

                    if (legDuration.TotalSeconds > 5)
                    {
                        _isLanded = true;
                        _landingTime = DateTime.Now;
                        _landingLat = data.Latitude;
                        _landingLon = data.Longitude;
                        _currentFlight.LandingRate = Math.Abs(_lastVerticalSpeed);

                        // Find nearest airport for this landing
                        _landingAirport = _airportDatabase.FindNearestAirport(data.Latitude, data.Longitude, 25.0);
                        _logger.Info($"Landed at {_landingAirport}, planned destination: {_currentFlight.PlannedDestination}");
                    }
                    else
                    {
                        _logger.Info("Leg too short, ignoring this landing");
                    }
                }
                _wasOnGround = true;
            }

            // Auto-complete at destination when parked
            if (data.OnGround && _isLanded && _currentFlight != null && data.GroundSpeed < 5 && data.ParkingBrake)
            {
                var plannedDest = _currentFlight.PlannedDestination;
                bool isAtDestination = !string.IsNullOrEmpty(plannedDest) &&
                                       _landingAirport.Equals(plannedDest, StringComparison.OrdinalIgnoreCase);

                if (isAtDestination)
                {
                    _logger.Info($"At destination {_landingAirport} with parking brake set - completing flight");
                    CompleteFlight(data);
                }
                else if (!_wasParkingBrakeOn)
                {
                    // Not at destination - log only on parking brake state change
                    _logger.Info($"Parking brake engaged at {_landingAirport}, but destination is {plannedDest}. Use manual completion or continue flying.");
                }
            }

            if (_currentFlight != null && !data.OnGround && data.SimulationRate > 1.0)
            {
                if (!_usedSimRateAcceleration)
                {
                    _logger.Info($"Sim rate acceleration detected: {data.SimulationRate:F0}x - flight duration will use estimated values");
                    _usedSimRateAcceleration = true;
                }
            }

            _wasParkingBrakeOn = data.ParkingBrake;
        }

        private void CompleteFlight(FlightData data)
        {
            if (_currentFlight == null) return;

            // Calculate totals including all intermediate legs
            var totalDuration = DateTime.Now - _currentFlight.Date;
            var directDistance = CalculateDistance(_startLat, _startLon, _landingLat, _landingLon);

            // Update current flight with final data
            _currentFlight.Status = "Landed";
            _currentFlight.Duration = totalDuration;
            _currentFlight.DistanceNM = directDistance;
            _currentFlight.Arrival = _landingAirport;
            _currentFlight.LegType = FlightLegType.Complete;

            // If we had intermediate legs, link them
            if (_intermediateLegs.Count > 0)
            {
                _logger.Info($"Flight completed with {_intermediateLegs.Count} intermediate stop(s)");
            }

            CalculateAndSaveFlightEarnings();

            // Reset state
            _isLanded = false;
            _intermediateLegs.Clear();
        }

        private void RequestNearestAirport(double lat, double lon)
        {
            try
            {
                if (_simConnect == null)
                {
                    _logger.Warn("SimConnect not available for airport request");
                    HandleAirportNotFound();
                    return;
                }

                _logger.Info($"Requesting nearest airport to {lat:F4}, {lon:F4}");

                string icao = _airportDatabase.FindNearestAirport(lat, lon, 25.0);
                _logger.Info($"Nearest airport from database: {icao}");

                if (_waitingForDepartureAirport && _currentFlight != null)
                {
                    _currentFlight.Departure = icao;
                    _waitingForDepartureAirport = false;
                    _logger.Info($"Departure airport set to: {icao}");
                }
                else if (_waitingForArrivalAirport && _currentFlight != null)
                {
                    _currentFlight.Arrival = icao;
                    _waitingForArrivalAirport = false;
                    _logger.Info($"Arrival airport set to: {icao}, saving flight...");

                    CalculateAndSaveFlightEarnings();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to find nearest airport", ex);
                HandleAirportNotFound();
            }
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double r = 3440.065;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * r * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        private FlightPhase DetermineFlightPhase(bool onGround, double groundSpeed, double verticalSpeed)
        {
            if (_activeFlightPlanService.IsFlightPlanCompleted())
            {
                return FlightPhase.Completed;
            }

            if (_currentFlight == null)
            {
                return FlightPhase.Ready;
            }

            if (_isLanded)
            {
                bool atDestination = !string.IsNullOrEmpty(_currentFlight.PlannedDestination) &&
                                     _landingAirport.Equals(_currentFlight.PlannedDestination, StringComparison.OrdinalIgnoreCase);
                return atDestination ? FlightPhase.AtDestination : FlightPhase.Landed;
            }

            if (onGround)
            {
                return groundSpeed > 5 ? FlightPhase.Taxi : FlightPhase.Ready;
            }

            if (verticalSpeed > 300)
            {
                return FlightPhase.Climbing;
            }
            if (verticalSpeed < -300)
            {
                return FlightPhase.Descending;
            }
            return FlightPhase.Cruise;
        }
    }
}
