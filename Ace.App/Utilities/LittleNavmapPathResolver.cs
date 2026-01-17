using System;
using System.Collections.Generic;
using System.IO;
using Ace.App.Interfaces;

namespace Ace.App.Utilities
{
    public static class LittleNavmapPathResolver
    {
        private static readonly string DefaultLnmDbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ABarthel", "little_navmap_db");

        public static string? FindDatabase(ISettingsService settingsService, string databaseFileName)
        {
            var customPath = settingsService.CurrentSettings.LittleNavmapDatabasePath;

            var searchPaths = new List<string>();

            if (!string.IsNullOrEmpty(customPath) && Directory.Exists(customPath))
            {
                searchPaths.Add(Path.Combine(customPath, databaseFileName));
            }

            searchPaths.Add(Path.Combine(DefaultLnmDbPath, databaseFileName));

            foreach (var path in searchPaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        public static string? FindAirportDatabase(ISettingsService settingsService)
        {
            var dbNames = new[] { "little_navmap_msfs24.sqlite", "little_navmap_navigraph.sqlite", "little_navmap_.sqlite" };

            foreach (var dbName in dbNames)
            {
                var path = FindDatabase(settingsService, dbName);
                if (path != null)
                    return path;
            }

            return null;
        }

        public static string? FindNavigraphDatabase(ISettingsService settingsService)
        {
            return FindDatabase(settingsService, "little_navmap_navigraph.sqlite");
        }

        public static string GetDefaultPath() => DefaultLnmDbPath;
    }
}
