using Newtonsoft.Json;
using Rhetos.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhetos.HttpNotifications
{

    /// <summary>
    /// Event handler / task generator.
    /// </summary>
    public class HttpNotificationsDispatcher
    {
        // TODO: Can this be singleton?

        private readonly IBackgroundJob _backgroundJob;
        private readonly ISubscriptions _subscriptions;

        public HttpNotificationsDispatcher(IBackgroundJob backgroundJob, ISubscriptions subscriptions, HttpNotificationsOptions options)
        {
            _backgroundJob = backgroundJob;
            _subscriptions = subscriptions;
            SuppressAll = options.SuppressAll;
            SuppressEventTypes = options.SuppressEventTypes?.ToHashSet(StringComparer.Ordinal); // Making copy to avoid modifying the global setting when modifying this instance.
        }

        /// <summary>
        /// Suppress all notification in current scope (in current web request, e.g.).
        /// Initialized from <see cref="HttpNotificationsOptions"/>.
        /// </summary>
        public bool SuppressAll { get; set; }

        /// <summary>
        /// Suppress notification for specific event types in current scope (in current web request, e.g.).
        /// Initialized from <see cref="HttpNotificationsOptions"/>.
        /// </summary>
        public HashSet<string> SuppressEventTypes { get; set; }

        public void NotifySubscribers(string eventType, object eventData)
        {
            if (SuppressAll || (SuppressEventTypes != null && SuppressEventTypes.Contains(eventType)))
                return;

            var eventSubscriptions = _subscriptions.GetSubscriptions(eventType);
            if (eventSubscriptions.Any())
            {
                var payload = new { EventType = eventType, Data = eventData };
                string payloadJson = JsonConvert.SerializeObject(payload, Formatting.Indented);

                foreach (var subscription in eventSubscriptions)
                {
                    // TODO: EnqueueAction is unusable from plugin packages, since we need to reference Action type from the generated app that does not exist here.
                    var sendNotificationsJob = new //RhetosHttpNotifications.SendHttpNotification
                    {
                        NotificationId = Guid.NewGuid(),
                        Url = subscription.CallbackUrl,
                        Payload = payloadJson,
                    };
                    _backgroundJob.EnqueueAction(sendNotificationsJob, executeInUserContext: false, optimizeDuplicates: true);
                }
            }
        }
    }
}