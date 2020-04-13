using Microsoft.Extensions.Caching.Memory;
using Music4Me.Common.Extensions;
using Music4Me.Common.Http;
using Music4Me.Common.Infrastructure.Spotify.Resources;
using Music4Me.Common.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Music4Me.Common.Infrastructure.Spotify
{
    public class SpotifyService : ApiClient, IMusicService
    {
        private readonly SpotifyServiceSettings settings;
        private readonly IMemoryCache cache;

        public SpotifyService(
            IHttpClientFactory httpClientFactory,
            SpotifyServiceSettings settings,
            IMemoryCache cache) : base(httpClientFactory, settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.cache = cache;
        }
        
        public async Task<IList<T>> GetCategories<T>(string country)
        {
            var uri = "browse/categories";
            
            if (!country.IsNullOrEmpty()) {
                uri = $"{uri}?country={country}";
            }
            var response = await SendRequest(() => this.http.Get<JObject>(uri));
            return response["categories"]["items"].ToString().Deserialize<List<T>>();
        }

        public async Task<IList<T>> GetArtists<T>(string category, string country)
        {
            // Get playlists for category
            var playlists = await GetPlaylists(category, country);

            // Get artist ids from playlist tracks
            var artistIds = await GetArtistsIds(playlists);

            // Get artists
            var artists = await GetArtists<T>(artistIds);

            return artists;
        }

        public async Task<IList<T>> GetRelatedArtists<T>(IList<string> artistIds)
        {
            var relatedArtists = new List<T>();
            foreach (var id in artistIds) {
                var response = await SendRequest(() => this.http.Get<JObject>($"artists/{id}/related-artists"));
                relatedArtists.AddRange(response["artists"].ToString().Deserialize<List<T>>());
            }
            return relatedArtists;
        }
        private async Task<IList<T>> GetArtists<T>(IList<string> artistIds)
        {
            var artists = new List<T>();
            foreach (var ids in artistIds.Distinct().Paged(50)) {
                var response = await SendRequest(() => this.http.Get<JObject>($"artists?ids={string.Join(",", ids)}"));
                artists.AddRange(response["artists"].ToString().Deserialize<List<T>>());
            }

            return artists.ToList();
        }

        private async Task<IList<Playlist>> GetPlaylists(string category, string country)
        {
            var uri = $"browse/categories/{category}/playlists";

            if (!country.IsNullOrEmpty()) {
                uri = $"{uri}?country={country}";
            }

            var response = await SendRequest(() => this.http.Get<JObject>(uri));
            if (response == null) throw new Exception($"Failed to get artists for category {category}");

            return response["playlists"]["items"].ToString().Deserialize<List<Playlist>>();
        }

        private async Task<IList<string>> GetArtistsIds(IList<Playlist> playlists)
        {
            var artistIds = new List<string>();
            foreach (var playlist in playlists) {
                var response = await SendRequest(() => this.http.Get<JObject>(playlist.TracksUrl.Href));
                var playlistItems = response["items"].ToString().Deserialize<List<PlaylistItem>>();

                artistIds.AddRange(playlistItems.Where(i => i.Track?.Artists != null)
                                                 .SelectMany(i => i.Track.Artists)
                                                 .Select(a => a.Id));
            }
            return artistIds;
        }

        private async Task<T> SendRequest<T>(Func<Task<T>> action)
        {
            T response;
            try {
                await SetClientCredentialsAuthToken();
                response = await action();

            } catch (HttpClientRequestException ex) {
                if (ex.StatusCode == HttpStatusCode.Unauthorized) {
                    await SetClientCredentialsAuthToken(true);
                    response = await action();
                } else throw;
            }
            return response;
        }

        private async Task SetClientCredentialsAuthToken(bool forceRefresh = false)
        {
            if (forceRefresh || this.http.AuthorizationHeader == null) {
                var cacheKey = "Spotify.ClientCredentialsAuthToken";
                var token = this.cache.Get<string>(cacheKey);

                if (token.IsNullOrEmpty()) {
                    var tokenFetchTime = DateTime.Now;

                    var webClient = new WebClient();
                    var postparams = new NameValueCollection();
                    postparams.Add("grant_type", "client_credentials");

                    var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{this.settings.ClientId}:{this.settings.ClientSecret}"));
                    webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

                    var tokenResponse = await webClient.UploadValuesTaskAsync(this.settings.TokenEndpoint, postparams);

                    var spotifyToken = Encoding.UTF8.GetString(tokenResponse).Deserialize<SpotifyToken>();

                    token = spotifyToken.AccessToken;

                    // Save token to cache
                    var tokenExpirationTime = tokenFetchTime.AddSeconds(spotifyToken.ExpirationPeriod);
                    this.cache.Set(cacheKey, token, new DateTimeOffset(tokenExpirationTime));
                }
                this.http.AddHeader("Authorization", $"Bearer {token}");
            }
        }
    }
}
