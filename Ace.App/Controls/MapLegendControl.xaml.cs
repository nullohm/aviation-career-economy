using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ace.App.Controls
{
    public partial class MapLegendControl : UserControl
    {
        public static readonly DependencyProperty LegendEntriesProperty =
            DependencyProperty.Register(nameof(LegendEntries), typeof(List<MapLegendEntry>), typeof(MapLegendControl),
                new PropertyMetadata(null, OnLegendEntriesChanged));

        public List<MapLegendEntry> LegendEntries
        {
            get => (List<MapLegendEntry>)GetValue(LegendEntriesProperty);
            set => SetValue(LegendEntriesProperty, value);
        }

        public MapLegendControl()
        {
            InitializeComponent();
        }

        private static void OnLegendEntriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MapLegendControl control)
            {
                control.UpdateLegend();
            }
        }

        private void UpdateLegend()
        {
            LegendItems.Children.Clear();

            if (LegendEntries == null) return;

            foreach (var entry in LegendEntries)
            {
                var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 4) };

                if (entry.IsLine)
                {
                    var line = new Line
                    {
                        X1 = 0,
                        Y1 = 5,
                        X2 = 16,
                        Y2 = 5,
                        Stroke = entry.Color,
                        StrokeThickness = entry.IsDashed ? 2 : 3,
                        StrokeDashArray = entry.IsDashed ? new DoubleCollection { 2, 2 } : null,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 8, 0)
                    };
                    row.Children.Add(line);
                }
                else
                {
                    var ellipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = entry.Color,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 8, 0)
                    };
                    row.Children.Add(ellipse);
                }

                var label = new TextBlock
                {
                    Text = entry.Label,
                    Foreground = Brushes.White,
                    FontSize = 11,
                    VerticalAlignment = VerticalAlignment.Center
                };
                row.Children.Add(label);

                LegendItems.Children.Add(row);
            }
        }

        public void SetEntries(List<MapLegendEntry> entries)
        {
            LegendEntries = entries;
        }
    }

    public class MapLegendEntry
    {
        public string Label { get; set; } = string.Empty;
        public Brush Color { get; set; } = Brushes.White;
        public bool IsLine { get; set; }
        public bool IsDashed { get; set; }

        public MapLegendEntry(string label, Brush color, bool isLine = false, bool isDashed = false)
        {
            Label = label;
            Color = color;
            IsLine = isLine;
            IsDashed = isDashed;
        }
    }
}
