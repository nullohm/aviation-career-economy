using System.IO;
using Ace.App.Interfaces;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly ILoggingService _log;

        public UserDataService(ILoggingService log)
        {
            _log = log;
        }

        public void EnsureUserDataFiles()
        {
            EnsureSavegameDirectories();
            EnsurePilotImages();
        }

        private void EnsureSavegameDirectories()
        {
            var savegameDir = PathUtilities.GetSavegameDirectory();
            if (!Directory.Exists(savegameDir))
            {
                Directory.CreateDirectory(savegameDir);
                _log.Info($"UserDataService: Created savegame directory: {savegameDir}");
            }

            var imagesDir = PathUtilities.GetSavegameImagesDirectory();
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
                _log.Info($"UserDataService: Created savegame images directory: {imagesDir}");
            }

            var pilotImagesDir = PathUtilities.GetSavegamePilotImagesDirectory();
            if (!Directory.Exists(pilotImagesDir))
            {
                Directory.CreateDirectory(pilotImagesDir);
                _log.Info($"UserDataService: Created pilot images directory: {pilotImagesDir}");
            }

            var aircraftImagesDir = PathUtilities.GetSavegameAircraftImagesDirectory();
            if (!Directory.Exists(aircraftImagesDir))
            {
                Directory.CreateDirectory(aircraftImagesDir);
                _log.Info($"UserDataService: Created aircraft images directory: {aircraftImagesDir}");
            }
        }

        private void EnsurePilotImages()
        {
            var sourceDir = PathUtilities.GetTemplatePilotImagesDirectory();
            var targetDir = PathUtilities.GetSavegamePilotImagesDirectory();

            if (!Directory.Exists(sourceDir))
            {
                _log.Debug($"UserDataService: Template pilot images directory not found: {sourceDir}");
                return;
            }

            var extensions = new[] { "*.bmp", "*.jpg", "*.jpeg", "*.png" };
            int copied = 0;

            foreach (var pattern in extensions)
            {
                foreach (var sourceFile in Directory.GetFiles(sourceDir, pattern))
                {
                    var fileName = Path.GetFileName(sourceFile);
                    var targetFile = Path.Combine(targetDir, fileName);

                    if (!File.Exists(targetFile))
                    {
                        File.Copy(sourceFile, targetFile);
                        copied++;
                    }
                }
            }

            if (copied > 0)
            {
                _log.Info($"UserDataService: Copied {copied} pilot images to savegame folder");
            }
            else
            {
                _log.Debug($"UserDataService: All pilot images already exist in savegame folder");
            }
        }
    }
}
