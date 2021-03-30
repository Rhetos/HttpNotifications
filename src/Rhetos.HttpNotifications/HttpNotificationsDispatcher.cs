using Newtonsoft.Json;
using Rhetos.Dom;
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
        private readonly IBackgroundJob _backgroundJob;
        private readonly ISubscriptions _subscriptions;
        private readonly IDomainObjectModel _domainObjectModel;

        public HttpNotificationsDispatcher(IBackgroundJob backgroundJob, ISubscriptions subscriptions, HttpNotificationsOptions options, IDomainObjectModel domainObjectModel)
        {
            _backgroundJob = backgroundJob;
            _subscriptions = subscriptions;
            _domainObjectModel = domainObjectModel;
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
                Guid notificationId = Guid.NewGuid();
                var payload = new { EventType = eventType, NotificationId = notificationId, Data = eventData };
                string payloadJson = JsonConvert.SerializeObject(payload);

                foreach (var subscription in eventSubscriptions)
                {
                    // TODO: EnqueueAction is difficult to use from a plugin packages, since we need to reference Action type from the generated app that does not exist here.

                    // TODO: Remove this after refactoring Rhetos.Jobs to support any job executer instead of DSL Actions only.
                    var sendNotificationsJob = (ISendHttpNotification)Activator.CreateInstance(_domainObjectModel.GetType("RhetosHttpNotifications.SendHttpNotification"));
                    sendNotificationsJob.Url = subscription.CallbackUrl;
                    sendNotificationsJob.Payload = payloadJson;

                    _backgroundJob.EnqueueAction(sendNotificationsJob, executeInUserContext: false, optimizeDuplicates: true);
                }
            }
        }
    }
}