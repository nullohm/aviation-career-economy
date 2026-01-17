using System;
using System.IO;

namespace Ace.App.Utilities
{
    public static class PathUtilities
    {
        public static string? FindSolutionRoot()
        {
            try
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                while (dir != null)
                {
                    var slnFiles = dir.GetFiles("*.sln");
                    if (slnFiles.Length > 0)
                    {
                        return dir.FullName;
                    }
                    dir = dir.Parent;
                }
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is IOException)
            {
                System.Diagnostics.Debug.WriteLine($"PathUtilities: Could not find solution root: {ex.Message}");
                return null;
            }
            return null;
        }

        public static string GetLogsDirectory()
        {
            var solutionRoot = FindSolutionRoot();
            return Path.Combine(solutionRoot ?? AppContext.BaseDirectory, "Logs");
        }

        public static string GetDataDirectory()
        {
            var solutionRoot = FindSolutionRoot();
            return Path.Combine(solutionRoot ?? AppContext.BaseDirectory, "Data");
        }

        public static string GetTemplateDbPath()
        {
            return Path.Combine(GetDataDirectory(), "template.db");
        }

        public static string GetTemplateImagesDirectory()
        {
            return Path.Combine(GetDataDirectory(), "Images");
        }

        public static string GetTemplatePilotImagesDirectory()
        {
            return Path.Combine(GetTemplateImagesDirectory(), "Pilots");
        }

        public static string GetTemplateAircraftImagesDirectory()
        {
            return Path.Combine(GetTemplateImagesDirectory(), "Aircraft");
        }

        public static string GetSavegameDirectory()
        {
            var solutionRoot = FindSolutionRoot();
            return Path.Combine(solutionRoot ?? AppContext.BaseDirectory, "Savegames", "Current");
        }

        public static string GetSavegameDbPath()
        {
            return Path.Combine(GetSavegameDirectory(), "ace.db");
        }

        public static string GetSavegameImagesDirectory()
        {
            return Path.Combine(GetSavegameDirectory(), "Images");
        }

        public static string GetSavegamePilotImagesDirectory()
        {
            return Path.Combine(GetSavegameImagesDirectory(), "Pilots");
        }

        public static string GetSavegameAircraftImagesDirectory()
        {
            return Path.Combine(GetSavegameImagesDirectory(), "Aircraft");
        }

        public static string GetDefaultPilotImagePath()
        {
            return Path.Combine(GetSavegamePilotImagesDirectory(), "myPilot.bmp");
        }

        public static string GetPilotImagePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return GetDefaultPilotImagePath();

            if (Path.IsPathRooted(fileName))
                return fileName;

            return Path.Combine(GetSavegamePilotImagesDirectory(), fileName);
        }

        public static string GetDefaultAircraftImage(string sizeCategory)
        {
            var fileName = sizeCategory.ToLower().Replace("-", "_").Replace(" ", "_") + ".png";
            return Path.Combine(GetTemplateAircraftImagesDirectory(), fileName);
        }

        public static string GetManualPath()
        {
            var solutionRoot = FindSolutionRoot();
            return Path.Combine(solutionRoot ?? AppContext.BaseDirectory, "MANUAL.md");
        }

        public static string GetAirspacesDirectory()
        {
            return Path.Combine(GetDataDirectory(), "Airspaces");
        }

        public static string GetAirportImagesDirectory()
        {
            return Path.Combine(GetDataDirectory(), "Images", "Airports");
        }

        public static string GetThemesDirectory()
        {
            return Path.Combine(GetDataDirectory(), "Themes");
        }

        public static string GetCustomThemesDirectory()
        {
            return Path.Combine(GetThemesDirectory(), "custom");
        }
    }
}
