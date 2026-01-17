using System.IO;
using Ace.App.Interfaces;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class AircraftImageService : IAircraftImageService
    {
        private readonly ILoggingService _log;

        public AircraftImageService(ILoggingService log)
        {
            _log = log;
        }

        public string GetImagePath(string? customImagePath, string sizeCategory, string? title = null)
        {
            if (!string.IsNullOrEmpty(customImagePath))
            {
                var savegamePath = Path.Combine(PathUtilities.GetSavegameAircraftImagesDirectory(), customImagePath);
                if (File.Exists(savegamePath))
                {
                    return savegamePath;
                }

                var templatePath = Path.Combine(PathUtilities.GetTemplateAircraftImagesDirectory(), customImagePath);
                if (File.Exists(templatePath))
                {
                    return templatePath;
                }
            }

            if (!string.IsNullOrEmpty(title))
            {
                var titleBasedImage = FindTemplateImageByTitle(title);
                if (titleBasedImage != null)
                {
                    return titleBasedImage;
                }
            }

            return PathUtilities.GetDefaultAircraftImage(sizeCategory);
        }

        private string? FindTemplateImageByTitle(string title)
        {
            var templateDir = PathUtilities.GetTemplateAircraftImagesDirectory();
            if (!Directory.Exists(templateDir))
            {
                return null;
            }

            var safeTitle = MakeSafeFileName(title);
            var patterns = new[]
            {
                $"{safeTitle}_custom.*",
                $"{safeTitle}.*"
            };

            foreach (var pattern in patterns)
            {
                var files = Directory.GetFiles(templateDir, pattern);
                if (files.Length > 0)
                {
                    return files[0];
                }
            }

            return null;
        }

        public string SetCustomImage(string sourceImagePath, string identifier)
        {
            var imagesDir = PathUtilities.GetSavegameAircraftImagesDirectory();
            Directory.CreateDirectory(imagesDir);

            var safeIdentifier = MakeSafeFileName(identifier);
            var extension = Path.GetExtension(sourceImagePath);
            var fileName = $"{safeIdentifier}_custom{extension}";
            var targetPath = Path.Combine(imagesDir, fileName);

            File.Copy(sourceImagePath, targetPath, overwrite: true);
            _log.Info($"AircraftImageService: Set custom image for {identifier}: {fileName}");

            return fileName;
        }

        public string? CopyCustomImage(string? existingCustomImagePath, string newIdentifier)
        {
            if (string.IsNullOrEmpty(existingCustomImagePath))
            {
                return null;
            }

            var imagesDir = PathUtilities.GetSavegameAircraftImagesDirectory();
            var sourcePath = Path.Combine(imagesDir, existingCustomImagePath);

            if (!File.Exists(sourcePath))
            {
                _log.Debug($"AircraftImageService: Source image not found: {sourcePath}");
                return null;
            }

            var safeIdentifier = MakeSafeFileName(newIdentifier);
            var extension = Path.GetExtension(existingCustomImagePath);
            var newFileName = $"{safeIdentifier}_custom{extension}";
            var targetPath = Path.Combine(imagesDir, newFileName);

            File.Copy(sourcePath, targetPath, overwrite: true);
            _log.Info($"AircraftImageService: Copied image for {newIdentifier}: {newFileName}");

            return newFileName;
        }

        public void RemoveCustomImage(string identifier)
        {
            var imagesDir = PathUtilities.GetSavegameAircraftImagesDirectory();
            if (!Directory.Exists(imagesDir))
            {
                return;
            }

            var safeIdentifier = MakeSafeFileName(identifier);
            var pattern = $"{safeIdentifier}_custom.*";

            foreach (var file in Directory.GetFiles(imagesDir, pattern))
            {
                File.Delete(file);
                _log.Info($"AircraftImageService: Removed custom image: {Path.GetFileName(file)}");
            }
        }

        private static string MakeSafeFileName(string input)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var safe = input;
            foreach (var c in invalid)
            {
                safe = safe.Replace(c, '_');
            }
            return safe.Replace(' ', '_');
        }
    }
}
