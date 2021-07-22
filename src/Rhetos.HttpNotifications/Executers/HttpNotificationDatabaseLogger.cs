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
using Rhetos.Persistence;
using System.Data.SqlClient;

namespace Rhetos.HttpNotifications
{
    /// <summary>
    /// Writes notifications to database log (Common.Log table).
    /// </summary>
    public class HttpNotificationDatabaseLogger : IHttpNotificationSender
    {
        public static readonly string LogActionName = "HttpNotification";

        private readonly IPersistenceTransaction _persistenceTransaction;

        public HttpNotificationDatabaseLogger(IPersistenceTransaction persistenceTransaction)
        {
            _persistenceTransaction = persistenceTransaction;
        }

        public void Execute(HttpNotificationRequest job)
        {
            string sql = "INSERT INTO Common.Log (Action, Description) SELECT @p0, @p1";
            var command = new SqlCommand(sql, (SqlConnection)_persistenceTransaction.Connection, (SqlTransaction)_persistenceTransaction.Transaction);
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@p0", LogActionName),
                new SqlParameter("@p1", JsonConvert.SerializeObject(job)),
            });
            command.ExecuteNonQuery();
        }
    }
}