using System.Collections.Generic;
using Mapsui;
using Mapsui.Styles;

namespace Ace.App.Helpers
{
    public static class AircraftMapSymbolHelper
    {
        public static IEnumerable<IStyle> CreateAircraftStyle(double heading, double scale = 0.6, bool showLabel = false, string? label = null)
        {
            var styles = new List<IStyle>();

            styles.Add(new SymbolStyle
            {
                Fill = new Brush(Color.FromString("#FFD700")),
                Outline = new Pen(Color.FromString("#B8860B"), 2),
                SymbolScale = scale,
                SymbolType = SymbolType.Triangle,
                SymbolRotation = heading
            });

            styles.Add(new SymbolStyle
            {
                Fill = null,
                Outline = new Pen(Color.White, 1),
                SymbolScale = scale * 0.85,
                SymbolType = SymbolType.Triangle,
                SymbolRotation = heading
            });

            if (showLabel && !string.IsNullOrEmpty(label))
            {
                styles.Add(new LabelStyle
                {
                    Text = label,
                    ForeColor = Color.White,
                    BackColor = new Brush(Color.FromArgb(200, 0, 0, 0)),
                    Offset = new Offset(0, -30),
                    Font = new Font { Size = 11, Bold = true }
                });
            }

            return styles;
        }

        public static IEnumerable<IStyle> CreateAircraftStyleCompact(double heading)
        {
            return new List<IStyle>
            {
                new SymbolStyle
                {
                    Fill = new Brush(Color.FromString("#FFD700")),
                    Outline = new Pen(Color.FromString("#000000"), 2),
                    SymbolScale = 0.5,
                    SymbolType = SymbolType.Triangle,
                    SymbolRotation = heading
                }
            };
        }
    }
}
