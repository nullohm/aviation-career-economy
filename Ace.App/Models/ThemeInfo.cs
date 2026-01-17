namespace Ace.App.Models;

public record ThemeInfo(
    string Name,
    string DisplayName,
    string FilePath,
    bool IsBuiltIn,
    bool IsCustom
);
