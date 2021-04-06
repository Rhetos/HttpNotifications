using Rhetos.Jobs;

namespace Rhetos.HttpNotifications
{
    public interface IHttpNotificationSender : IJobExecuter<HttpNotificationRequest>
    {
    }
}