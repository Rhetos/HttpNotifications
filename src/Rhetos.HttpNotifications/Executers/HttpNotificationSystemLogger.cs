using Newtonsoft.Json;
using Rhetos.Logging;

namespace Rhetos.HttpNotifications
{
    /// <summary>
    /// Writes notifications to system log (<see cref="ILogProvider"/>).
    /// </summary>
    public class HttpNotificationSystemLogger : IHttpNotificationSender
    {
        public static readonly string LogEventName = "HttpNotification";

        private readonly ILogger _logger;

        public HttpNotificationSystemLogger(ILogProvider logProvider)
        {
            _logger = logProvider.GetLogger(LogEventName);
        }

        public void Execute(HttpNotificationRequest job)
        {
            _logger.Info(() => JsonConvert.SerializeObject(job));
        }
    }
}