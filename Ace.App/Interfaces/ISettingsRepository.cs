using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface ISettingsRepository
    {
        AppSettingsEntity? GetSettings();
        void SaveSettings(AppSettingsEntity settings);
        void EnsureSettingsExist();
    }
}
