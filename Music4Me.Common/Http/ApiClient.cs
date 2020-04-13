using Music4Me.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Music4Me.Common.Http
{
    public abstract class ApiClient
    {
        protected readonly HttpClientWrapper http;

        public ApiClient(
            IHttpClientFactory httpClientFactory,
            ApiClientSettings settings)
        {
            var httpClient = httpClientFactory.CreateClient();
            if (!settings.EndPoint.IsNullOrEmpty()) {
                httpClient.BaseAddress = new Uri(settings.EndPoint.EndsWith("/") ? settings.EndPoint : settings.EndPoint + "/");
            }
            this.http = new HttpClientWrapper(httpClient);
        }
    }
}
