using System;

namespace Ace.App.Interfaces
{
    public interface ILoggingService
    {
        void Initialize(string? logsDir = null);
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Error(string message, Exception? ex = null);
        void Database(string operation, int? recordCount = null);
    }
}
