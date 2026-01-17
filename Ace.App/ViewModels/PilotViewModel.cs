using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class PilotViewModel : INotifyPropertyChanged
    {
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly Pilot _pilot;
        private string _status = string.Empty;
        private string _statusDetail = string.Empty;
        private bool _hasStatus;

        public PilotViewModel(Pilot pilot, ILoggingService logger, ISettingsService settingsService)
        {
            _pilot = pilot ?? throw new ArgumentNullException(nameof(pilot));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public int Id => _pilot.Id;
        public string Name => _pilot.Name;
        public DateTime Birthday => _pilot.Birthday;
        public string ImagePath => _pilot.ImagePath;
        public double TotalFlightHours => _pilot.TotalFlightHours;
        public double TotalDistanceNM => _pilot.TotalDistanceNM;
        public bool IsEmployed => _pilot.IsEmployed;
        public bool IsPlayer => _pilot.IsPlayer;
        public decimal SalaryPerMonth => _pilot.SalaryPerMonth;

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public string StatusDetail
        {
            get => _statusDetail;
            set
            {
                if (_statusDetail != value)
                {
                    _statusDetail = value;
                    OnPropertyChanged(nameof(StatusDetail));
                }
            }
        }

        public bool HasStatus
        {
            get => _hasStatus;
            set
            {
                if (_hasStatus != value)
                {
                    _hasStatus = value;
                    OnPropertyChanged(nameof(HasStatus));
                }
            }
        }

        public bool HasImage => PilotImage != null;

        public BitmapImage? PilotImage
        {
            get
            {
                if (string.IsNullOrEmpty(_pilot.ImagePath))
                    return null;

                var imagePath = _pilot.ImagePath;
                if (!Path.IsPathRooted(imagePath))
                    imagePath = Utilities.PathUtilities.GetPilotImagePath(imagePath);

                if (!File.Exists(imagePath))
                {
                    _logger.Debug($"PilotViewModel: Image not found at {imagePath}");
                    return null;
                }

                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                    bitmap.EndInit();
                    return bitmap;
                }
                catch (Exception ex)
                {
                    _logger.Error($"PilotViewModel: Failed to load image from {imagePath}: {ex.Message}");
                    return null;
                }
            }
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - _pilot.Birthday.Year;
                if (_pilot.Birthday.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public PilotRankType Rank
        {
            get => PilotRank.GetRank(_pilot.TotalFlightHours, _settingsService.CurrentSettings);
        }

        public string RankName
        {
            get => PilotRank.GetRankName(Rank);
        }

        public decimal BaseSalary
        {
            get => _settingsService.CurrentSettings.PilotBaseSalary;
        }

        public decimal AdjustedSalary
        {
            get => PilotRank.CalculateAdjustedSalary(Rank, _settingsService.CurrentSettings);
        }

        public Pilot GetPilot() => _pilot;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
