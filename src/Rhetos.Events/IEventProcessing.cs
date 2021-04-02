using System.Collections.Generic;

namespace Rhetos.Events
{
    /// <summary>
    /// This interface represents an event channel, and is implemented by the central event processing component.
    /// The processing component is extended by code generators, to contain various internal event handlers.
    /// This is similar to the publish-subscribe pattern, but with subscribers specified at build-time.
    /// The event handlers are executed synchronously, but they might generate asynchronous tasks or background jobs.
    /// </summary>
    /// <remarks>
    /// There is no need to use the event processing for a specific event that is intended for a single specific event handler.
    /// The emitted events are expected to be handled by generic event handlers (for example, a handler that manages run-time event subscriptions),
    /// or to allow implementation of an event handler that has no information on which component emits the event.
    /// </remarks>
    public interface IEventProcessing
    {
        /// <summary>
        /// Emitting an event allows various event handlers to process them (for example, HTTP notifications to an external system).
        /// </summary>
        /// <param name="eventName">
        /// Name of the specific event type, for example Bookstore_Book_Inserted.
        /// The event must be registered at design-time by <see cref="EventInfo"/> concept.
        /// </param>
        /// <param name="eventData">
        /// Optional (set to null if not needed).
        /// Event-specific data to be used by event handlers.
        /// Note that this data may be sent to an external system as event notification (in webhooks, for example).
        /// The <paramref name="eventData"/> value type must match the registered event data type in <see cref="EventInfo"/> concept, if specified.
        /// </param>
        void EmitEvent(string eventName, object eventData);

        /// <summary>
        /// Returns names of all event types specified in the application.
        /// </summary>
        IEnumerable<string> GetEventNames();
    }
}