using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ace.App.Controls
{
    public partial class MapLegendPanelControl : UserControl
    {
        private bool _isExpanded = true;

        public event System.Action<bool>? ExpandedStateChanged;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    UpdateExpandedState();
                }
            }
        }

        public MapLegendPanelControl()
        {
            InitializeComponent();
        }

        private void Header_Click(object sender, MouseButtonEventArgs e)
        {
            _isExpanded = !_isExpanded;
            UpdateExpandedState();
            ExpandedStateChanged?.Invoke(_isExpanded);
        }

        private void UpdateExpandedState()
        {
            LegendContent.Visibility = _isExpanded ? Visibility.Visible : Visibility.Collapsed;
            CollapseIcon.Text = _isExpanded ? "▼" : "▶";
        }

        public void SetRouteLegend(List<MapLegendEntry> entries)
        {
            RouteLegendItems.Children.Clear();

            if (entries == null || entries.Count == 0)
            {
                RouteLegendSection.Visibility = Visibility.Collapsed;
                return;
            }

            foreach (var entry in entries)
            {
                AddLegendItem(RouteLegendItems, entry);
            }

            RouteLegendSection.Visibility = Visibility.Visible;
        }

        public void SetFBOLegend(List<MapLegendEntry> entries)
        {
            FBOLegendItems.Children.Clear();

            if (entries == null || entries.Count == 0)
            {
                FBOLegendSection.Visibility = Visibility.Collapsed;
                return;
            }

            foreach (var entry in entries)
            {
                AddLegendItem(FBOLegendItems, entry);
            }

            FBOLegendSection.Visibility = Visibility.Visible;
        }

        public void SetRunwayLegend(List<MapLegendEntry> entries)
        {
            RunwayLegendItems.Children.Clear();

            if (entries == null || entries.Count == 0)
            {
                RunwayLegendSection.Visibility = Visibility.Collapsed;
                return;
            }

            foreach (var entry in entries)
            {
                AddLegendItem(RunwayLegendItems, entry);
            }

            RunwayLegendSection.Visibility = Visibility.Visible;
        }

        public void ShowTerrainLegend(bool show)
        {
            TerrainLegendSection.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            RunwayLegendSection.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        public void ShowAirspaceLegend(bool show)
        {
            AirspaceLegendSection.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void AddLegendItem(Panel container, MapLegendEntry entry)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2, 0, 0) };

            if (entry.IsLine)
            {
                var line = new Line
                {
                    X1 = 0,
                    Y1 = 5,
                    X2 = 14,
                    Y2 = 5,
                    Stroke = entry.Color,
                    StrokeThickness = entry.IsDashed ? 2 : 3,
                    StrokeDashArray = entry.IsDashed ? new DoubleCollection { 2, 2 } : null,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 6, 0)
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
                    Margin = new Thickness(0, 0, 6, 0)
                };
                row.Children.Add(ellipse);
            }

            var label = new TextBlock
            {
                Text = entry.Label,
                Foreground = Brushes.White,
                FontSize = 9,
                VerticalAlignment = VerticalAlignment.Center
            };
            row.Children.Add(label);

            container.Children.Add(row);
        }
    }
}
