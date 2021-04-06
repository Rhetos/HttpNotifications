using Newtonsoft.Json;
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

        public void Execute(HttpNotificationRequest job)
        {
            string payloadJson = JsonConvert.SerializeObject(job.Payload);
            var httpContent = new StringContent(payloadJson, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(job.Url, httpContent).Result;

            if (!response.IsSuccessStatusCode)
            {
                string responseContent;
                try
                {
                    responseContent = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    responseContent = $"Error while reading HTTP response content. {e}";
                }

                // TODO: Log full error, put summary in the exception.

                throw new HttpRequestException(
                    $"{GetType().Name}: HTTP POST failed with {response.StatusCode} {response.ReasonPhrase}. Callback URL: {job.Url}" +
                    $"{Environment.NewLine}Response content: {responseContent.Limit(5000, true)}" +
                    $"{Environment.NewLine}Request content: {payloadJson.Limit(5000, true)}");
            }
        }
    }
}