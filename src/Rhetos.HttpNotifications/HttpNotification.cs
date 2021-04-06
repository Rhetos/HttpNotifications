using System;

namespace Rhetos.HttpNotifications
{
    public class HttpNotification
    {
        public string EventType { get; set; }
        public Guid NotificationId { get; set; }
        public object Data { get; set; }
    }

    public class HttpNotificationRequest
    {
        public string Url { get; set; }
        public HttpNotification Payload { get; set; }
    }
}