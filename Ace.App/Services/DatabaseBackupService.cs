using System;
using System.IO;
using System.Linq;
using Ace.App.Interfaces;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class DatabaseBackupService : IDatabaseBackupService
    {
        private readonly ILoggingService _logger;
        private const int MaxBackupCount = 10;

        public DatabaseBackupService(ILoggingService logger)
        {
            _logger = logger;
        }

        public void CreateBackup()
        {
            try
            {
                var root = PathUtilities.FindSolutionRoot() ?? AppContext.BaseDirectory;
                var savegamesDir = Path.Combine(root, "Savegames");
                var currentDir = Path.Combine(savegamesDir, "Current");

                if (!Directory.Exists(currentDir))
                {
                    _logger.Debug("DatabaseBackupService: No Current savegame folder found to backup");
                    return;
                }

                var filesToBackup = Directory.GetFiles(currentDir, "*", SearchOption.AllDirectories);
                if (filesToBackup.Length == 0)
                {
                    _logger.Debug("DatabaseBackupService: No files in Current folder to backup");
                    return;
                }

                var backupsDir = Path.Combine(savegamesDir, "Backups");
                if (!Directory.Exists(backupsDir))
                {
                    Directory.CreateDirectory(backupsDir);
                    _logger.Info($"DatabaseBackupService: Created backups directory at {backupsDir}");
                }

                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var backupFolderName = $"ace_backup_{timestamp}";
                var backupPath = Path.Combine(backupsDir, backupFolderName);

                CopyDirectory(currentDir, backupPath);

                _logger.Info($"DatabaseBackupService: Backup created at {backupPath} ({filesToBackup.Length} files)");

                CleanupOldBackups(backupsDir);
            }
            catch (Exception ex)
            {
                _logger.Error($"DatabaseBackupService: Failed to create backup: {ex.Message}");
            }
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var fileName = Path.GetFileName(file);
                var destPath = Path.Combine(destDir, fileName);
                File.Copy(file, destPath);
            }

            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                var dirName = Path.GetFileName(subDir);
                var destSubDir = Path.Combine(destDir, dirName);
                CopyDirectory(subDir, destSubDir);
            }
        }

        private void CleanupOldBackups(string backupsDir)
        {
            try
            {
                var backupFolders = new DirectoryInfo(backupsDir)
                    .GetDirectories("ace_backup_*")
                    .OrderByDescending(d => d.CreationTime)
                    .ToArray();

                if (backupFolders.Length <= MaxBackupCount)
                {
                    return;
                }

                for (var i = MaxBackupCount; i < backupFolders.Length; i++)
                {
                    backupFolders[i].Delete(recursive: true);
                    _logger.Debug($"DatabaseBackupService: Deleted old backup {backupFolders[i].Name}");
                }

                _logger.Info($"DatabaseBackupService: Cleaned up {backupFolders.Length - MaxBackupCount} old backup(s)");
            }
            catch (Exception ex)
            {
                _logger.Error($"DatabaseBackupService: Failed to cleanup old backups: {ex.Message}");
            }
        }
    }
}
