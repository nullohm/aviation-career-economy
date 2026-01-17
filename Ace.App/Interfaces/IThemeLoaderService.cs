using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IThemeLoaderService
    {
        IReadOnlyList<ThemeInfo> DiscoverThemes();
        ThemeColors? LoadThemeColors(string themeName);
        void ApplyColorsToResources(ThemeColors colors);
        void CreateThemeFromCurrent(string newName, string currentThemeName);
        void OpenThemeInEditor(string themeName);
        string? GetThemeFilePath(string themeName);
    }
}
