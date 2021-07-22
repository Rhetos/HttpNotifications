/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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