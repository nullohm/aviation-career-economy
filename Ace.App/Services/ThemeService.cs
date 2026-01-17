using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class ThemeService : IThemeService
    {
        private readonly ISettingsService _settingsService;
        private readonly ILoggingService _loggingService;
        private readonly IThemeLoaderService _themeLoaderService;
        private readonly Dictionary<string, string> _xamlThemeFiles;
        private IReadOnlyList<ThemeInfo>? _cachedThemes;

        public string CurrentTheme => _settingsService.CurrentSettings.Theme;

        public IReadOnlyList<ThemeInfo> AvailableThemes
        {
            get
            {
                _cachedThemes ??= _themeLoaderService.DiscoverThemes();
                return _cachedThemes;
            }
        }

        public ThemeService(
            ISettingsService settingsService,
            ILoggingService loggingService,
            IThemeLoaderService themeLoaderService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _themeLoaderService = themeLoaderService ?? throw new ArgumentNullException(nameof(themeLoaderService));

            _xamlThemeFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Dark", "Themes/DarkTheme.xaml" },
                { "Light", "Themes/LightTheme.xaml" }
            };
        }

        public void SetTheme(string themeName)
        {
            if (string.IsNullOrEmpty(themeName))
            {
                _loggingService.Warn("ThemeService: Attempted to set empty theme name");
                return;
            }

            var themeExists = AvailableThemes.Any(t =>
                t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));

            if (!themeExists)
            {
                _loggingService.Warn($"ThemeService: Unknown theme '{themeName}'");
                return;
            }

            if (themeName.Equals(CurrentTheme, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _settingsService.CurrentSettings.Theme = themeName;
            _settingsService.Save();
            ApplyCurrentTheme();

            _loggingService.Info($"ThemeService: Theme changed to '{themeName}'");
        }

        public void ApplyCurrentTheme()
        {
            try
            {
                var themeName = CurrentTheme;

                var colors = _themeLoaderService.LoadThemeColors(themeName);
                if (colors == null)
                {
                    _loggingService.Warn($"ThemeService: Failed to load colors for '{themeName}', falling back to dark");
                    colors = _themeLoaderService.LoadThemeColors("dark");
                }

                if (colors != null)
                {
                    _themeLoaderService.ApplyColorsToResources(colors);
                }

                var xamlThemeFile = GetXamlThemeFile(themeName);
                LoadXamlTheme(xamlThemeFile);

                _loggingService.Info($"ThemeService: Applied theme '{themeName}'");
            }
            catch (Exception ex)
            {
                _loggingService.Error("ThemeService: Failed to apply theme", ex);
            }
        }

        public void ReloadCurrentTheme()
        {
            _loggingService.Info("ThemeService: Reloading current theme");
            ApplyCurrentTheme();
        }

        public void CreateThemeFromCurrent(string newName)
        {
            _themeLoaderService.CreateThemeFromCurrent(newName, CurrentTheme);
            _cachedThemes = null;
        }

        public void OpenCurrentThemeInEditor()
        {
            _themeLoaderService.OpenThemeInEditor(CurrentTheme);
        }

        private string GetXamlThemeFile(string themeName)
        {
            if (_xamlThemeFiles.TryGetValue(themeName, out var themeFile))
            {
                return themeFile;
            }

            return _xamlThemeFiles["Dark"];
        }

        private void LoadXamlTheme(string themeFile)
        {
            var themeUri = new Uri(themeFile, UriKind.Relative);
            var newTheme = new ResourceDictionary { Source = themeUri };

            var app = Application.Current;
            if (app == null)
            {
                _loggingService.Warn("ThemeService: Application.Current is null");
                return;
            }

            var mergedDictionaries = app.Resources.MergedDictionaries;

            ResourceDictionary? existingTheme = null;
            foreach (var dict in mergedDictionaries)
            {
                if (dict.Source != null && dict.Source.OriginalString.Contains("Theme.xaml"))
                {
                    existingTheme = dict;
                    break;
                }
            }

            if (existingTheme != null)
            {
                mergedDictionaries.Remove(existingTheme);
            }

            mergedDictionaries.Insert(0, newTheme);

            _loggingService.Debug($"ThemeService: Loaded XAML theme from '{themeFile}'");
        }
    }
}
