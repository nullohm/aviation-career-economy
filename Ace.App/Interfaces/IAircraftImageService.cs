namespace Ace.App.Interfaces
{
    public interface IAircraftImageService
    {
        string GetImagePath(string? customImagePath, string sizeCategory, string? title = null);
        string SetCustomImage(string sourceImagePath, string identifier);
        string? CopyCustomImage(string? existingCustomImagePath, string newIdentifier);
        void RemoveCustomImage(string identifier);
    }
}
