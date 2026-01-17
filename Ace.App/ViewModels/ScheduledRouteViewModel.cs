using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ace.App.Models;

namespace Ace.App.ViewModels
{
    public class ScheduledRouteViewModel : INotifyPropertyChanged
    {
        public int Id { get; }
        public int OriginFBOId { get; }
        public int DestinationFBOId { get; }
        public string OriginICAO { get; }
        public string DestinationICAO { get; }
        public string OriginName { get; }
        public string DestinationName { get; }
        public string RouteDisplay => $"{OriginICAO} ↔ {DestinationICAO}";
        public string RouteDescription => $"{OriginName} - {DestinationName}";
        public string? AircraftRegistration { get; }
        public string? AircraftType { get; }
        public string? PilotName { get; }
        public bool HasAircraft => !string.IsNullOrEmpty(AircraftRegistration);
        public string StatusText => HasAircraft ? "Active" : "Unassigned";
        public string StatusColor => HasAircraft ? "#4CAF50" : "#FFA726";
        public bool IsActive { get; }
        public int? AssignedAircraftId { get; }

        public ScheduledRouteViewModel(
            ScheduledRoute route,
            FBO originFBO,
            FBO destFBO,
            Aircraft? aircraft = null,
            Pilot? pilot = null)
        {
            Id = route.Id;
            OriginFBOId = route.OriginFBOId;
            DestinationFBOId = route.DestinationFBOId;
            OriginICAO = originFBO.ICAO;
            DestinationICAO = destFBO.ICAO;
            OriginName = originFBO.AirportName;
            DestinationName = destFBO.AirportName;
            AircraftRegistration = aircraft?.Registration;
            AircraftType = aircraft?.Type;
            PilotName = pilot?.Name;
            IsActive = route.IsActive;
            AssignedAircraftId = route.AssignedAircraftId;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
