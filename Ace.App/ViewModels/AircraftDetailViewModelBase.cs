using System.Windows.Media;

namespace Ace.App.ViewModels
{
    public abstract class AircraftDetailViewModelBase : ViewModelBase
    {
        private string _registration = string.Empty;
        public string Registration
        {
            get => _registration;
            set => SetProperty(ref _registration, value);
        }

        private string _typeVariant = string.Empty;
        public string TypeVariant
        {
            get => _typeVariant;
            set => SetProperty(ref _typeVariant, value);
        }

        private string _homeBase = string.Empty;
        public string HomeBase
        {
            get => _homeBase;
            set => SetProperty(ref _homeBase, value);
        }

        private string _status = string.Empty;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private Brush _statusColor = Brushes.Gray;
        public Brush StatusColor
        {
            get => _statusColor;
            set => SetProperty(ref _statusColor, value);
        }

        private decimal _currentValue;
        public decimal CurrentValue
        {
            get => _currentValue;
            set => SetProperty(ref _currentValue, value);
        }

        private int _maxPassengers;
        public int MaxPassengers
        {
            get => _maxPassengers;
            set => SetProperty(ref _maxPassengers, value);
        }

        private double _cruiseSpeedKts;
        public double CruiseSpeedKts
        {
            get => _cruiseSpeedKts;
            set => SetProperty(ref _cruiseSpeedKts, value);
        }

        private double _maxRangeNM;
        public double MaxRangeNM
        {
            get => _maxRangeNM;
            set => SetProperty(ref _maxRangeNM, value);
        }

        private double _fuelCapacityGal;
        public double FuelCapacityGal
        {
            get => _fuelCapacityGal;
            set => SetProperty(ref _fuelCapacityGal, value);
        }

        private double _fuelBurnGalPerHour;
        public double FuelBurnGalPerHour
        {
            get => _fuelBurnGalPerHour;
            set => SetProperty(ref _fuelBurnGalPerHour, value);
        }

        private decimal _hourlyOperatingCost;
        public decimal HourlyOperatingCost
        {
            get => _hourlyOperatingCost;
            set => SetProperty(ref _hourlyOperatingCost, value);
        }
    }
}
