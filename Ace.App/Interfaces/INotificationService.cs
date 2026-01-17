using System;
using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface INotificationService
    {
        event Action? NotificationsChanged;

        List<Notification> GetAllNotifications();
        List<Notification> GetNotificationsByCategory(NotificationCategory category);
        int GetNotificationCount();
        int GetWarningCount();
        int GetDangerCount();

        void RefreshNotifications();
    }
}
