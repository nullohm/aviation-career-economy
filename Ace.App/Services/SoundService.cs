using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using Ace.App.Interfaces;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class SoundService : ISoundService
    {
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly Dictionary<SoundType, MediaPlayer> _players = new();
        private readonly Dictionary<SoundType, string> _soundFiles = new()
        {
            { SoundType.FlightCompleted, "flight_completed" },
            { SoundType.AchievementUnlocked, "achievement" },
            { SoundType.Warning, "warning" },
            { SoundType.Notification, "notification" },
            { SoundType.ButtonClick, "click" },
            { SoundType.TopOfDescent, "top_of_descent" },
            { SoundType.StallWarning, "stall_warning" }
        };

        private static readonly string[] SupportedExtensions = { ".mp3", ".wav" };

        public bool IsSoundEnabled
        {
            get => _settingsService.CurrentSettings.SoundEnabled;
            set
            {
                _settingsService.CurrentSettings.SoundEnabled = value;
                _settingsService.Save();
            }
        }

        public double Volume
        {
            get => _settingsService.CurrentSettings.SoundVolume;
            set
            {
                _settingsService.CurrentSettings.SoundVolume = Math.Clamp(value, 0.0, 1.0);
                _settingsService.Save();
            }
        }

        public SoundService(ILoggingService logger, ISettingsService settingsService)
        {
            _logger = logger;
            _settingsService = settingsService;
            InitializePlayers();
        }

        private void InitializePlayers()
        {
            var soundsDir = GetSoundsDirectory();

            foreach (var kvp in _soundFiles)
            {
                var filePath = FindSoundFile(soundsDir, kvp.Value);
                if (filePath == null)
                    continue;

                try
                {
                    var player = new MediaPlayer();
                    player.Open(new Uri(filePath, UriKind.Absolute));
                    _players[kvp.Key] = player;
                    _logger.Debug($"SoundService: Loaded sound {kvp.Key} from {filePath}");
                }
                catch (Exception ex)
                {
                    _logger.Debug($"SoundService: Failed to load sound {kvp.Key}: {ex.Message}");
                }
            }

            _logger.Info($"SoundService: Initialized with {_players.Count} sounds loaded");
        }

        private static string? FindSoundFile(string directory, string baseName)
        {
            foreach (var ext in SupportedExtensions)
            {
                var filePath = Path.Combine(directory, baseName + ext);
                if (File.Exists(filePath))
                    return filePath;
            }
            return null;
        }

        private string GetSoundsDirectory()
        {
            var solutionRoot = PathUtilities.FindSolutionRoot();
            return Path.Combine(solutionRoot ?? AppContext.BaseDirectory, "Data", "Sounds");
        }

        public void Play(SoundType soundType)
        {
            if (!IsSoundEnabled)
                return;

            if (!IsSoundTypeEnabled(soundType))
                return;

            if (!_players.TryGetValue(soundType, out var player))
                return;

            try
            {
                player.Dispatcher.Invoke(() =>
                {
                    player.Volume = Volume;
                    player.Position = TimeSpan.Zero;
                    player.Play();
                });
            }
            catch (Exception ex)
            {
                _logger.Debug($"SoundService: Error playing sound {soundType}: {ex.Message}");
            }
        }

        private bool IsSoundTypeEnabled(SoundType soundType)
        {
            return soundType switch
            {
                SoundType.FlightCompleted => _settingsService.CurrentSettings.SoundFlightCompletedEnabled,
                SoundType.AchievementUnlocked => _settingsService.CurrentSettings.SoundAchievementEnabled,
                SoundType.TopOfDescent => _settingsService.CurrentSettings.SoundTopOfDescentEnabled,
                SoundType.Warning => _settingsService.CurrentSettings.SoundWarningEnabled,
                SoundType.Notification => _settingsService.CurrentSettings.SoundNotificationEnabled,
                SoundType.ButtonClick => _settingsService.CurrentSettings.SoundButtonClickEnabled,
                SoundType.StallWarning => _settingsService.CurrentSettings.SoundWarningEnabled,
                _ => false
            };
        }

        public void PlayFlightCompleted() => Play(SoundType.FlightCompleted);
        public void PlayAchievementUnlocked() => Play(SoundType.AchievementUnlocked);
        public void PlayWarning() => Play(SoundType.Warning);
        public void PlayNotification() => Play(SoundType.Notification);
        public void PlayTopOfDescent() => Play(SoundType.TopOfDescent);
        public void PlayStallWarning() => Play(SoundType.StallWarning);
    }
}
