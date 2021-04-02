namespace Rhetos.HttpNotifications
{
    public interface IHttpNotificationSender
    {
        /// <summary>
        /// Prepare notification content once, to be sent to multiple subscribers.
        /// </summary>
        object PrepareContent(HttpNotification notification);

        /// <summary>
        /// Send HTTP POST request.
        /// </summary>
        /// <param name="content">
        /// Content object created by <see cref="PrepareContent(HttpNotification)"/> method.
        /// </param>
        void Post(string url, object content);
    }
}