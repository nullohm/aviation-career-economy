using Ace.App.Services;

namespace Ace.App.Interfaces
{
    public interface ISettingsService
    {
        AppSettings CurrentSettings { get; }
        void Save();
        void Load();
    }
}
