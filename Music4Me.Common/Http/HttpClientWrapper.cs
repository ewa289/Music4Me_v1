using Music4Me.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Music4Me.Common.Http
{
    public class HttpClientWrapper
    {
        private readonly HttpClient httpClient;

        public AuthenticationHeaderValue AuthorizationHeader {
            get { return this.httpClient.DefaultRequestHeaders.Authorization; } 
        }
        public HttpClientWrapper(
            HttpClient client)
        {
            this.httpClient = client ?? throw new ArgumentNullException(nameof(client));

            this.httpClient.DefaultRequestHeaders.Accept.Clear();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

        public void AddHeader(string name, string value)
        {
            this.httpClient.DefaultRequestHeaders.Add(name, value);
        }
       
        public void DisableCache(bool noCache)
        {
            this.httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue {
                NoCache = noCache
            };
        }

        public void SetTimeout(double seconds)
        {
            this.httpClient.Timeout = TimeSpan.FromSeconds(seconds);
        }


        public async Task<T> Get<T>(string uri)
        {
            try {
                var response = await MakeRequest(() => this.httpClient.GetAsync(uri));
                return await Deserialize<T>(response);
            } 
            catch (HttpClientRequestException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<(T result, HttpResponseHeaders headers)> GetWithHeaders<T>(string uri)
        {
            try {
                var response = await MakeRequest(() => this.httpClient.GetAsync(uri));
                return (await Deserialize<T>(response), response.Headers);
            } 
            catch (HttpClientRequestException) { throw; } 
            catch (Exception) { throw; }
        }

        public async Task<T> Post<T>(string uri, HttpContent content)
        {
            try {
                var response = await MakeRequest(() => this.httpClient.PostAsync(uri, content));
                return await Deserialize<T>(response);
            } 
            catch (HttpClientRequestException) { throw; } 
            catch (Exception) { throw; }
        }

        public async Task<T> Post<T>(string uri, object item)
        {
            return await Post<T>(uri, Content(item));
        }

        public async Task Post(string uri, HttpContent content)
        {
            try {
                await MakeRequest(() => this.httpClient.PostAsync(uri, content));
            } 
            catch (HttpClientRequestException) { throw; } 
            catch (Exception) { throw; }
        }

        public async Task Post(string uri, object item)
        {
            await Post(uri, Content(item));
        }

        public async Task<T> Put<T>(string uri, HttpContent content)
        {
            try {
                var response = await MakeRequest(() => this.httpClient.PutAsync(uri, content));
                return await Deserialize<T>(response);
            } 
            catch (HttpClientRequestException) { throw; } 
            catch (Exception) { throw; }
        }

        public async Task<T> Put<T>(string uri, object item)
        {
            return await Put<T>(uri, Content(item));
        }

        public async Task Put(string uri, HttpContent content)
        {
            try {
                await MakeRequest(() => this.httpClient.PutAsync(uri, content));
            } 
            catch (HttpClientRequestException) { throw; } 
            catch (Exception) { throw; }
        }

        public async Task Put(string uri, object item)
        {
            await Put(uri, Content(item));
        }

        public async Task Delete(string uri)
        {
            try {
                await MakeRequest(() => this.httpClient.DeleteAsync(uri));
            } 
            catch (HttpClientRequestException) { throw; } 
            catch (Exception) { throw; }
        }

        public async void Download(string uri, string path, int? timeoutInSeconds = null)
        {
            try {
                if (timeoutInSeconds != null) {
                    try {
                        this.httpClient.Timeout = new TimeSpan(0, 0, timeoutInSeconds.Value);
                    } catch (InvalidOperationException) {
                    }
                }
                using (var stream = await MakeRequest(() => this.httpClient.GetAsync(uri)).Result.Content.ReadAsStreamAsync()) {
                    using (var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)) {
                        stream.CopyTo(file);
                    }
                }
            } catch (Exception ex) {
                throw new InvalidOperationException("Failed to call " + String.Format(uri), ex);
            }
        }

        public async Task<Stream> OpenRead(string uri)
        {
            return await this.httpClient.GetStreamAsync(uri);
        }

        private static StringContent Content(object item)
        {
            return new StringContent(item.Serialize(), Encoding.UTF8, "application/json");
        }

        private async Task<T> MakeRequest<T>(Func<Task<T>> action) where T : HttpResponseMessage
        {
            var response = await action();
            
            if (response.IsSuccessStatusCode) {
                response.EnsureSuccessStatusCode();
            } else {
                var serverMsg = await response.Content.ReadAsStringAsync();
                throw new HttpClientRequestException(response.StatusCode, serverMsg.NullIfEmpty());
            }
            return response;
        }

        private async static Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            if (response == null) return default;

            var json = await response?.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTHH:mm:sszzz",
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}