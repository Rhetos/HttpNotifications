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

    // TODO: Can this be singletone? No.


    public class EventProcessing : IEventProcessing // TODO: IBroker  IEventBus  IChannel
    {
        private readonly Rhetos.HttpNotifications.HttpNotificationsDispatcher _httpNotificationsDispatcher;
        /*fields*/

        public EventProcessing(
            Rhetos.HttpNotifications.HttpNotificationsDispatcher httpNotificationsDispatcher
            /*constructor*/)
        {
            _httpNotificationsDispatcher = httpNotificationsDispatcher;
        }

        public void EmitEvent(string eventType, object eventData)
        {
            // Cancel or override events:
            /*Before event handled*/

            // TODO: Specific event handlers should be inserted directly at the code that emits them? There is an issue with dependency on the code generator. IEventProcessing in intended for generic event handler, but also to specific event handles that cannot have direct reference to the component that emits the event.
            // Specific event handlers by event type:
            switch (eventType)
            {
                // TODO: One specific event cases could be created by a DSL concept, so that this switch will have only the ones that are used.
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

namespace Bookstore
{
    using Rhetos.Events;

    public class BookHelper
    {
        private readonly IEventProcessing _eventProcessingEngine;

        public BookHelper(IEventProcessing eventProcessingEngine)
        {
            _eventProcessingEngine = eventProcessingEngine;
        }

        public void EmitCrudEvents(IEnumerable<Book> insertedNew, IEnumerable<Book> updatedNew, IEnumerable<Book> deletedIds)
        {
            //========================================================================================
            // THIS CODE CAN BE PLACED IN THE DSL SCRIPT OR GENERATED (SaveMethod AfterSave).
            //========================================================================================

            // DSL concept: EmitsCrudEvents => macro: BusinessEventType Bookstore_Book_InsertedAny, SaveMethod AfterSave.

            if (deletedIds.Any())
                _eventProcessingEngine.EmitEvent("Bookstore_Book_Deleted", deletedIds.Select(item => item.ID));

            if (updatedNew.Any())
                _eventProcessingEngine.EmitEvent("Bookstore_Book_Updated", updatedNew.Select(item => item.ID));

            if (insertedNew.Any())
                _eventProcessingEngine.EmitEvent("Bookstore_Book_Inserted", insertedNew.Select(item => item.ID));

            // Custom event example: Only one event per web request.
            // DSL concepts: BusinessEventType Bookstore_Book_InsertedAny, SaveMethod AfterSave.

            if (insertedNew.Any())
                _eventProcessingEngine.EmitEvent("Bookstore_Book_InsertedAny", (object)null);

            // Custom event example: "Important" books inserted.

            if (insertedNew.Any())
            {
                var notifyForItems = insertedNew.Where(book => book.Title.Contains("important")).Select(item => new { item.ID, item.Title }).ToList();
                if (notifyForItems.Any())
                    _eventProcessingEngine.EmitEvent("Bookstore_Book_InsertedImportantBook", notifyForItems);
            }
        }
    }
}
