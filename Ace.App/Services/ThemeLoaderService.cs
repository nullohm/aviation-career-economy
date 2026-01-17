using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class ThemeLoaderService : IThemeLoaderService
    {
        private readonly ILoggingService _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ThemeLoaderService(ILoggingService loggingService)
        {
            _logger = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
        }

        public IReadOnlyList<ThemeInfo> DiscoverThemes()
        {
            var themes = new List<ThemeInfo>();

            try
            {
                var themesDir = PathUtilities.GetThemesDirectory();
                var customDir = PathUtilities.GetCustomThemesDirectory();

                EnsureDirectoryExists(themesDir);
                EnsureDirectoryExists(customDir);

                foreach (var file in Directory.GetFiles(themesDir, "*.json"))
                {
                    var theme = LoadThemeInfo(file, isBuiltIn: true, isCustom: false);
                    if (theme != null)
                        themes.Add(theme);
                }

                foreach (var file in Directory.GetFiles(customDir, "*.json"))
                {
                    var theme = LoadThemeInfo(file, isBuiltIn: false, isCustom: true);
                    if (theme != null)
                        themes.Add(theme);
                }

                _logger.Info($"ThemeLoaderService: Discovered {themes.Count} themes");
            }
            catch (Exception ex)
            {
                _logger.Error("ThemeLoaderService: Failed to discover themes", ex);
            }

            return themes;
        }

        public ThemeColors? LoadThemeColors(string themeName)
        {
            var filePath = GetThemeFilePath(themeName);
            if (filePath == null)
            {
                _logger.Warn($"ThemeLoaderService: Theme file not found for '{themeName}'");
                return null;
            }

            return LoadThemeColorsFromFile(filePath);
        }

        public string? GetThemeFilePath(string themeName)
        {
            var themesDir = PathUtilities.GetThemesDirectory();
            var customDir = PathUtilities.GetCustomThemesDirectory();

            var builtInPath = Path.Combine(themesDir, $"{themeName.ToLower()}.json");
            if (File.Exists(builtInPath))
                return builtInPath;

            var customPath = Path.Combine(customDir, $"{themeName.ToLower()}.json");
            if (File.Exists(customPath))
                return customPath;

            var customWithOriginalCase = Path.Combine(customDir, $"{themeName}.json");
            if (File.Exists(customWithOriginalCase))
                return customWithOriginalCase;

            return null;
        }

        public void ApplyColorsToResources(ThemeColors colors)
        {
            if (colors?.Colors == null)
            {
                _logger.Warn("ThemeLoaderService: Cannot apply null colors");
                return;
            }

            var app = Application.Current;
            if (app == null)
            {
                _logger.Warn("ThemeLoaderService: Application.Current is null");
                return;
            }

            try
            {
                var c = colors.Colors;

                SetColorResource(app, "BgPrimaryColor", c.BgPrimary);
                SetColorResource(app, "BgSecondaryColor", c.BgSecondary);
                SetColorResource(app, "CardBgColor", c.CardBg);

                SetColorResource(app, "AccentColor", c.Accent);
                SetColorResource(app, "AccentSecondaryColor", c.AccentSecondary);
                SetColorResource(app, "AccentGradientStartColor", c.AccentGradientStart);
                SetColorResource(app, "AccentGradientEndColor", c.AccentGradientEnd);

                SetColorResource(app, "ForegroundColor", c.Foreground);
                SetColorResource(app, "SubtleForegroundColor", c.SubtleForeground);

                SetColorResource(app, "BorderColor", c.Border);

                SetColorResource(app, "SuccessColor", c.Success);
                SetColorResource(app, "SuccessGradientStartColor", c.SuccessGradientStart);
                SetColorResource(app, "SuccessGradientEndColor", c.SuccessGradientEnd);
                SetColorResource(app, "WarningColor", c.Warning);
                SetColorResource(app, "DangerColor", c.Danger);

                SetColorResource(app, "PurpleAccentColor", c.PurpleAccent);
                SetColorResource(app, "CyanAccentColor", c.CyanAccent);
                SetColorResource(app, "GoldColor", c.Gold);
                SetColorResource(app, "EpauletteBackgroundColor", c.EpauletteBackground);

                SetColorResource(app, "GlassColor", c.Glass);
                SetColorResource(app, "GlassBorderColor", c.GlassBorder);

                SetColorResource(app, "NavHoverColor", c.NavHover);
                SetColorResource(app, "NavSelectedColor", c.NavSelected);

                _logger.Info($"ThemeLoaderService: Applied colors from theme '{colors.Meta?.Name ?? "unknown"}'");
            }
            catch (Exception ex)
            {
                _logger.Error("ThemeLoaderService: Failed to apply colors", ex);
            }
        }

        public void CreateThemeFromCurrent(string newName, string currentThemeName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                _logger.Warn("ThemeLoaderService: Cannot create theme with empty name");
                return;
            }

            try
            {
                var customDir = PathUtilities.GetCustomThemesDirectory();
                EnsureDirectoryExists(customDir);

                var currentColors = LoadThemeColors(currentThemeName);
                if (currentColors == null)
                {
                    _logger.Warn($"ThemeLoaderService: Cannot load current theme '{currentThemeName}' for copying");
                    return;
                }

                var sanitizedName = SanitizeFileName(newName);
                var newFilePath = Path.Combine(customDir, $"{sanitizedName}.json");

                if (File.Exists(newFilePath))
                {
                    _logger.Warn($"ThemeLoaderService: Theme file already exists: {newFilePath}");
                    return;
                }

                currentColors.Meta = new ThemeMeta
                {
                    Name = newName,
                    DisplayName = newName,
                    Author = "Custom",
                    Version = "1.0",
                    Description = $"Custom theme based on {currentThemeName}"
                };

                var json = JsonSerializer.Serialize(currentColors, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(newFilePath, json);
                _logger.Info($"ThemeLoaderService: Created new theme '{newName}' at {newFilePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"ThemeLoaderService: Failed to create theme '{newName}'", ex);
            }
        }

        public void OpenThemeInEditor(string themeName)
        {
            var filePath = GetThemeFilePath(themeName);
            if (filePath == null)
            {
                _logger.Warn($"ThemeLoaderService: Cannot find theme file for '{themeName}'");
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
                _logger.Info($"ThemeLoaderService: Opened theme file in editor: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"ThemeLoaderService: Failed to open theme file '{filePath}'", ex);
            }
        }

        private ThemeInfo? LoadThemeInfo(string filePath, bool isBuiltIn, bool isCustom)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var colors = JsonSerializer.Deserialize<ThemeColors>(json, _jsonOptions);

                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var displayName = colors?.Meta?.DisplayName ?? fileName;

                return new ThemeInfo(
                    Name: fileName,
                    DisplayName: displayName,
                    FilePath: filePath,
                    IsBuiltIn: isBuiltIn,
                    IsCustom: isCustom
                );
            }
            catch (Exception ex)
            {
                _logger.Debug($"ThemeLoaderService: Failed to load theme info from {filePath}: {ex.Message}");
                return null;
            }
        }

        private ThemeColors? LoadThemeColorsFromFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var colors = JsonSerializer.Deserialize<ThemeColors>(json, _jsonOptions);

                if (colors?.Colors == null)
                {
                    _logger.Warn($"ThemeLoaderService: Theme file has no colors section: {filePath}");
                    return null;
                }

                _logger.Debug($"ThemeLoaderService: Loaded theme colors from {filePath}");
                return colors;
            }
            catch (JsonException ex)
            {
                _logger.Error($"ThemeLoaderService: Invalid JSON in theme file {filePath}", ex);
                return null;
            }
            catch (IOException ex)
            {
                _logger.Error($"ThemeLoaderService: Cannot read theme file {filePath}", ex);
                return null;
            }
        }

        private void SetColorResource(Application app, string key, string hexColor)
        {
            var color = ParseColor(hexColor, key);
            if (color.HasValue)
            {
                app.Resources[key] = color.Value;
            }
        }

        private Color? ParseColor(string hexColor, string colorName)
        {
            if (string.IsNullOrWhiteSpace(hexColor))
            {
                _logger.Debug($"ThemeLoaderService: Missing color value for {colorName}");
                return null;
            }

            try
            {
                return (Color)ColorConverter.ConvertFromString(hexColor);
            }
            catch (FormatException)
            {
                _logger.Warn($"ThemeLoaderService: Invalid color format for {colorName}: {hexColor}");
                return null;
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _logger.Debug($"ThemeLoaderService: Created directory {path}");
            }
        }

        private string SanitizeFileName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new string(name.Where(c => !invalidChars.Contains(c)).ToArray());
            return sanitized.ToLower().Replace(" ", "_");
        }
    }
}
