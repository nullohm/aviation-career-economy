using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Ace.App.Models;

namespace Ace.App.Controls
{
    public enum RankBadgeSize
    {
        Small,
        Medium,
        Large
    }

    public partial class RankBadgeControl : UserControl
    {
        public static readonly DependencyProperty RankProperty =
            DependencyProperty.Register(
                nameof(Rank),
                typeof(PilotRankType),
                typeof(RankBadgeControl),
                new PropertyMetadata(PilotRankType.Junior, OnRankChanged));

        public static readonly DependencyProperty BadgeSizeProperty =
            DependencyProperty.Register(
                nameof(BadgeSize),
                typeof(RankBadgeSize),
                typeof(RankBadgeControl),
                new PropertyMetadata(RankBadgeSize.Small, OnBadgeSizeChanged));

        public PilotRankType Rank
        {
            get => (PilotRankType)GetValue(RankProperty);
            set => SetValue(RankProperty, value);
        }

        public RankBadgeSize BadgeSize
        {
            get => (RankBadgeSize)GetValue(BadgeSizeProperty);
            set => SetValue(BadgeSizeProperty, value);
        }

        public RankBadgeControl()
        {
            InitializeComponent();
            Loaded += (_, _) => UpdateVisual();
        }

        private static void OnRankChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RankBadgeControl control)
                control.UpdateVisual();
        }

        private static void OnBadgeSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RankBadgeControl control)
                control.UpdateVisual();
        }

        private void UpdateVisual()
        {
            StripesContainer.Children.Clear();

            var (width, height, stripeHeight, stripeSpacing, diamondSize) = GetSizeDimensions();

            EpauletteBorder.Width = width;
            EpauletteBorder.Height = height;

            int stripeCount = GetStripeCount();
            bool showDiamond = Rank == PilotRankType.ChiefPilot;

            double totalStripesHeight = stripeCount * stripeHeight + (stripeCount - 1) * stripeSpacing;
            double diamondHeight = showDiamond ? diamondSize + stripeSpacing : 0;
            double totalContentHeight = totalStripesHeight + diamondHeight;
            double startY = (height - 4 - totalContentHeight) / 2;

            if (showDiamond)
            {
                var diamondCanvas = new Canvas
                {
                    Width = width - 4,
                    Height = diamondSize
                };

                var diamond = new Polygon
                {
                    Fill = FindResource("GoldBrush") as Brush,
                    Points = new PointCollection
                    {
                        new Point(diamondSize / 2, 0),
                        new Point(diamondSize, diamondSize / 2),
                        new Point(diamondSize / 2, diamondSize),
                        new Point(0, diamondSize / 2)
                    }
                };

                double diamondX = (width - 4 - diamondSize) / 2;
                Canvas.SetLeft(diamond, diamondX);
                Canvas.SetTop(diamond, 0);
                diamondCanvas.Children.Add(diamond);

                diamondCanvas.Margin = new Thickness(0, startY, 0, 0);
                diamondCanvas.VerticalAlignment = VerticalAlignment.Top;
                StripesContainer.Children.Add(diamondCanvas);

                startY += diamondSize + stripeSpacing;
            }

            var stripesPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, startY, 0, 0)
            };

            for (int i = 0; i < stripeCount; i++)
            {
                var stripe = new Border
                {
                    Height = stripeHeight,
                    Background = FindResource("GoldBrush") as Brush,
                    CornerRadius = new CornerRadius(1),
                    Margin = new Thickness(0, i > 0 ? stripeSpacing : 0, 0, 0)
                };
                stripesPanel.Children.Add(stripe);
            }

            StripesContainer.Children.Add(stripesPanel);
        }

        private int GetStripeCount()
        {
            return Rank switch
            {
                PilotRankType.Junior => 1,
                PilotRankType.Senior => 2,
                PilotRankType.Captain => 3,
                PilotRankType.SeniorCaptain => 4,
                PilotRankType.ChiefPilot => 4,
                _ => 1
            };
        }

        private (double width, double height, double stripeHeight, double stripeSpacing, double diamondSize) GetSizeDimensions()
        {
            return BadgeSize switch
            {
                RankBadgeSize.Small => (14, 22, 2, 2, 5),
                RankBadgeSize.Medium => (18, 28, 3, 2, 6),
                RankBadgeSize.Large => (24, 38, 4, 3, 8),
                _ => (14, 22, 2, 2, 5)
            };
        }
    }
}
