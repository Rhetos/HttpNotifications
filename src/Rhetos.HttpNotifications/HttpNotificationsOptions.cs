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

using Rhetos.Logging;

namespace Rhetos.HttpNotifications
{
    [Options("HttpNotifications")]
    public class HttpNotificationsOptions
    {
        /// <summary>
        /// Suppress all notification.
        /// Note that this option can also be configured per scope (per web request) in <see cref="HttpNotificationsDispatcher"/> class.
        /// </summary>
        public bool SuppressAll { get; set; }

        /// <summary>
        /// Suppress notification for specific event types.
        /// Note that this option can also be configured per scope (per web request) in <see cref="HttpNotificationsDispatcher"/> class.
        /// </summary>
        public string[] SuppressEventTypes { get; set; }

        /// <summary>
        /// Override sending HTTP notifications in test environment by writing them to database or system log instead.
        /// </summary>
        public SendNotificationsMethod SendNotifications { get; set; } = SendNotificationsMethod.Http;
    }

    public enum SendNotificationsMethod
    {
        /// <summary>
        /// Sends HTTP POST request.
        /// </summary>
        Http,
        /// <summary>
        /// Writes notifications to database log (Common.Log table).
        /// </summary>
        DatabaseLog,
        /// <summary>
        /// Logs notifications to system log (<see cref="ILogProvider"/>).
        /// </summary>
        SystemLog,
    };
}