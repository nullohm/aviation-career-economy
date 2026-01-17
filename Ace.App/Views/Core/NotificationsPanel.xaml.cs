using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Core
{
    public partial class NotificationsPanel : UserControl
    {
        private readonly INotificationService _notificationService;

        public NotificationsPanel()
        {
            InitializeComponent();

            _notificationService = ServiceLocator.GetService<INotificationService>();
            _notificationService.NotificationsChanged += OnNotificationsChanged;

            Loaded += NotificationsPanel_Loaded;
        }

        private void NotificationsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            _notificationService.RefreshNotifications();
        }

        private void OnNotificationsChanged()
        {
            Dispatcher.Invoke(() =>
            {
                var notifications = _notificationService.GetAllNotifications();
                var dangerCount = _notificationService.GetDangerCount();
                var warningCount = _notificationService.GetWarningCount();

                DangerBadge.Visibility = dangerCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                TxtDangerCount.Text = dangerCount.ToString();

                WarningBadge.Visibility = warningCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                TxtWarningCount.Text = warningCount.ToString();

                TxtNoNotifications.Visibility = notifications.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                RenderNotifications(notifications);
            });
        }

        private void RenderNotifications(List<Notification> notifications)
        {
            NotificationsList.Children.Clear();

            if (notifications.Count == 0)
            {
                NotificationsList.Children.Add(TxtNoNotifications);
                return;
            }

            foreach (var notification in notifications)
            {
                var item = CreateNotificationItem(notification);
                NotificationsList.Children.Add(item);
            }
        }

        private Border CreateNotificationItem(Notification notification)
        {
            var borderBrush = notification.Type switch
            {
                NotificationType.Danger => (Brush)FindResource("DangerBrush"),
                NotificationType.Warning => (Brush)FindResource("WarningBrush"),
                NotificationType.Success => (Brush)FindResource("SuccessBrush"),
                _ => (Brush)FindResource("AccentBrush")
            };

            var iconColor = notification.Type switch
            {
                NotificationType.Danger => (Brush)FindResource("DangerBrush"),
                NotificationType.Warning => (Brush)FindResource("WarningBrush"),
                NotificationType.Success => (Brush)FindResource("SuccessBrush"),
                _ => (Brush)FindResource("AccentBrush")
            };

            var border = new Border
            {
                Background = (Brush)FindResource("CardBg"),
                BorderBrush = borderBrush,
                BorderThickness = new Thickness(3, 0, 0, 0),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(0, 0, 0, 8)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var icon = new TextBlock
            {
                Text = notification.Icon,
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                FontSize = 20,
                Foreground = iconColor,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 12, 0)
            };
            Grid.SetColumn(icon, 0);
            grid.Children.Add(icon);

            var textStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

            var title = new TextBlock
            {
                Text = notification.Title,
                FontWeight = FontWeights.SemiBold,
                FontSize = 13,
                Foreground = (Brush)FindResource("ForegroundBrush")
            };
            textStack.Children.Add(title);

            var message = new TextBlock
            {
                Text = notification.Message,
                FontSize = 12,
                Foreground = (Brush)FindResource("SubtleForegroundBrush"),
                TextWrapping = TextWrapping.Wrap
            };
            textStack.Children.Add(message);

            Grid.SetColumn(textStack, 1);
            grid.Children.Add(textStack);

            border.Child = grid;
            return border;
        }
    }
}
