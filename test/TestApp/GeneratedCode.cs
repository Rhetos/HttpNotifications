using Newtonsoft.Json;
using Rhetos.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhetos.Events
{
    //=========================================================================
    // This is generated code in application domain object model.
    //=========================================================================

    // TODO: Rename to "Event" or "RhetosEventType" or "BusinessEventType". "EventType" should not be used because of conflict with Rhetos.Logging.EventType.
    //public enum RhetosEventType
    //{
    //    Bookstore_Book_Deleted,
    //    Bookstore_Book_Updated,
    //    Bookstore_Book_Inserted,
    //    Bookstore_Book_InsertedAny,
    //    Bookstore_Book_InsertedImportantBook
    //}

    public static class EventName
    {
        public const string Bookstore_Book_Deleted = "Bookstore_Book_Deleted";
        /*eventtype*/
    }

    public class EventProcessing : IEventProcessing
    {
        private readonly Rhetos.HttpNotifications.HttpNotificationsDispatcher _httpNotificationsDispatcher;
        /*fields*/

        public EventProcessing(
            // Note for extensions: Use Lazy dependencies for event-specific handlers.
            Rhetos.HttpNotifications.HttpNotificationsDispatcher httpNotificationsDispatcher
            /*constructor*/)
        {
            _httpNotificationsDispatcher = httpNotificationsDispatcher;
        }

        private static readonly Dictionary<string, Type> _eventTypes = new (string EventType, Type EventDataType)[]
        {
            ("Bookstore_Book_Deleted", typeof(IEnumerable<Guid>)),
            ("Bookstore_Book_Updated", typeof(IEnumerable<Guid>)),
            ("Bookstore_Book_Inserted", typeof(IEnumerable<Guid>)),
            ("Bookstore_Book_InsertedAny", typeof(object)),
            ("Bookstore_Book_InsertedImportantBook", typeof(object)),
            /*eventtype*/
        }.ToDictionary(e => e.EventType, e => e.EventDataType, StringComparer.Ordinal);

        public IEnumerable<string> GetEventTypes() => _eventTypes.Keys;

        public void EmitEvent(string eventType, object eventData)
        {
            if (_eventTypes.TryGetValue(eventType, out Type eventDataType))
            {
                if (eventData != null && !eventDataType.IsInstanceOfType(eventData))
                    throw new ArgumentException($"Invalid data type provided for event '{eventType}'. Expected '{eventDataType}', provided '{eventData.GetType()}'.");
            }
            else
                throw new ArgumentException($"Cannot emit event type '{eventType}' because it is not registered.");

            // Cancel or override events:
            /*Before event handled*/

            // TODO: Specific event handlers should be inserted directly at the code that emits them? There is an issue with dependency on the code generator. IEventProcessing in intended for generic event handler, but also to specific event handles that cannot have direct reference to the component that emits the event.
            // Specific event handlers by event type:
            switch (eventType)
            {
                // TODO: Specific event cases could be created by a DSL concept, so that this switch will have only the cases that are used.
                case "Bookstore_Book_Deleted":
                    /*Event type handler Bookstore_Book_Deleted*/
                    break;
                case "Bookstore_Book_Updated":
                    /*Event type handler Bookstore_Book_Updated*/
                    break;
                case "Bookstore_Book_Inserted":
                    /*Event type handler Bookstore_Book_Inserted*/
                    break;
                case "Bookstore_Book_InsertedAny":
                    /*Event type handler Bookstore_Book_InsertedAny*/
                    break;
                case "Bookstore_Book_InsertedImportantBook":
                    /*Event type handler Bookstore_Book_InsertedImportantBook*/
                    break;
            }

            // Other event handlers:
            _httpNotificationsDispatcher.NotifySubscribers(eventType, eventData);
            /*Event handler*/
        }
    }
}
