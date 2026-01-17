using System;
using System.IO;
using FluentAssertions;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    /// <summary>
    /// Unit tests for LoggingService
    /// </summary>
    public class LoggingServiceTests : IDisposable
    {
        private readonly LoggingService _loggingService;
        private readonly string _testLogDir;

        public LoggingServiceTests()
        {
            // Use a unique test directory for each test run
            _testLogDir = Path.Combine(Path.GetTempPath(), "AceTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testLogDir);
            _loggingService = new LoggingService();
        }

        public void Dispose()
        {
            _loggingService?.Dispose();

            // Cleanup test directory
            try
            {
                if (Directory.Exists(_testLogDir))
                {
                    Directory.Delete(_testLogDir, true);
                }
            }
            catch
            {
                // Best effort cleanup
            }
        }

        [Fact]
        public void Constructor_ShouldNotThrow()
        {
            // Act & Assert
            Action act = () => new LoggingService();
            act.Should().NotThrow();
        }

        [Fact]
        public void Initialize_ShouldCreateLogFile()
        {
            // Act
            _loggingService.Initialize(_testLogDir);

            // Assert
            var files = Directory.GetFiles(_testLogDir, "*.log");
            files.Should().NotBeEmpty();
        }

        [Fact]
        public void Initialize_ShouldNotThrow_WhenCalledMultipleTimes()
        {
            // Act
            Action act = () =>
            {
                _loggingService.Initialize(_testLogDir);
                _loggingService.Initialize(_testLogDir);
                _loggingService.Initialize(_testLogDir);
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Info_ShouldNotThrow()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Info("Test info message");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Debug_ShouldNotThrow()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Debug("Test debug message");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Warn_ShouldNotThrow()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Warn("Test warning message");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Error_ShouldNotThrow_WithoutException()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Error("Test error message");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Error_ShouldNotThrow_WithException()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);
            var exception = new InvalidOperationException("Test exception");

            // Act
            Action act = () => _loggingService.Error("Test error message", exception);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Database_ShouldNotThrow_WithoutRecordCount()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Database("SELECT * FROM Aircraft");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Database_ShouldNotThrow_WithRecordCount()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Database("SELECT * FROM Aircraft", 42);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Market_ShouldNotThrow_WithMinimalParameters()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Market("Aircraft listed");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Market_ShouldNotThrow_WithAircraftName()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Market("Aircraft listed", "Cessna 172");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Market_ShouldNotThrow_WithPrice()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () => _loggingService.Market("Aircraft sold", "Cessna 172", 50000m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_ShouldNotThrow()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);
            _loggingService.Info("Test message");

            // Act
            Action act = () => _loggingService.Dispose();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Dispose_ShouldNotThrow_WhenCalledMultipleTimes()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);

            // Act
            Action act = () =>
            {
                _loggingService.Dispose();
                _loggingService.Dispose();
                _loggingService.Dispose();
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Info_ShouldNotThrow_BeforeInitialization()
        {
            // Arrange - Don't initialize
            var service = new LoggingService();

            // Act
            Action act = () => service.Info("Test message");

            // Assert
            act.Should().NotThrow();

            service.Dispose();
        }

        [Fact]
        public void Info_ShouldWriteToLogFile()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);
            var message = "Unique test message " + Guid.NewGuid();

            // Act
            _loggingService.Info(message);
            _loggingService.Dispose(); // Flush the log

            // Assert
            var logFiles = Directory.GetFiles(_testLogDir, "*.log");
            logFiles.Should().NotBeEmpty();
            var logContent = File.ReadAllText(logFiles[0]);
            logContent.Should().Contain(message);
            logContent.Should().Contain("INFO");
        }

        [Fact]
        public void Error_ShouldIncludeExceptionInLog()
        {
            // Arrange
            _loggingService.Initialize(_testLogDir);
            var errorMessage = "Test error " + Guid.NewGuid();
            var exception = new InvalidOperationException("Test exception details");

            // Act
            _loggingService.Error(errorMessage, exception);
            _loggingService.Dispose(); // Flush the log

            // Assert
            var logFiles = Directory.GetFiles(_testLogDir, "*.log");
            var logContent = File.ReadAllText(logFiles[0]);
            logContent.Should().Contain(errorMessage);
            logContent.Should().Contain("ERROR");
            logContent.Should().Contain("Exception");
        }

        [Fact]
        public async System.Threading.Tasks.Task LogMethods_ShouldHandleConcurrentAccess()
        {
            _loggingService.Initialize(_testLogDir);

            var tasks = new System.Threading.Tasks.Task[10];
            for (int i = 0; i < tasks.Length; i++)
            {
                var index = i;
                tasks[i] = System.Threading.Tasks.Task.Run(() =>
                {
                    _loggingService.Info($"Concurrent message {index}");
                    _loggingService.Debug($"Debug message {index}");
                    _loggingService.Warn($"Warning message {index}");
                });
            }

            await System.Threading.Tasks.Task.WhenAll(tasks);
            _loggingService.Dispose();

            var logFiles = Directory.GetFiles(_testLogDir, "*.log");
            logFiles.Should().NotBeEmpty();
        }

        [Fact]
        public void Initialize_ShouldCreateLogsDirectory_IfNotExists()
        {
            // Arrange
            var newDir = Path.Combine(_testLogDir, "NewSubDir");
            Directory.Exists(newDir).Should().BeFalse();

            // Act
            _loggingService.Initialize(newDir);

            // Assert
            Directory.Exists(newDir).Should().BeTrue();
        }

        [Fact]
        public void LogFileName_ShouldContainTimestamp()
        {
            // Act
            _loggingService.Initialize(_testLogDir);

            // Assert
            var logFiles = Directory.GetFiles(_testLogDir, "ace_*.log");
            logFiles.Should().NotBeEmpty();
            logFiles[0].Should().MatchRegex(@"ace_\d{8}_\d{6}\.log");
        }
    }
}
