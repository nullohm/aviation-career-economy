using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IThemeService
    {
        string CurrentTheme { get; }
        IReadOnlyList<ThemeInfo> AvailableThemes { get; }
        void SetTheme(string themeName);
        void ApplyCurrentTheme();
        void ReloadCurrentTheme();
        void CreateThemeFromCurrent(string newName);
        void OpenCurrentThemeInEditor();
    }
}
