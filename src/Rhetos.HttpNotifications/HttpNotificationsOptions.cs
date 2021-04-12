using Rhetos.Logging;

namespace Rhetos.HttpNotifications
{
    [Options("HttpNotifications")]
    public class HttpNotificationsOptions
    {
        /// <summary>
        /// Suppress all notification.
        /// Note that this option can also be configured per scope (per web request) in <see cref="HttpNotificationsDispatcher"/> class.
        /// </summary>
        public bool SuppressAll { get; set; }

        /// <summary>
        /// Suppress notification for specific event types.
        /// Note that this option can also be configured per scope (per web request) in <see cref="HttpNotificationsDispatcher"/> class.
        /// </summary>
        public string[] SuppressEventTypes { get; set; }

        /// <summary>
        /// Override sending HTTP notifications in test environment by writing them to database or system log instead.
        /// </summary>
        public SendNotificationsMethod SendNotifications { get; set; } = SendNotificationsMethod.Http;
    }

    public enum SendNotificationsMethod
    {
        /// <summary>
        /// Sends HTTP POST request.
        /// </summary>
        Http,
        /// <summary>
        /// Writes notifications to database log (Common.Log table).
        /// </summary>
        DatabaseLog,
        /// <summary>
        /// Logs notifications to system log (<see cref="ILogProvider"/>).
        /// </summary>
        SystemLog,
    };
}