using System;
using System.IO;
using System.Threading;
using Ace.App.Interfaces;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class LoggingService : ILoggingService, IDisposable
    {
        private readonly object _sync = new();
        private StreamWriter? _writer;
        private string? _logPath;
        private bool _initialized;

        public LoggingService() { }

        public void Initialize(string? logsDir = null)
        {
            lock (_sync)
            {
                if (_initialized) return;

                try
                {
                    var root = PathUtilities.FindSolutionRoot();
                    if (root == null)
                    {
                        var dirInfo = new DirectoryInfo(AppContext.BaseDirectory);
                        for (int i = 0; i < 6 && dirInfo != null; i++)
                        {
                            if (Directory.Exists(Path.Combine(dirInfo.FullName, "Docs")) ||
                                Directory.Exists(Path.Combine(dirInfo.FullName, "Savegames")) ||
                                Directory.Exists(Path.Combine(dirInfo.FullName, "Logs")))
                            {
                                root = dirInfo.FullName;
                                break;
                            }
                            dirInfo = dirInfo.Parent;
                        }
                    }

                    var dir = logsDir ?? Path.Combine(root ?? AppContext.BaseDirectory, "Logs");
                    Directory.CreateDirectory(dir);
                    var fileName = $"ace_{DateTime.Now:yyyyMMdd_HHmmss}.log";
                    _logPath = Path.Combine(dir, fileName);
                    var fs = new FileStream(_logPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                    _writer = new StreamWriter(fs) { AutoFlush = true };
                    _initialized = true;
                    LogInternal("INFO", "Logging initialized. Logfile: " + _logPath);
                }
                catch (Exception ex)
                {
                    try
                    {
                        Console.Error.WriteLine("Failed to initialize logger: " + ex);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("Console.Error failed during logger initialization");
                    }
                }
            }
        }

        private void LogInternal(string level, string message)
        {
            try
            {
                lock (_sync)
                {
                    if (_writer == null)
                    {
                        Console.WriteLine($"{DateTime.Now:o} [{level}] {message}");
                        return;
                    }

                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    _writer.WriteLine($"{DateTime.Now:o} [{level}] [T{threadId}] {message}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logging failed: {ex.Message}");
            }
        }

        public void Info(string message) => LogInternal("INFO", message);
        public void Debug(string message) => LogInternal("DEBUG", message);
        public void Warn(string message) => LogInternal("WARN", message);
        public void Error(string message, Exception? ex = null)
        {
            var full = message + (ex != null ? " Exception: " + ex : string.Empty);
            LogInternal("ERROR", full);
        }

        public void Database(string operation, int? recordCount = null)
        {
            var msg = recordCount.HasValue
                ? $"[DB] {operation} - {recordCount} records"
                : $"[DB] {operation}";
            LogInternal("DATABASE", msg);
        }

        public void Market(string operation, string aircraftName = "", decimal? price = null)
        {
            var msg = $"[MARKET] {operation}";
            if (!string.IsNullOrEmpty(aircraftName))
                msg += $" - {aircraftName}";
            if (price.HasValue)
                msg += $" - {price:N0} €";
            LogInternal("MARKET", msg);
        }

        public void Dispose()
        {
            lock (_sync)
            {
                try
                {
                    _writer?.Flush();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Logger flush failed: {ex.Message}");
                }
                try
                {
                    _writer?.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Logger dispose failed: {ex.Message}");
                }
                _writer = null;
                _initialized = false;
            }
        }
    }
}
