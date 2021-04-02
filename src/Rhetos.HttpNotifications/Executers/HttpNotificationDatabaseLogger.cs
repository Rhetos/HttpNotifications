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

        public object PrepareContent(HttpNotification notification)
        {
            // TODO: Review if this method should return HttpNotification directly, after refactoring Rhetos.Jobs to allow executer that is not a DSL action.
            var payload = JsonConvert.SerializeObject(notification);
            return payload;
        }

        public void Post(string url, object content)
        {
            string sql = "INSERT INTO Common.Log (Action, Description) SELECT @p0, @p1";

            var command = new SqlCommand(sql, (SqlConnection)_persistenceTransaction.Connection, (SqlTransaction)_persistenceTransaction.Transaction);
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@p0", LogActionName),
                new SqlParameter("@p1", $"URL: {url}|Payload: {content}"), // TODO: Simplify logging as a single JSON object, after refactoring Rhetos.Jobs to allow executer that is not a DSL action.
            });
            command.ExecuteNonQuery();
        }
    }
}