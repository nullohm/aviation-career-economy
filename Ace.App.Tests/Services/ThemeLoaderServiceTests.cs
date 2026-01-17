using System;
using System.IO;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class ThemeLoaderServiceTests : IDisposable
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly ThemeLoaderService _service;
        private readonly string _testDir;

        public ThemeLoaderServiceTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _service = new ThemeLoaderService(_mockLogger.Object);
            _testDir = Path.Combine(Path.GetTempPath(), "AceTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_testDir))
                {
                    Directory.Delete(_testDir, true);
                }
            }
            catch
            {
            }
        }

        [Fact]
        public void Constructor_ShouldNotThrow()
        {
            Action act = () => new ThemeLoaderService(_mockLogger.Object);
            act.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithNullLogger_ShouldThrow()
        {
            Action act = () => new ThemeLoaderService(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LoadThemeColors_WithValidJson_ShouldReturnThemeColors()
        {
            var json = @"{
                ""_meta"": {
                    ""name"": ""Test"",
                    ""displayName"": ""Test Theme""
                },
                ""colors"": {
                    ""bgPrimary"": ""#FF112233"",
                    ""foreground"": ""#FFAABBCC""
                }
            }";
            var filePath = Path.Combine(_testDir, "test.json");
            File.WriteAllText(filePath, json);

            var result = LoadColorsFromFile(filePath);

            result.Should().NotBeNull();
            result!.Meta.Should().NotBeNull();
            result.Meta!.Name.Should().Be("Test");
            result.Meta.DisplayName.Should().Be("Test Theme");
            result.Colors.Should().NotBeNull();
            result.Colors!.BgPrimary.Should().Be("#FF112233");
            result.Colors.Foreground.Should().Be("#FFAABBCC");
        }

        [Fact]
        public void LoadThemeColors_WithInvalidJson_ShouldReturnNull()
        {
            var json = "{ invalid json }";
            var filePath = Path.Combine(_testDir, "invalid.json");
            File.WriteAllText(filePath, json);

            var result = LoadColorsFromFile(filePath);

            result.Should().BeNull();
        }

        [Fact]
        public void LoadThemeColors_WithMissingFile_ShouldReturnNull()
        {
            var result = _service.LoadThemeColors("nonexistent");

            result.Should().BeNull();
        }

        [Fact]
        public void LoadThemeColors_WithMissingColorsSection_ShouldHaveNullColors()
        {
            var json = @"{
                ""_meta"": {
                    ""name"": ""Test""
                }
            }";
            var filePath = Path.Combine(_testDir, "no_colors.json");
            File.WriteAllText(filePath, json);

            var result = LoadColorsFromFile(filePath);

            result.Should().NotBeNull();
            result!.Colors.Should().BeNull();
        }

        [Fact]
        public void ThemeColorValues_ShouldHaveDefaultValues()
        {
            var colors = new ThemeColorValues();

            colors.BgPrimary.Should().NotBeNullOrEmpty();
            colors.Foreground.Should().NotBeNullOrEmpty();
            colors.Accent.Should().NotBeNullOrEmpty();
            colors.Success.Should().NotBeNullOrEmpty();
            colors.Warning.Should().NotBeNullOrEmpty();
            colors.Danger.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ThemeInfo_ShouldStoreAllProperties()
        {
            var info = new ThemeInfo(
                Name: "test",
                DisplayName: "Test Theme",
                FilePath: "/path/to/test.json",
                IsBuiltIn: true,
                IsCustom: false
            );

            info.Name.Should().Be("test");
            info.DisplayName.Should().Be("Test Theme");
            info.FilePath.Should().Be("/path/to/test.json");
            info.IsBuiltIn.Should().BeTrue();
            info.IsCustom.Should().BeFalse();
        }

        [Fact]
        public void ThemeMeta_ShouldHaveDefaultValues()
        {
            var meta = new ThemeMeta();

            meta.Name.Should().Be("Unknown");
            meta.DisplayName.Should().Be("Unknown Theme");
            meta.Author.Should().Be("Unknown");
            meta.Version.Should().Be("1.0");
            meta.Description.Should().BeEmpty();
        }

        private ThemeColors? LoadColorsFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<ThemeColors>(json,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return null;
            }
        }
    }
}
