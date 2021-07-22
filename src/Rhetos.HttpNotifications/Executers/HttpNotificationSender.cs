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