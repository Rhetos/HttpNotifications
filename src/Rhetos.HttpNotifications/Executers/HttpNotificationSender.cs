using Newtonsoft.Json;
using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.Net.Http;
using System.Text;

namespace Rhetos.HttpNotifications
{
    /// <summary>
    /// Sends HTTP POST request.
    /// </summary>
    public class HttpNotificationSender : IHttpNotificationSender
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly ILogProvider _logProvider;

        public HttpNotificationSender(ILogProvider logProvider) // This class is registered as singleton.
        {
            _logProvider = logProvider;
        }

        public object PrepareContent(HttpNotification notification)
        {
            // TODO: Review if this method should return HttpNotification directly after refactoring Rhetos.Jobs to allow executer that is not a DSL action.
            var payload = JsonConvert.SerializeObject(notification);
            return payload;
        }

        public void Post(string url, object content)
        {
            string stringContent = (string)content;
            var httpContent = new StringContent(stringContent, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, httpContent).Result;

            if (!response.IsSuccessStatusCode)
            {
                string responseContent;
                try
                {
                    responseContent = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    responseContent = $"Error while reading HTTP response content. {e.GetType()}: {e.Message}";
                }

                // TODO: Log full error, put summary in the exception.

                throw new HttpRequestException(
                    $"{GetType().Name}: HTTP POST failed with {response.StatusCode} {response.ReasonPhrase}. Callback URL: {url}" +
                    $"{Environment.NewLine}Response content: {responseContent.Limit(5000, true)}" +
                    $"{Environment.NewLine}Request content: {stringContent.Limit(5000, true)}");
            }
        }
    }
}