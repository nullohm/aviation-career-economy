using System;

namespace Ace.App.Models
{
    public enum NotificationType
    {
        Info,
        Warning,
        Danger,
        Success
    }

    public enum NotificationCategory
    {
        Maintenance,
        Loan,
        Fleet,
        FBO,
        Pilot
    }

    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public NotificationType Type { get; set; }
        public NotificationCategory Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? RelatedEntityId { get; set; }
    }
}
