using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class FBOListViewModel : ViewModelBase
    {
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IFBORepository _fboRepository;
        private readonly IAirportDatabase _airportDatabase;
        private readonly IAchievementService _achievementService;
        private readonly IScheduledRouteService _routeService;

        private ObservableCollection<FBOViewModel> _fbos = new();
        private string _icaoInput = string.Empty;
        private string _selectedFBOType = "Local";
        private string _rentMessage = string.Empty;

        public ObservableCollection<FBOViewModel> FBOs
        {
            get => _fbos;
            set => SetProperty(ref _fbos, value);
        }

        public string ICAOInput
        {
            get => _icaoInput;
            set => SetProperty(ref _icaoInput, value);
        }

        public string SelectedFBOType
        {
            get => _selectedFBOType;
            set => SetProperty(ref _selectedFBOType, value);
        }

        public string RentMessage
        {
            get => _rentMessage;
            set => SetProperty(ref _rentMessage, value);
        }

        public FBOListViewModel(
            ILoggingService logger,
            ISettingsService settingsService,
            IFBORepository fboRepository,
            IAirportDatabase airportDatabase,
            IAchievementService achievementService,
            IScheduledRouteService routeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _achievementService = achievementService ?? throw new ArgumentNullException(nameof(achievementService));
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
        }

        public void LoadFBOs()
        {
            try
            {
                _logger.Info("FBOListViewModel: Loading FBOs");

                var fbos = _fboRepository.GetAllFBOs();
                _logger.Database("FBOs query completed", fbos.Count);

                var settings = _settingsService.CurrentSettings;

                FBOs.Clear();
                foreach (var fbo in fbos)
                {
                    var outgoingRouteCount = _routeService.GetOutgoingRouteCountForFBO(fbo.Id);
                    var maxRouteSlots = fbo.Type switch
                    {
                        FBOType.Local => settings.RouteSlotLimitLocal,
                        FBOType.Regional => settings.RouteSlotLimitRegional,
                        FBOType.International => settings.RouteSlotLimitInternational,
                        _ => settings.RouteSlotLimitLocal
                    };

                    FBOs.Add(new FBOViewModel(fbo, _settingsService, outgoingRouteCount, maxRouteSlots));
                }

                _logger.Info($"FBOListViewModel: Loaded {FBOs.Count} FBOs");
            }
            catch (Exception ex)
            {
                _logger.Error("FBOListViewModel: Error loading FBOs", ex);
            }
        }

        public (bool Success, string Message) ValidateRentFBO()
        {
            if (string.IsNullOrWhiteSpace(ICAOInput))
            {
                return (false, "Please enter an ICAO code");
            }

            if (ICAOInput.Length != 4)
            {
                return (false, "ICAO code must be exactly 4 characters");
            }

            try
            {

                var existingFBO = _fboRepository.GetAllFBOs().FirstOrDefault(f => f.ICAO.ToUpper() == ICAOInput.ToUpper());
                if (existingFBO != null)
                {
                    return (false, $"FBO at {ICAOInput.ToUpper()} already exists");
                }


                var airport = _airportDatabase.GetAirport(ICAOInput.ToUpper());
                if (airport == null)
                {
                    return (false, $"Airport {ICAOInput.ToUpper()} not found");
                }

                return (true, airport.Name);
            }
            catch (Exception ex)
            {
                _logger.Error("FBOListViewModel: Error validating ICAO", ex);
                return (false, "Error validating ICAO code");
            }
        }

        public bool RentFBO(string airportName)
        {
            try
            {
                var fboType = Enum.Parse<FBOType>(SelectedFBOType);
                var settings = _settingsService.CurrentSettings;
                decimal monthlyRent = fboType switch
                {
                    FBOType.Local => settings.FBORentLocal,
                    FBOType.Regional => settings.FBORentRegional,
                    FBOType.International => settings.FBORentInternational,
                    _ => settings.FBORentLocal
                };

                var airport = _airportDatabase.GetAirport(ICAOInput.ToUpper());
                var runwayLength = airport?.LongestRunwayFt ?? 0;

                var newFBO = new FBO
                {
                    ICAO = ICAOInput.ToUpper(),
                    AirportName = airportName,
                    Type = fboType,
                    MonthlyRent = monthlyRent,
                    RentedSince = DateTime.Now,
                    RunwayLengthFt = runwayLength
                };

                _fboRepository.AddFBO(newFBO);

                _logger.Info($"FBOListViewModel: Rented FBO at {newFBO.ICAO} ({newFBO.AirportName}) - {fboType} - {monthlyRent:C}/month");

                CheckFBOAchievements();

                ICAOInput = string.Empty;
                SelectedFBOType = "Local";
                RentMessage = string.Empty;


                LoadFBOs();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("FBOListViewModel: Error renting FBO", ex);
                return false;
            }
        }

        private void CheckFBOAchievements()
        {
            var fboCount = _fboRepository.GetAllFBOs().Count;
            _achievementService.CheckFBOAchievements(fboCount);
        }

        public bool TerminateFBO(int fboId)
        {
            try
            {
                _logger.Info($"FBOListViewModel: Terminating FBO {fboId}");

                var fbo = _fboRepository.GetFBOById(fboId);
                if (fbo == null)
                {
                    _logger.Warn($"FBOListViewModel: FBO {fboId} not found");
                    return false;
                }

                _fboRepository.DeleteFBO(fboId);

                _logger.Info($"FBOListViewModel: Terminated FBO at {fbo.ICAO}");


                LoadFBOs();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"FBOListViewModel: Error terminating FBO {fboId}", ex);
                return false;
            }
        }
    }
}
