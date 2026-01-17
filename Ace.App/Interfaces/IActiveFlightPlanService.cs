using System;
using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IActiveFlightPlanService
    {
        event Action<Services.ActiveFlightPlan?>? FlightPlanChanged;

        Services.ActiveFlightPlan? GetActivePlan();
        bool HasValidFlightPlan();
        void ActivateFlightPlan(string departureIcao, string arrivalIcao, int aircraftId, string aircraftRegistration, string aircraftType, double distanceNM, int passengers = 0, double cargoKg = 0, bool isCargo = false);
        void ClearFlightPlan();
        void MarkFlightPlanCompleted();
        bool IsFlightPlanCompleted();

        List<AltitudePoint> GetAltitudeTrack();
        double GetMaxAltitudeRecorded();
        double GetPlannedCruiseAltitude();
        void SetPlannedCruiseAltitude(double altitude);
        void AddAltitudePoint(double distanceNM, double altitudeFt, double groundSpeedKts = 0, double verticalSpeedFpm = 0);
        void ResetAltitudeTracking();
    }
}
