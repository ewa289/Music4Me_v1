using Music4Me.Common.Application;
using Music4Me.Common.Http;
using System;

namespace Music4Me.Common.Infrastructure.Spotify
{
    public class SpotifyServiceSettings : ApiClientSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenEndpoint { get; set; }
    }
}
