using Newtonsoft.Json;
using Rhetos.Dom;
using Rhetos.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhetos.HttpNotifications
{

    /// <summary>
    /// Event handler. Generates background tasks based on subscriptions.
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

        /// <summary>
        /// Generates background tasks for sending HTTP notifications to subscribers.
        /// Subscribers are matched by <paramref name="eventType"/>.
        /// The HTTP notifications will contain <paramref name="eventData"/> in request body.
        /// </summary>
        /// <param name="eventType">
        /// Event type is used to select subscribers. It is included in HTTP request body.
        /// </param>
        /// <param name="eventData">
        /// The data that is sent to subscribers in HTTP request body.
        /// </param>
        /// <param name="aggregationGroup">
        /// Duplicate notifications within the same <paramref name="aggregationGroup"/> will be removed,
        /// or combined into a single notification with <paramref name="jobAggregator"/>.
        /// If set to null, the default grouping will be used, that removes duplicate notifications with same event type and data,
		/// The value can be any type (e.g. a string or anonymous type). If a custom class is used, it should override <see cref="object.Equals(object)"/> and <see cref="object.GetHashCode"/>.
        /// The aggregation group is automatically extended with the subscription callback URL.
		/// See <see cref="IBackgroundJob.AddJob"/> parameters for more info.
        /// </param>
        /// <param name="jobAggregator">
        /// Combines multiple notifications within the same <paramref name="aggregationGroup"/> into a single one.
        /// If null, duplicate notifications within the group are removed, leaving only the last one.
        /// Aggregation works in a single DI scope (unit of work).
        /// </param>
        public void NotifySubscribers(
            string eventType,
            object eventData,
            object aggregationGroup = null,
            JobAggregator<HttpNotificationRequest> jobAggregator = null)
        {
            if (SuppressAll || (SuppressEventTypes != null && SuppressEventTypes.Contains(eventType)))
                return;

            var eventSubscriptions = _subscriptions.GetSubscriptions(eventType);
            if (eventSubscriptions.Any())
            {
                var payload = new HttpNotification
                {
                    EventType = eventType,
                    NotificationId = Guid.NewGuid(),
                    Data = eventData
                };

                foreach (var subscription in eventSubscriptions)
                {
                    var request = new HttpNotificationRequest
                    {
                        Url = subscription.CallbackUrl,
                        Payload = payload
                    };

                    // Removing duplicate notifications within a single unit of work.
                    object requestAggregationGroup;
                    if (aggregationGroup != null)
                        requestAggregationGroup = new
                        {
                            request.Url,
                            aggregationGroup
                        };
                    else
                        requestAggregationGroup = new
                        {
                            request.Url,
                            request.Payload.EventType,
                            DataJson = JsonConvert.SerializeObject(request.Payload.Data)
                        };

                    _backgroundJob.AddJob<IHttpNotificationSender, HttpNotificationRequest>(
                        request,
                        false,
                        requestAggregationGroup,
                        jobAggregator);
                }
            }
        }
    }
}