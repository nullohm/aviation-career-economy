using System;
using System.Collections.Generic;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class ActiveFlightPlan
    {
        public string DepartureIcao { get; set; } = string.Empty;
        public string ArrivalIcao { get; set; } = string.Empty;
        public int AircraftId { get; set; }
        public string AircraftRegistration { get; set; } = string.Empty;
        public string AircraftType { get; set; } = string.Empty;
        public double DistanceNM { get; set; }
        public int Passengers { get; set; }
        public double CargoKg { get; set; }
        public DateTime ActivatedAt { get; set; }
        public bool IsCompleted { get; set; }

        public string LoadDisplay => $"{Passengers} PAX + {CargoKg:F0} kg";
    }

    public sealed class ActiveFlightPlanService : IActiveFlightPlanService
    {
        private readonly ILoggingService _loggingService;
        private ActiveFlightPlan? _activePlan;
        private List<AltitudePoint> _altitudeTrack = new();
        private double _maxAltitudeRecorded;
        private double _plannedCruiseAltitude;

        public event Action<ActiveFlightPlan?>? FlightPlanChanged;

        public ActiveFlightPlanService(ILoggingService loggingService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public ActiveFlightPlan? GetActivePlan() => _activePlan;

        public bool HasValidFlightPlan() => _activePlan != null;

        public void ActivateFlightPlan(string departureIcao, string arrivalIcao, int aircraftId, string aircraftRegistration, string aircraftType, double distanceNM, int passengers = 0, double cargoKg = 0, bool isCargo = false)
        {
            _altitudeTrack.Clear();
            _maxAltitudeRecorded = 0;
            _plannedCruiseAltitude = 0;

            _activePlan = new ActiveFlightPlan
            {
                DepartureIcao = departureIcao,
                ArrivalIcao = arrivalIcao,
                AircraftId = aircraftId,
                AircraftRegistration = aircraftRegistration,
                AircraftType = aircraftType,
                DistanceNM = distanceNM,
                Passengers = passengers,
                CargoKg = cargoKg,
                ActivatedAt = DateTime.Now
            };

            _loggingService.Info($"Flight plan activated: {departureIcao} → {arrivalIcao}, Aircraft: {aircraftRegistration} ({aircraftType}), Distance: {distanceNM:F1} NM, {passengers} PAX, {cargoKg:F0} kg");
            FlightPlanChanged?.Invoke(_activePlan);
        }

        public void ClearFlightPlan()
        {
            if (_activePlan != null)
            {
                _loggingService.Info($"Flight plan cleared: {_activePlan.DepartureIcao} → {_activePlan.ArrivalIcao}");
                _activePlan = null;
                _altitudeTrack.Clear();
                _maxAltitudeRecorded = 0;
                _plannedCruiseAltitude = 0;
                FlightPlanChanged?.Invoke(null);
            }
        }

        public void MarkFlightPlanCompleted()
        {
            if (_activePlan != null && !_activePlan.IsCompleted)
            {
                _activePlan.IsCompleted = true;
                _loggingService.Info($"Flight plan marked as completed: {_activePlan.DepartureIcao} → {_activePlan.ArrivalIcao}");
                FlightPlanChanged?.Invoke(_activePlan);
            }
        }

        public bool IsFlightPlanCompleted() => _activePlan?.IsCompleted ?? false;

        public List<AltitudePoint> GetAltitudeTrack() => _altitudeTrack;

        public double GetMaxAltitudeRecorded() => _maxAltitudeRecorded;

        public double GetPlannedCruiseAltitude() => _plannedCruiseAltitude;

        public void SetPlannedCruiseAltitude(double altitude)
        {
            _plannedCruiseAltitude = altitude;
        }

        public void AddAltitudePoint(double distanceNM, double altitudeFt, double groundSpeedKts = 0, double verticalSpeedFpm = 0)
        {
            var lastPoint = _altitudeTrack.Count > 0 ? _altitudeTrack[_altitudeTrack.Count - 1] : null;
            if (lastPoint != null && Math.Abs(distanceNM - lastPoint.DistanceNM) < 1.0)
                return;

            if (altitudeFt > _maxAltitudeRecorded)
                _maxAltitudeRecorded = altitudeFt;

            _altitudeTrack.Add(new AltitudePoint
            {
                DistanceNM = distanceNM,
                AltitudeFt = altitudeFt,
                GroundSpeedKts = groundSpeedKts,
                VerticalSpeedFpm = verticalSpeedFpm
            });
        }

        public void ResetAltitudeTracking()
        {
            _altitudeTrack.Clear();
            _maxAltitudeRecorded = 0;
            _plannedCruiseAltitude = 0;
        }
    }
}
