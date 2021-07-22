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

        public HttpNotificationSystemLogger(ILogProvider logProvider)
        {
            _logger = logProvider.GetLogger(LogEventName);
        }

        public void Execute(HttpNotificationRequest job)
        {
            _logger.Info(() => JsonConvert.SerializeObject(job));
        }
    }
}