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

        public HttpNotificationSystemLogger(ILogProvider logProvider) // This class is registered as singleton.
        {
            _logger = logProvider.GetLogger(LogEventName);
        }

        public object PrepareContent(HttpNotification notification)
        {
            // TODO: Review if this method should return HttpNotification directly, after refactoring Rhetos.Jobs to allow executer that is not a DSL action.
            var payload = JsonConvert.SerializeObject(notification);
            return payload;
        }

        public void Post(string url, object content)
        {
            _logger.Info($"URL: {url}|Payload: {content}"); // TODO: Simplify logging as a single JSON object, after refactoring Rhetos.Jobs to allow executer that is not a DSL action.
        }
    }
}