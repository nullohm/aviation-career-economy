using System.Text.Json.Serialization;

namespace Ace.App.Models;

public class ThemeColors
{
    [JsonPropertyName("_meta")]
    public ThemeMeta? Meta { get; set; }

    [JsonPropertyName("colors")]
    public ThemeColorValues? Colors { get; set; }
}

public class ThemeMeta
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "Unknown";

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = "Unknown Theme";

    [JsonPropertyName("author")]
    public string Author { get; set; } = "Unknown";

    [JsonPropertyName("version")]
    public string Version { get; set; } = "1.0";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}

public class ThemeColorValues
{
    [JsonPropertyName("bgPrimary")]
    public string BgPrimary { get; set; } = "#FF0D1117";

    [JsonPropertyName("bgSecondary")]
    public string BgSecondary { get; set; } = "#FF161B22";

    [JsonPropertyName("cardBg")]
    public string CardBg { get; set; } = "#FF21262D";

    [JsonPropertyName("accent")]
    public string Accent { get; set; } = "#FF58A6FF";

    [JsonPropertyName("accentSecondary")]
    public string AccentSecondary { get; set; } = "#FF238636";

    [JsonPropertyName("accentGradientStart")]
    public string AccentGradientStart { get; set; } = "#FF58A6FF";

    [JsonPropertyName("accentGradientEnd")]
    public string AccentGradientEnd { get; set; } = "#FF1F6FEB";

    [JsonPropertyName("foreground")]
    public string Foreground { get; set; } = "#FFF0F6FC";

    [JsonPropertyName("subtleForeground")]
    public string SubtleForeground { get; set; } = "#FF8B949E";

    [JsonPropertyName("border")]
    public string Border { get; set; } = "#FF30363D";

    [JsonPropertyName("success")]
    public string Success { get; set; } = "#FF238636";

    [JsonPropertyName("successGradientStart")]
    public string SuccessGradientStart { get; set; } = "#FF2EA043";

    [JsonPropertyName("successGradientEnd")]
    public string SuccessGradientEnd { get; set; } = "#FF238636";

    [JsonPropertyName("warning")]
    public string Warning { get; set; } = "#FFD29922";

    [JsonPropertyName("danger")]
    public string Danger { get; set; } = "#FFF85149";

    [JsonPropertyName("purpleAccent")]
    public string PurpleAccent { get; set; } = "#FF8957E5";

    [JsonPropertyName("cyanAccent")]
    public string CyanAccent { get; set; } = "#FF79C0FF";

    [JsonPropertyName("gold")]
    public string Gold { get; set; } = "#FFD4AF37";

    [JsonPropertyName("epauletteBackground")]
    public string EpauletteBackground { get; set; } = "#FF1A1A1A";

    [JsonPropertyName("glass")]
    public string Glass { get; set; } = "#1AFFFFFF";

    [JsonPropertyName("glassBorder")]
    public string GlassBorder { get; set; } = "#33FFFFFF";

    [JsonPropertyName("navHover")]
    public string NavHover { get; set; } = "#FF30363D";

    [JsonPropertyName("navSelected")]
    public string NavSelected { get; set; } = "#FF21262D";
}
