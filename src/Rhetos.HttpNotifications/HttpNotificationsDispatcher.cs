using Newtonsoft.Json;
using Rhetos.Dom;
using Rhetos.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhetos.HttpNotifications
{

    /// <summary>
    /// Event handler. Generates tasks based on subscriptions.
    /// </summary>
    public class HttpNotificationsDispatcher
    {
        private readonly IBackgroundJob _backgroundJob;
        private readonly ISubscriptions _subscriptions;
        private readonly IDomainObjectModel _domainObjectModel;
        private readonly IHttpNotificationSender _httpNotificationSender;

        public HttpNotificationsDispatcher(
            IBackgroundJob backgroundJob,
            ISubscriptions subscriptions,
            HttpNotificationsOptions options,
            IDomainObjectModel domainObjectModel,
            IHttpNotificationSender httpNotificationSender)
        {
            _backgroundJob = backgroundJob;
            _subscriptions = subscriptions;
            _domainObjectModel = domainObjectModel;
            _httpNotificationSender = httpNotificationSender;
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
                var notification = new HttpNotification { EventType = eventType, NotificationId = Guid.NewGuid(), Data = eventData };
                var payload = _httpNotificationSender.PrepareContent(notification);

                foreach (var subscription in eventSubscriptions)
                {
                    // TODO: EnqueueAction is difficult to use from a plugin packages, since we need to reference Action type from the generated app that does not exist here.
                    // TODO: Remove this after refactoring Rhetos.Jobs to support any job executer instead of DSL Actions only.
                    var sendNotificationsJob = (ISendHttpNotification)Activator.CreateInstance(_domainObjectModel.GetType("RhetosHttpNotifications.SendHttpNotification"));
                    sendNotificationsJob.Url = subscription.CallbackUrl;
                    sendNotificationsJob.Payload = (string)payload;
                    _backgroundJob.EnqueueAction(sendNotificationsJob, executeInUserContext: false, optimizeDuplicates: true);
                }
            }
        }
    }
}